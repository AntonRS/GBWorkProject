using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Towers;

public class Arrow : BaseAmmunition
{
    
    protected override void HitTarget()
    {
        base.HitTarget();
        var tempImpact = Instantiate(hitImpact, transform.position, transform.rotation);
        Destroy(tempImpact, 2f);
    }
    
    

}
