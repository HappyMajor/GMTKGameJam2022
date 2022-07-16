using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Routines : MonoBehaviour
{
    public static IEnumerator DoLater(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
       
    }
}
