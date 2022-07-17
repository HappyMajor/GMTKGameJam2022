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
                GameObject rollableField = Instantiate(rollableFieldPrefab);
                rollableField.transform.position = new Vector3(x * 10, y * 10, 0);
            }
        }
    }
}
