using UnityEngine;
using Cinemachine;

public class ScreenshakeManager : Singleton<ScreenshakeManager>
{
    [Header("Screenshake Settings")]
    public NoiseProfile shakeSettings;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;

    protected override void Awake()
    {
        base.Awake();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null)
        {
            noise = virtualCamera.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    public void ApplyScreenshake()
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = shakeSettings.amplitude;
            noise.m_FrequencyGain = shakeSettings.frequency;
            StartCoroutine(ResetScreenshakeAfterDelay());
        }
    }

    private System.Collections.IEnumerator ResetScreenshakeAfterDelay()
    {
        yield return new WaitForSeconds(shakeSettings.duration);
        noise.m_AmplitudeGain = 0.0f;
        noise.m_FrequencyGain = 0.0f;
    }
}
