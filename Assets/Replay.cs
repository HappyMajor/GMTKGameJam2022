using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    // [SerializeField] private float delayUntilGameOverSound = 1.0f;

    void Start() {
        // Play game over sound after a delay
        OneShotAudio.Play("event:/sfx/game over");
    }

    public void OnReplayClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
