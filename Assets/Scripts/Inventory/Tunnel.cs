using UnityEngine;

public abstract class Tunnel : MonoBehaviour
{
    public Transform _Transform;
    public Inventory _ParentInventory;

    private void Awake()
    {
        _Transform = transform;
    }
}
