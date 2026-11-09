using UnityEngine;

/// <summary>
/// Geometry helper for offside decisions. Call only at the instant a teammate plays the ball.
/// It is a foundation: deflections, deliberate opponent play, and goalkeeper interference need referee context.
/// </summary>
public static class FootballOffsideDetector
{
    public static bool IsInOffsidePosition(
        Vector3 attackerPosition,
        Vector3 ballPosition,
        Vector3[] defendingOutfieldPlayers,
        bool attackingTowardPositiveZ,
        float tolerance = 0.05f)
    {
        if (defendingOutfieldPlayers == null || defendingOutfieldPlayers.Length < 2) return false;

        float attackCoordinate = attackingTowardPositiveZ ? attackerPosition.z : -attackerPosition.z;
        float ballCoordinate = attackingTowardPositiveZ ? ballPosition.z : -ballPosition.z;
        float[] defenders = new float[defendingOutfieldPlayers.Length];
        for (int i = 0; i < defendingOutfieldPlayers.Length; i++)
            defenders[i] = attackingTowardPositiveZ ? defendingOutfieldPlayers[i].z : -defendingOutfieldPlayers[i].z;

        System.Array.Sort(defenders);
        float secondLastDefender = defenders[defenders.Length - 2];
        float offsideLine = Mathf.Max(ballCoordinate, secondLastDefender);
        return attackCoordinate > 0f && attackCoordinate > offsideLine + tolerance;
    }

    public static bool IsOffsideAtPass(
        Vector3 attackerPosition,
        Vector3 ballPosition,
        Vector3[] defendingOutfieldPlayers,
        bool attackingTowardPositiveZ,
        bool isInOwnHalf,
        bool receivedFromGoalKickThrowInOrCorner)
    {
        if (isInOwnHalf || receivedFromGoalKickThrowInOrCorner) return false;
        return IsInOffsidePosition(attackerPosition, ballPosition, defendingOutfieldPlayers, attackingTowardPositiveZ);
    }
}
