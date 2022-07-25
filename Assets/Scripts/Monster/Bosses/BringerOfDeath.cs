using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeath : MonoBehaviour, IMonster
{
    [SerializeField] int health = 10;
    public float moveSpeed;
    public float attackRange;
    public float sightRange;
    public BringerOfDeathAttack bringerOfDeathAttack;
    public GameObject thunderPrefab;

    private Animator animator;
    [SerializeField] private Behaviour currentBehaviour;
    private MonsterBehaviourState currentBehaviourState;
    private Vector3 moveToPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetBehaviour(Behaviour.IDLE);
        this.moveToPosition = transform.position;
    }

    private void Update()
    {
        currentBehaviourState.Update();
        Move();
    }

    public void SetBehaviour(Behaviour behaviour)
    {
        if(currentBehaviourState != null) currentBehaviourState.OnEnd();

        this.currentBehaviour = behaviour;

        switch(behaviour)
        {
            case Behaviour.IDLE:
                this.currentBehaviourState = new BringerOfDeathIdle(this);
                break;
            case Behaviour.CHASE_ATTACK:
                this.currentBehaviourState = new BringerOfDeathChaseAttack(this);
                break;
            case Behaviour.MAGIC_ATTACK:
                this.currentBehaviourState = new BringerOfDeathMagicAttack(this);
                break;
            case Behaviour.TELEPORT_AWAY:
                this.currentBehaviourState = new BringerOfDeathTeleportAway(this);
                break;
        }

        this.currentBehaviourState.Start();
    }

    public void ApplyDamage(float dmg)
    {
        currentBehaviourState.ApplyDamage(dmg);

        // Take damage
        dmg -= 1;

        // TODO: sorry Alec not sure how the behavior state code works so you'll have to move the appropriate place
        // Die if out of health
        if (health <= 0) {
            die();
        }
    }

    void die() {
        // Play death sound
        // TODO: apply rigid body to sound when its added
        OneShotAudio.Play("event:/sfx/bringer of death/death", transform);
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
        CHASE_ATTACK, IDLE, MAGIC_ATTACK, TELEPORT_AWAY
    }
}
