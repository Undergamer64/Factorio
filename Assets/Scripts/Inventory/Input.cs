using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class Input : Tunnel
{
    public Output _Output;


    //check if there's space in inventory
    //if yes try and remove item from partner
    //if yes add item to inventory
    private void Start()
    {
        FindPartner();
    }
    public void FindPartner()
    {

        List<Collider2D> _Outputs = Physics2D.OverlapBoxAll(transform.position, Vector2.one*.1f, 0).ToList();
        foreach (Collider2D collider in _Outputs)
        {
            if (collider.TryGetComponent<Output>(out Output output))
            {
                if (output == _Output)
                {
                    break;
                }
                _Output = output;
                _Output.FindPartner();
                break;
            }
        }
    }
}
