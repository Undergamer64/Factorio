using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _Progress;
    [SerializeField] private TextMeshProUGUI _item;
    public void UpdateDisplay(Level level, List<int> amount)
    {
        _level.text = "level : " + level._Level.ToString();
        _item.text = "items : \n";
        foreach(ItemsWithQuantity items in level._Items)
        {
            _item.text += (items._Item.Name + "\n");
        }
        UpdateProgress(level._Items, amount);
    }

    public void UpdateProgress( List<ItemsWithQuantity> objective, List<int> amount)
    {
        _Progress.text = "";
        for (int i = 0; i < objective.Count; i++)
        {
            _Progress.text += amount[i] + " / " + objective[i]._Quantity + "\n";
        }
    }
}
