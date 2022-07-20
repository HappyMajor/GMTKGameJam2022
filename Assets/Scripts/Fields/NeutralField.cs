using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralField : Field
{
    public GameObject skeletonPrefab;
    public override void Reveal(int diceRollResult)
    {
        for (int i = 0; i < diceRollResult; i++)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.position, 10, 10);
            Instantiate(skeletonPrefab, randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }
}
