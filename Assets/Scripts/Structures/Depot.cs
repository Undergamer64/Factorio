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
            _Input.EmptyInventory();
        }
        else
        {
            bool failed = false;
            for (int i = 0; i<_Input._WhiteListItems.Count; i++)
            {
                _amount[i] += _Input.CountItem(_Input._WhiteListItems[i]);
                foreach (ItemsWithQuantity ItemQuant in _levels[_level - 1]._Items)
                {
                    if (_Input._WhiteListItems[i] == ItemQuant._Item)
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
            _Input.EmptyInventory();
            if (!failed && _level <= _levels.Count)
            {
                if(_level == _levels.Count)
                {
                    _audioSource.PlayOneShot(_victoryClip);
                    _menuManager.Win();
                }
                else
                {
                    _audioSource.PlayOneShot(_levelUpClip);
                }
                SetObjective(_levels[_level]);
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
        _Input._WhiteListItems.Clear();
        foreach(ItemsWithQuantity item in level._Items)
        {
            _Input._WhiteListItems.Add(item._Item);
            _amount.Add(0);
        }
        _progressScript.UpdateDisplay(level, _amount);
        _Input.EmptyInventory();
        _level = level._Level;
        
        UpdateSprite();
    }

    public override void UpdateSprite()
    {
        if (!_isTrashCan)
        {
            SetSprite(_Input._WhiteListItems[0].Sprite);
        }
    }
}
