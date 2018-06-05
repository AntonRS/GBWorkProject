using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GeekBrains;

public class Bot : MonoBehaviour, ISetDamage
{

    NavMeshAgent agent;
    public Transform destination;
    public Transform botTransform;
    public float hp;
    [Range(1, 90)]
    public float physicalArmor;
    [Range(1,90)]
    public float magicArmor;
    public float speed;
    public bool isFlying;
    bool isDead;


    public void ApplyDamage(float damage, AttackType attackType)
    {

        if (hp > 0)
        {
            if (attackType == AttackType.physical)
            {
                hp -= (damage/100)* physicalArmor;
            }
            if (attackType == AttackType.magic)
            {
                hp -= (damage / 100) * magicArmor;
            }
            if (attackType == AttackType.pure)
            {
                hp -= damage;
            }
            
        }
        if (hp <= 0)
        {
            hp = 0;
            isDead = true;
            Dead();
        }
    }

    private void Start()
    {
        
        botTransform = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
        Main.Instance.AddEnemy(this);
        
    }
    private void Dead()
    {
        Main.Instance.DeleteEnemy(this);
        Destroy(gameObject, 1f);
    }

}
