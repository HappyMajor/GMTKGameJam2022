using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathThunder : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Routines.DoLater(1f, () =>
        {
            Destroy(this.gameObject);
        }));
    }
}
