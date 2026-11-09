using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public sealed class FootballFbxImportPipeline : AssetPostprocessor
{
    private const string Incoming = "Assets/Incoming/FootballAssets";
    private const string ResourcesFootballers = "Assets/Resources/Footballers";
    private const string AnimationFolder = "Assets/Animations/Football";
    private const string ControllerPath = ResourcesFootballers + "/Footballer.controller";

    private void OnPreprocessModel()
    {
        if (!assetPath.StartsWith(Incoming + "/", StringComparison.OrdinalIgnoreCase)) return;
        var importer = (ModelImporter)assetImporter;
        importer.animationType = ModelImporterAnimationType.Human;
        importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        importer.importAnimation = true;
        importer.optimizeGameObjects = false;
        importer.importMaterials = true;
        importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
    }

    [MenuItem("Ultimate Football/Assets/Import FBX And Build Player Prefabs")]
    public static void ImportAndBuild()
    {
        EnsureFolder("Assets/Incoming");
        EnsureFolder(Incoming);
        EnsureFolder("Assets/Resources");
        EnsureFolder(ResourcesFootballers);
        EnsureFolder("Assets/Animations");
        EnsureFolder(AnimationFolder);

        string[] modelGuids = AssetDatabase.FindAssets("t:Model", new[] { Incoming });
        var modelPaths = modelGuids.Select(AssetDatabase.GUIDToAssetPath)
            .Where(path => path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase))
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (modelPaths.Length == 0)
        {
            EditorUtility.DisplayDialog(
                "No FBX files found",
                "Place licensed FBX files in Assets/Incoming/FootballAssets, then run this command again.",
                "OK");
            return;
        }

        foreach (string path in modelPaths)
        {
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null &&
                (importer.animationType != ModelImporterAnimationType.Human ||
                 !importer.importAnimation || importer.optimizeGameObjects))
            {
                importer.animationType = ModelImporterAnimationType.Human;
                importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                importer.importAnimation = true;
                importer.optimizeGameObjects = false;
                importer.SaveAndReimport();
            }
        }

        AssetDatabase.Refresh();
        var allClips = new List<AnimationClip>();
        foreach (string path in modelPaths)
        {
            foreach (UnityEngine.Object asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (asset is AnimationClip clip && !clip.name.StartsWith("__preview__", StringComparison.OrdinalIgnoreCase))
                {
                    allClips.Add(clip);
                    string extractedPath = AnimationFolder + "/" + SafeName(Path.GetFileNameWithoutExtension(path) + "_" + clip.name) + ".anim";
                    if (!File.Exists(extractedPath))
                        AssetDatabase.CreateAsset(UnityEngine.Object.Instantiate(clip), extractedPath);
                }
            }
        }

        BuildAnimatorController(allClips);
        GameObject source = AssetDatabase.LoadAssetAtPath<GameObject>(modelPaths[0]);
        if (source == null)
        {
            EditorUtility.DisplayDialog("FBX import failed", "Unity could not load the first FBX as a model asset.", "OK");
            return;
        }

        BuildPlayerPrefab(source, ResourcesFootballers + "/BluePlayer.prefab");
        BuildPlayerPrefab(source, ResourcesFootballers + "/RedPlayer.prefab");
        BuildNetworkBallPrefab(ResourcesFootballers + "/NetworkBall.prefab");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog(
            "Football assets prepared",
            "Imported " + modelPaths.Length + " FBX model(s) and extracted " + allClips.Count +
            " animation clip(s). Player prefabs and the network-ball prefab were generated. " +
            "Open the scene and run the Unity validation menu before gameplay testing.",
            "OK");
    }

    private static void BuildPlayerPrefab(GameObject source, string destination)
    {
        GameObject instance = PrefabUtility.InstantiatePrefab(source) as GameObject;
        if (instance == null) instance = UnityEngine.Object.Instantiate(source);
        instance.name = Path.GetFileNameWithoutExtension(destination);

        Animator animator = instance.GetComponentInChildren<Animator>(true);
        if (animator == null)
        {
            animator = instance.AddComponent<Animator>();
            string modelPath = AssetDatabase.GetAssetPath(source);
            Avatar avatar = AssetDatabase.LoadAllAssetsAtPath(modelPath).OfType<Avatar>().FirstOrDefault(a => a.isHuman);
            if (avatar != null) animator.avatar = avatar;
        }
        RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(ControllerPath);
        if (controller != null) animator.runtimeAnimatorController = controller;
        animator.applyRootMotion = false;

        if (instance.GetComponent<FootballCharacterAnimationDriver>() == null)
            instance.AddComponent<FootballCharacterAnimationDriver>();
        if (instance.GetComponent<NetworkObject>() == null)
            instance.AddComponent<NetworkObject>();
        if (instance.GetComponent<NetworkTransform>() == null)
            instance.AddComponent<NetworkTransform>();

        PrefabUtility.SaveAsPrefabAsset(instance, destination);
        UnityEngine.Object.DestroyImmediate(instance);
    }

    private static void BuildNetworkBallPrefab(string destination)
    {
        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ball.name = "NetworkBall";
        ball.transform.localScale = Vector3.one * 0.22f;
        Rigidbody body = ball.AddComponent<Rigidbody>();
        body.mass = 0.43f;
        body.linearDamping = 0.015f;
        body.angularDamping = 0.035f;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        ball.AddComponent<NetworkObject>();
        ball.AddComponent<NetworkTransform>();
        ball.AddComponent<NetworkRigidbody>();
        PrefabUtility.SaveAsPrefabAsset(ball, destination);
        UnityEngine.Object.DestroyImmediate(ball);
    }

    private static void BuildAnimatorController(List<AnimationClip> clips)
    {
        if (clips.Count == 0) return;
        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath) != null)
            AssetDatabase.DeleteAsset(ControllerPath);

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("IsSprinting", AnimatorControllerParameterType.Bool);

        string[] triggers = { "Kick", "Pass", "Tackle", "Celebrate", "Header", "Dive", "Save", "Dribble", "ThrowIn" };
        foreach (string trigger in triggers) controller.AddParameter(trigger, AnimatorControllerParameterType.Trigger);

        AnimatorStateMachine machine = controller.layers[0].stateMachine;
        AnimationClip idle = FindClip(clips, "idle") ?? clips[0];
        AnimationClip walk = FindClip(clips, "walk");
        AnimationClip run = FindClip(clips, "run") ?? FindClip(clips, "jog");
        AnimationClip sprint = FindClip(clips, "sprint");

        var locomotion = new BlendTree
        {
            name = "Football Locomotion",
            blendType = BlendTreeType.Simple1D,
            blendParameter = "Speed",
            useAutomaticThresholds = false
        };
        locomotion.AddChild(idle, 0f);
        if (walk != null) locomotion.AddChild(walk, 1.8f);
        if (run != null) locomotion.AddChild(run, 3.8f);
        if (sprint != null) locomotion.AddChild(sprint, 6f);
        AssetDatabase.AddObjectToAsset(locomotion, controller);

        AnimatorState idleState = machine.AddState("Locomotion");
        idleState.motion = locomotion;
        machine.defaultState = idleState;

        foreach (string trigger in triggers)
        {
            AnimationClip action = FindClip(clips, trigger.ToLowerInvariant());
            if (action == null) continue;
            AnimatorState actionState = machine.AddState(trigger);
            actionState.motion = action;
            AnimatorStateTransition enter = machine.AddAnyStateTransition(actionState);
            enter.hasExitTime = false;
            enter.duration = 0.08f;
            enter.AddCondition(AnimatorConditionMode.If, 0f, trigger);
            AnimatorStateTransition exit = actionState.AddTransition(idleState);
            exit.hasExitTime = true;
            exit.exitTime = 0.92f;
            exit.duration = 0.10f;
        }

        EditorUtility.SetDirty(controller);
    }

    private static AnimationClip FindClip(IEnumerable<AnimationClip> clips, string token)
    {
        return clips.FirstOrDefault(c => c.name.IndexOf(token, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private static string SafeName(string value)
    {
        foreach (char invalid in Path.GetInvalidFileNameChars()) value = value.Replace(invalid, '_');
        return value.Replace(' ', '_');
    }

    private static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        string parent = Path.GetDirectoryName(path).Replace('\\', '/');
        string name = Path.GetFileName(path);
        if (!AssetDatabase.IsValidFolder(parent)) EnsureFolder(parent);
        AssetDatabase.CreateFolder(parent, name);
    }
}
