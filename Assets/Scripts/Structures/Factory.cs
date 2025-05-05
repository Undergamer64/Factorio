using UnityEngine;

public class Factory : Structure
{
    public float _CraftCooldown { get; protected set; } = 0;
    public Recipe _Recipe;

    public bool _CanCraft { get; private set; } = false;
    private int _failedCraftIndex = -1;
    private int _failedCraftQuantity = 0;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        if (_Recipe == null)
        {
            return;
        }
        foreach (var item in _Recipe._InputItem)
        {
            _Input._WhiteListItems.Add(item._Item);
        }
    }


    private void CraftUpdate()
    {
        if (!_CanCraft)
        {
            TryToStartCraft();
        }

        if (_CanCraft)
        {
            _CraftCooldown -= Time.deltaTime;
        }
        if (_CraftCooldown < 0)
        {
            if (_failedCraftIndex < 0)
            {
                if (_Recipe == null)
                {
                    return;
                }
                foreach (var item in _Recipe._InputItem)
                {
                    if (!(_Input.CountItem(item._Item) >= item._Quantity))
                    {
                        _CanCraft = false;
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
        if (_Recipe == null)
        {
            return false;
        }
        if (!_Output.IsInventoryEmpty())
        {
            foreach (Slot slot in _Output._Slots)
            {
                if (slot.Item == null || slot.Quantity == 0) continue;

                if (_Output.PullOutInventory(slot.Item, slot.Quantity))
                {
                    return true;
                }
            }
            
            /*
            bool hasAtLeastOnefail = false;
            foreach (ItemsWithQuantity item in _Recipe._OutputItem)
            {
                if (_Output.CountItem(item._Item) < 1)
                {
                    hasAtLeastOnefail = true;
                    continue;
                }

                bool hasfailed = true;
                /*
                foreach (Output output in outputs)
                {
                    List<ItemBase> _whiteList = output._Input._ParentInventory._WhiteListItems;
                    if (_whiteList.Count == 0)
                    {
                        output._Input._ParentInventory._WhiteListItems.Add(item._Item);
                        
                        hasfailed = !output.PullOutInventory(item._Item, item._Quantity, InputOrOutput._OutputSlots);
                        break;
                    }
                    else if (_whiteList.Contains(item._Item))
                    {
                        hasfailed = !output.PullOutInventory(item._Item, item._Quantity, InputOrOutput._OutputSlots);
                        break;
                    }
                }
                if (hasfailed)
                { 
                    hasAtLeastOnefail = true;
                }
            }*/
            //return !hasAtLeastOnefail;
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
        if (_Recipe != null)
        {
            foreach (var InputItem in _Recipe._InputItem)
            {
                _Input.TryRemoveItems(InputItem._Item, InputItem._Quantity);
            }
        }
    }


    public void TryAddCraftOutput()
    {
        if (_Recipe != null)
        {
            int temp = _failedCraftIndex;
            if (_failedCraftIndex < 0)
            {
                temp = 0;
            }
            for (int i = temp; i < _Recipe._OutputItem.Count; i++)
            {
                int remainingQuantity = 0;
                if (i == _failedCraftIndex)
                {
                    remainingQuantity = _Output.TryAddItems(_Recipe._OutputItem[i]._Item, _failedCraftQuantity);
                }
                else
                {
                    remainingQuantity = _Output.TryAddItems(_Recipe._OutputItem[i]._Item, _Recipe._OutputItem[i]._Quantity);
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
            _CanCraft = false;
        }
    }

    public void TryToStartCraft()
    {
        if (_CraftCooldown > 0 || _Recipe == null)
        {
            return;
        }
        foreach (var inputItem in _Recipe._InputItem)
        {
            if (_Input.CountItem(inputItem._Item) < inputItem._Quantity)
            {
                return;
            }
        }
        if (_CanCraft)
        {
            return;
        }
        _CraftCooldown = _Recipe._Cooldown;
        UpdateSprite();
        _CanCraft = true;
    }

    public override void UpdateSprite()
    {
        SetSprite(_Recipe._OutputItem[0]._Item.Sprite);
    }
}
