using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathAttack : MonoBehaviour
{
    public void Activate()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void DeactivateAfterTime(float seconds)
    {
        StartCoroutine(Routines.DoLater(seconds, () => GetComponent<BoxCollider2D>().enabled = false));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collide with something!");
        if (collision.gameObject.name == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector3 knockback = (player.transform.position - gameObject.transform.position).normalized * 6;
            player.ApplyKnockback(knockback);
            Debug.Log("Collide with player!");
        }
    }

}
