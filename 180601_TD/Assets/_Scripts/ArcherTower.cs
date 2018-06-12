using UnityEngine;
using Game.Towers;


public class ArcherTower : BaseTower {

    [SerializeField] private Transform rotateHead;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float turnSpeed = 5;
    [SerializeField] private float bulletSpeed;

    private float fireCountDown = 0f;

    protected override void Update()
    {
        base.Update();
        LookAtTarget();
    }
    public override void Fire()
    {
        if (fireCountDown <= 0f)
        {
            var tempArrow = Instantiate(ammunition, firePoint.position, firePoint.rotation);
            tempArrow.Target = target;
            tempArrow.Speed = bulletSpeed;
            tempArrow.Damage = damage;
            tempArrow.AttackType = attackType;

            fireCountDown = 1f / attackPerSecond;
        }
        fireCountDown -= Time.deltaTime;
    }

    private void LookAtTarget()// Tower Head Fallows the target
    {
        if (target != null)
        {
            var direction = target.EnemyTransform.position - rotateHead.position;
            Quaternion lookRotation = Quaternion.Lerp(rotateHead.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      Time.deltaTime * turnSpeed);
            rotateHead.rotation = lookRotation;
        }
       
    }




}
