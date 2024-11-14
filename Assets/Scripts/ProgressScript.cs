using TMPro;
using UnityEngine;

public class ProgressScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _Progress;
    [SerializeField] private TextMeshProUGUI _item;
    public void UpdateDisplay(Level level, int amount)
    {
        _level.text = "level : " + level._Level.ToString();
        _item.text = "items : \n";
        foreach(ItemsWithQuantity items in level._Items)
        {
            _item.text += (items._Item.Name + "\n");
            UpdateProgress(items._Quantity, amount);
        }
    }

    public void UpdateProgress(int objective, int amount)
    {
        _Progress.text += amount + " / " + objective + "\n";
    }
}
