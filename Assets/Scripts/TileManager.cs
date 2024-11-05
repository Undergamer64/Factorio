using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager _Instance {get; private set;}

    [SerializeField]
    private Tilemap _defaultTileMap;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// contains the methods needed to know if you can place a gameobject
    /// to place it, or round your position to the center of a tile.
    /// </summary>
    /// <param name="TileMap">the Tilemap where you want to create the object</param>
    /// <param name="StructPrefab">the object you want to create</param>
    /// <param name="WorldPosition"> the position you are in the world</param>
    /// <param name="Rotation">the rotation of the object you want to create</param>
    public void Place(Tilemap TileMap, GameObject StructPrefab, int SizeX, int SizeY, Vector3 WorldPosition, Quaternion Rotation = new Quaternion())
    {
        //create a gameobject at the center of the cell it's in
        Vector3 offset = new Vector3(SizeX / 2f, SizeY / 2f, 0);
        Instantiate(StructPrefab, RoundToCell(TileMap, WorldPosition) + offset, Rotation, TileMap.transform);
    }

    public void Place(GameObject StructPrefab, int SizeX, int SizeY, Vector3 WorldPosition, Quaternion Rotation = new Quaternion())
    {
        if (_defaultTileMap != null)
        {
            Vector3 offset = new Vector3(SizeX / 2f, SizeY / 2f, 0);
            Instantiate(StructPrefab, RoundToCell(_defaultTileMap, WorldPosition) + offset, Rotation, _defaultTileMap.transform);
        }
    }

    public Vector3 RoundToCell(Tilemap TileMap,Vector3 WorldPosition)
    {
        //return the world position of the cell the sent position is in
        return (TileMap.CellToWorld(TileMap.WorldToCell(WorldPosition)));
    }

    public bool CanPlace(Vector2 WorldPosition, int SizeX, int SizeY)
    {
        //return true if you can place a GameObject at the desired location
        Vector2 position = new Vector2(RoundToCell(_defaultTileMap, WorldPosition).x + (SizeX / 2f), RoundToCell(_defaultTileMap, WorldPosition).y + (SizeY / 2f));

        List<Collider2D> colliders = Physics2D.OverlapBoxAll(position, new Vector2(SizeX - 0.5f, SizeY - 0.5f), 0).ToList();

        if (colliders.Where(x => x.GetComponent<CharacterController>() != null).Count() > 0) { return false; }

        return (colliders.Where(x => x.GetComponentInParent<Structure>() != null).Count() == 0);
    }
}
