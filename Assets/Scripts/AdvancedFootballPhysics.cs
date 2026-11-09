using UnityEngine;

/// <summary>
/// Lightweight, tunable football-ball simulation layer.
/// Uses Unity Rigidbody contacts plus aerodynamic drag, spin force and rolling resistance.
/// </summary>
[DisallowMultipleComponent]
public sealed class AdvancedFootballPhysics : MonoBehaviour
{
    [Header("Aerodynamics")]
    [SerializeField, Min(0f)] private float airDrag = 0.018f;
    [SerializeField, Min(0f)] private float magnusCoefficient = 0.0018f;
    [SerializeField, Min(0f)] private float rollingResistance = 1.15f;
    [SerializeField, Min(1f)] private float maximumSpeed = 38f;
    [SerializeField, Min(0f)] private float maximumSpin = 42f;

    private Rigidbody body;

    public void Initialize(Rigidbody target)
    {
        body = target;
        if (body == null) body = GetComponent<Rigidbody>();
        if (body == null) return;

        body.mass = 0.43f;
        body.linearDamping = 0.015f;
        body.angularDamping = 0.035f;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        // CCD is intentionally limited to the fast-moving ball, not all 22 players.
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void Awake()
    {
        if (body == null) Initialize(GetComponent<Rigidbody>());
    }

    private void FixedUpdate()
    {
        if (body == null || body.isKinematic) return;

        Vector3 velocity = body.linearVelocity;
        float speed = velocity.magnitude;

        if (speed > 0.05f)
        {
            body.AddForce(-velocity * (airDrag * speed), ForceMode.Acceleration);

            // Approximate Magnus effect: spin axis crossed with travel direction.
            Vector3 magnus = Vector3.Cross(body.angularVelocity, velocity) * magnusCoefficient;
            body.AddForce(Vector3.ClampMagnitude(magnus, 4.5f), ForceMode.Acceleration);
        }

        if (speed > maximumSpeed)
            body.linearVelocity = velocity.normalized * maximumSpeed;

        if (body.angularVelocity.sqrMagnitude > maximumSpin * maximumSpin)
            body.angularVelocity = body.angularVelocity.normalized * maximumSpin;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (body == null || collision.contactCount == 0) return;

        bool onGround = false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y > 0.55f)
            {
                onGround = true;
                break;
            }
        }

        if (!onGround) return;

        Vector3 velocity = body.linearVelocity;
        Vector3 planar = new Vector3(velocity.x, 0f, velocity.z);
        if (planar.sqrMagnitude < 0.0004f) return;

        Vector3 reduction = Vector3.ClampMagnitude(planar, rollingResistance * Time.fixedDeltaTime);
        body.AddForce(-reduction, ForceMode.VelocityChange);
    }

    public void Kick(Vector3 direction, float targetSpeed, float lift, Vector3 spin)
    {
        if (body == null) return;

        Vector3 shotDirection = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector3.forward;
        shotDirection = (shotDirection + Vector3.up * Mathf.Clamp(lift, 0f, 0.65f)).normalized;

        // Convert target speed into an impulse so ball mass remains meaningful.
        Vector3 desiredVelocity = shotDirection * Mathf.Max(0f, targetSpeed);
        Vector3 velocityChange = desiredVelocity - body.linearVelocity;
        body.AddForce(velocityChange * body.mass, ForceMode.Impulse);
        body.AddTorque(Vector3.ClampMagnitude(spin, maximumSpin), ForceMode.Impulse);
    }

    public void ResetBall(Vector3 position)
    {
        if (body == null) return;
        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.position = position;
    }
}
