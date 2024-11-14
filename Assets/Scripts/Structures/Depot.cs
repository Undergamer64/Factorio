using UnityEngine;
using System.Collections.Generic;

public class Depot : Structure
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private ProgressScript _progressScript;
    private int _objectiveAmount;
    private int _amount;
    private int _level;
    private bool _isTrashCan;

    protected override bool CallOutput()
    {
        return true;
    }

    public override void Process()
    {
        if(_isTrashCan)
        {
            _Inventory.EmptyInventory(InputOrOutput._InputSlots);
        }
        else
        {
            foreach (ItemBase item in _Inventory._WhiteListItems)
            {
                _amount = _Inventory.CountItem(item, InputOrOutput._InputSlots);
                _progressScript.UpdateProgress(_objectiveAmount, _amount);
                if (_amount >= _objectiveAmount)
                {
                    if (_level < _levels.Count - 1)
                    {
                        SetObjective(_levels[_level]);
                        return;
                    }
                }
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
        _Inventory.UpdateWhiteList(level._Items);
        _Inventory.EmptyInventory(InputOrOutput._InputSlots);
        _level = level._Level;
        _objectiveAmount = level._Amount;
        _amount = 0;
        _progressScript.UpdateDisplay(level, _amount);
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
