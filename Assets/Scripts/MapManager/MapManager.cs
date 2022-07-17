using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int mapWidth = 10;
    public int mapHeight = 10;
    public GameObject rollableFieldPrefab;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {

        for(int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                if(x == 0 && y == 0)
                {
                    GameObject rollableField = Instantiate(rollableFieldPrefab);
                    rollableField.transform.position = new Vector3(x * 10, y * 10, 0);
                    RollableField field = rollableField.GetComponent<RollableField>();
                    field.Roll(diceResult: 1);

                } else
                {
                    GameObject rollableField = Instantiate(rollableFieldPrefab);
                    rollableField.transform.position = new Vector3(x * 10, y * 10, 0);
                    RollableField field = rollableField.GetComponent<RollableField>();

                    if (x % 2 == 0)
                    {
                        if (y % 2 == 0)
                        {
                            field.AssignDarkColor();
                        }
                        else
                        {
                            field.AssignLightColor();
                        }
                    }
                    else
                    {
                        if (y % 2 != 0)
                        {
                            field.AssignDarkColor();
                        }
                        else
                        {
                            field.AssignLightColor();
                        }
                    }
                }

               
            }
        }
    }
}
