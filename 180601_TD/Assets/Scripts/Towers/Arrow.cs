
using UnityEngine;
namespace Game.Towers
{
    public class Arrow : BaseAmmunition
    {
        protected override void HitTarget()
        {
            base.HitTarget();
            if (_hitImpactPath == null || hitImpact == null)
            {
                Debug.Log("Ammunition " + name + " cant find impact GameObject");
                return;
            }
            var tempImpact = Instantiate(hitImpact, transform.position, transform.rotation);
            Destroy(tempImpact, 2f);
        }
    }
}

