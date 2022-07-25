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
    private GameObject player;
    public GameObject highRiskIndicator;
    public GameObject highRewardIndicator;
    public GameObject highRewardHighRiskIndicator;
    public GameObject pentagram;
    public GameObject healthPotionPrefab;
    public GameObject speedPotionPrefab;
    public GameObject powerupPrefab;
    public GameObject goldPrefab;
    public GameObject dicePrefab;
    private int diceRollResult = 0;
    private bool isRolled = false;
    public GameObject skeletonPrefab;
    public FieldType CurrentFieldType { get; set; }

    private void Start()
    {
        fieldDatabase = GameObject.Find("FieldDatabase").GetComponent<FieldDatabase>();
        baseTilemap = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
        middleTilemap = GameObject.Find("Tilemap_Middle").GetComponent<Tilemap>();
        highTilemap = GameObject.Find("Tilemap_High").GetComponent<Tilemap>();
        player = GameObject.Find("Player");
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
            //spriteRenderer.color = Color.yellow;
            fieldType = FieldType.HIGH_RISK_HIGH_REWARD;
            highRewardHighRiskIndicator.SetActive(true);

        } else if (randomNumber >= 20 && randomNumber < 30)
        {
            //10%
            fieldType = FieldType.HIGH_RISK;
            highRiskIndicator.SetActive(true);
           // spriteRenderer.color = Color.red;
        } else if(randomNumber >= 30 && randomNumber < 40)
        {
            //10%
            fieldType = FieldType.HIGH_REWARD;
            highRewardIndicator.SetActive(true);
           // spriteRenderer.color = Color.green;
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
        bool superSkeletons = false;

        if (CurrentFieldType == FieldType.NEUTRAL)
        {
            minCount = 0;
            maxCount = diceRollResult * 2;
        }
        if (CurrentFieldType == FieldType.HIGH_RISK)
        {
           minCount = diceRollResult;
           maxCount = diceRollResult * 5;
           superSkeletons = true;
        }
        if (CurrentFieldType == FieldType.HIGH_REWARD)
        {
            SpawnRandomUpgrades(diceRollResult);
            SpawnMoney(diceRollResult * 4);
        } else if(CurrentFieldType == FieldType.HIGH_RISK_HIGH_REWARD)
        {
            SpawnRandomUpgrades(diceRollResult);
            minCount = diceRollResult;
            maxCount = diceRollResult * 5;
        }
        int skeletonCount = UnityEngine.Random.Range(minCount, maxCount);
        for(int i = 0; i < skeletonCount; i++)
        {
            if (superSkeletons)
            {
                Skeleton skeleton = Instantiate(skeletonPrefab, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<Skeleton>();
                Skeleton.ConfigureSuperSkeleton(skeleton: skeleton);
            } 
            else
            {
                Instantiate(skeletonPrefab, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
       
    }

    public void SpawnSkeletons (int amount, bool superSkeletons)
    {
        for (int i = 0; i < amount; i++)
        {
            if (superSkeletons)
            {
                Skeleton skeleton = Instantiate(skeletonPrefab, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<Skeleton>();
                Skeleton.ConfigureSuperSkeleton(skeleton: skeleton);
            }
            else
            {
                Instantiate(skeletonPrefab, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
    }

    public void SpawnMoney(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(goldPrefab, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    public void SpawnRandomUpgrades(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject potionToSpawn = null;
            int randomPotion = UnityEngine.Random.Range(0, 3);

            if(randomPotion == 0)
            {
                potionToSpawn = powerupPrefab;
            } else
            if (randomPotion == 1)
            {
                potionToSpawn = speedPotionPrefab;
            } else
            {
                potionToSpawn = healthPotionPrefab;
            }

            Instantiate(potionToSpawn, GetRandomPositionInField(), Quaternion.Euler(new Vector3(0, 0, 0)));
        }
    }

    public Vector3 GetRandomPositionInField()
    {
        float x = UnityEngine.Random.Range(-5f, 5f);
        float y = UnityEngine.Random.Range(-5f, 5f);

        return transform.position + new Vector3(x, y, 0);
    }

    public void Roll(int diceResult)
    {
        fieldDatabase = GameObject.Find("FieldDatabase").GetComponent<FieldDatabase>();
        baseTilemap = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
        middleTilemap = GameObject.Find("Tilemap_Middle").GetComponent<Tilemap>();
        highTilemap = GameObject.Find("Tilemap_High").GetComponent<Tilemap>();
        player = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        diceRollResult = diceResult;
        int randomField = UnityEngine.Random.Range(0, fieldDatabase.fieldDataList.Count);
        FieldData fieldData = fieldDatabase.fieldDataList[randomField];
        
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

        // Mark that we've rolled for this field
        isRolled = true;

        // Add enemies to the field
        SpawnEnemies();

        // Deactivate the pentagram
        pentagram.SetActive(false);

        // Start tracking collision triggers for the field
        GetComponent<Collider2D>().isTrigger = true;
    }

    public void ThrowDice()
    {
        // Remember that we've thrown the dice
        isRolled = true;

        // Spawn a nice die
        Dice dice = Instantiate(dicePrefab, player.transform.position, Quaternion.Euler(new Vector3(0,0,0))).GetComponent<Dice>();

        // Throw die at the field
        dice.ThrowAtTarget(transform.position, (int result) =>
        {
            // Roll the die
            Roll(result);
        });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Already rolled
        if(isRolled) {
            return;
        }

        // The the dice at the field
        ThrowDice();
    }

    public enum FieldType
    {
        HIGH_RISK_HIGH_REWARD, NEUTRAL, HIGH_RISK, HIGH_REWARD
    }
}
