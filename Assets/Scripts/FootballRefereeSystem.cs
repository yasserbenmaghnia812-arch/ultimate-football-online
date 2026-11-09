using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Prototype referee decision layer: offside at pass time, restart classification,
/// foul registration, penalty-area checks and player discipline bookkeeping.
/// Contact detection and IFAB edge cases still require gameplay integration and testing.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballRefereeSystem : MonoBehaviour
{
    public event Action<string, Vector3> Whistle;
    public event Action<FootballRestartType, Vector3, bool> RestartCalled;
    public float RestartDelay { get; private set; } = 1.5f;

    private FootballMatchRules rules;
    private FootballMatchStatistics statistics;
    private readonly Dictionary<string, int> foulsByPlayer = new Dictionary<string, int>();

    private void Awake()
    {
        rules = GetComponent<FootballMatchRules>();
        statistics = GetComponent<FootballMatchStatistics>();
    }

    public void Bind(FootballMatchRules matchRules, FootballMatchStatistics matchStats)
    {
        rules = matchRules;
        statistics = matchStats;
    }

    public bool CheckOffsideAtPass(
        Transform receiver, Vector3 ballPosition, Transform[] defendingOutfieldPlayers,
        bool attackTowardPositiveZ, bool receiverInOwnHalf, bool fromRestart)
    {
        if (receiver == null || defendingOutfieldPlayers == null) return false;
        var defenders = new Vector3[defendingOutfieldPlayers.Length];
        for (int i = 0; i < defendingOutfieldPlayers.Length; i++)
        {
            if (defendingOutfieldPlayers[i] == null) return false;
            defenders[i] = defendingOutfieldPlayers[i].position;
        }

        bool offside = FootballOffsideDetector.IsOffsideAtPass(
            receiver.position, ballPosition, defenders, attackTowardPositiveZ,
            receiverInOwnHalf, fromRestart);
        if (offside)
        {
            Vector3 restart = receiver.position;
            restart.y = 0.22f;
            CallRestart(FootballRestartType.IndirectFreeKick, restart, false, "Offside");
            if (statistics != null) statistics.RegisterOffside(true);
        }
        return offside;
    }

    public void RegisterFoul(bool committedByHome, string playerId, Vector3 location,
        bool insidePenaltyArea, bool denyingObviousGoalScoringOpportunity = false,
        bool recklessChallenge = false, bool excessiveForce = false)
    {
        if (string.IsNullOrWhiteSpace(playerId)) playerId = "unknown";
        statistics?.RegisterFoul(committedByHome);
        int count;
        foulsByPlayer.TryGetValue(playerId, out count);
        foulsByPlayer[playerId] = ++count;

        bool awardedToHome = !committedByHome;
        if (insidePenaltyArea)
        {
            CallRestart(FootballRestartType.Penalty, location, awardedToHome, "Penalty");
        }
        else
        {
            CallRestart(FootballRestartType.DirectFreeKick, location, awardedToHome, "Foul");
        }

        if (denyingObviousGoalScoringOpportunity || excessiveForce)
            IssueCard(committedByHome, playerId, FootballDisciplineCard.Red);
        else if (recklessChallenge)
            IssueCard(committedByHome, playerId, FootballDisciplineCard.Yellow);
    }

    public void CallRestart(FootballRestartType type, Vector3 location, bool awardedToHome, string reason)
    {
        location.y = Mathf.Max(0.22f, location.y);
        rules?.AwardRestart(type, location, awardedToHome);
        RestartCalled?.Invoke(type, location, awardedToHome);
        Whistle?.Invoke(reason + " - " + type, location);
    }

    public bool IssueCard(bool homeTeam, string playerId, FootballDisciplineCard card)
    {
        bool sentOff = rules != null && rules.IssuePlayerCard(homeTeam, playerId, card);
        Whistle?.Invoke(card + " card: " + playerId, Vector3.zero);
        return sentOff;
    }

    public static bool IsInsidePenaltyArea(Vector3 position, bool positiveZGoal)
    {
        float goalDistance = positiveZGoal ? 21.8f - position.z : position.z + 21.8f;
        return Mathf.Abs(position.x) <= 6.5f && goalDistance >= 0f && goalDistance <= 6f;
    }

    public void Signal(string decision, Vector3 location)
    {
        Whistle?.Invoke(decision, location);
    }
}
