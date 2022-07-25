using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathTeleportAway : MonsterBehaviourState
{

    private BringerOfDeath bringerOfDeath;
    private GameObject player;
    private Animator animator;
    public BringerOfDeathTeleportAway(BringerOfDeath bringerOfDeath)
    {
        this.bringerOfDeath = bringerOfDeath;
    }
    void MonsterBehaviourState.Start()
    {
        player = GameObject.Find("Player");
        animator = bringerOfDeath.gameObject.GetComponent<Animator>();

        bringerOfDeath.StartCoroutine(DoBehaviour());
    }

    IEnumerator DoBehaviour()
    {
        //Start casting animation
        animator.SetTrigger("Shadow");

        // Play teleport sound
        OneShotAudio.Play("event:/sfx/bringer of death/teleport", bringerOfDeath.transform);

        //Wait for the animation to finish
        yield return new WaitForSeconds(0.9f);

        //Start casting animation
        animator.SetTrigger("Shadow_Reverse");

        TeleportTo(GetRandomPositionCloseToPlayer());

        //wait again before switching state
        yield return new WaitForSeconds(1.1f);


        if(IsPlayerCloseToAttackRange())
        {
            //Player is close so go melee attack
            bringerOfDeath.SetBehaviour(BringerOfDeath.Behaviour.CHASE_ATTACK);
        } else
        {
            //Player is far away so go ranged attack
            bringerOfDeath.SetBehaviour(BringerOfDeath.Behaviour.MAGIC_ATTACK);
        }
    }


    bool IsPlayerCloseToAttackRange()
    {
       if (Vector3.Distance(bringerOfDeath.transform.position, player.transform.position) <= bringerOfDeath.attackRange + 1.5)
       {
            return true;
       }
        return false;
    }

    public void ApplyDamage(float dmg)
    {
        throw new System.NotImplementedException();
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        throw new System.NotImplementedException();
    }

    public void OnEnd()
    {

    }

    private void TeleportTo(Vector3 position)
    {
        bringerOfDeath.transform.position = position;
        bringerOfDeath.SetMoveTarget(position);
    }


    private Vector3 GetRandomPositionCloseToPlayer()
    {
        Vector3 randomPositionCloseToPlayer = Util.GetRandomPositionOfRectangle(player.transform.position, 3, 3);
        return randomPositionCloseToPlayer;
    }

    void MonsterBehaviourState.Update()
    {
    }
}
