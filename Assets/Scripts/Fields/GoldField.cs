using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldField : Field
{
    public GameObject skeletonPrefab;
    public GameObject speedPotionPrefab;
    public GameObject healthPotionPrefab;
    public GameObject powerupPrefab;
    public GameObject goldPrefab;
    public override void Reveal(int diceRollResult)
    {
        for(int i = 0; i < diceRollResult * 4; i++)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.position, 10, 10);
            Instantiate(goldPrefab, randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }

        if(diceRollResult == 6)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.position, 10, 10);
            Instantiate(powerupPrefab, randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }

        if(diceRollResult > 3)
        {
            Vector3 randomPosition = Util.GetRandomPositionOfRectangle(transform.position, 10, 10);
            Instantiate(healthPotionPrefab, randomPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        }

    }
}
