using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    public Inventory _Inventory;
    public StructureItem _Item;
    [SerializeField] protected float _maxOutputCooldown = 10;
    protected float _cooldown;

    public virtual void Process() { }
    public virtual void Init() 
    {
        Process();
    }

    protected virtual void Update()
    {
        if (!_Inventory.IsInventoryEmpty(InputOrOutput._OutputSlots))
        {
            _cooldown -= Time.deltaTime;
        }
        if (_cooldown <= 0)
        {
            if (CallOutput())
            {
                _cooldown = _maxOutputCooldown;
            }
        }
    }

    protected abstract bool CallOutput();
}
