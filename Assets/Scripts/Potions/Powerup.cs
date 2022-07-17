using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
  private AudioSource audioSource;
  [SerializeField] public AudioClip sound;

  void Start()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public void PlaySound()
  {
    audioSource.PlayOneShot(sound);
  }
}
