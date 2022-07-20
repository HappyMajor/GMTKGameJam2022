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

    public static Vector3 GetRandomPositionOfRectangle(Vector3 center, float width, float height)
    {
        float x = UnityEngine.Random.Range(-width/2, width/2);
        float y = UnityEngine.Random.Range(-height/2, height/2);

        return center + new Vector3(x, y, 0);
    }
}
