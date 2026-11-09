using UnityEngine;

public class FootballPrototype : MonoBehaviour
{
    private Transform player;
    private Transform ball;
    private Camera gameCamera;
    private float speed = 6f;
    private Vector2 touchMove;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void StartPrototype()
    {
        if (FindObjectOfType<FootballPrototype>() == null)
            new GameObject("FootballPrototype").AddComponent<FootballPrototype>();
    }

    private void Start()
    {
        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        QualitySettings.antiAliasing = 0;
        QualitySettings.shadows = ShadowQuality.Disable;
        BuildPitch();
        BuildPlayer();
        BuildBall();
        BuildCamera();
        RenderSettings.ambientLight = new Color(0.72f, 0.76f, 0.8f);
    }

    private Material Mat(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        var material = new Material(shader);
        material.color = color;
        return material;
    }

    private GameObject Cube(string name, Vector3 pos, Vector3 scale, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.position = pos;
        go.transform.localScale = scale;
        go.GetComponent<Renderer>().material = Mat(color);
        return go;
    }

    private void BuildPitch()
    {
        Cube("Pitch", Vector3.zero, new Vector3(28, 0.2f, 44), new Color(0.12f, 0.42f, 0.18f));
        Cube("CenterLine", new Vector3(0, 0.12f, 0), new Vector3(27.5f, 0.025f, 0.12f), Color.white);
        Cube("LeftTouchline", new Vector3(-13.8f, 0.12f, 0), new Vector3(0.12f, 0.025f, 44), Color.white);
        Cube("RightTouchline", new Vector3(13.8f, 0.12f, 0), new Vector3(0.12f, 0.025f, 44), Color.white);
        Cube("TopGoalLine", new Vector3(0, 0.12f, 21.8f), new Vector3(28, 0.025f, 0.12f), Color.white);
        Cube("BottomGoalLine", new Vector3(0, 0.12f, -21.8f), new Vector3(28, 0.025f, 0.12f), Color.white);
        Cube("GoalLeftPost", new Vector3(-3, 1, 21.5f), new Vector3(0.18f, 2, 0.18f), Color.white);
        Cube("GoalRightPost", new Vector3(3, 1, 21.5f), new Vector3(0.18f, 2, 0.18f), Color.white);
        Cube("GoalCrossbar", new Vector3(0, 2, 21.5f), new Vector3(6, 0.18f, 0.18f), Color.white);
        var light = new GameObject("Sun").AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.1f;
        light.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    private void BuildPlayer()
    {
        var body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.name = "Player";
        body.transform.position = new Vector3(0, 1.1f, -8);
        body.GetComponent<Renderer>().material = Mat(new Color(0.08f, 0.25f, 0.9f));
        player = body.transform;
    }

    private void BuildBall()
    {
        var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.name = "Football";
        obj.transform.position = new Vector3(0, 0.45f, -4);
        obj.transform.localScale = Vector3.one * 0.8f;
        obj.GetComponent<Renderer>().material = Mat(Color.white);
        var rb = obj.AddComponent<Rigidbody>();
        rb.mass = 0.45f;
        rb.linearDamping = 0.4f;
        ball = obj.transform;
    }

    private void BuildCamera()
    {
        var cam = new GameObject("GameCamera").AddComponent<Camera>();
        cam.tag = "MainCamera";
        cam.fieldOfView = 60;
        gameCamera = cam;
        cam.transform.position = player.position + new Vector3(0, 8, -10);
        cam.transform.LookAt(player.position + Vector3.forward * 5);
    }

    private void Update()
    {
        if (player == null) return;
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input.sqrMagnitude < 0.01f)
            input = new Vector3(touchMove.x, 0, touchMove.y);
        input = Vector3.ClampMagnitude(input, 1);
        player.position += input * speed * Time.deltaTime;
        player.position = new Vector3(Mathf.Clamp(player.position.x, -13, 13), 1.1f,
            Mathf.Clamp(player.position.z, -21, 21));

        if (input.sqrMagnitude > 0.01f)
            player.rotation = Quaternion.LookRotation(input, Vector3.up);

        if (ball != null && Vector3.Distance(player.position, ball.position) < 1.6f &&
            Input.GetKeyDown(KeyCode.Space))
        {
            var rb = ball.GetComponent<Rigidbody>();
            rb.AddForce((player.forward + Vector3.up * 0.18f) * 8f, ForceMode.Impulse);
        }

        if (gameCamera != null)
        {
            Vector3 target = player.position + new Vector3(0, 8, -10);
            gameCamera.transform.position = Vector3.Lerp(gameCamera.transform.position, target, Time.deltaTime * 5);
            gameCamera.transform.LookAt(player.position + Vector3.forward * 4);
        }
    }

    private void OnGUI()
    {
        float w = 78, h = 64, pad = 18;
        float x = pad, y = Screen.height - h - pad;
        GUI.skin.button.fontSize = 26;
        Color oldBackground = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.72f, 0.025f, 0.08f, 0.96f);
        GUI.skin.button.normal.textColor = Color.white;
        GUI.skin.button.hover.textColor = Color.white;
        if (GUI.RepeatButton(new Rect(x + w, y - h, w, h), "↑")) touchMove.y = 1;
        else if (GUI.RepeatButton(new Rect(x + w, y + h, w, h), "↓")) touchMove.y = -1;
        else touchMove.y = 0;
        if (GUI.RepeatButton(new Rect(x, y, w, h), "←")) touchMove.x = -1;
        else if (GUI.RepeatButton(new Rect(x + 2*w, y, w, h), "→")) touchMove.x = 1;
        else touchMove.x = 0;
        if (GUI.Button(new Rect(Screen.width - 120, Screen.height - 100, 100, 76), "SHOOT"))
        {
            if (ball != null && Vector3.Distance(player.position, ball.position) < 2.2f)
                ball.GetComponent<Rigidbody>().AddForce((player.forward + Vector3.up * 0.2f) * 10f, ForceMode.Impulse);
        }
        GUI.Label(new Rect(12, 12, 340, 32), "ULTIMATE FOOTBALL");
        GUI.backgroundColor = oldBackground;
    }
}
