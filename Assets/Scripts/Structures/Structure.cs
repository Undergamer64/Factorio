using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    public Inventory _Inventory;
    public StructureItem _Item;
    [SerializeField] protected float _maxOutputCooldown = 10;
    [SerializeField] private SpriteRenderer _resourceRenderer;
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

    public abstract void UpdateSprite();

    protected virtual void SetSprite(Sprite resourceSprite)
    {
        _resourceRenderer.sprite = resourceSprite;
    }
}
