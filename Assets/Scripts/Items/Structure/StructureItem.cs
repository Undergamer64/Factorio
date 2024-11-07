using UnityEngine;

[CreateAssetMenu(fileName = "Structure", menuName = "ScriptableObjects/Item/Structure", order = 1)]
public class StructureItem : ItemBase
{
    [SerializeField]
    private GameObject _structure;

    public int _SizeX = 1;

    public int _SizeY = 1;

    public GameObject Structure {  get { return _structure; } }
}
