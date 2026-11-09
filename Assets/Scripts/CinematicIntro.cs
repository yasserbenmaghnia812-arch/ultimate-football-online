using UnityEngine;

public sealed class CinematicIntro : MonoBehaviour
{
    private const float Duration = 3f;
    private float startedAt;
    private Texture2D pixel;
    private GUIStyle titleStyle;
    private GUIStyle subtitleStyle;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void ShowIntro()
    {
        if (FindObjectOfType<CinematicIntro>() == null)
            new GameObject("CinematicIntro").AddComponent<CinematicIntro>();
    }

    private void Awake()
    {
        startedAt = Time.unscaledTime;
        pixel = new Texture2D(1, 1);
        pixel.SetPixel(0, 0, Color.white);
        pixel.Apply();
        DontDestroyOnLoad(gameObject);
    }

    private void OnGUI()
    {
        float t = Mathf.Clamp01((Time.unscaledTime - startedAt) / Duration);
        float fadeIn = Mathf.Clamp01(t / 0.18f);
        float fadeOut = 1f - Mathf.Clamp01((t - 0.78f) / 0.22f);
        float alpha = fadeIn * fadeOut;
        float pulse = 1f + Mathf.Sin(t * Mathf.PI * 2f) * 0.012f;

        GUI.color = new Color(1f, 1f, 1f, alpha);
        DrawRect(new Rect(0, 0, Screen.width, Screen.height), new Color(0.48f, 0.015f, 0.045f, 1f));
        DrawRect(new Rect(0, 0, Screen.width, Screen.height * 0.16f), new Color(0.22f, 0.005f, 0.025f, 0.55f));
        DrawRect(new Rect(0, Screen.height * 0.84f, Screen.width, Screen.height * 0.16f), new Color(0.18f, 0.003f, 0.02f, 0.65f));

        float lineWidth = Screen.width * Mathf.Lerp(0.08f, 0.72f, Mathf.SmoothStep(0, 1, t));
        DrawRect(new Rect((Screen.width - lineWidth) * 0.5f, Screen.height * 0.59f, lineWidth, 3f),
            new Color(1f, 1f, 1f, alpha * 0.9f));

        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = Mathf.Clamp(Screen.width / 13, 25, 54),
                normal = { textColor = Color.white }
            };
            subtitleStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.Clamp(Screen.width / 48, 12, 20),
                normal = { textColor = new Color(1f, 1f, 1f, 0.82f) }
            };
        }

        Matrix4x4 oldMatrix = GUI.matrix;
        Vector2 center = new Vector2(Screen.width * 0.5f, Screen.height * 0.49f);
        GUI.matrix = Matrix4x4.TRS(center, Quaternion.identity, new Vector3(pulse, pulse, 1f)) *
                     Matrix4x4.TRS(-center, Quaternion.identity, Vector3.one);
        GUI.color = new Color(1f, 1f, 1f, alpha);
        GUI.Label(new Rect(0, Screen.height * 0.39f, Screen.width, 90), "ULTIMATE FOOTBALL", titleStyle);
        GUI.matrix = oldMatrix;
        GUI.color = new Color(1f, 1f, 1f, alpha * 0.86f);
        GUI.Label(new Rect(0, Screen.height * 0.52f, Screen.width, 42),
            "THE GAME IS YOURS  •  استعد للمباراة", subtitleStyle);
        GUI.color = Color.white;

        if (Time.unscaledTime - startedAt >= Duration)
            Destroy(gameObject);
    }

    private void DrawRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, pixel);
    }

    private void OnDestroy()
    {
        if (pixel != null) Destroy(pixel);
    }
}
