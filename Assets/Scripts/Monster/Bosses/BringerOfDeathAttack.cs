using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathAttack : MonoBehaviour
{
    [SerializeField] int damage = 2;

    public void Activate()
    {
        GetComponent<BoxCollider2D>().enabled = true;

        // Play attack sound
        OneShotAudio.Play("event:/sfx/bringer of death/scythe", gameObject.transform);
    }

    public void DeactivateAfterTime(float seconds)
    {
        StartCoroutine(Routines.DoLater(seconds, () => GetComponent<BoxCollider2D>().enabled = false));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector3 knockback = (player.transform.position - gameObject.transform.position).normalized * 6;
            player.ApplyDamage(damage, knockback);
        }
    }

}
