using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Procedural, low-cost stadium dressing for the mobile prototype.
/// Crowd figures are instanced meshes; no third-party or unlicensed assets are bundled.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballStadiumEnvironment : MonoBehaviour
{
    [SerializeField, Range(8, 24)] private int crowdRows = 12;
    [SerializeField, Range(12, 40)] private int seatsPerRow = 30;
    private readonly List<Matrix4x4> crowdMatrices = new List<Matrix4x4>();
    private Mesh crowdMesh;
    private Material crowdMaterial;
    private Material standMaterial;
    private AudioSource ambience;

    public void Build()
    {
        if (crowdMesh != null) return;
        crowdMesh = BuildCubeMesh();
        crowdMaterial = MakeMaterial(new Color(0.82f, 0.84f, 0.88f));
        standMaterial = MakeMaterial(new Color(0.12f, 0.14f, 0.19f));
        BuildStand(-1f);
        BuildStand(1f);
        BuildFloodlights();
        BuildAdvertisingBoards();
        BuildCrowdInstances();
        BuildCrowdAmbience();
    }

    private Material MakeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        var material = new Material(shader);
        material.color = color;
        return material;
    }

    private void BuildStand(float side)
    {
        for (int row = 0; row < 4; row++)
        {
            var step = GameObject.CreatePrimitive(PrimitiveType.Cube);
            step.name = "StadiumStand";
            step.transform.position = new Vector3(side * (16.2f + row * 0.85f), 0.55f + row * 0.52f, 0f);
            step.transform.localScale = new Vector3(1.05f, 1.05f, 46f);
            step.GetComponent<Renderer>().sharedMaterial = standMaterial;
            step.isStatic = true;
        }
        var roof = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roof.name = "StadiumCanopy";
        roof.transform.position = new Vector3(side * 19.4f, 4.3f, 0f);
        roof.transform.localScale = new Vector3(1.1f, 0.35f, 47f);
        roof.GetComponent<Renderer>().sharedMaterial = standMaterial;
        roof.isStatic = true;
    }

    private void BuildFloodlights()
    {
        for (int side = -1; side <= 1; side += 2)
        for (int end = -1; end <= 1; end += 2)
        {
            var pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "StadiumFloodlightPole";
            pole.transform.position = new Vector3(side * 20f, 8f, end * 17f);
            pole.transform.localScale = new Vector3(0.22f, 8f, 0.22f);
            pole.GetComponent<Renderer>().sharedMaterial = standMaterial;
            var lamp = new GameObject("Floodlight").AddComponent<Light>();
            lamp.type = LightType.Spot;
            lamp.range = 34f;
            lamp.spotAngle = 78f;
            lamp.intensity = 1.1f;
            lamp.transform.position = pole.transform.position + Vector3.up * 7f;
            lamp.transform.rotation = Quaternion.LookRotation(new Vector3(-side, -0.6f, -end * 0.3f));
        }
    }

    private void BuildAdvertisingBoards()
    {
        Material boardMaterial = MakeMaterial(new Color(0.72f, 0.025f, 0.08f));
        for (int side = -1; side <= 1; side += 2)
        {
            var board = GameObject.CreatePrimitive(PrimitiveType.Cube);
            board.name = "PitchSideAdvertising";
            board.transform.position = new Vector3(side * 14.35f, 0.55f, 0f);
            board.transform.localScale = new Vector3(0.18f, 0.8f, 43f);
            board.GetComponent<Renderer>().sharedMaterial = boardMaterial;
            board.isStatic = true;
        }
    }

    private void BuildCrowdInstances()
    {
        for (int side = -1; side <= 1; side += 2)
        for (int row = 0; row < crowdRows; row++)
        for (int seat = 0; seat < seatsPerRow; seat++)
        {
            float z = -21f + seat * (42f / Mathf.Max(1, seatsPerRow - 1));
            float x = side * (16.2f + row * 0.85f);
            float y = 1.15f + row * 0.52f;
            float jitter = ((row * 17 + seat * 11) % 7) * 0.025f;
            crowdMatrices.Add(Matrix4x4.TRS(
                new Vector3(x, y + jitter, z),
                Quaternion.Euler(0f, side < 0 ? 90f : -90f, 0f),
                new Vector3(0.28f, 0.62f, 0.28f)));
        }
    }

    private void BuildCrowdAmbience()
    {
        const int sampleRate = 22050;
        const int seconds = 4;
        int count = sampleRate * seconds;
        var samples = new float[count];
        for (int i = 0; i < count; i++)
        {
            float t = i / (float)sampleRate;
            float swell = 0.55f + 0.25f * Mathf.Sin(t * 1.4f) + 0.12f * Mathf.Sin(t * 3.7f);
            float murmur = Mathf.Sin(t * 71f) * 0.25f + Mathf.Sin(t * 113f) * 0.18f +
                           Mathf.Sin(t * 167f) * 0.12f;
            samples[i] = murmur * swell * 0.12f;
        }
        AudioClip clip = AudioClip.Create("ProceduralCrowdAmbience", count, 1, sampleRate, false);
        clip.SetData(samples, 0);
        ambience = gameObject.AddComponent<AudioSource>();
        ambience.clip = clip;
        ambience.loop = true;
        ambience.volume = 0.18f;
        ambience.spatialBlend = 0f;
        ambience.Play();
    }

    private void LateUpdate()
    {
        if (crowdMesh == null || crowdMaterial == null) return;
        const int batchSize = 1023;
        var batch = new Matrix4x4[Mathf.Min(batchSize, crowdMatrices.Count)];
        for (int start = 0; start < crowdMatrices.Count; start += batchSize)
        {
            int count = Mathf.Min(batchSize, crowdMatrices.Count - start);
            if (batch.Length != count) batch = new Matrix4x4[count];
            crowdMatrices.CopyTo(start, batch, 0, count);
            Graphics.DrawMeshInstanced(crowdMesh, 0, crowdMaterial, batch, count);
        }
    }

    private Mesh BuildCubeMesh()
    {
        var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Mesh source = temp.GetComponent<MeshFilter>().sharedMesh;
        var mesh = Instantiate(source);
        mesh.name = "InstancedCrowdMesh";
        Destroy(temp);
        return mesh;
    }

    private void OnDestroy()
    {
        if (ambience != null) ambience.Stop();
        if (crowdMesh != null) Destroy(crowdMesh);
        if (crowdMaterial != null) Destroy(crowdMaterial);
        if (standMaterial != null) Destroy(standMaterial);
    }
}
