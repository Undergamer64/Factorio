using UnityEngine;

public abstract class Structure : MonoBehaviour
{
    //public Inventory _Inventory;
    public Input _Input;
    public Output _Output;
    public StructureItem _Item;
    [SerializeField] protected float _maxOutputCooldown = 10;
    [SerializeField] private SpriteRenderer _resourceRenderer;
    [SerializeField] public GameObject _Visuals;
    protected float _cooldown;

    public virtual void Process() { }
    public virtual void Init() 
    {
        if (_Input != null)
        {
            _Input.RefreshSprite = UpdateSprite;
            _Input.StartProcess = Process;
        }
        _cooldown = _maxOutputCooldown;
        Process();
    }

    protected virtual void Update()
    {
        if (_Output == null) return;
        if (!_Output.IsInventoryEmpty())
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
