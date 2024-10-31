using UnityEngine;
using System.Collections.Generic;

public class Depot : Structure
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    private int _objectiveAmount;
    private int _amount;

    protected override bool CallOutput()
    {
        return true;
    }

    public override void Process()
    {
        
    }

    //count on input and delete object

    private void Start()
    {
        SetObjective(_levels[0]);
    }

    private void SetObjective(Level level)
    {
        _objectiveAmount = level._amount;
        _amount = 0;
    }
}
