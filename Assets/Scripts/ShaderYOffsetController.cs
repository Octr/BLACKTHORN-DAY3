using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderYOffsetController : MonoBehaviour
{
    private Renderer targetRenderer;
    public float yOffsetRate = 0.1f;

    private Material material;
    private float yOffset;

    void Start()
    {
        targetRenderer = GetComponent<Renderer>();
        // Get the material from the target Renderer
        material = targetRenderer.material;

        // Store the initial Y offset value
        material.SetTextureOffset("_MainTex", new Vector2(0, yOffset));
        material.SetTextureOffset("_EmissionMap", new Vector2(0, yOffset));
    }

    void Update()
    {
        // Increment the Y offset value at a constant rate
        yOffset += yOffsetRate * Time.deltaTime;

        // Set the updated Y offset value to the material
        material.SetTextureOffset("_MainTex", new Vector2(0, yOffset));
        material.SetTextureOffset("_EmissionMap", new Vector2(0, yOffset));
    }
}
