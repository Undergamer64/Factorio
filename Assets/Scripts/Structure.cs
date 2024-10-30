using UnityEngine;

public class Structure : MonoBehaviour
{
    public Inventory _Inventory;
    public StructureItem _Item;

    public virtual void Process() { }
    public virtual void Init() { }
}
