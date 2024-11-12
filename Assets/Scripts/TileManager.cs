using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager _Instance {get; private set;}

    [SerializeField]
    private Tilemap _defaultTileMap;

    public Vector3 _TileOffset { get; private set; } = new Vector3(0.5f,0.5f,0);

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
    public GameObject Place(Tilemap TileMap, GameObject StructPrefab, int SizeX, int SizeY, Vector3 WorldPosition, Quaternion Rotation = new Quaternion())
    {
        //create a gameobject at the center of the cell it's in
        Vector3 sizeOffset = new Vector3(SizeX / 2f - 0.5f, SizeY / 2f - 0.5f, 0);

        sizeOffset = new Vector3(
            sizeOffset.x * Mathf.Cos(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)) - sizeOffset.y * Mathf.Sin(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)),
            sizeOffset.x * Mathf.Sin(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)) + sizeOffset.y * Mathf.Cos(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)),
            0
        );

        return Instantiate(StructPrefab, RoundToCell(TileMap, WorldPosition) + sizeOffset + _TileOffset, Rotation, TileMap.transform);
    }

    public GameObject Place(GameObject StructPrefab, int SizeX, int SizeY, Vector3 WorldPosition, Quaternion Rotation = new Quaternion())
    {
        if (_defaultTileMap != null)
        {
            return Place(_defaultTileMap, StructPrefab, SizeX, SizeY, WorldPosition, Rotation);
        }
        return null;
    }

    public Vector3 RoundToCell(Tilemap TileMap,Vector3 WorldPosition)
    {
        //return the world position of the cell the sent position is in
        return (TileMap.CellToWorld(TileMap.WorldToCell(WorldPosition)));
    }

    public Vector3 RoundToCell(Vector3 WorldPosition)
    {
        //return the world position of the cell the sent position is in
        return RoundToCell(_defaultTileMap, WorldPosition);
    }

    public bool CanPlace(Vector3 WorldPosition, int SizeX, int SizeY, Quaternion Rotation = new Quaternion())
    {
        //return true if you can place a GameObject at the desired location
        Vector3 sizeOffset = new Vector3(SizeX / 2f - 0.5f, SizeY / 2f - 0.5f, 0);

        sizeOffset = new Vector3(
            sizeOffset.x * Mathf.Cos(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)) - sizeOffset.y * Mathf.Sin(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)),
            sizeOffset.x * Mathf.Sin(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)) + sizeOffset.y * Mathf.Cos(-Rotation.eulerAngles.z * (2 * Mathf.PI / 360f)),
            0
        );

        Vector3 position = RoundToCell(WorldPosition) + sizeOffset + _TileOffset;

        List<Collider2D> colliders = Physics2D.OverlapBoxAll(position, new Vector2(SizeX-0.1f, SizeY-1f), 0).ToList();

        if (colliders.Where(x => x.GetComponent<CharacterController>() != null).Count() > 0) { return false; }

        return (colliders.Where(x => x.GetComponentInParent<Structure>() != null && x.transform.parent.parent.GetComponent<Tilemap>() == _defaultTileMap).Count() == 0);
    }
}
