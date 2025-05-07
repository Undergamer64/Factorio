using System.Collections.Generic;
using System.Linq;
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
            List<ItemBase> items = new List<ItemBase>();

            items = _levels[_level - 1]._Items.Select(x => x._Item).ToList();
            
            bool failed = false;

            foreach (Slot slot in _Input._Slots)
            {
                if (items.Contains(slot.Item))
                {
                    _amount[items.IndexOf(slot.Item)] += slot.Quantity;
                }
            }
            
            _progressScript.UpdateProgress(_levels[_level-1]._Items, _amount);
            _Input.EmptyInventory();
            if (CheckProgress() && _level <= _levels.Count)
            {
                if(_level == _levels.Count)
                {
                    //_audioSource.PlayOneShot(_victoryClip);
                    _menuManager.Win();
                }
                else
                {
                    //_audioSource.PlayOneShot(_levelUpClip);
                }
                SetObjective(_levels[_level]);
            }

        }
        
    }

    private bool CheckProgress()
    {
        for (int i = 0; i < _levels[_level-1]._Items.Count; i++)
        {
            if (_amount[i] < _levels[_level - 1]._Items[i]._Quantity)
            {
                return false;
            }
        }
        return true;
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
        /*foreach(ItemsWithQuantity item in level._Items)
        {
            _Input._WhiteListItems.Add(item._Item);
            _amount.Add(0);
        }*/
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
