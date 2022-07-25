using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IMonster
{
    [SerializeField] const float shockDuration = 1;
    [SerializeField] const float timeAfterDeathUntilDestroy = 3;
    [SerializeField] const float superSkeletonSpeedMultiplier = 1.5f;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float health;
    Vector3 movePosition;
    Rigidbody2D rigidBody;
    GameObject player;
    Animator animator;
    bool isDead = false;
    bool isInShock = false;

    public static void ConfigureSuperSkeleton(Skeleton skeleton) {
        Configure(skeleton: skeleton, color: Color.red, isSuperSkeleton: true);
    }

    public static void Configure(Skeleton skeleton, Color color, bool isSuperSkeleton) {
        // Set its color
        skeleton.gameObject.GetComponent<SpriteRenderer>().color = color;

        // Super skeleton
        if (isSuperSkeleton) {
            // Moves faster than normal skeletons
            skeleton.moveSpeed *= superSkeletonSpeedMultiplier;
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (!isInShock && !isDead)
        {
            movePosition = player.transform.position;
            Vector3 motion = (movePosition - transform.position).normalized * moveSpeed * Time.deltaTime;
            rigidBody.MovePosition(transform.position + motion);

            // Walk animation when moving
            if (motion.magnitude >= 0)
            {
                animator.SetBool("Walk",true);
            }

            // Pick direction to face
            if (Util.IsALeftOfB(transform.position, player.transform.position))
            {
                //is left of the player so turn right
                transform.localScale = new Vector3(1, 1, 1);
            } 
            else
            {
                //is right of the player so turn left
                transform.localScale = new Vector3(-1, 1, 1);
            }

        } 
        else {
          // Idle animation
          animator.SetBool("Walk", false);
        }
    }

    public void Die()
    {
        // Death animation
        animator.SetBool("Death", true);

        // Play death sound
        OneShotAudio.Play("event:/sfx/skeleton/death", transform, rigidBody);

        // Destroy the game object after a delay
        StartCoroutine(Routines.DoLater(timeAfterDeathUntilDestroy, () =>
        {
            Destroy(this.gameObject);
        }));

        // Flag as dead
        isDead = true;

        // Deactivate collision so it does not block other skeletons and wont damage the player anymore
        GetComponent<CircleCollider2D>().enabled = false;
    }

    public void ApplyDamage(float dmg)
    {
        // Already dead!
        if (isDead) {
            return;
        }

        // Take damage
        this.health -= dmg;

        // Die if out of health
        if (this.health <= 0)
        {
            Die();
        }
    }


    public void ApplyKnockback(Vector3 knockback)
    {
        // Can't get knocked back while dead
        if (isDead) {
            return;
        }

        // Flag as being in shock
        isInShock = true;

        // Change to hurt animation
        animator.SetTrigger("Hurt");

        // Play sound
        OneShotAudio.Play("event:/sfx/skeleton/knockback", transform, rigidBody);

        // Apply physics knockback
        rigidBody.AddForce(knockback, ForceMode2D.Impulse);

        // Mark as no longer in shock after shock is over
        StartCoroutine(Routines.DoLater(shockDuration, () =>
        {
            isInShock = false;
        }));
    }
}
