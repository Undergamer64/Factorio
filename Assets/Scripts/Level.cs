using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 1)]
public class Level : MonoBehaviour
{
    public ItemBase _item;
    public int _amount;
}

