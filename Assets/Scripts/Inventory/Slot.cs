using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot
{
    private ItemBase _item = null;
    private int _quantity = 0;

    [SerializeField]
    private TextMeshProUGUI _quantityText;

    //[SerializeField]
    //private TextMeshProUGUI _tierText;

    [SerializeField]
    private GameObject _quantityContainerText;

    [SerializeField]
    private Image _itemSprite;

    [SerializeField]
    private Image _itemSlotSprite;

    /// <summary>
    /// method to add 1 item to the empty slot
    /// </summary>
    public void AddItemToSlot(ItemBase item)
    {
        _item = item;
        UpdateQuantity(1);
    }

    /// <summary>
    /// Updates the sprite of the item in the slot
    /// </summary>
    public void UpdateItemSprite()
    {
        if (_item != null)
        {
            _itemSprite.sprite = _item.Sprite;
            _itemSprite.color = Color.white;

            _quantityContainerText.SetActive(false);
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = new Color(1f, 1f, 1f, 0);
        }
    }

    /// <summary>
    /// Changes the quantity of the current item to "quantity"
    /// Also handles if the quantity is 0 or less (deletes the item)
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        //_tierText.gameObject.SetActive(false);
        if (quantity > 0 && _item != null)
        {
            _quantity = quantity;
            if (_quantityText == null || _itemSprite)
            {
                return;
            }
            UpdateItemSprite();
            _quantityText.text = _quantity.ToString();
        }
        else
        {
            _item = null;
            _quantity = 0;
            if (_quantityText == null || _itemSprite)
            {
                return;
            }
            UpdateItemSprite();
            _quantityText.gameObject.SetActive(false);
            _quantityContainerText.SetActive(false);
        }
    }

    public ItemBase Item { get { return _item; } set { _item = value; } }
    public int Quantity { get { return _quantity; } }

}
