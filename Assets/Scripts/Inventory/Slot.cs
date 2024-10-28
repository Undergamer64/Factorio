using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public ItemBase _Item;

    public int _MaxItems;
    public int _Quantity;

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

    private void Start()
    {
        UpdateQuantity(_Quantity);
    }

    /// <summary>
    /// method to add 1 item to the empty slot
    /// </summary>
    public void AddItemToSlot(ItemBase item)
    {
        _Item = item;
        UpdateQuantity(1);
    }

    /// <summary>
    /// Updates the sprite of the item in the slot
    /// </summary>
    public void UpdateItemSprite()
    {
        if (_Item != null)
        {
            _itemSprite.sprite = _Item.Sprite;
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
        if (quantity > 0 && _Item != null)
        {
            UpdateItemSprite();
            _Quantity = quantity;
            _quantityText.text = _Quantity.ToString();
        }
        else
        {
            _Item = null;
            UpdateItemSprite();
            _Quantity = 0;
            _quantityText.gameObject.SetActive(false);
            _quantityContainerText.SetActive(false);
        }
    }

    public ItemBase Item { get { return _Item; } set { _Item = value; } }
    public int Quantity { get { return _Quantity; } }

}
