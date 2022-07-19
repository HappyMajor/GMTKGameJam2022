using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static float AngleDir(Vector2 A, Vector2 B)
    {
        return -A.x * B.y + A.y * B.x;
    }

    public static bool IsALeftOfB(Vector3 a, Vector3 b)
    {
        if (a.x - b.x < 0) return true;

        return false;
    }
}
