using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeekBrains;

public class Arrow : BaseAmmunition
{
    
    public GameObject arrowImpact;
    
    protected override void HitTarget()
    {
        base.HitTarget();
        var tempImpact = Instantiate(arrowImpact, transform.position, transform.rotation);
        Destroy(tempImpact, 2f);
    }
    
    

}
