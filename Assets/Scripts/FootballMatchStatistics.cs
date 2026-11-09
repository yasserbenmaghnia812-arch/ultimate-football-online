using System;
using UnityEngine;

[Serializable]
public sealed class FootballTeamStatistics
{
    public int shots;
    public int shotsOnTarget;
    public int passes;
    public int completedPasses;
    public int tackles;
    public int fouls;
    public int corners;
    public int offsides;
    public int possessionSeconds;

    public float PassAccuracy => passes <= 0 ? 0f : 100f * completedPasses / passes;
    public float ShotAccuracy => shots <= 0 ? 0f : 100f * shotsOnTarget / shots;
}

/// <summary>Match statistics accumulator. Feed it events from the gameplay and referee systems.</summary>
public sealed class FootballMatchStatistics : MonoBehaviour
{
    [SerializeField] private FootballTeamStatistics home = new FootballTeamStatistics();
    [SerializeField] private FootballTeamStatistics away = new FootballTeamStatistics();

    public FootballTeamStatistics Home => home;
    public FootballTeamStatistics Away => away;

    public void RegisterShot(bool isHome, bool onTarget)
    {
        FootballTeamStatistics team = Select(isHome);
        team.shots++;
        if (onTarget) team.shotsOnTarget++;
    }

    public void RegisterPass(bool isHome, bool completed)
    {
        FootballTeamStatistics team = Select(isHome);
        team.passes++;
        if (completed) team.completedPasses++;
    }

    public void RegisterTackle(bool isHome) { Select(isHome).tackles++; }
    public void RegisterFoul(bool isHome) { Select(isHome).fouls++; }
    public void RegisterCorner(bool isHome) { Select(isHome).corners++; }
    public void RegisterOffside(bool isHome) { Select(isHome).offsides++; }
    public void TickPossession(bool homeHasBall, float deltaTime)
    {
        if (deltaTime <= 0f) return;
        Select(homeHasBall).possessionSeconds += Mathf.RoundToInt(deltaTime);
    }

    public void ResetStatistics()
    {
        home = new FootballTeamStatistics();
        away = new FootballTeamStatistics();
    }

    public float HomePossessionPercent
    {
        get
        {
            int total = home.possessionSeconds + away.possessionSeconds;
            return total == 0 ? 50f : 100f * home.possessionSeconds / total;
        }
    }

    private FootballTeamStatistics Select(bool isHome) { return isHome ? home : away; }
}
