using System;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>Import pipeline for original/licensed humanoid footballer FBX files.
/// Place source FBX files in Assets/ThirdParty/Footballers before running the menu.</summary>
public sealed class FootballModelImportPipeline : AssetPostprocessor
{
    private const string SourceFolder = "Assets/ThirdParty/Footballers";
    private const string OutputFolder = "Assets/Resources/Footballers";

    private void OnPreprocessModel()
    {
        if (!assetPath.StartsWith(SourceFolder, StringComparison.OrdinalIgnoreCase)) return;
        ModelImporter importer = (ModelImporter)assetImporter;
        importer.animationType = ModelImporterAnimationType.Human;
        importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
        importer.importAnimation = true;
        importer.optimizeGameObjects = false;
        importer.importCameras = false;
        importer.importLights = false;
        importer.materialImportMode = ModelImporterMaterialImportMode.None;
    }

    [MenuItem("Ultimate Football/Import Pipeline/Build Player Prefabs From FBX")]
    public static void BuildPlayerPrefabs()
    {
        if (!AssetDatabase.IsValidFolder(SourceFolder))
        {
            Directory.CreateDirectory(SourceFolder);
            AssetDatabase.Refresh();
            Debug.LogError("Add licensed humanoid FBX files to " + SourceFolder + " then run again.");
            return;
        }
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { SourceFolder });
        if (guids.Length == 0)
        {
            Debug.LogError("No FBX/model assets found in " + SourceFolder);
            return;
        }
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
        if (!AssetDatabase.IsValidFolder(OutputFolder))
            AssetDatabase.CreateFolder("Assets/Resources", "Footballers");

        GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[0]));
        if (model == null) { Debug.LogError("Could not load selected model."); return; }
        GameObject instance = PrefabUtility.InstantiatePrefab(model) as GameObject;
        if (instance == null) return;
        instance.name = "LicensedFootballer";
        Animator animator = instance.GetComponentInChildren<Animator>();
        if (animator == null)
            Debug.LogWarning("FBX has no Animator. Configure a valid Humanoid Avatar in Model Importer.");
        PrefabUtility.SaveAsPrefabAsset(instance, OutputFolder + "/BluePlayer.prefab");
        UnityEngine.Object.DestroyImmediate(instance);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Created BluePlayer prefab. Duplicate as RedPlayer and assign a distinct team material.");
    }
}
