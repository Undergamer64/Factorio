using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class Input : Inventory
{
    public List<Output> _Outputs = new List<Output>();
    public List<Collider2D> _LinkingPoints = new List<Collider2D>();

    //check if there's space in inventory
    //if yes try and remove item from partner
    //if yes add item to inventory
    private void Start()
    {
        for (int i = 0; i < _LinkingPoints.Count; i++)
        {
            _Outputs.Add(null);
        }
        FindPartner();
    }

    public void FindPartner()
    {
        foreach (Collider2D col in _LinkingPoints)
        {
            List<Collider2D> Outputs = Physics2D.OverlapBoxAll(col.transform.position, col.bounds.size, 0).ToList();
            foreach (Collider2D collider in Outputs)
            {
                if (collider.TryGetComponent(out Output output ))
                {
                    if (output.GetComponentInParent<Structure>().enabled)
                    {
                        if (_Outputs.Contains(output))
                        {
                            break;
                        }
                        _Outputs[_LinkingPoints.IndexOf(col)] = output;
                        output.FindPartner();
                        break;
                    }
                }
            }
        }
    }
}
