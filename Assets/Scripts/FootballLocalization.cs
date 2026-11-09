using System.Collections.Generic;
using UnityEngine;

public enum FootballLanguage { Arabic, French, English }

public sealed class FootballLocalization : MonoBehaviour
{
    public static FootballLocalization Instance { get; private set; }
    public FootballLanguage Language { get; private set; } = FootballLanguage.Arabic;

    private readonly Dictionary<string, string[]> words = new Dictionary<string, string[]>
    {
        { "play", new[] { "العب", "Jouer", "Play" } },
        { "settings", new[] { "الإعدادات", "Paramètres", "Settings" } },
        { "home", new[] { "الرئيسية", "Accueil", "Home" } },
        { "shoot", new[] { "تسديد", "Tirer", "Shoot" } },
        { "pass", new[] { "تمرير", "Passe", "Pass" } },
        { "tackle", new[] { "افتكاك", "Tacle", "Tackle" } },
        { "pause", new[] { "إيقاف", "Pause", "Pause" } },
        { "resume", new[] { "متابعة", "Reprendre", "Resume" } },
        { "full_time", new[] { "نهاية المباراة", "Fin du match", "Full time" } }
    };

    public bool IsRightToLeft => Language == FootballLanguage.Arabic;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        int saved = PlayerPrefs.GetInt("football_language", (int)FootballLanguage.Arabic);
        SetLanguage((FootballLanguage)Mathf.Clamp(saved, 0, 2));
    }

    public void SetLanguage(FootballLanguage language)
    {
        Language = language;
        PlayerPrefs.SetInt("football_language", (int)language);
        PlayerPrefs.Save();
    }

    public string Get(string key)
    {
        if (!words.TryGetValue(key, out string[] values)) return key;
        return values[(int)Language];
    }
}
