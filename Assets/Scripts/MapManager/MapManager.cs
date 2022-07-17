using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public int mapWidth = 5;
    public int mapHeight = 5;
    public GameObject rollableFieldPrefab;
    public GameObject exitPrefab;
    public GameObject invisibleWallPrefab;

    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        //GENERATE WALL LEFT SIDE

        for(int y = 0; y < mapHeight; y++)
        {
            GameObject wall = Instantiate(invisibleWallPrefab);
            wall.transform.position = new Vector3(-1 * 10, y * 10, 0);
        }

        //Generate wall right side

        for (int y = 0; y < mapHeight; y++)
        {
            GameObject wall = Instantiate(invisibleWallPrefab);
            wall.transform.position = new Vector3(mapWidth * 10, y * 10, 0);
        }

        //Generate wall bottom

        for (int x = 0; x < mapWidth; x++)
        {
            GameObject wall = Instantiate(invisibleWallPrefab);
            wall.transform.position = new Vector3(x * 10, mapHeight * 10, 0);
        }

        //Generate wall top

        for (int x = 0; x < mapWidth; x++)
        {
            GameObject wall = Instantiate(invisibleWallPrefab);
            wall.transform.position = new Vector3(x * 10, -1 * 10, 0);
        }

        //Generate board

        for (int x = 0; x < mapWidth; x++)
        {
            for(int y = 0; y < mapHeight; y++)
            {
                if(y == mapHeight - 1 && x == mapWidth - 1)
                {
                    //TOP RIGHT CORNER OF MAP EXIT HERE BABY
                    GameObject rollableField = Instantiate(exitPrefab);
                    rollableField.transform.position = new Vector3(x * 10, y * 10, 0);
                    RollableField field = rollableField.GetComponent<RollableField>();
                    field.Roll(diceResult: 1);
                } else if(x == 0 && y == 0)
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
