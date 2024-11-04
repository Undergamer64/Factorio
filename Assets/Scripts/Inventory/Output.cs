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
        List<Collider2D> _Inputs = Physics2D.OverlapBoxAll(transform.position, transform.localScale, 0).ToList();
        foreach (Collider2D collider in _Inputs)
        {
            if (collider.TryGetComponent<Input>(out Input input))
            {
                _Input = input;
                break;
            }
        }
    }

    public void PullOutInventory(ItemBase item, int quantity)
    {
        if (!_Input._ParentInventory.IsInventoryFull(item,quantity, InputOrOutput._InputSlots))
        {
            if (_Input._ParentInventory.CanAddItem(item, InputOrOutput._InputSlots))
            {
                _Input._ParentInventory.TryAddItems(item, quantity, InputOrOutput._InputSlots);
                _Input.GetComponentInParent<Transform>().GetComponentInParent<Structure>().Process();
            }
        }
    }
    
}
