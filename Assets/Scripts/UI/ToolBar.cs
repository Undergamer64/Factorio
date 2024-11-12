using System.Collections.Generic;
using UnityEngine;

public class ToolBar : MonoBehaviour
{
    [SerializeField] private CharacterData _characterData;
    [SerializeField] private List<StructureItem> _structures = new();

    public void ChangeStructure(int structureIndex) 
    { 
        if (structureIndex < 0 || structureIndex >= _structures.Count || _characterData._PlacedStructureItem == _structures[structureIndex])
        {
            _characterData._PlacedStructureItem = null;
            return;
        }

        _characterData._PlacedStructureItem = _structures[structureIndex];
    }


}
