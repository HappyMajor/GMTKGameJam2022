using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : MonoBehaviour, IMonster
{

    public float moveSpeed;
    public float attackRange;
    public float sightRange;
    public BringerOfDeathAttack bringerOfDeathAttack;

    private Animator animator;
    [SerializeField]
    private Behaviour currentBehaviour;
    private MonsterBehaviourState currentBehaviourState;
    private Vector3 moveToPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetBehaviour(Behaviour.IDLE);
    }

    private void Update()
    {
        currentBehaviourState.Update();
        Move();
    }

    public void SetBehaviour(Behaviour behaviour)
    {
        if(currentBehaviourState != null)
        {
            currentBehaviourState.OnEnd();
        }

        switch(behaviour)
        {
            case Behaviour.IDLE:
                this.currentBehaviour = Behaviour.IDLE;
                this.currentBehaviourState = new BringerOfDeathIdle(this);
                this.currentBehaviourState.Start();
                break;
            case Behaviour.AGGRESSIVE:
                this.currentBehaviour = Behaviour.AGGRESSIVE;
                this.currentBehaviourState = new BringerOfDeathAggressive(this);
                this.currentBehaviourState.Start();
                break;
            case Behaviour.EXHAUSTED:

                break;
        }
    }

    public void ApplyDamage(float dmg)
    {
        currentBehaviourState.ApplyDamage(dmg);
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        currentBehaviourState.ApplyKnockback(knockback);
    }

    public void SetMoveTarget(Vector3 moveToPosition)
    {
        this.moveToPosition = moveToPosition;
    }

    private void Move()
    {
        if (transform.position != moveToPosition)
        {
            Vector3 moveDirection = (moveToPosition - transform.position).normalized;

            //Calculate the new position after walk
            Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;

            //Check if the new position is either left or right of this monster 
            if (Util.IsALeftOfB(newPosition, transform.position))
            {
                //face left
                transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                //face right
                transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z);
            }

            //check if the distance is closer than the movespeed (if we dont do this then this monster will vibrate at the target position)
            if (Vector3.Distance(transform.position, moveToPosition) <= moveSpeed * Time.deltaTime)
            {
                newPosition = moveToPosition;
                animator.SetBool("Walk", false);
            }
            else
            {
                animator.SetBool("Walk", true);
            }

            //set the new position
            transform.position = newPosition;
        }
    }

    public enum Behaviour
    {
        AGGRESSIVE, IDLE, EXHAUSTED
    }
}
