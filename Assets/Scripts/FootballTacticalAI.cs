using UnityEngine;

public enum FootballTeamStyle { Balanced, Possession, QuickCounter, HighPress, DeepBlock, WingPlay }
public enum FootballRole { Goalkeeper, Defender, Midfielder, Winger, Striker }

public static class FootballTacticalAI
{
    public static Vector3 GetRoleTarget(
        FootballRole role, int lane, Vector3 ball, Vector3 home, bool attacking,
        FootballTeamStyle style, float pitchHalfLength = 22f)
    {
        float direction = attacking ? 1f : -1f;
        Vector3 target = home;
        float ballProgress = Mathf.Clamp(ball.z / pitchHalfLength, -1f, 1f);

        switch (role)
        {
            case FootballRole.Goalkeeper:
                target = new Vector3(Mathf.Clamp(ball.x * 0.22f, -2.2f, 2.2f), 1.0f, -direction * (pitchHalfLength - 3f));
                break;
            case FootballRole.Defender:
                target = new Vector3(home.x + ball.x * 0.12f, 1.0f, home.z + ballProgress * 2.4f);
                break;
            case FootballRole.Midfielder:
                target = new Vector3(home.x * 0.7f + ball.x * 0.3f, 1.0f, home.z + ballProgress * 4.5f);
                break;
            case FootballRole.Winger:
                target = new Vector3(Mathf.Sign(lane == 0 ? -1 : 1) * Mathf.Max(7f, Mathf.Abs(home.x)), 1.0f, home.z + direction * 3f);
                break;
            case FootballRole.Striker:
                target = new Vector3(home.x * 0.4f + ball.x * 0.15f, 1.0f, home.z + direction * 5f);
                break;
        }

        if (style == FootballTeamStyle.HighPress && !attacking)
            target = Vector3.Lerp(target, new Vector3(ball.x, 1.0f, ball.z - direction * 2f), 0.28f);
        else if (style == FootballTeamStyle.DeepBlock && !attacking)
            target = Vector3.Lerp(target, new Vector3(home.x, 1.0f, home.z - direction * 2f), 0.35f);
        else if (style == FootballTeamStyle.QuickCounter && attacking)
            target.z += direction * 2.5f;

        target.x = Mathf.Clamp(target.x, -12.5f, 12.5f);
        target.z = Mathf.Clamp(target.z, -20.5f, 20.5f);
        target.y = 1.0f;
        return target;
    }

    public static float GetPressingWeight(float stamina, float teamStyleMultiplier)
    {
        float fatigueFactor = Mathf.Lerp(0.35f, 1f, Mathf.Clamp01(stamina / 100f));
        return Mathf.Clamp01(fatigueFactor * teamStyleMultiplier);
    }
}
