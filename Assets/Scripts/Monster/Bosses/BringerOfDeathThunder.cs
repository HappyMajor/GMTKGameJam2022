using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathThunder : MonoBehaviour
{
    private void Start()
    {
        // Play lightning sound
        OneShotAudio.Play("event:/sfx/bringer of death/lightning");

        StartCoroutine(Routines.DoLater(1f, () =>
        {
            Destroy(this.gameObject);
        }));
    }
}
