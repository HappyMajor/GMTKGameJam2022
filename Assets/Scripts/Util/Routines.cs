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

    public static IEnumerator DoEverySeconds(float seconds, Action action)
    {
        while(true)
        {
            action.Invoke();
            yield return new WaitForSeconds(seconds);
        }
    }

    public static IEnumerator DoEverySecondsAndEndAfter(float interval, float end, Action action)
    {
        float deltaTime = 0f;
        bool firstStep = true;
        while (true)
        {
            if (deltaTime >= end) break;
            if (firstStep != true) deltaTime += interval;
            firstStep = false;
            action.Invoke();
            yield return new WaitForSeconds(interval);
        }
    }

    public static IEnumerator DoEverySecondsAndEndAfter(float interval, float end, Action action, Action endAction)
    {
        float deltaTime = 0f;
        bool firstStep = true;
        while (true)
        {
            if (deltaTime >= end) break;
            if (firstStep != true) deltaTime += interval;
            firstStep = false;
            action.Invoke();
            yield return new WaitForSeconds(interval);
        }

        endAction.Invoke();
    }
}
