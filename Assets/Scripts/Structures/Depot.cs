using UnityEngine;
using System.Collections.Generic;

public class Depot : Structure
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private ProgressScript _progressScript;
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
            _amount = _Inventory.CountItem(item, InputOrOutput._InputSlots);
            _progressScript.UpdateProgress(_objectiveAmount, _amount);
            if(_amount >= _objectiveAmount)
            {
                Debug.Log("You won");
                if (_level < _levels.Count-1) 
                {
                    SetObjective(_levels[_level + 1]);
                }
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
        _Inventory.EmptyInventory(InputOrOutput._InputSlots);
        _level = level._Level;
        _objectiveAmount = level._Amount;
        _amount = 0;
        _progressScript.UpdateDisplay(level, _amount);
    }

    public override void UpdateSprite()
    {
    }
}
