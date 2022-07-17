using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

#if UNITY_EDITOR
public class FieldEditorManager : MonoBehaviour
{
    public static Tilemap baseTilemap;
    public static Tilemap middleTilemap;
    public static Tilemap highTilemap;
        
    [MenuItem("Assets/Create/SAVE NEW FIELD")]
    public static void CreateMyAsset()
    {
        FieldData asset = ScriptableObject.CreateInstance<FieldData>();

        asset.baseTiles = new List<Tile>();
        asset.middleTiles = new List<Tile>();
        asset.highTiles = new List<Tile>();

        baseTilemap = GameObject.Find("Tilemap_Base").GetComponent<Tilemap>();
        middleTilemap = GameObject.Find("Tilemap_Middle").GetComponent<Tilemap>();
        highTilemap = GameObject.Find("Tilemap_High").GetComponent<Tilemap>();


        for (int x = 1; x <= 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Tile tile = (Tile)baseTilemap.GetTile(baseTilemap.origin + new Vector3Int(x,y, 0));
                asset.baseTiles.Add(tile);
            }
        }

        for (int x = 1; x <= 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Tile tile = (Tile)middleTilemap.GetTile(middleTilemap.origin + new Vector3Int(x, y, 0));
                asset.middleTiles.Add(tile);
            }
        }

        for (int x = 1; x <= 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Tile tile = (Tile)highTilemap.GetTile(highTilemap.origin + new Vector3Int(x, y, 0));
                asset.highTiles.Add(tile);
            }
        }

        if (AssetDatabase.FindAssets("FieldData", new string[] { "Assets/Data/Fields/" }).Length > 0)
        {
            for(int i = 0; i < 1000; i++)
            {
                if (AssetDatabase.FindAssets("FieldData" + i, new string[] { "Assets/Data/Fields/" }).Length == 0)
                {
                    AssetDatabase.CreateAsset(asset, "Assets/Data/Fields/FieldData" + i + ".asset");
                    break;
                }
            }
        } else
        {
            AssetDatabase.CreateAsset(asset, "Assets/Data/Fields/FieldData.asset");
        }
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }

}
#endif