using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakeLights : MonoBehaviour
{
    [ColorUsage(false,true)]
    public Color litColor = Color.red;       // Color when brake lights are lit
    public Color unlitColor = Color.black;   // Color when brake lights are not lit

    private Renderer brakeLightsRenderer;

    void Start()
    {
        brakeLightsRenderer = GetComponent<Renderer>();
        UpdateBrakeLights();
    }

    void Update()
    {
        // Check if the car is braking (moving backward)
        bool isBraking = Input.GetAxis("Vertical") < 0;

        if (isBraking)
        {
            brakeLightsRenderer.material.SetColor("_EmissionColor", litColor);
        }
        else
        {
            brakeLightsRenderer.material.SetColor("_EmissionColor", unlitColor);
        }
    }

    // Call this method if you want to update the brake lights color externally
    public void UpdateBrakeLights()
    {
        bool isBraking = Input.GetAxis("Vertical") < 0;

        if (isBraking)
        {
            brakeLightsRenderer.material.SetColor("_EmissionColor", litColor);
        }
        else
        {
            brakeLightsRenderer.material.SetColor("_EmissionColor", unlitColor);
        }
    }
}
