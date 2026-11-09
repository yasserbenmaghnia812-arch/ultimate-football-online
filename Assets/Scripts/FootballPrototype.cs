using System.Collections.Generic;
using UnityEngine;

public sealed class FootballPrototype : MonoBehaviour
{
    private const int TeamSize = 11;
    private readonly List<Transform> blueTeam = new List<Transform>();
    private readonly List<Transform> redTeam = new List<Transform>();
    private Transform player, ball;
    private Rigidbody ballBody;
    private Camera gameCamera;
    private Vector2 touchMove;
    private bool shootPressed, passPressed;
    private int blueScore, redScore;
    private float matchTime = 180f;
    private float aiThink;
    private GUIStyle hud, buttonStyle;
    private Material blueMat, redMat, whiteMat, grassMat;
    private bool use60Fps;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void StartPrototype()
    {
        if (FindObjectOfType<FootballPrototype>() == null)
            new GameObject("FootballPrototype").AddComponent<FootballPrototype>();
    }

    private void Start()
    {
        use60Fps = SystemInfo.systemMemorySize >= 5000 && SystemInfo.processorCount >= 6;
        ApplyPerformanceSettings();
        blueMat = Mat(new Color(0.05f, 0.22f, 0.88f));
        redMat = Mat(new Color(0.78f, 0.035f, 0.06f));
        whiteMat = Mat(Color.white);
        grassMat = Mat(new Color(0.10f, 0.38f, 0.16f));
        BuildPitch();
        BuildTeams();
        BuildBall();
        BuildCamera();
        RenderSettings.ambientLight = new Color(0.72f, 0.76f, 0.8f);
    }

    private void ApplyPerformanceSettings()
    {
        Application.targetFrameRate = use60Fps ? 60 : 30;
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.pixelLightCount = 1;
        QualitySettings.lodBias = 0.7f;
        QualitySettings.maximumLODLevel = 1;
        QualitySettings.softParticles = false;
        Time.fixedDeltaTime = use60Fps ? 0.02f : 0.03f;
    }

    private Material Mat(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        var m = new Material(shader);
        m.color = color;
        return m;
    }

