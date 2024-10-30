using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Output : Tunnel
{
    public Input _Input;
    public int _PulledItem;

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

        /// <summary>
        /// only used in PushToInventory from Input
        /// </summary>
        public bool PullOutInventory(ItemBase item,int quantity)
    {
        _PulledItem = _ParentInventory.TryRemoveItems(item, quantity);
        if (_PulledItem < quantity)
        {
            _PulledItem = quantity - _PulledItem;
            return true;
        }
        return false;
    }
}
