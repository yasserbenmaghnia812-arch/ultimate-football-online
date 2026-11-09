using System.Collections.Generic;
using UnityEngine;

public sealed class FootballPrototype : MonoBehaviour
{
    private const int TeamSize = 11;
    private readonly List<Transform> blueTeam = new List<Transform>();
    private readonly List<Transform> redTeam = new List<Transform>();
    private Transform player, ball;
    private Rigidbody ballBody;
    private AdvancedFootballPhysics ballPhysics;
    private FootballPlayerAttributes playerAttributes;
    private Camera gameCamera;
    private Vector2 touchMove;
    private bool shootPressed, passPressed, tacklePressed, switchPressed, sprintPressed;
    private FootballMatchRules matchRules;
    private FootballMatchStatistics matchStatistics;
    private FootballRefereeSystem referee;
    private float possessionSample;
    private FootballCareerProfile careerProfile;
    private bool careerResultSaved;
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
        gameObject.AddComponent<FootballStadiumEnvironment>().Build();
        BuildTeams();
        BuildBall();
        matchRules = gameObject.AddComponent<FootballMatchRules>();
        matchStatistics = gameObject.AddComponent<FootballMatchStatistics>();
        referee = gameObject.AddComponent<FootballRefereeSystem>();
        referee.Whistle += OnRefereeWhistle;
        careerProfile = FootballCareerSaveSystem.Load();
        matchRules.StateChanged += OnMatchStateChanged;
        matchRules.StartMatch();
        if (FindObjectOfType<FootballLocalization>() == null)
            new GameObject("FootballLocalization").AddComponent<FootballLocalization>();
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
        var attributes = go.AddComponent<FootballPlayerAttributes>();
        GameObject modelPrefab = Resources.Load<GameObject>(
            controlled ? "Footballers/BluePlayer" : "Footballers/RedPlayer");
        var animationDriver = go.AddComponent<FootballCharacterAnimationDriver>();
        if (modelPrefab != null)
        {
            GameObject model = Instantiate(modelPrefab, go.transform);
            model.name = "LicensedFootballerModel";
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
            go.GetComponent<Renderer>().enabled = false;
            animationDriver.Bind(model.GetComponentInChildren<Animator>());
        }
        else
        {
            var visual = go.AddComponent<FootballCharacterVisual>();
            visual.Initialize(mat);
        }
        if (controlled)
        {
            player = go.transform;
            playerAttributes = attributes;
        }
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
            new Vector3(0,1.0f,-18), new Vector3(-10,1.0f,-12), new Vector3(-3,1.0f,-13),
            new Vector3(3,1.0f,-13), new Vector3(10,1.0f,-12), new Vector3(-9,1.0f,-5),
            new Vector3(-3,1.0f,-6), new Vector3(3,1.0f,-6), new Vector3(9,1.0f,-5),
            new Vector3(-4,1.0f,-1), new Vector3(4,1.0f,-1)
        };
        for (int i = 0; i < TeamSize; i++)
        {
            Transform home = MakePlayer("Blue_" + (i + 1), formation[i], blueMat, i == 9);
            blueTeam.Add(home);
            Vector3 away = new Vector3(formation[i].x, 1.0f, -formation[i].z);
            redTeam.Add(MakePlayer("Red_" + (i + 1), away, redMat, false));
        }
    }

    private void BuildBall()
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = "Football";
        // 1 Unity unit = 1 metre; regulation-size ball diameter is approximately 0.22 m.
        obj.transform.position = new Vector3(0, 0.21f, 0);
        obj.transform.localScale = Vector3.one * 0.22f;
        obj.GetComponent<Renderer>().sharedMaterial = whiteMat;
        ballBody = obj.AddComponent<Rigidbody>();
        ballBody.mass = 0.43f;
        ballBody.linearDamping = 0.015f;
        ballBody.angularDamping = 0.035f;
        ballBody.interpolation = RigidbodyInterpolation.Interpolate;
        ballBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        ballPhysics = obj.AddComponent<AdvancedFootballPhysics>();
        ballPhysics.Initialize(ballBody);
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
        if (matchRules != null && (matchRules.State == FootballMatchState.FullTime || matchRules.State == FootballMatchState.Paused)) return;
        if (switchPressed || Input.GetKeyDown(KeyCode.Tab)) SwitchControlledPlayer();
        sprintPressed = sprintPressed || Input.GetKey(KeyCode.LeftShift);
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude < 0.01f) input = new Vector3(touchMove.x, 0, touchMove.y);
        input = Vector3.ClampMagnitude(input, 1);
        if (playerAttributes != null)
        {
            playerAttributes.TickMovement(input.sqrMagnitude > 0.01f, Time.deltaTime);
            input *= playerAttributes.MovementMultiplier();
            if (sprintPressed && input.sqrMagnitude > 0.01f)
            {
                input *= 1.42f;
                playerAttributes.stamina = Mathf.Max(0f, playerAttributes.stamina - 0.55f * Time.deltaTime);
            }
        }
        player.position += input * 6.2f * Time.deltaTime;
        player.position = new Vector3(Mathf.Clamp(player.position.x, -13, 13), 1.0f, Mathf.Clamp(player.position.z, -21, 21));
        if (input.sqrMagnitude > 0.01f) player.rotation = Quaternion.LookRotation(input);
        if (shootPressed || Input.GetKeyDown(KeyCode.Space)) Kick(10f, 0.16f);
        if (passPressed || Input.GetKeyDown(KeyCode.E)) Kick(5.5f, 0.04f);
        if (tacklePressed || Input.GetKeyDown(KeyCode.Q)) Tackle();
        shootPressed = passPressed = tacklePressed = switchPressed = sprintPressed = false;
        if (matchRules != null)
        {
            matchRules.Tick(Time.deltaTime);
            if (matchRules.State == FootballMatchState.HalfTime) matchRules.ResumeSecondHalf();
        }
        UpdateAI();
        SeparatePlayers();
        UpdatePossessionStatistics();
        matchTime -= Time.deltaTime;
        if (matchTime <= 0) matchTime = 0;
        CheckGoal();
        Vector3 camTarget = player.position + new Vector3(0, 10, -12);
        gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, camTarget, Time.deltaTime * 3.5f);
        gameCamera.transform.LookAt(player.position + Vector3.forward * 5);
    }

    private void Kick(float power, float lift)
    {
        FootballCharacterAnimationDriver animationDriver = player != null ? player.GetComponent<FootballCharacterAnimationDriver>() : null;
        if (power >= 9f) animationDriver?.PlayKick();
        else animationDriver?.PlayPass();
        if (Vector3.Distance(player.position, ball.position) > 2.25f) return;
        Vector3 direction = player.forward.sqrMagnitude > 0.1f ? player.forward : Vector3.forward;
        if (power >= 9f)
        {
            if (matchStatistics != null) matchStatistics.RegisterShot(true, false);
        }
        else
        {
            Transform receiver = FindForwardPassReceiver(direction);
            if (receiver != null && referee != null)
            {
                Transform[] defenders = new Transform[redTeam.Count];
                for (int i = 0; i < redTeam.Count; i++) defenders[i] = redTeam[i];
                bool inOwnHalf = receiver.position.z < 0f;
                if (referee.CheckOffsideAtPass(receiver, ball.position, defenders, true, inOwnHalf, false))
                {
                    // The referee system records the offside once and awards the restart.
                    return;
                }
            }
            if (matchStatistics != null) matchStatistics.RegisterPass(true, false);
        }
        float baseSpeed = power >= 9f ? 22f : 13f;
        float shotSpeed = playerAttributes != null ? playerAttributes.GetKickSpeed(baseSpeed) : baseSpeed;
        float accuracy = playerAttributes != null ? playerAttributes.GetShotAccuracy() : 0.7f;
        float boundedAimVariation = (1f - accuracy) * 0.10f;
        direction = Quaternion.Euler(0f, Random.Range(-boundedAimVariation, boundedAimVariation) * 90f, 0f) * direction;
        Vector3 spin = Vector3.Cross(Vector3.up, direction) * (power >= 9f ? 2.2f : 0.7f);
        if (ballPhysics != null) ballPhysics.Kick(direction, shotSpeed, lift, spin);
        else ballBody.AddForce((direction + Vector3.up * lift).normalized * shotSpeed * ballBody.mass, ForceMode.Impulse);
    }

    private void UpdateAI()
    {
        aiThink -= Time.deltaTime;
        if (aiThink > 0) return;
        aiThink = 0.06f;
        for (int i = 0; i < redTeam.Count; i++)
        {
            Transform bot = redTeam[i];
            Vector3 home = GetFormationPosition(i, false);
            FootballRole role = GetRole(i);
            Vector3 target = FootballTacticalAI.GetRoleTarget(role, i % 2, ball.position, home, false, FootballTeamStyle.Balanced);
            float pressure = FootballTacticalAI.GetPressingWeight(bot.GetComponent<FootballPlayerAttributes>().stamina, 1f);
            target = Vector3.Lerp(target, ball.position, role == FootballRole.Midfielder ? 0.10f * pressure : 0.035f * pressure);
            MoveBot(bot, target, role == FootballRole.Goalkeeper ? 3.0f : 3.6f);
        }
        for (int i = 0; i < blueTeam.Count; i++)
        {
            Transform bot = blueTeam[i];
            if (bot == player) continue;
            FootballRole role = GetRole(i);
            Vector3 home = GetFormationPosition(i, true);
            Vector3 target = FootballTacticalAI.GetRoleTarget(role, i % 2, ball.position, home, true, FootballTeamStyle.Balanced);
            float pressure = FootballTacticalAI.GetPressingWeight(bot.GetComponent<FootballPlayerAttributes>().stamina, 1f);
            target = Vector3.Lerp(target, ball.position, role == FootballRole.Midfielder ? 0.12f * pressure : 0.04f * pressure);
            MoveBot(bot, target, 3.1f);
        }
    }

    private void MoveBot(Transform bot, Vector3 target, float speed)
    {
        Vector3 delta = target - bot.position;
        delta.y = 0;
        if (delta.magnitude > 0.25f)
        {
            FootballPlayerAttributes attributes = bot.GetComponent<FootballPlayerAttributes>();
            if (attributes != null)
            {
                attributes.TickMovement(true, Time.deltaTime);
                speed *= attributes.MovementMultiplier();
            }
            Vector3 step = delta.normalized * speed * Time.deltaTime;
            bot.position += Vector3.ClampMagnitude(step, delta.magnitude);
            bot.rotation = Quaternion.LookRotation(delta.normalized);
        }
        else
        {
            FootballPlayerAttributes restingAttributes = bot.GetComponent<FootballPlayerAttributes>();
            if (restingAttributes != null) restingAttributes.TickMovement(false, Time.deltaTime);
        }
        if (Vector3.Distance(bot.position, ball.position) < 1.35f && Random.value < 0.025f)
        {
            Vector3 towardGoal = bot.position.z > 0 ? Vector3.back : Vector3.forward;
            ballBody.AddForce((towardGoal + Vector3.up * 0.08f) * 4.2f, ForceMode.Impulse);
        }
    }

    private FootballRole GetRole(int index)
    {
        if (index == 0) return FootballRole.Goalkeeper;
        if (index <= 4) return FootballRole.Defender;
        if (index <= 8) return FootballRole.Midfielder;
        return FootballRole.Striker;
    }

    private Vector3 GetFormationPosition(int index, bool blue)
    {
        float sign = blue ? -1f : 1f;
        if (index == 0) return new Vector3(0, 1.0f, sign * 18f);
        if (index <= 4)
        {
            float[] x = { -10f, -3f, 3f, 10f };
            return new Vector3(x[index - 1], 1.0f, sign * 12f);
        }
        if (index <= 8)
        {
            float[] x = { -9f, -3f, 3f, 9f };
            return new Vector3(x[index - 5], 1.0f, sign * 5f);
        }
        return new Vector3(index == 9 ? -4f : 4f, 1.0f, sign * 1f);
    }

    private void SwitchControlledPlayer()
    {
        Transform candidate = null;
        float best = float.MaxValue;
        for (int i = 1; i < blueTeam.Count; i++)
        {
            Transform teammate = blueTeam[i];
            float distance = Vector3.Distance(teammate.position, ball.position);
            if (teammate != player && distance < best)
            {
                best = distance;
                candidate = teammate;
            }
        }
        if (candidate == null) return;
        player = candidate;
        playerAttributes = player.GetComponent<FootballPlayerAttributes>();
    }

    private void SeparatePlayers()
    {
        for (int a = 0; a < blueTeam.Count; a++)
        {
            for (int b = 0; b < redTeam.Count; b++)
                SeparatePair(blueTeam[a], redTeam[b]);
            for (int b = a + 1; b < blueTeam.Count; b++)
                SeparatePair(blueTeam[a], blueTeam[b]);
        }
        for (int a = 0; a < redTeam.Count; a++)
            for (int b = a + 1; b < redTeam.Count; b++)
                SeparatePair(redTeam[a], redTeam[b]);
    }

    private void SeparatePair(Transform a, Transform b)
    {
        Vector3 delta = a.position - b.position;
        delta.y = 0f;
        float distance = delta.magnitude;
        const float minimumDistance = 0.78f;
        if (distance <= 0.001f || distance >= minimumDistance) return;
        Vector3 correction = delta.normalized * ((minimumDistance - distance) * 0.5f);
        if (a != player) a.position += correction;
        if (b != player) b.position -= correction;
    }

    private void Tackle()
    {
        if (player != null) player.GetComponent<FootballCharacterAnimationDriver>()?.PlayTackle();
        if (player == null || ballBody == null) return;
        if (playerAttributes != null && playerAttributes.stamina < 4f) return;
        Transform nearest = null;
        float nearestDistance = 2.2f;
        for (int i = 0; i < redTeam.Count; i++)
        {
            float distance = Vector3.Distance(player.position, redTeam[i].position);
            if (distance < nearestDistance) { nearestDistance = distance; nearest = redTeam[i]; }
        }
        if (nearest == null || Vector3.Distance(player.position, ball.position) > 2.8f) return;
        Vector3 away = ball.position - nearest.position;
        away.y = 0.08f;
        if (ballPhysics != null) ballPhysics.Kick(away.normalized, 6.5f, 0.08f, Vector3.zero);
        if (matchStatistics != null) matchStatistics.RegisterTackle(true);
        if (playerAttributes != null) playerAttributes.stamina = Mathf.Max(0f, playerAttributes.stamina - 3.5f);
    }

    private Transform FindForwardPassReceiver(Vector3 direction)
    {
        Transform best = null;
        float bestScore = float.MaxValue;
        for (int i = 0; i < blueTeam.Count; i++)
        {
            Transform candidate = blueTeam[i];
            if (candidate == player) continue;
            Vector3 delta = candidate.position - player.position;
            float distance = delta.magnitude;
            if (distance < 1.5f || distance > 14f) continue;
            float forwardDot = Vector3.Dot(direction.normalized, delta.normalized);
            if (forwardDot < 0.45f) continue;
            float score = distance + (1f - forwardDot) * 4f;
            if (score < bestScore) { bestScore = score; best = candidate; }
        }
        return best;
    }

    private void OnRefereeWhistle(string decision, Vector3 location)
    {
        if (ballBody == null) return;
        ballBody.linearVelocity = Vector3.zero;
        ballBody.angularVelocity = Vector3.zero;
        ballBody.position = location;
    }

    private void UpdatePossessionStatistics()
    {
        if (matchStatistics == null || ball == null) return;
        possessionSample += Time.deltaTime;
        if (possessionSample < 0.5f) return;
        float nearestBlue = float.MaxValue;
        float nearestRed = float.MaxValue;
        for (int i = 0; i < blueTeam.Count; i++)
            nearestBlue = Mathf.Min(nearestBlue, Vector3.Distance(ball.position, blueTeam[i].position));
        for (int i = 0; i < redTeam.Count; i++)
            nearestRed = Mathf.Min(nearestRed, Vector3.Distance(ball.position, redTeam[i].position));
        if (Mathf.Min(nearestBlue, nearestRed) < 2.2f)
            matchStatistics.TickPossession(nearestBlue <= nearestRed, possessionSample);
        possessionSample = 0f;
    }

    private void OnMatchStateChanged(FootballMatchState state)
    {
        if (state != FootballMatchState.FullTime || careerResultSaved) return;
        careerResultSaved = true;
        if (careerProfile == null) careerProfile = FootballCareerSaveSystem.Load();
        careerProfile.RecordMatch(blueScore, redScore, blueScore > redScore ? 120 : blueScore == redScore ? 60 : 30);
        FootballCareerSaveSystem.Save(careerProfile);
    }

    private void CheckGoal()
    {
        if (ball.position.z > 21.4f && ball.position.y < 1.95f && Mathf.Abs(ball.position.x) < 3.1f)
        {
            redScore++;
            if (matchRules != null) matchRules.AddGoal(false);
            if (matchStatistics != null) matchStatistics.RegisterGoal(false);
            ResetKickoff();
        }
        else if (ball.position.z < -21.4f && ball.position.y < 1.95f && Mathf.Abs(ball.position.x) < 3.1f)
        {
            blueScore++;
            if (matchRules != null) matchRules.AddGoal(true);
            if (matchStatistics != null) matchStatistics.RegisterGoal(true);
            ResetKickoff();
        }
    }

    private void ResetKickoff()
    {
        if (ballPhysics != null) ballPhysics.ResetBall(new Vector3(0, 0.21f, 0));
        else
        {
            ballBody.linearVelocity = Vector3.zero;
            ballBody.angularVelocity = Vector3.zero;
            ballBody.position = new Vector3(0, 0.21f, 0);
        }
        player.position = new Vector3(0, 1.0f, -8);
        for (int i = 0; i < blueTeam.Count; i++) if (blueTeam[i] != player) blueTeam[i].position = GetFormationPosition(i, true);
        for (int i = 0; i < redTeam.Count; i++) redTeam[i].position = GetFormationPosition(i, false);
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
        if (GUI.Button(new Rect(Screen.width - 104, Screen.height - 84, 88, 64), (FootballLocalization.Instance != null ? FootballLocalization.Instance.Get("shoot") : "SHOOT"), buttonStyle)) shootPressed = true;
        if (GUI.Button(new Rect(Screen.width - 104, Screen.height - 158, 88, 60), (FootballLocalization.Instance != null ? FootballLocalization.Instance.Get("pass") : "PASS"), buttonStyle)) passPressed = true;
        if (GUI.Button(new Rect(Screen.width - 104, Screen.height - 232, 88, 60), (FootballLocalization.Instance != null ? FootballLocalization.Instance.Get("tackle") : "TACKLE"), buttonStyle)) tacklePressed = true;
        if (GUI.Button(new Rect(Screen.width - 196, Screen.height - 158, 84, 60), "SWITCH", buttonStyle)) switchPressed = true;
        sprintPressed = GUI.RepeatButton(new Rect(Screen.width - 196, Screen.height - 84, 84, 64), "SPRINT", buttonStyle);
        GUI.color = Color.white;
        GUI.backgroundColor = Color.white;
    }
}
