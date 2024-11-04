using UnityEngine;
using System.Collections.Generic;

public class Depot : Structure
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    private int _objectiveAmount;
    private int _amount;
    private int _level;

    protected override bool CallOutput()
    {
        return true;
    }

    public override void Process()
    {
        foreach (ItemBase item in _Inventory._WhiteListItems)
        {
            _amount = _Inventory.CountItem(item);
            if(_amount >= _objectiveAmount)
            {
                SetObjective(_levels[_level + 1]);
            }
        }
    }

    //count on input and delete object

    private void Start()
    {
        SetObjective(_levels[0]);
    }

    private void SetObjective(Level level)
    {
        _Inventory.UpdateWhiteList(level._Items);
        _Inventory.EmptyInventory();
        _level = level._Level;
        _objectiveAmount = level._Amount;
        _amount = 0;
    }
}
