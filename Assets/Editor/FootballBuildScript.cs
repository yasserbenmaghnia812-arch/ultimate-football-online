#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class FootballBuildScript
{
    private const string ScenePath = "Assets/Scenes/Main.unity";
    private const string OutputPath = "build/Android/UltimateFootball.apk";

    public static void BuildAndroid()
    {
        EnsureDefaultScene();
        Directory.CreateDirectory("build/Android");

        PlayerSettings.companyName = "Yasser Benmaghnia";
        PlayerSettings.productName = "Ultimate Football Online";
        PlayerSettings.applicationIdentifier = "com.yasserbenmaghnia.ultimatefootball";
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.runInBackground = false;

        var options = new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = OutputPath,
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
            throw new System.Exception("Android build failed: " + report.summary.result +
                " | errors: " + report.summary.totalErrors);
        Debug.Log("Android APK created: " + OutputPath);
    }

    private static void EnsureDefaultScene()
    {
        if (!Directory.Exists("Assets/Scenes")) Directory.CreateDirectory("Assets/Scenes");
        if (!File.Exists(ScenePath))
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(scene, ScenePath);
        }

        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenePath, true)
        };
    }
}
#endif
