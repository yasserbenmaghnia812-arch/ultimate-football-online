using UnityEngine;

public enum FootballCardTier { Bronze, Silver, Gold, Elite, Legend, Icon }

[CreateAssetMenu(menuName = "Ultimate Football/Player Card", fileName = "PlayerCard")]
public sealed class FootballPlayerCard : ScriptableObject
{
    [Header("Identity")]
    public string playerName = "New Player";
    public string position = "ST";
    public int shirtNumber = 9;
    public FootballCardTier tier = FootballCardTier.Gold;

    [Header("Base ratings")]
    [Range(1, 99)] public int pace = 75;
    [Range(1, 99)] public int shooting = 75;
    [Range(1, 99)] public int passing = 70;
    [Range(1, 99)] public int dribbling = 75;
    [Range(1, 99)] public int defending = 45;
    [Range(1, 99)] public int physical = 70;
    [Range(1, 99)] public int stamina = 75;

    [Header("Progression")]
    [Min(0)] public int progressionPoints;
    [Range(1, 99)] public int overall = 75;

    public bool SpendPoint(string attribute, int amount = 1)
    {
        if (amount <= 0 || progressionPoints < amount) return false;
        int current = GetAttribute(attribute);
        if (current < 0 || current + amount > 99) return false;
        SetAttribute(attribute, current + amount);
        progressionPoints -= amount;
        RecalculateOverall();
        return true;
    }

    public void AwardProgression(int points)
    {
        progressionPoints = Mathf.Max(0, progressionPoints + points);
    }

    public int GetAttribute(string attribute)
    {
        switch (attribute.ToLowerInvariant())
        {
            case "pace": return pace;
            case "shooting": return shooting;
            case "passing": return passing;
            case "dribbling": return dribbling;
            case "defending": return defending;
            case "physical": return physical;
            case "stamina": return stamina;
            default: return -1;
        }
    }

    private void SetAttribute(string attribute, int value)
    {
        switch (attribute.ToLowerInvariant())
        {
            case "pace": pace = value; break;
            case "shooting": shooting = value; break;
            case "passing": passing = value; break;
            case "dribbling": dribbling = value; break;
            case "defending": defending = value; break;
            case "physical": physical = value; break;
            case "stamina": stamina = value; break;
        }
    }

    private void OnValidate() { RecalculateOverall(); }

    private void RecalculateOverall()
    {
        overall = Mathf.Clamp(Mathf.RoundToInt((pace + shooting + passing + dribbling + defending + physical + stamina) / 7f), 1, 99);
        tier = overall >= 97 ? FootballCardTier.Icon :
               overall >= 92 ? FootballCardTier.Legend :
               overall >= 85 ? FootballCardTier.Elite :
               overall >= 75 ? FootballCardTier.Gold :
               overall >= 65 ? FootballCardTier.Silver : FootballCardTier.Bronze;
    }
}
