using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMonster : MonoBehaviour, IMonster
{
    public float health;
    public Vector3 movePosition;
    public float moveSpeed = 1;
    public float weight = 1;
    public float shockDuration = 1;

    private bool isInShock = false;
    private Rigidbody2D rigidBody;
    private GameObject player;


    public void Start()
    {
        player = GameObject.Find("Player");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        if(!isInShock)
        {
            movePosition = player.transform.position;
            Vector3 motion = (movePosition - transform.position).normalized * moveSpeed * Time.deltaTime;
            rigidBody.MovePosition(transform.position + motion);
        }

    }

    public void ApplyDamage(float dmg)
    {
        // Keep track of damage
        this.health -= dmg;

        // Die
        if(this.health <= 0)
        {
            die();
        }
    }

    void die() {
        // TODO: Play death sound

        // Clean up game object
        Destroy(this.gameObject);
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        isInShock = true;
        rigidBody.AddForce(knockback, ForceMode2D.Impulse);
        StartCoroutine(Routines.DoLater(shockDuration, () =>
        {
            isInShock = false;
        }));
    }
}
