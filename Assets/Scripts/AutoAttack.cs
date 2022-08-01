using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public float Damage { get; set; }
    public Vector3 Knockback { get; set; }
    public Vector3 Direction { get; set; }

    public AnimationCurve acceleration = AnimationCurve.Linear(0, 0, 1, 1);
    public float Range { get; set; }
    public float Speed { get; set; }
    private Vector3 startingPosition;
    private float passedTimeSinceStart = 0f;

    public void Start() {
        // Keep track of starting position
        startingPosition = transform.position;

        // Rotation in the direction
        transform.rotation = Quaternion.LookRotation(Vector3.forward, Direction);
    }

    public void FixedUpdate() {
        transform.position += Direction * Time.fixedDeltaTime * Speed * acceleration.Evaluate(passedTimeSinceStart);

        // Destroy if the attack's range is exceeded
        if (Vector3.Distance(startingPosition, transform.position) > Range) {
            Destroy(gameObject);
        }

        passedTimeSinceStart += Time.fixedDeltaTime;
    }


    public void DestroyAfterTime(float seconds)
    {
        StartCoroutine(Routines.DoLater(seconds, () =>
        {
            Destroy(gameObject);
        }));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            IMonster monster = collision.gameObject.GetComponent<IMonster>();
            monster.ApplyDamage(Damage);
            monster.ApplyKnockback(Knockback);
        }
    }
}
