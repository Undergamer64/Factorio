using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 1)]
public class ItemBase : ScriptableObject
{
    [SerializeField]
    protected string _name;

    [SerializeField]
    protected int _maxStack = 64;

    [SerializeField]
    protected Sprite _sprite;

    public string Name { get { return _name; } }

    public int MaxStack { get { return _maxStack; } }
    
    public Sprite Sprite { get { return _sprite; } }
}
