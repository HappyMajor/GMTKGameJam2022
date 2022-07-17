using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackLevel {
    One, Two, Three, Four
}

public struct AttackLevelStats
{
  public AttackLevelStats(
    float speed,
    float range,
    float duration,
    float knockback,
    int damage
    )
  {
    Duration = duration;
    Knockback = knockback;
    Range = range;
    Speed = speed;
    Damage = damage;
  }

  public float Duration { get; }
  public float Knockback { get; }
  public float Range { get; }
  //defined as attacks per second
  public float Speed { get; }
  public float Damage { get; }
}

public class PlayerController : MonoBehaviour
{
    private IDictionary<AttackLevel, AttackLevelStats> weaponLevelStats = new Dictionary<AttackLevel, AttackLevelStats>()
    {
        { AttackLevel.One, new AttackLevelStats(speed: 1.5f, range: 1.0f, duration: 0.05f, knockback: 1.0f, damage: 1) },
        { AttackLevel.Two, new AttackLevelStats(speed: 5.0f, range: 4.0f, duration: 1.00f, knockback: 1.4f, damage: 1) },
        { AttackLevel.Three, new AttackLevelStats(speed: 5.0f, range: 4.0f, duration: 1.00f, knockback: 1.4f, damage: 1) },
        { AttackLevel.Four, new AttackLevelStats(speed: 8.0f, range: 6.0f, duration: 1.00f, knockback: 1.4f, damage: 1) }
    };
    [SerializeField] private AttackLevel attackLevel = AttackLevel.One;
    public float moveSpeed;
    public float autoAttackMovementDelay;
    public GameObject autoAttackPrefab;

    private float lastAuttoAttackTime;
    private bool isMovementBlockedByAttack = false;
    private Vector3 playerBounds;
    private Camera viewCamera;

    public void Start() {
        playerBounds = GetComponent<SpriteRenderer>().bounds.extents;
        viewCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

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
        Vector3 mouseWorldPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        Vector3 mouseDirection = (mouseWorldPos - transform.position).normalized;

        // Attack stats
        var stats = getAttackStats();

        // Attack
        if (attackLevel == AttackLevel.Three) {
            // Spawn multiple fireballs at angles
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, 10) * mouseDirection);
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, -10) * mouseDirection);
        }
        else if (attackLevel == AttackLevel.Four) {
            // Spawn multiple fireballs at angles
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, 15) * mouseDirection);
            spawnAttack(stats: stats, direction: mouseDirection);
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, -15) * mouseDirection);
        } else {
            spawnAttack(stats: stats, direction: mouseDirection);
        }
    }
    
    private void spawnAttack(AttackLevelStats stats, Vector3 direction) {
        Debug.Log("Spawning attack");
        // Create an autoattack
        // Position the projectile at the player's position, but outside the player's sprite bounds
        Vector3 pos = transform.position + direction * playerBounds.x*2;
        GameObject go = Instantiate(autoAttackPrefab, pos, Quaternion.Euler(new Vector3(0, 0, 0)));
        AutoAttack autoAttack = go.GetComponent<AutoAttack>();

        // Set its properties
        autoAttack.Damage = stats.Damage;
        autoAttack.Range = stats.Range;
        autoAttack.Speed = stats.Speed;
        autoAttack.Direction = direction;
        autoAttack.Knockback = direction * stats.Knockback;

        // Mark it for destruction after some time
        autoAttack.DestroyAfterTime(stats.Duration);
    }

    public bool CanMove()
    {
        return !isMovementBlockedByAttack;
    }

    public bool CanAutoAttack()
    {
        return Time.time >= lastAuttoAttackTime + 1f/ getAttackStats().Speed;
    }

    // Attack stats for the current weapon level
    public AttackLevelStats getAttackStats() {
        return weaponLevelStats[attackLevel];
    }

    private AttackLevel getNextWeaponLevel() {
        switch(attackLevel) {
            case AttackLevel.One:
                return AttackLevel.Two;
            case AttackLevel.Two:
                return AttackLevel.Three;
            case AttackLevel.Three:
                return AttackLevel.One;
            default:
                return AttackLevel.One;
        }
    }
}
