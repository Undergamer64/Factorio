using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    /// <summary>
    /// contains the methods needed to know if you can place a gameobject
    /// to place it, or round your position to the center of a tile.
    /// </summary>
    /// <param name="TileMap">the Tilemap where you want to create the object</param>
    /// <param name="StructPrefab">the object you want to create</param>
    /// <param name="WorldPosition"> the position you are in the world</param>
    /// <param name="Rotation">the rotation of the object you want to create</param>
    
    public void Place(Tilemap TileMap,GameObject StructPrefab, Vector3 WorldPosition,Quaternion Rotation)
    {
        //create a gameobject at the center of the cell it's in
        Instantiate(StructPrefab, RoundToCell(TileMap,WorldPosition),Rotation,TileMap.transform);
    }

    public Vector3 RoundToCell(Tilemap TileMap,Vector3 WorldPosition)
    {
        //return the world position of the cell the sent position is in
        return (TileMap.CellToWorld(TileMap.WorldToCell(WorldPosition)));
    }

    public bool CanPlace(Tilemap TileMap, Vector2 WorldPosition, int SizeX, int SizeY)
    {
        //return true if you can place a gameobject at the desired location
        Vector2 offset = new Vector2(WorldPosition.x + SizeX/2-0.5f, WorldPosition.y + SizeY / 2 - 0.5f);
        return (Physics2D.OverlapBoxAll(offset, new Vector2(SizeX, SizeY), 0) == null);
    }
}
