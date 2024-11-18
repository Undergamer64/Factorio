using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : Factory
{
    [SerializeField] private Transform _childTransform;

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        List<Collider2D> collider2Ds  = Physics2D.OverlapBoxAll(_childTransform.position, _childTransform.localScale * 0.9f, 0,1 << 0).ToList();
        foreach(Collider2D collider in collider2Ds)
        {   
            if (collider.TryGetComponent<ResourceOre>(out ResourceOre resource))
            {
                _Recipe = resource._Recipe;
                break;
            }
        }
        base.Init();
    }
}
