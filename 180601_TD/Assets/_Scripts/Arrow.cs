using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeekBrains;

public class Arrow : BaseAmmunition
{
    
    public GameObject arrowImpact;
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        MoveToTarget();
    }
    void HitTarget()
    {
        
        var tempImpact = Instantiate(arrowImpact, transform.position, transform.rotation);
        SetDamage(target);
        Destroy(tempImpact, 2f);
        Destroy(gameObject);
    }
    void MoveToTarget()
    {
        Vector3 dir = target.botTransform.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }
    private void SetDamage(ISetDamage obj)
    {
        if (obj != null)
        {
            obj.ApplyDamage(damage,attackType);
        }
    }

}
