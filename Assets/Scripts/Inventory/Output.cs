using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Output : Tunnel
{
    public Input _Input;

    private void Start()
    {
        FindPartner();
    }
    public void FindPartner()
    {
        List<Collider2D> _Inputs = Physics2D.OverlapBoxAll(transform.position, Vector2.one*.1f, 0).ToList();
        foreach (Collider2D collider in _Inputs)
        {
            if (collider.TryGetComponent<Input>(out Input input))
            {
                if (input == _Input)
                {
                    break;
                }
                _Input = input;
                _Input.FindPartner();
                break;
            }
        }
    }

    public bool PullOutInventory(ItemBase item, int quantity,InputOrOutput slots)
    {
        if (!_Input._ParentInventory.IsInventoryFull(item, 1, InputOrOutput._InputSlots))
        {
            if (_Input._ParentInventory.CanAddItem(item, InputOrOutput._InputSlots))
            {
                int LeftToAdd = _Input._ParentInventory.TryAddItems(item, quantity, InputOrOutput._InputSlots);
                _ParentInventory.TryRemoveItems(item, quantity - LeftToAdd, slots);
                _Input._ParentInventory.GetComponent<Structure>().UpdateSprite();
                _Input.transform.parent.GetComponentInParent<Structure>().Process();
                return true;
            }
        }
        return false;
    } 
}
