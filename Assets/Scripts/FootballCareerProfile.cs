using System;
using UnityEngine;

/// <summary>Local career progression persistence. Replace with authenticated cloud storage for cross-device accounts.</summary>
[Serializable]
public sealed class FootballCareerProfile
{
    public string profileName = "Player";
    public int matchesPlayed;
    public int wins;
    public int draws;
    public int losses;
    public int goals;
    public int coins = 500;
    public int experience;
    public int level = 1;
    public string selectedFormation = "4-3-3";
    public long lastSavedUnix;

    public void RecordMatch(int scored, int conceded, int rewardCoins)
    {
        matchesPlayed++;
        goals += Mathf.Max(0, scored);
        coins = Mathf.Max(0, coins + Mathf.Max(0, rewardCoins));
        if (scored > conceded) wins++;
        else if (scored == conceded) draws++;
        else losses++;
        experience += scored > conceded ? 100 : scored == conceded ? 50 : 25;
        level = Mathf.Max(1, 1 + experience / 500);
        lastSavedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}

public static class FootballCareerSaveSystem
{
    private const string SaveKey = "ultimate_football_career_v1";

    public static FootballCareerProfile Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, string.Empty);
        if (string.IsNullOrEmpty(json)) return new FootballCareerProfile();
        try
        {
            FootballCareerProfile profile = JsonUtility.FromJson<FootballCareerProfile>(json);
            return profile ?? new FootballCareerProfile();
        }
        catch (Exception exception)
        {
            Debug.LogWarning("Career save could not be read: " + exception.Message);
            return new FootballCareerProfile();
        }
    }

    public static void Save(FootballCareerProfile profile)
    {
        if (profile == null) return;
        profile.lastSavedUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(profile));
        PlayerPrefs.Save();
    }

    public static void Delete()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }
}
