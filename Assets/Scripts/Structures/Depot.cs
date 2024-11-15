using System.Collections.Generic;
using UnityEngine;

public class Depot : Structure
{
    [SerializeField] private List<Level> _levels = new();
    [SerializeField] private ProgressScript _progressScript;
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _victoryClip;
    [SerializeField] private AudioClip _levelUpClip;
    private List<int> _amount = new();
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
            bool failed = false;
            for (int i = 0; i<_Inventory._WhiteListItems.Count; i++)
            {
                _amount[i] += _Inventory.CountItem(_Inventory._WhiteListItems[i], InputOrOutput._InputSlots);
                foreach (ItemsWithQuantity ItemQuant in _levels[_level - 1]._Items)
                {
                    if (_Inventory._WhiteListItems[i] == ItemQuant._Item)
                    {
                        if (_amount[i] < ItemQuant._Quantity)
                        {
                            failed = true;
                            break;
                        }
                    }
                }
            }
            _progressScript.UpdateProgress(_levels[_level-1]._Items, _amount);
            _Inventory.EmptyInventory(InputOrOutput._InputSlots);
            if (!failed && _level < _levels.Count)
            {
                if(_level == _levels.Count - 1)
                {
                    _audioSource.PlayOneShot(_victoryClip);
                    _menuManager.Win();
                }
                _audioSource.PlayOneShot(_levelUpClip);
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
        _amount.Clear();
        _Inventory._WhiteListItems.Clear();
        foreach(ItemsWithQuantity item in level._Items)
        {
            _Inventory._WhiteListItems.Add(item._Item);
            _amount.Add(0);
        }
        _progressScript.UpdateDisplay(level, _amount);
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
