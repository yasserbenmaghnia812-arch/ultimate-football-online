using System;
using UnityEngine;

/// <summary>
/// Match referee decision layer. This implementation wires the offside decision
/// to the instant a controlled player releases a pass. It is not a complete IFAB referee simulation.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballRefereeSystem : MonoBehaviour
{
    public event Action<string, Vector3> Whistle;
    public float RestartDelay { get; private set; }

    public bool CheckOffsideAtPass(
        Transform receiver,
        Vector3 ballPosition,
        Transform[] defendingOutfieldPlayers,
        bool attackTowardPositiveZ,
        bool isInOwnHalf,
        bool fromRestart)
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
            isInOwnHalf, fromRestart);

        if (offside)
        {
            Vector3 restart = ballPosition;
            restart.y = 0.22f;
            Whistle?.Invoke("Offside - indirect free kick", restart);
        }
        return offside;
    }

    public void Signal(string decision, Vector3 location)
    {
        Whistle?.Invoke(decision, location);
    }
}
