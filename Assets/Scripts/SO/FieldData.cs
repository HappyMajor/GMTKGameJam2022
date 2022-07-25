using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Data", menuName = "FieldData/Create FieldData", order = 1)]
public class FieldData : ScriptableObject
{
    public List<Tile> baseTiles;
    public List<Tile> middleTiles;
    public List<Tile> highTiles;
    public string fieldName;
}
