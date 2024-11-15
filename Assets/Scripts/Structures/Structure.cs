using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    public Inventory _Inventory;
    public StructureItem _Item;
    [SerializeField] protected float _maxOutputCooldown = 10;
    [SerializeField] private SpriteRenderer _resourceRenderer;
    [SerializeField] public GameObject _Visuals;
    protected float _cooldown;

    public virtual void Process() { }
    public virtual void Init() 
    {
        _cooldown = _maxOutputCooldown;
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

    public abstract void UpdateSprite();

    protected virtual void SetSprite(Sprite resourceSprite)
    {
        _resourceRenderer.sprite = resourceSprite;
    }
}
