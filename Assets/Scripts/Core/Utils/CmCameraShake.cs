using System;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class CmCameraShake : MonoSingleton<CmCameraShake>
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }


    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }


    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShakeCamera(2f, .1f);
        }
    }
}