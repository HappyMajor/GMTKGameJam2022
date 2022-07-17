using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
public class RollableField : MonoBehaviour, IPointerClickHandler
{
    private FieldDatabase fieldDatabase;
    private Tilemap baseTilemap;
    private Tilemap middleTilemap;
    private Tilemap highTilemap;

    private void Start()
    {
        fieldDatabase = GameObject.Find("FieldDatabase").GetComponent<FieldDatabase>();
        baseTilemap = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
        middleTilemap = GameObject.Find("Tilemap_Middle").GetComponent<Tilemap>();
        highTilemap = GameObject.Find("Tilemap_High").GetComponent<Tilemap>();
    }
    public void Roll()
    {
        FieldData fieldData = fieldDatabase.fieldDataList[0];
        Vector3Int topLeftCornerCell = baseTilemap.WorldToCell(transform.position);

        int counter = 0;
        for(int x = 1; x <= 10; x++)
        {
            for(int y = 0; y < 10; y++)
            {
                if (counter >= fieldData.baseTiles.Count) break;

                Tile baseTile = fieldData.baseTiles[counter];
                Tile middleTile = fieldData.middleTiles[counter];
                Tile highTile = fieldData.highTiles[counter];

                if(baseTile != null)
                baseTilemap.SetTile(topLeftCornerCell + new Vector3Int(x - 6, y - 5, 0), baseTile);

                if(middleTile != null)
                middleTilemap.SetTile(topLeftCornerCell + new Vector3Int(x - 6, y - 5, 0), middleTile);

                if(highTile != null)
                highTilemap.SetTile(topLeftCornerCell + new Vector3Int(x - 6, y - 5, 0), highTile);

                counter++;
            }
        }

        GetComponent<SpriteRenderer>().sprite = null;
        Debug.Log("Roll Successful!");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Roll();
        Debug.Log("CLICK!");
    }
}
