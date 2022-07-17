using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float autoAttackMovementDelay;
    //defined as attacks per second
    public float attackSpeed;
    public float attackRange;
    public float attackDuration;
    public float attackKnockback;

    public GameObject autoAttackPrefab;

    private float lastAuttoAttackTime;
    private bool isMovementBlockedByAttack = false;

    public void Update()
    {
        CheckAndDoMovement();
        CheckAndDoAttack();
    }

    public void CheckAndDoMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(CanMove())
        {
            Vector3 motion = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
            transform.position = transform.position + motion;
        }
    }

    public void CheckAndDoAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(CanAutoAttack())
            {
                Attack();
            } else
            {
                Debug.Log("Autoattack on cooldown!");
            }
        }
    }

    public void Attack()
    {
        lastAuttoAttackTime = Time.time;
        isMovementBlockedByAttack = true;

        CreateAttackSliceEffect();


        //remove the block in a few seconds defined by @autoAttackMovementDelay 
        StartCoroutine(Routines.DoLater(autoAttackMovementDelay, () =>
        {
            isMovementBlockedByAttack = false;
        }));
    }

    public void CreateAttackSliceEffect()
    {
        // Get mouse direction
        Vector3 mouseWorldPos = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        Vector3 mouseDirection = (mouseWorldPos - transform.position).normalized;

        // Create an autoattack
        AutoAttack autoAttack = Instantiate(autoAttackPrefab, transform.position + mouseDirection * attackRange, Quaternion.Euler(new Vector3(0, 0, 0)), transform).GetComponent<AutoAttack>();
        autoAttack.Damage = 1;
        autoAttack.Direction = mouseDirection;
        autoAttack.Knockback = mouseDirection * attackKnockback;
        autoAttack.DestroyAfterTime(attackDuration);
    }

    public bool CanMove()
    {
        if(isMovementBlockedByAttack)
        {
            return false;
        }
        return true;
    }

    public bool CanAutoAttack()
    {
        if(Time.time >= lastAuttoAttackTime + 1f/attackSpeed)
        {
            return true;
        } else
        {
            return false;
        }
    }

}
