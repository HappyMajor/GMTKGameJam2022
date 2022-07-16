using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public float Damage { get; set; }
    public Vector3 Knockback { get; set; }


    public void DestroyAfterTime(float seconds)
    {
        StartCoroutine(Routines.DoLater(seconds, () =>
        {
            Destroy(gameObject);
        }));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision ENTER!");
        if (collision.gameObject.tag == "Monster")
        {
            Debug.Log("Collision with Monster!");
            IMonster monster = collision.gameObject.GetComponent<IMonster>();
            monster.ApplyDamage(Damage);
            monster.ApplyKnockback(Knockback);
        }
    }
}
