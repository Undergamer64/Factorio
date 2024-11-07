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
        _cooldown -= Time.deltaTime;
        if (CallOutput() && _cooldown <= 0)
        {
            _cooldown = _maxOutputCooldown;
        }
    }

    protected abstract bool CallOutput();
}
