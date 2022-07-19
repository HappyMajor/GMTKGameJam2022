using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IMonster
{
    public float health;
    public Vector3 movePosition;
    public float moveSpeed = 1;
    public float weight = 1;
    public float shockDuration = 1;

    private bool isInShock = false;
    private Rigidbody2D rigidBody;
    private GameObject player;
    private Animator animator;
    private bool isDead = false;
    private AudioSource audioSource;
    [SerializeField] private AudioClip deathSound;


    public void Start()
    {
        player = GameObject.Find("Player");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FixedUpdate()
    {
        if (!isInShock && !isDead)
        {
            movePosition = player.transform.position;
            Vector3 motion = (movePosition - transform.position).normalized * moveSpeed * Time.deltaTime;
            rigidBody.MovePosition(transform.position + motion);
            

            if(motion.magnitude >= 0)
            {
                animator.SetBool("Walk",true);
            }

            if (Util.IsALeftOfB(transform.position, player.transform.position))
            {
                //is left of the player so turn right
                transform.localScale = new Vector3(1, 1, 1);
            } else 
            {
                //is right of the player so turn left
                transform.localScale = new Vector3(-1, 1, 1);
            }

        } else
        {
          animator.SetBool("Walk", false);
        }

    }

    public void Die()
    {
        StartCoroutine(Routines.DoLater(3, () =>
        {
            Destroy(this.gameObject);
        }));
        animator.SetBool("Death", true);
        isDead = true;

        //Deactivate collision so it does not block other skeletons and wont damage the player anymore
        GetComponent<CircleCollider2D>().enabled = false;

        // Play sound
        audioSource.PlayOneShot(deathSound);
    }

    public void ApplyDamage(float dmg)
    {
        if(!isDead)
        {
            this.health -= dmg;
            if (this.health <= 0)
            {
                Die();
            }
        }
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        if(!isDead)
        {
            isInShock = true;
            animator.SetTrigger("Hurt");
            rigidBody.AddForce(knockback, ForceMode2D.Impulse);
            StartCoroutine(Routines.DoLater(shockDuration, () =>
            {
                isInShock = false;
            }));
        }
    }


}
