using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    public Inventory _Inventory;
    public StructureItem _Item;
    [SerializeField] private float _maxOutputCooldown = 10;
    private float _cooldown;

    public virtual void Process() { }
    public virtual void Init() { }

    protected virtual void Update()
    {
        _cooldown -= Time.deltaTime;
        if (CallOutput())
        {
            _cooldown = _maxOutputCooldown;
        }
    }

    protected abstract bool CallOutput();
}
