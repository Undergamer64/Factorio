using System.Collections.Generic;
using UnityEngine;

public class Depot : Structure
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private ProgressScript _progressScript;
    private List<int> _amount;
    private int _level;
    private bool _isTrashCan;

    protected override bool CallOutput()
    {
        return true;
    }

    public override void Process()
    {
        if (_isTrashCan)
        {
            _Inventory.EmptyInventory(InputOrOutput._InputSlots);
        }
        else
        {
            foreach (ItemBase item in _Inventory._WhiteListItems)
            {
                foreach (ItemsWithQuantity ItemQuant in _levels[_level - 1]._Items)
                {
                    _amount.Add(_Inventory.CountItem(item, InputOrOutput._InputSlots));
                    if (item == ItemQuant._Item)
                    {
                        if (_Inventory.CountItem(item, InputOrOutput._InputSlots) < ItemQuant._Quantity)
                        {
                            return;
                        }
                    }
                }
            }
            if (_level < _levels.Count - 1)
            {
                SetObjective(_levels[_level]);
                return;
            }
        }
    }

    //count on input and delete object

    private void Start()
    {
        if (_progressScript == null)
        {
            _isTrashCan = true;
        }
        else
        {
            SetObjective(_levels[0]);
        }
    }

    private void SetObjective(Level level)
    {
        foreach(ItemsWithQuantity item in level._Items)
        {
            _Inventory._WhiteListItems.Add(item._Item);
        }
        _progressScript.UpdateDisplay(level, 0);
        _Inventory.EmptyInventory(InputOrOutput._InputSlots);
        _level = level._Level;
        
        UpdateSprite();
    }

    public override void UpdateSprite()
    {
        if (!_isTrashCan)
        {
                SetSprite(_Inventory._WhiteListItems[0].Sprite);
        }
    }
}
