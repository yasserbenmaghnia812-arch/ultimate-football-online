using UnityEngine;

/// <summary>
/// Procedural low-poly footballer silhouette for the prototype.
/// Replace with licensed rigged humanoid models and Animator controllers in the art pass.
/// </summary>
[DisallowMultipleComponent]
public sealed class FootballCharacterVisual : MonoBehaviour
{
    private Transform leftArm, rightArm, leftLeg, rightLeg;
    private Vector3 lastPosition;
    private Material shirtMaterial;
    private static Material skinMaterial, shortsMaterial, bootMaterial;

    public void Initialize(Material teamMaterial)
    {
        shirtMaterial = teamMaterial;
        if (skinMaterial == null) skinMaterial = ColorMaterial(new Color(0.72f, 0.52f, 0.38f));
        if (shortsMaterial == null) shortsMaterial = ColorMaterial(new Color(0.08f, 0.09f, 0.12f));
        if (bootMaterial == null) bootMaterial = ColorMaterial(Color.black);

        Renderer baseRenderer = GetComponent<Renderer>();
        if (baseRenderer != null) baseRenderer.sharedMaterial = shirtMaterial;

        AddPart("Head", PrimitiveType.Sphere, new Vector3(0f, 0.98f, 0f), new Vector3(0.30f, 0.34f, 0.30f), skinMaterial);
        AddPart("Neck", PrimitiveType.Cylinder, new Vector3(0f, 0.72f, 0f), new Vector3(0.10f, 0.12f, 0.10f), skinMaterial);
        leftArm = AddPart("LeftArm", PrimitiveType.Capsule, new Vector3(-0.48f, 0.05f, 0f), new Vector3(0.18f, 0.48f, 0.18f), shirtMaterial);
        rightArm = AddPart("RightArm", PrimitiveType.Capsule, new Vector3(0.48f, 0.05f, 0f), new Vector3(0.18f, 0.48f, 0.18f), shirtMaterial);
        leftLeg = AddPart("LeftLeg", PrimitiveType.Capsule, new Vector3(-0.20f, -0.42f, 0f), new Vector3(0.22f, 0.48f, 0.22f), shortsMaterial);
        rightLeg = AddPart("RightLeg", PrimitiveType.Capsule, new Vector3(0.20f, -0.42f, 0f), new Vector3(0.22f, 0.48f, 0.22f), shortsMaterial);
        AddPart("LeftBoot", PrimitiveType.Cube, new Vector3(-0.20f, -0.79f, 0.10f), new Vector3(0.22f, 0.12f, 0.36f), bootMaterial);
        AddPart("RightBoot", PrimitiveType.Cube, new Vector3(0.20f, -0.79f, 0.10f), new Vector3(0.22f, 0.12f, 0.36f), ColorMaterial(Color.black));
        lastPosition = transform.position;
    }

    private Transform AddPart(string partName, PrimitiveType primitive, Vector3 localPosition, Vector3 localScale, Material material)
    {
        GameObject part = GameObject.CreatePrimitive(primitive);
        part.name = partName;
        part.transform.SetParent(transform, false);
        part.transform.localPosition = localPosition;
        part.transform.localScale = localScale;
        Renderer renderer = part.GetComponent<Renderer>();
        if (renderer != null) renderer.sharedMaterial = material;
        Collider collider = part.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
        return part.transform;
    }

    private Material ColorMaterial(Color color)
    {
        Material material = new Material(Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard"));
        material.color = color;
        return material;
    }

    private void LateUpdate()
    {
        if (leftArm == null || rightArm == null || leftLeg == null || rightLeg == null) return;
        float speed = (transform.position - lastPosition).magnitude / Mathf.Max(Time.deltaTime, 0.0001f);
        lastPosition = transform.position;
        float swing = Mathf.Sin(Time.time * Mathf.Clamp(speed * 2.2f, 1f, 12f)) * Mathf.Clamp01(speed / 4f) * 32f;
        leftArm.localRotation = Quaternion.Euler(swing, 0f, -8f);
        rightArm.localRotation = Quaternion.Euler(-swing, 0f, 8f);
        leftLeg.localRotation = Quaternion.Euler(-swing, 0f, 0f);
        rightLeg.localRotation = Quaternion.Euler(swing, 0f, 0f);
    }
}
