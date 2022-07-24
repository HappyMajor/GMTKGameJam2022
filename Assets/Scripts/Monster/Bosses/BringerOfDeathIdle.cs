using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathIdle : MonsterBehaviourState
{
    private BringerOfDeath bringerOfDeath;
    private Vector3 moveToPosition;
    private Coroutine routine;
    private GameObject player;

    public BringerOfDeathIdle(BringerOfDeath bringerOfDeath)
    {
        this.bringerOfDeath = bringerOfDeath;
    }

    public void Start ()
    {
        player = GameObject.Find("Player");
        routine = bringerOfDeath.StartCoroutine(DoBehaviour());
    }

    IEnumerator DoBehaviour()
    {
        while(!IsPlayerInSight())
        {
            if(HasReachedPosition())
            {
                //Wait a bit 
                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 4f));

                //Move to a new position
                ChooseRandomPositionToMove();
            }

            yield return new WaitForEndOfFrame();
        }

        //Player is in sight so set to aggressive
        bringerOfDeath.SetBehaviour(BringerOfDeath.Behaviour.CHASE_ATTACK);
    }

    private bool HasReachedPosition()
    {
        if(bringerOfDeath.transform.position != moveToPosition)
        {
            return false;
        } else
        {
            return true;
        }
    }


    private bool IsPlayerInSight()
    {
        if (Vector3.Distance(bringerOfDeath.transform.position, player.transform.position) <= bringerOfDeath.sightRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ChooseRandomPositionToMove()
    {
        moveToPosition = Util.GetRandomPositionOfRectangle(bringerOfDeath.transform.position, 5, 5);
        bringerOfDeath.SetMoveTarget(moveToPosition);
    }

    //will be called by the BringerOfDeath class if this behaviour is the current behaviour (Behaviour.Idle)
    public void Update()
    {
    }

    public void OnEnd()
    {
        bringerOfDeath.StopCoroutine(routine);
    }

    public void ApplyDamage(float dmg)
    {
    }

    public void ApplyKnockback(Vector3 knockback)
    {

    }
}
