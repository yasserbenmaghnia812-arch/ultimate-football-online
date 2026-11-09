#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FootballProjectValidator
{
    [MenuItem("Ultimate Football/Validate Project")]
    public static void ValidateProject()
    {
        List<string> errors = new List<string>();
        List<string> warnings = new List<string>();

        string manifestPath = "Packages/manifest.json";
        string manifest = File.Exists(manifestPath) ? File.ReadAllText(manifestPath) : "";
        Require(manifest.Contains("com.unity.netcode.gameobjects"), "Netcode for GameObjects package is missing.", errors);
        Require(manifest.Contains("com.unity.transport"), "Unity Transport package is missing.", errors);
        Require(manifest.Contains("com.unity.services.multiplayer"), "Multiplayer Services package is missing.", errors);
        Require(File.Exists("Assets/Scenes/Main.unity"), "Main scene is missing; run the build script once to generate it.", errors);

        bool androidSupported = BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android);
        Require(androidSupported, "Android Build Support module is not installed in this Unity Editor.", errors);

        bool bluePrefab = File.Exists("Assets/Resources/Footballers/BluePlayer.prefab");
        bool redPrefab = File.Exists("Assets/Resources/Footballers/RedPlayer.prefab");
        if (!bluePrefab || !redPrefab)
            warnings.Add("Licensed BluePlayer/RedPlayer prefabs are not both imported; the game will use procedural placeholders.");

        string[] controllers = AssetDatabase.FindAssets("t:AnimatorController");
        if (controllers.Length == 0)
            warnings.Add("No Animator Controller asset found; imported player prefabs will not have configured football action states.");

        Require(Application.unityVersion.StartsWith("6000.0"), "Project should be opened with Unity 6000.0.x for its pinned package set. Current: " + Application.unityVersion, errors);

        AssetDatabase.Refresh();
        foreach (string warning in warnings) Debug.LogWarning("[Football Validator] " + warning);
        foreach (string error in errors) Debug.LogError("[Football Validator] " + error);

        string report = "Unity: " + Application.unityVersion + "\nErrors: " + errors.Count + "\nWarnings: " + warnings.Count;
        if (errors.Count > 0) report += "\n\n" + string.Join("\n", errors.ToArray());
        if (warnings.Count > 0) report += "\n\nWarnings:\n" + string.Join("\n", warnings.ToArray());
        EditorUtility.DisplayDialog("Ultimate Football Validation", report, "OK");
    }

    private static void Require(bool condition, string message, List<string> errors)
    {
        if (!condition) errors.Add(message);
    }
}
#endif
