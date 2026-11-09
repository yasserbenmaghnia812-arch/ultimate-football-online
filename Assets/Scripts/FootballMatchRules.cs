using System;
using UnityEngine;

public enum FootballMatchState { PreMatch, FirstHalf, HalfTime, SecondHalf, FullTime, Paused }
public enum FootballDisciplineCard { Yellow, Red }

public sealed class FootballMatchRules : MonoBehaviour
{
    public event Action<int, int> ScoreChanged;
    public event Action<FootballMatchState> StateChanged;
    public event Action<string> MatchEvent;

    public FootballMatchState State { get; private set; } = FootballMatchState.PreMatch;
    public int HomeScore { get; private set; }
    public int AwayScore { get; private set; }
    public float MatchSeconds { get; private set; }
    public int HomeYellowCards { get; private set; }
    public int AwayYellowCards { get; private set; }
    public int HomeRedCards { get; private set; }
    public int AwayRedCards { get; private set; }

    [SerializeField, Min(60f)] private float halfDurationSeconds = 2700f;

    public void StartMatch()
    {
        HomeScore = AwayScore = 0;
        MatchSeconds = 0f;
        HomeYellowCards = AwayYellowCards = HomeRedCards = AwayRedCards = 0;
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

    public void IssueCard(bool homeTeam, FootballDisciplineCard card)
    {
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
        if (paused && State != FootballMatchState.Paused) SetState(FootballMatchState.Paused);
        else if (!paused && State == FootballMatchState.Paused) SetState(MatchSeconds < halfDurationSeconds ? FootballMatchState.FirstHalf : FootballMatchState.SecondHalf);
    }

    private void SetState(FootballMatchState state)
    {
        State = state;
        StateChanged?.Invoke(state);
    }
}