    private GameObject Cube(string name, Vector3 pos, Vector3 scale, Material mat)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetPositionAndRotation(pos, Quaternion.identity);
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }

    private Transform MakePlayer(string name, Vector3 pos, Material mat, bool controlled)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = name;
        go.transform.position = pos;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        var collider = go.GetComponent<CapsuleCollider>();
        collider.height = 1.8f;
        if (controlled) player = go.transform;
        return go.transform;
    }

    private void BuildPitch()
    {
        Cube("Pitch", Vector3.zero, new Vector3(28, 0.2f, 44), grassMat);
        Cube("CenterLine", new Vector3(0, 0.12f, 0), new Vector3(27.5f, 0.025f, 0.12f), whiteMat);
        Cube("LeftTouchline", new Vector3(-13.8f, 0.12f, 0), new Vector3(0.12f, 0.025f, 44), whiteMat);
        Cube("RightTouchline", new Vector3(13.8f, 0.12f, 0), new Vector3(0.12f, 0.025f, 44), whiteMat);
        Cube("TopGoalLine", new Vector3(0, 0.12f, 21.8f), new Vector3(28, 0.025f, 0.12f), whiteMat);
        Cube("BottomGoalLine", new Vector3(0, 0.12f, -21.8f), new Vector3(28, 0.025f, 0.12f), whiteMat);
        Cube("PenaltyBoxTop", new Vector3(0, 0.13f, 15.8f), new Vector3(13, 0.025f, 0.1f), whiteMat);
        Cube("PenaltyBoxBottom", new Vector3(0, 0.13f, -15.8f), new Vector3(13, 0.025f, 0.1f), whiteMat);
        BuildGoal(21.5f);
        BuildGoal(-21.5f);
        var sun = new GameObject("Sun").AddComponent<Light>();
        sun.type = LightType.Directional;
        sun.intensity = 1.05f;
        sun.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    private void BuildGoal(float z)
    {
        Cube("GoalPost", new Vector3(-3, 1, z), new Vector3(0.18f, 2, 0.18f), whiteMat);
        Cube("GoalPost", new Vector3(3, 1, z), new Vector3(0.18f, 2, 0.18f), whiteMat);
        Cube("GoalBar", new Vector3(0, 2, z), new Vector3(6, 0.18f, 0.18f), whiteMat);
    }

    private void BuildTeams()
    {
        Vector3[] formation = {
            new Vector3(0,1.1f,-18), new Vector3(-10,1.1f,-12), new Vector3(-3,1.1f,-13),
            new Vector3(3,1.1f,-13), new Vector3(10,1.1f,-12), new Vector3(-9,1.1f,-5),
            new Vector3(-3,1.1f,-6), new Vector3(3,1.1f,-6), new Vector3(9,1.1f,-5),
            new Vector3(-4,1.1f,-1), new Vector3(4,1.1f,-1)
        };
        for (int i = 0; i < TeamSize; i++)
        {
            Transform home = MakePlayer("Blue_" + (i + 1), formation[i], blueMat, i == 9);
            blueTeam.Add(home);
            Vector3 away = new Vector3(formation[i].x, 1.1f, -formation[i].z);
            redTeam.Add(MakePlayer("Red_" + (i + 1), away, redMat, false));
        }
    }

    private void BuildBall()
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = "Football";
        obj.transform.position = new Vector3(0, 0.55f, 0);
        obj.transform.localScale = Vector3.one * 0.72f;
        obj.GetComponent<Renderer>().sharedMaterial = whiteMat;
        ballBody = obj.AddComponent<Rigidbody>();
        ballBody.mass = 0.43f;
        ballBody.linearDamping = 0.22f;
        ballBody.angularDamping = 0.08f;
        ballBody.interpolation = RigidbodyInterpolation.Interpolate;
        ballBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        ball = obj.transform;
    }

    private void BuildCamera()
    {
        gameCamera = new GameObject("GameCamera").AddComponent<Camera>();
        gameCamera.tag = "MainCamera";
        gameCamera.fieldOfView = 62;
        gameCamera.transform.position = player.position + new Vector3(0, 10, -12);
        gameCamera.transform.LookAt(Vector3.zero);
    }

    private void Update()
    {
        if (player == null || ball == null) return;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude < 0.01f) input = new Vector3(touchMove.x, 0, touchMove.y);
        input = Vector3.ClampMagnitude(input, 1);
        player.position += input * 6.2f * Time.deltaTime;
        player.position = new Vector3(Mathf.Clamp(player.position.x, -13, 13), 1.1f, Mathf.Clamp(player.position.z, -21, 21));
        if (input.sqrMagnitude > 0.01f) player.rotation = Quaternion.LookRotation(input);
        if (shootPressed || Input.GetKeyDown(KeyCode.Space)) Kick(10f, 0.16f);
        if (passPressed || Input.GetKeyDown(KeyCode.E)) Kick(5.5f, 0.04f);
        shootPressed = passPressed = false;
        UpdateAI();
        matchTime -= Time.deltaTime;
        if (matchTime <= 0) matchTime = 0;
        CheckGoal();
        Vector3 camTarget = player.position + new Vector3(0, 10, -12);
        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, camTarget, Time.deltaTime * 3.5f);
        gameCamera.transform.LookAt(player.position + Vector3.forward * 5);
    }

    private void Kick(float power, float lift)
    {
        if (Vector3.Distance(player.position, ball.position) > 2.25f) return;
        Vector3 direction = player.forward.sqrMagnitude > 0.1f ? player.forward : Vector3.forward;
        ballBody.AddForce((direction + Vector3.up * lift) * power, ForceMode.Impulse);
    }

    private void UpdateAI()
    {
        aiThink -= Time.deltaTime;
        if (aiThink > 0) return;
        aiThink = 0.06f;
        for (int i = 0; i < redTeam.Count; i++)
        {
            Transform bot = redTeam[i];
            Vector3 target = i == 0 ? new Vector3(0, 1.1f, 18) :
                Vector3.Lerp(new Vector3(bot.position.x * 0.45f, 1.1f, bot.position.z), ball.position, i % 3 == 0 ? 0.58f : 0.12f);
            if (i == 0 && ball.position.z > 12) target = new Vector3(ball.position.x * 0.3f, 1.1f, 18);
            MoveBot(bot, target, i == 0 ? 3.0f : 3.6f);
        }
        for (int i = 0; i < blueTeam.Count; i++)
        {
            Transform bot = blueTeam[i];
            if (bot == player) continue;
            Vector3 target = Vector3.Lerp(new Vector3(bot.position.x * 0.55f, 1.1f, bot.position.z), ball.position, i % 4 == 0 ? 0.42f : 0.08f);
            MoveBot(bot, target, 3.1f);
        }
    }

    private void MoveBot(Transform bot, Vector3 target, float speed)
    {
        Vector3 delta = target - bot.position;
        delta.y = 0;
        if (delta.magnitude > 0.25f)
        {
            Vector3 step = delta.normalized * speed * 0.06f;
            bot.position += Vector3.ClampMagnitude(step, delta.magnitude);
            bot.rotation = Quaternion.LookRotation(delta.normalized);
        }
        if (Vector3.Distance(bot.position, ball.position) < 1.35f && Random.value < 0.025f)
        {
            Vector3 towardGoal = bot.position.z > 0 ? Vector3.back : Vector3.forward;
            ballBody.AddForce((towardGoal + Vector3.up * 0.08f) * 4.2f, ForceMode.Impulse);
        }
    }

    private void CheckGoal()
    {
        if (ball.position.z > 21.4f && Mathf.Abs(ball.position.x) < 3.1f) { redScore++; ResetKickoff(); }
        else if (ball.position.z < -21.4f && Mathf.Abs(ball.position.x) < 3.1f) { blueScore++; ResetKickoff(); }
    }

    private void ResetKickoff()
    {
        ballBody.linearVelocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;
        ballBody.position = new Vector3(0, 0.55f, 0);
        player.position = new Vector3(0, 1.1f, -8);
    }

    private void OnGUI()
    {
        if (hud == null)
        {
            hud = new GUIStyle(GUI.skin.label) { fontSize = 22, alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            hud.normal.textColor = Color.white;
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 22, fontStyle = FontStyle.Bold };
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = Color.white;
        }
        GUI.color = new Color(0.72f, 0.025f, 0.08f, 0.96f);
        GUI.backgroundColor = new Color(0.72f, 0.025f, 0.08f, 0.96f);
        GUI.Label(new Rect(Screen.width * 0.32f, 8, Screen.width * 0.36f, 38),
            string.Format("{0}  -  {1}     {2:00}:{3:00}", blueScore, redScore, Mathf.FloorToInt(matchTime / 60), Mathf.FloorToInt(matchTime % 60)), hud);
        float w = 66, h = 58, x = 18, y = Screen.height - h - 20;
        if (GUI.RepeatButton(new Rect(x + w, y - h, w, h), "↑", buttonStyle)) touchMove.y = 1; else touchMove.y = 0;
        if (GUI.RepeatButton(new Rect(x, y, w, h), "←", buttonStyle)) touchMove.x = -1;
        else if (GUI.RepeatButton(new Rect(x + 2*w, y, w, h), "→", buttonStyle)) touchMove.x = 1;
        else touchMove.x = 0;
        if (GUI.RepeatButton(new Rect(x + w, y + h, w, h), "↓", buttonStyle)) touchMove.y = -1;
        if (GUI.Button(new Rect(Screen.width - 104, Screen.height - 84, 88, 64), "SHOOT", buttonStyle)) shootPressed = true;
        if (GUI.Button(new Rect(Screen.width - 104, Screen.height - 158, 88, 60), "PASS", buttonStyle)) passPressed = true;
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;
    }
}
