using UnityEngine;

/// <summary>
/// Lightweight YASSER GTA startup title. Runs before the first scene and
/// overlays the game for exactly three seconds without blocking game loading.
/// </summary>
public sealed class YasserSplashScreen : MonoBehaviour
{
    private const float DurationSeconds = 3f;
    private static bool s_created;
    private float m_startedAt;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateBeforeFirstScene()
    {
        if (s_created)
            return;

        s_created = true;
        var splashObject = new GameObject("YASSER GTA - Splash");
        DontDestroyOnLoad(splashObject);
        splashObject.AddComponent<YasserSplashScreen>();
    }

    private void Awake()
    {
        m_startedAt = Time.realtimeSinceStartup;
    }

    private void OnGUI()
    {
        float elapsed = Time.realtimeSinceStartup - m_startedAt;
        if (elapsed >= DurationSeconds)
        {
            Destroy(gameObject);
            return;
        }

        float fadeIn = Mathf.Clamp01(elapsed / 0.25f);
        float fadeOut = Mathf.Clamp01((DurationSeconds - elapsed) / 0.35f);
        float alpha = Mathf.Min(fadeIn, fadeOut);

        var previousColor = GUI.color;
        GUI.color = new Color(0.78f, 0.035f, 0.055f, 1f);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);

        int fontSize = Mathf.Clamp(Mathf.RoundToInt(Screen.height * 0.055f), 26, 58);
        var titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = fontSize,
            fontStyle = FontStyle.Bold,
            wordWrap = false
        };
        titleStyle.normal.textColor = new Color(1f, 1f, 1f, alpha);
        titleStyle.font = GUI.skin.font;

        GUI.color = Color.white;
        GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), "Yasser Benmaghnia", titleStyle);
        GUI.color = previousColor;
    }
}
