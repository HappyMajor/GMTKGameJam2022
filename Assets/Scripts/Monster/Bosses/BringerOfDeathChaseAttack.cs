using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathChaseAttack : MonsterBehaviourState
{
    private GameObject player;
    private BringerOfDeath bringerOfDeath;
    private Animator animator;
    private BringerOfDeathAttack attack;

    private Coroutine currentCoroutine;

    public BringerOfDeathChaseAttack(BringerOfDeath bringerOfDeath)
    {
        this.bringerOfDeath = bringerOfDeath;
    }

    void MonsterBehaviourState.Start()
    {
        player = GameObject.Find("Player");
        animator = bringerOfDeath.gameObject.GetComponent<Animator>();
        attack = bringerOfDeath.bringerOfDeathAttack;

        DoBehaviourLoop();
    }

    void DoBehaviourLoop()
    {
        //Try to move to the player every 0.3 seconds
        currentCoroutine = bringerOfDeath.StartCoroutine(Routines.DoEverySecondsAndEndAfter(interval: 0.3f, end: Random.Range(1,4), 
            action: () =>
            {
                if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("BringerOfDeath_Attack"))
                {
                    if (IsInAttackRange())
                    {
                        AttackPlayer();
                    }
                    else
                    {
                        //Move to player
                        bringerOfDeath.SetMoveTarget(GetClosestAttackPosition());
                    }
                }
            }, 
            endAction: () =>
            {
                //take a break and retry again
                currentCoroutine = bringerOfDeath.StartCoroutine(Routines.DoLater(seconds: 2f, 
                    action: () =>
                    {
                        bringerOfDeath.SetBehaviour(BringerOfDeath.Behaviour.TELEPORT_AWAY);
                    }
                ));
            }
        ));
    }

    bool IsInAttackRange()
    {
        if (Mathf.Abs(player.transform.position.y - bringerOfDeath.transform.position.y) <= 0.5)
        {
            if (Vector3.Distance(bringerOfDeath.transform.position, player.transform.position) <= bringerOfDeath.attackRange + 1)
            {
                return true;
            }
        }

        return false;
    }

    void AttackPlayer()
    {
        //Stop moving
        bringerOfDeath.SetMoveTarget(bringerOfDeath.transform.position);
        animator.SetBool("Walk", false);

        if (Util.IsALeftOfB(player.transform.position, bringerOfDeath.transform.position))
        {
            bringerOfDeath.transform.localScale = new Vector3(Mathf.Abs(bringerOfDeath.transform.localScale.x), bringerOfDeath.transform.localScale.y, bringerOfDeath.transform.localScale.z);
        }
        else
        {
            bringerOfDeath.transform.localScale = new Vector3(-Mathf.Abs(bringerOfDeath.transform.localScale.x), bringerOfDeath.transform.localScale.y, bringerOfDeath.transform.localScale.z);
        }
        animator.SetTrigger("Attack");
        bringerOfDeath.StartCoroutine(Routines.DoLater(0.3f, () =>
        {
            attack.Activate();
            attack.DeactivateAfterTime(0.2f);
        }));
    }

    void MonsterBehaviourState.OnEnd()
    {
        bringerOfDeath.StopCoroutine(currentCoroutine);
    }

    Vector3 GetClosestAttackPosition()
    {
        if(Util.IsALeftOfB(player.transform.position, bringerOfDeath.transform.position))
        {
            return player.transform.position + new Vector3(bringerOfDeath.attackRange, 0, 0);
        } else
        {
            return player.transform.position + new Vector3(-bringerOfDeath.attackRange, 0, 0);
        }
    }

    void MonsterBehaviourState.Update()
    {

    }

    public void ApplyDamage(float dmg)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        throw new System.NotImplementedException();
    }
}
