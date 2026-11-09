using System;
using UnityEngine;

public enum FootballMatchState { PreMatch, FirstHalf, HalfTime, SecondHalf, FullTime, Paused }
public enum FootballDisciplineCard { Yellow, Red }
public enum FootballRestartType { None, KickOff, DirectFreeKick, IndirectFreeKick, Penalty, ThrowIn, Corner, GoalKick }

public sealed class FootballMatchRules : MonoBehaviour
{
    public event Action<int, int> ScoreChanged;
    public event Action<FootballMatchState> StateChanged;
    public event Action<string> MatchEvent;\n    public event Action<FootballRestartType, Vector3, bool> RestartAwarded;

    public FootballMatchState State { get; private set; } = FootballMatchState.PreMatch;
    private FootballMatchState stateBeforePause = FootballMatchState.FirstHalf;
    public int HomeScore { get; private set; }
    public int AwayScore { get; private set; }
    public float MatchSeconds { get; private set; }
    public int HomeYellowCards { get; private set; }
    public int AwayYellowCards { get; private set; }
    public int HomeRedCards { get; private set; }
    public int AwayRedCards { get; private set; }
    public FootballRestartType CurrentRestart { get; private set; }
    public Vector3 RestartPosition { get; private set; }
    public bool RestartForHome { get; private set; }
    private readonly System.Collections.Generic.Dictionary<string, int> yellowByPlayer =
        new System.Collections.Generic.Dictionary<string, int>();

    [SerializeField, Min(60f)] private float halfDurationSeconds = 90f;

    public void StartMatch()
    {
        HomeScore = AwayScore = 0;
        MatchSeconds = 0f;
        HomeYellowCards = AwayYellowCards = HomeRedCards = AwayRedCards = 0;\n        yellowByPlayer.Clear();\n        CurrentRestart = FootballRestartType.KickOff;
        SetState(FootballMatchState.FirstHalf);
        MatchEvent?.Invoke("Kick-off");
    }

    public void Tick(float deltaTime)
    {
        if (State != FootballMatchState.FirstHalf && State != FootballMatchState.SecondHalf) return;
        MatchSeconds += deltaTime;
        if (MatchSeconds >= halfDurationSeconds)
        {
            if (State == FootballMatchState.FirstHalf)
            {
                SetState(FootballMatchState.HalfTime);
                MatchEvent?.Invoke("Half-time");
            }
            else
            {
                SetState(FootballMatchState.FullTime);
                MatchEvent?.Invoke("Full-time");
            }
        }
    }

    public void ResumeSecondHalf()
    {
        if (State != FootballMatchState.HalfTime) return;
        MatchSeconds = 0f;
        SetState(FootballMatchState.SecondHalf);
        MatchEvent?.Invoke("Second-half kick-off");
    }

    public void AddGoal(bool homeTeam)
    {
        if (State != FootballMatchState.FirstHalf && State != FootballMatchState.SecondHalf) return;
        if (homeTeam) HomeScore++; else AwayScore++;
        ScoreChanged?.Invoke(HomeScore, AwayScore);
        MatchEvent?.Invoke(homeTeam ? "Home goal" : "Away goal");
    }

    public bool IssuePlayerCard(bool homeTeam, string playerId, FootballDisciplineCard card)
    {
        if (string.IsNullOrWhiteSpace(playerId)) return false;
        if (card == FootballDisciplineCard.Yellow)
        {
            int count;
            yellowByPlayer.TryGetValue(playerId, out count);
            count++;
            yellowByPlayer[playerId] = count;
            IssueCard(homeTeam, FootballDisciplineCard.Yellow);
            if (count >= 2)
            {
                IssueCard(homeTeam, FootballDisciplineCard.Red);
                MatchEvent?.Invoke("Second yellow - player sent off: " + playerId);
                return true;
            }
            return false;
        }
        IssueCard(homeTeam, FootballDisciplineCard.Red);
        MatchEvent?.Invoke("Player sent off: " + playerId);
        return true;
    }

    public void AwardRestart(FootballRestartType type, Vector3 position, bool homeTeam)
    {
        if (type == FootballRestartType.None) return;
        CurrentRestart = type;
        RestartPosition = position;
        RestartForHome = homeTeam;
        RestartAwarded?.Invoke(type, position, homeTeam);
        FootballMatchStatistics stats = GetComponent<FootballMatchStatistics>();
        if (stats != null) stats.RegisterRestart(homeTeam, type);
        MatchEvent?.Invoke(type + (homeTeam ? " awarded to home" : " awarded to away"));
    }

    public void CompleteRestart()
    {
        CurrentRestart = FootballRestartType.None;
    }

    public void IssueCard(bool homeTeam, FootballDisciplineCard card)
    {
        FootballMatchStatistics stats = GetComponent<FootballMatchStatistics>();
        if (stats != null) stats.RegisterCard(homeTeam, card == FootballDisciplineCard.Red);
        if (card == FootballDisciplineCard.Yellow)
        {
            if (homeTeam) HomeYellowCards++; else AwayYellowCards++;
            MatchEvent?.Invoke(homeTeam ? "Home yellow card" : "Away yellow card");
        }
        else
        {
            if (homeTeam) HomeRedCards++; else AwayRedCards++;
            MatchEvent?.Invoke(homeTeam ? "Home red card" : "Away red card");
        }
    }

    public void SetPaused(bool paused)
    {
        if (paused && State != FootballMatchState.Paused)
        {
            stateBeforePause = State;
            SetState(FootballMatchState.Paused);
        }
        else if (!paused && State == FootballMatchState.Paused) SetState(stateBeforePause);
    }

    private void SetState(FootballMatchState state)
    {
        State = state;
        StateChanged?.Invoke(state);
    }
}
