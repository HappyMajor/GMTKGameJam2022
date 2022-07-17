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
    private SpriteRenderer spriteRenderer;

    private int diceRollResult = 0;

    private bool isRolled = false;

    public GameObject skeletonPrefab;

    public FieldType CurrentFieldType {get; set; }

    private void Start()
    {
        fieldDatabase = GameObject.Find("FieldDatabase").GetComponent<FieldDatabase>();
        baseTilemap = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
        middleTilemap = GameObject.Find("Tilemap_Middle").GetComponent<Tilemap>();
        highTilemap = GameObject.Find("Tilemap_High").GetComponent<Tilemap>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomFieldType();
    }

    public void SetRandomFieldType()
    {
        int randomNumber = UnityEngine.Random.Range(0, 100);
        FieldType fieldType = FieldType.NEUTRAL;

        if(randomNumber < 20)
        {
            //20%
            spriteRenderer.color = Color.yellow;
            fieldType = FieldType.HIGH_RISK_HIGH_REWARD;
        } else if (randomNumber >= 20 && randomNumber < 30)
        {
            //10%
            fieldType = FieldType.HIGH_RISK;
            spriteRenderer.color = Color.red;
        } else if(randomNumber >= 30 && randomNumber < 40)
        {
            //10%
            fieldType = FieldType.HIGH_REWARD;
            spriteRenderer.color = Color.green;
        } else if(randomNumber >= 40 && randomNumber < 100) 
        {
            //60%
            fieldType = FieldType.NEUTRAL;
        }

        CurrentFieldType = fieldType;
    }

    public void SpawnEnemies()
    {
        int minCount = 0;
        int maxCount = 3;

        if (CurrentFieldType == FieldType.NEUTRAL)
        {
            minCount = 0;
            maxCount = diceRollResult * 2;
        }
        if (CurrentFieldType == FieldType.HIGH_RISK)
        {
           minCount = diceRollResult;
           maxCount = diceRollResult * 5;
        }
        if (CurrentFieldType == FieldType.HIGH_REWARD)
        {
            minCount = 0;
            maxCount = 6 - diceRollResult;
        }
        int skeletonCount = UnityEngine.Random.Range(minCount, maxCount);
        for(int i = 0; i < skeletonCount; i++)
        {
           Instantiate(skeletonPrefab, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0,0,0)));
        }
       
    }

    public void SpawnUpgrade()
    {

    }

    public Vector3 GetRandomPositionInField()
    {
        float x = UnityEngine.Random.Range(-5f, 5f);
        float y = UnityEngine.Random.Range(-5f, 5f);

        return transform.position + new Vector3(x, y, 0);
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

        diceRollResult = UnityEngine.Random.Range(1, 6);
        isRolled = true;
        SpawnEnemies();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isRolled) 
        Roll();
    }

    public enum FieldType
    {
        HIGH_RISK_HIGH_REWARD, NEUTRAL, HIGH_RISK, HIGH_REWARD
    }
}
