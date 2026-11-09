using UnityEngine;

/// <summary>
/// Lightweight player ratings and fatigue model used by the prototype.
/// Values are gameplay tuning parameters, not real-world physiological measurements.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballPlayerAttributes : MonoBehaviour
{
    [Range(1, 99)] public int pace = 76;
    [Range(1, 99)] public int acceleration = 74;
    [Range(1, 99)] public int shooting = 72;
    [Range(1, 99)] public int finishing = 70;
    [Range(1, 99)] public int balance = 72;
    [Range(1, 99)] public int strength = 68;
    [Range(1, 99)] public int staminaRating = 75;

    [Range(0f, 100f)] public float stamina = 100f;

    [Header("Fatigue tuning")]
    [SerializeField, Min(0f)] private float movementDrainPerSecond = 0.42f;
    [SerializeField, Min(0f)] private float recoveryPerSecond = 0.28f;

    public void TickMovement(bool moving, float deltaTime)
    {
        float drain = moving ? movementDrainPerSecond * (0.65f + staminaRating / 200f) : -recoveryPerSecond;
        stamina = Mathf.Clamp(stamina - drain * deltaTime, 0f, 100f);
    }

    public float MovementMultiplier()
    {
        float fatigue = 1f - stamina / 100f;
        return Mathf.Lerp(1f, 0.78f, fatigue * fatigue) * (0.88f + acceleration / 850f);
    }

    public float GetKickSpeed(float baseSpeed)
    {
        float fatigue = 1f - stamina / 100f;
        float rating = Mathf.Lerp(0.78f, 1.12f, shooting / 99f);
        float fatigueFactor = 1f - fatigue * 0.22f;
        stamina = Mathf.Max(0f, stamina - 1.4f);
        return baseSpeed * rating * fatigueFactor;
    }

    public float GetShotAccuracy()
    {
        float fatigue = 1f - stamina / 100f;
        float finishingFactor = Mathf.Lerp(0.65f, 1f, finishing / 99f);
        float balanceFactor = Mathf.Lerp(0.80f, 1f, balance / 99f);
        return Mathf.Clamp01(finishingFactor * balanceFactor * (1f - fatigue * 0.28f));
    }
}
