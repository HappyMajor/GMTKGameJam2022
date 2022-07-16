using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMonster : MonoBehaviour, IMonster
{
    public float health;
    public Vector3 movePosition;
    public float moveSpeed = 1;
    public float weight = 1;

    private Rigidbody2D rigidBody;

    private GameObject player;

    public void Start()
    {
        player = GameObject.Find("Player");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        movePosition = player.transform.position;
        Vector3 motion = (movePosition - transform.position).normalized * moveSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + motion);
    }

    public void ApplyDamage(float dmg)
    {
        this.health -= dmg;
        if(this.health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        rigidBody.AddForce(knockback * 10, ForceMode2D.Impulse);
    }


}
