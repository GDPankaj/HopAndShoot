using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    CinemachineVirtualCamera VirtualCamera;
    [SerializeField] float shakeIntensity = 1f;
    [SerializeField] float shakeDuration = .2f;
    float timer;

    CinemachineBasicMultiChannelPerlin _bmcp;

    private void Start()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        _bmcp = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        StopCameraShake();

        
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        StopCameraShake();
    }
    public void ShakeCamera()
    {
        _bmcp.m_AmplitudeGain = shakeIntensity;

        timer = shakeDuration;
    }

    void StopCameraShake() 
    {
        if (timer <= 0f)
        {
            _bmcp.m_AmplitudeGain = 0f;
        }
    }

}
