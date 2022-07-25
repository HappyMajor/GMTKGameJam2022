using UnityEngine;
using Cinemachine;

// TODO: reduce intensity over time if we need that functionality
// source: https://www.youtube.com/watch?v=ACf1I27I6Tk
[DisallowMultipleComponent]
public class CameraShake : MonoBehaviour
{
  public static CameraShake Instance { get; private set; }
  CinemachineVirtualCamera cinemachineVirtualCamera;
  float shakeTime;

  void Awake()
  {
    Instance = this;
    cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
  }

  public void ShakeCamera(float intensity, float time)
  {
    // Set intensity of the shake and how long it will last
    setShakeIntensity(intensity);
    shakeTime = time;
  }

  void Update()
  {
    // update timer
    shakeTime -= Time.deltaTime;

    // not ready to shake
    if (shakeTime > 0)
    {
      return;
    }

    // stop shaking
    setShakeIntensity(0);
  }

  private void setShakeIntensity(float intensity)
  {
    var cinemachineBasicMultiChannelPerlin =
        cinemachineVirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
  }
}
