using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Server-owned replicated match score, clock, phase and latest event.
/// Attach to a NetworkObject spawned by the host. Player/ball simulation replication
/// still requires NetworkObjects, transforms and server-authoritative gameplay integration.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballNetworkMatchState : NetworkBehaviour
{
    public readonly NetworkVariable<int> HomeGoals = new NetworkVariable<int>(0);
    public readonly NetworkVariable<int> AwayGoals = new NetworkVariable<int>(0);
    public readonly NetworkVariable<float> MatchClock = new NetworkVariable<float>(0f);
    public readonly NetworkVariable<byte> MatchPhase = new NetworkVariable<byte>(0);
    public readonly NetworkVariable<FixedString128Bytes> LatestEvent =
        new NetworkVariable<FixedString128Bytes>(new FixedString128Bytes("Waiting for host"));
    private bool clockRunning;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        HomeGoals.Value = 0;
        AwayGoals.Value = 0;
        MatchClock.Value = 0f;
        MatchPhase.Value = 1;
        LatestEvent.Value = new FixedString128Bytes("Match started");
        clockRunning = true;
    }

    private void Update()
    {
        if (IsServer && clockRunning) MatchClock.Value += Time.deltaTime;
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void ReportGoalRpc(bool homeTeam)
    {
        if (!IsServer || MatchPhase.Value != 1) return;
        if (homeTeam) HomeGoals.Value++;
        else AwayGoals.Value++;
        LatestEvent.Value = new FixedString128Bytes(homeTeam ? "Home goal" : "Away goal");
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void ReportMatchEventRpc(FixedString128Bytes eventText)
    {
        if (!IsServer || MatchPhase.Value == 0 || MatchPhase.Value == 4) return;
        LatestEvent.Value = eventText;
    }

    [Rpc(SendTo.Server, RequireOwnership = false)]
    public void SetMatchPhaseRpc(byte phase)
    {
        if (!IsServer || phase > 4) return;
        MatchPhase.Value = phase;
        clockRunning = phase == 1 || phase == 2;
    }

    public void SetClockRunningOnServer(bool running)
    {
        if (IsServer) clockRunning = running;
    }
}
