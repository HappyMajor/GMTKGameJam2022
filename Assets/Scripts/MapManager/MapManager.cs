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


    public GameObject chestFieldPrefab;
    public GameObject monsterFieldPrefab;
    public GameObject goldFieldPrefab;
    public GameObject neutralFieldPrefab;

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
                } else if(x == 0 && y == 0)
                {
                    //Field the player spawns on
                    GameObject rollableField = Instantiate(neutralFieldPrefab);
                    rollableField.transform.position = new Vector3(x * 10, y * 10, 0);
                    NeutralField field = rollableField.GetComponent<NeutralField>();
                    field.ActivateField(diceRollResult: 0);

                } else
                {
                    //Spawn field
                    GameObject rollableField = Instantiate(ChooseRandomFieldType());
                    rollableField.transform.position = new Vector3(x * 10, y * 10, 0);
                }        
            }
        }
    }

    public GameObject ChooseRandomFieldType()
    {
        int randomNumber = UnityEngine.Random.Range(0, 100);
        GameObject fieldType = null;

        if (randomNumber < 20)
        {
            //20%
            fieldType = chestFieldPrefab;
        }
        else if (randomNumber >= 20 && randomNumber < 30)
        {
            //10%
            fieldType = monsterFieldPrefab;
        }
        else if (randomNumber >= 30 && randomNumber < 40)
        {
            //10%
            fieldType = goldFieldPrefab;
        }
        else if(randomNumber >= 40 && randomNumber < 100)
        {
            //60%
            fieldType = neutralFieldPrefab;
        } else
        {
            fieldType = neutralFieldPrefab;
        }

        return fieldType;
    }
}
