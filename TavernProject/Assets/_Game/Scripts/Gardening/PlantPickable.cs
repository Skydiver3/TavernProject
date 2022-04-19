using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPickable : PickableItem
{
    private bool _picked = false;
    [HideInInspector] public PlantBed bed;

    public override void Hide()
    {
        if (bed&&!_picked)
        {
            bed.Pick();
            _picked = true;
        }
        base.Hide();
    }
}
