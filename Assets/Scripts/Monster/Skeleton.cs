using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour, IMonster
{
    public float health;

    private Rigidbody2D rigidBody;
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    public void ApplyDamage(float dmg)
    {
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        Debug.Log("Apply force!");
        rigidBody.AddForce(knockback, ForceMode2D.Impulse);
    }

}
