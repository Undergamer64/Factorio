using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Slot> _Slots = new List<Slot>();
    public List<Input> _Inputs = new List<Input>();
    public List<Output> _Outputs = new List<Output>();

    public List<ItemBase> _WhiteListItems = new List<ItemBase>();
    public List<ItemBase> _BlackListItems = new List<ItemBase>();
}
