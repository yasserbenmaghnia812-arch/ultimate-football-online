using UnityEngine;

/// <summary>Mobile quality presets designed to avoid expensive defaults on low-memory Android devices.</summary>
public static class FootballRuntimeQuality
{
    public enum Preset { Low, Balanced, High }

    public static void Apply(Preset preset)
    {
        switch (preset)
        {
            case Preset.Low:
                QualitySettings.SetQualityLevel(0, true);
                Application.targetFrameRate = 30;
                QualitySettings.shadows = ShadowQuality.Disable;
                QualitySettings.antiAliasing = 0;
                break;
            case Preset.High:
                QualitySettings.SetQualityLevel(Mathf.Max(0, QualitySettings.names.Length - 1), true);
                Application.targetFrameRate = 60;
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.antiAliasing = 2;
                break;
            default:
                QualitySettings.SetQualityLevel(Mathf.Clamp(1, 0, Mathf.Max(0, QualitySettings.names.Length - 1)), true);
                Application.targetFrameRate = 45;
                QualitySettings.shadows = ShadowQuality.HardOnly;
                QualitySettings.antiAliasing = 0;
                break;
        }
        QualitySettings.vSyncCount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public static Preset RecommendedPreset()
    {
        return SystemInfo.systemMemorySize <= 4096 ? Preset.Low :
               SystemInfo.systemMemorySize <= 6144 ? Preset.Balanced : Preset.High;
    }
}
