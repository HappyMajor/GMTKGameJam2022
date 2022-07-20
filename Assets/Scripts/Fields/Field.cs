using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public abstract class Field : MonoBehaviour, IPointerClickHandler
{
    public GameObject dicePrefab;
    [Header("Fade Parent: All children with a sprite renderer will get faded on dice roll")]
    public GameObject fadeParent;
    private bool isRolled = false;
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    public abstract void Reveal(int diceRollResult);

    private void Start()
    {
        player = GameObject.Find("Player");
    }
    public void ThrowDice()
    {
        isRolled = true;
        Dice dice = Instantiate(dicePrefab, player.transform.position + new Vector3(0,0,-1), Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<Dice>();
        dice.ThrowAtTarget(transform.position, (int diceRollResult) =>
        {
            ActivateField(diceRollResult);
        });
    }

    public void ActivateField(int diceRollResult)
    {
        GenerateTileFieldSquare();
        Fade();
        MakeFieldPassable();
        Reveal(diceRollResult);
    }


    private void Fade()
    {
        SpriteRenderer[] spritesToFade = fadeParent.transform.GetComponentsInChildren<SpriteRenderer>();
        Coroutine fade = StartCoroutine(Routines.DoEverySecondsAndEndAfter(interval: 0.1f, end: 2f, action: () =>
        {
            foreach(SpriteRenderer sprite in spritesToFade)
            {
                sprite.color = Color.Lerp(sprite.color, new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0), 0.2f);
            }
        }));
    }

    private void MakeFieldPassable()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void GenerateTileFieldSquare()
    {
        FieldDatabase fieldDatabase = GameObject.Find("FieldDatabase").GetComponent<FieldDatabase>();
        Tilemap baseTilemap = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
        Tilemap middleTilemap = GameObject.Find("Tilemap_Middle").GetComponent<Tilemap>();
        Tilemap highTilemap = GameObject.Find("Tilemap_High").GetComponent<Tilemap>();

        int randomField = UnityEngine.Random.Range(0, fieldDatabase.fieldDataList.Count);
        FieldData fieldData = fieldDatabase.fieldDataList[randomField];

        Vector3Int topLeftCornerCell = baseTilemap.WorldToCell(transform.position);

        int counter = 0;
        for (int x = 1; x <= 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                if (counter >= fieldData.baseTiles.Count) break;

                Tile baseTile = fieldData.baseTiles[counter];
                Tile middleTile = fieldData.middleTiles[counter];
                Tile highTile = fieldData.highTiles[counter];

                if (baseTile != null)
                    baseTilemap.SetTile(topLeftCornerCell + new Vector3Int(x - 6, y - 5, 0), baseTile);

                if (middleTile != null)
                    middleTilemap.SetTile(topLeftCornerCell + new Vector3Int(x - 6, y - 5, 0), middleTile);

                if (highTile != null)
                    highTilemap.SetTile(topLeftCornerCell + new Vector3Int(x - 6, y - 5, 0), highTile);

                counter++;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isRolled)
        {
            ThrowDice();
        }
    }
}
