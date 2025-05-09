using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Factory : Structure
{
    public float _CraftCooldown { get; protected set; } = 0;
    [FormerlySerializedAs("_Recipe")] public Recipe _CurrentRecipe;
    private Recipe _nextRecipe;
    [SerializeField] private List<Recipe> _recipes;

    public bool _IsCrafting { get; private set; } = false;
    private int _failedCraftIndex = -1;
    private int _failedCraftQuantity = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        if (_CurrentRecipe == null)
        {
            return;
        }
    }


    private void CraftUpdate()
    {
        if (!_IsCrafting)
        {
            TryToStartCraft();
        }

        if (_IsCrafting)
        {
            _CraftCooldown -= Time.deltaTime;
        }
        if (_CraftCooldown <= 0)
        {
            if (_failedCraftIndex < 0)
            {
                if (_CurrentRecipe == null)
                {
                    return;
                }
                foreach (var item in _CurrentRecipe._InputItem)
                {
                    if (!(_Input.CountItem(item._Item) >= item._Quantity))
                    {
                        _IsCrafting = false;
                        return;
                    }
                }
                RemoveCraftInput();
            }
            TryAddCraftOutput();
        }
    }

    protected override bool CallOutput()
    {
        if (_CurrentRecipe == null)
        {
            return false;
            
        }
        if (!_Output.IsInventoryEmpty())
        {
            foreach (Slot slot in _Output._Slots)
            {
                if (slot.Item == null || slot.Quantity == 0) continue;

                if (_Output.PullOutInventory(_CurrentRecipe))
                {
                    return true;
                }
            }
        }
        return false;
    }

    protected override void Update()
    {
        CraftUpdate();
        base.Update();
    }

    public override void Process()
    {
        TryToStartCraft();
    }

    private void RemoveCraftInput()
    {
        if (_CurrentRecipe != null)
        {
            foreach (var InputItem in _CurrentRecipe._InputItem)
            {
                _Input.TryRemoveItems(InputItem._Item, InputItem._Quantity);
            }
        }
    }


    public void TryAddCraftOutput()
    {
        if (_CurrentRecipe != null)
        {
            int beginCraftIndex = _failedCraftIndex;
            if (_failedCraftIndex < 0)
            {
                beginCraftIndex = 0;
            }
            for (int i = beginCraftIndex; i < _CurrentRecipe._OutputItem.Count; i++)
            {
                int remainingQuantity = 0;
                if (i == _failedCraftIndex)
                {
                    remainingQuantity = _Output.TryAddItems(_CurrentRecipe._OutputItem[i]._Item, _failedCraftQuantity);
                }
                else
                {
                    remainingQuantity = _Output.TryAddItems(_CurrentRecipe._OutputItem[i]._Item, _CurrentRecipe._OutputItem[i]._Quantity);
                }

                if (remainingQuantity > 0)
                {
                    _failedCraftIndex = i;
                    _failedCraftQuantity = remainingQuantity;
                    return;
                }
            }
            _failedCraftQuantity = 0;
            _failedCraftIndex = -1;
            _IsCrafting = false;
        }
    }

    public void TryToStartCraft()
    {
        if (_nextRecipe != null)
        {
            _CurrentRecipe = _nextRecipe;
            _nextRecipe = null;
        }
        if (_CraftCooldown > 0 || _CurrentRecipe == null)
        {
            return;
        }
        foreach (var inputItem in _CurrentRecipe._InputItem)
        {
            if (_Input.CountItem(inputItem._Item) < inputItem._Quantity)
            {
                return;
            }
        }
        if (_IsCrafting)
        {
            return;
        }
        _CraftCooldown = _CurrentRecipe._Cooldown;
        UpdateSprite();
        _IsCrafting = true;
    }

    public override void UpdateSprite()
    {
        SetSprite(_CurrentRecipe._OutputItem[0]._Item.Sprite);
    }

    public void SelectRecipe(Recipe recipe)
    {
        if (_IsCrafting || recipe == null || _failedCraftIndex >= 0)
        {
            return;
        }
        if (_recipes.Contains(recipe))
        {
            _nextRecipe = recipe;
        }
    }
}
