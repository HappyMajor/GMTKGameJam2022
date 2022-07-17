using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int damage,
    float cooldown
    )
  {
    Duration = duration;
    Knockback = knockback;
    Range = range;
    Speed = speed;
    Damage = damage;
    Cooldown = cooldown;
  }

  public float Duration { get; }
  public float Knockback { get; }
  public float Range { get; }
  public float Speed { get; }
  public float Damage { get; }
  //defined as attacks per second
  public float Cooldown { get; }
}

public class PlayerController : MonoBehaviour
{
    private IDictionary<AttackLevel, AttackLevelStats> weaponLevelStats = new Dictionary<AttackLevel, AttackLevelStats>()
    {
        { AttackLevel.One, new AttackLevelStats(speed: 1.5f, range: 1.0f, duration: 0.05f, knockback: 1.0f, damage: 1, cooldown: 2.0f) },
        { AttackLevel.Two, new AttackLevelStats(speed: 5.0f, range: 4.0f, duration: 1.00f, knockback: 1.4f, damage: 1, cooldown: 2.0f) },
        { AttackLevel.Three, new AttackLevelStats(speed: 5.0f, range: 4.0f, duration: 1.00f, knockback: 1.4f, damage: 1, cooldown: 1.0f) },
        { AttackLevel.Four, new AttackLevelStats(speed: 8.0f, range: 6.0f, duration: 1.00f, knockback: 1.4f, damage: 1, cooldown: 0.8f) }
    };
    [SerializeField] private AttackLevel attackLevel = AttackLevel.One;

    public float health = 3;
    public float maxHealth = 3;
    public float gold = 0;

    public float moveSpeed;
    public float autoAttackMovementDelay;
    public Slider healthBarSlider;
    public GameObject autoAttackPrefab;

    private bool isMovementBlockedByAttack = false;
    private Vector3 playerBounds;
    private Camera viewCamera;
    private float attackCooldown;
    private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;

    public void Start() {
        playerBounds = GetComponent<SpriteRenderer>().bounds.extents;
        viewCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        CheckAndDoAttack();
    }

    private void FixedUpdate()
    {
        CheckAndDoMovement();
    }

    private void SetHealth(float value)
    {
        this.health = value;
        healthBarSlider.value = value / maxHealth;
        if (value <= 0)
        {
            //DEATH
        }
    }

    public void SetGold(float value)
    {
        this.gold = value;
    }

    public void CheckAndDoMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(CanMove())
        {
            Vector3 motion = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
            //transform.position = transform.position + motion;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + motion);
        }
    }

    public void CheckAndDoAttack()
    {
        // Update attack cooldown counter
        attackCooldown -= Time.deltaTime;

        // Can't attack during cooldown
        if (attackCooldown > 0) {
            return;
        }

        // Attack if pressing the attack button
        if (Input.GetMouseButtonDown(0)) {
            Attack();
        }
    }

    public void Attack()
    {
        // Attack stats
        var stats = getAttackStats();

        // We're going to attack, so set the cooldown before next attack is allowed
        attackCooldown += stats.Cooldown;

        // Can't move while attacking
        isMovementBlockedByAttack = true;

        // Attack direction
        var direction = getMouseDirection();

        // Create the fireballs
        CreateAttackEffect(stats: stats, direction: direction);

        // Allow player to move again after a delay
        StartCoroutine(Routines.DoLater(autoAttackMovementDelay, () =>
        {
            isMovementBlockedByAttack = false;
        }));
    }

    public void CreateAttackEffect(AttackLevelStats stats, Vector3 direction)
    {
        // Attack
        if (attackLevel == AttackLevel.Three) {
            // Spawn multiple fireballs at angles
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, 10) * direction);
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, -10) * direction);
        }
        else if (attackLevel == AttackLevel.Four) {
            // Spawn multiple fireballs at angles
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, 15) * direction);
            spawnAttack(stats: stats, direction: direction);
            spawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, -15) * direction);
        } else {
            spawnAttack(stats: stats, direction: direction);
        }
    }
    
    private Vector3 getMouseDirection() {
        Vector3 mouseWorldPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        
        return (mouseWorldPos - transform.position).normalized;
    }
    
    private void spawnAttack(AttackLevelStats stats, Vector3 direction) {
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

        // Play the attack sound
        audioSource.PlayOneShot(attackSound);
    }

    public bool CanMove()
    {
        return !isMovementBlockedByAttack;
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
                return AttackLevel.Four;
            case AttackLevel.Four:
                return AttackLevel.One;
            default:
                return AttackLevel.One;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            SetHealth(health - 1);
        }
        if(collision.gameObject.tag == "Consumable")
        {
            Debug.Log("HIT CONSUMABLE!");
            if (collision.gameObject.GetComponent<Money>() != null)
            {
                Destroy(collision.gameObject);
                SetGold(gold + 1);
            }
            if (collision.gameObject.GetComponent<SpeedPotion>() != null)
            {
                Destroy(collision.gameObject);
                moveSpeed += 1f;
            }
            if (collision.gameObject.GetComponent<HealthPotion>() != null)
            {
                Destroy(collision.gameObject);
                if(health + 1 >= maxHealth)
                {
                    SetHealth(maxHealth);
                } else
                {
                    SetHealth(health);
                }
            }
            if (collision.gameObject.GetComponent<Powerup>() != null)
            {
                Destroy(collision.gameObject);
                this.attackLevel = getNextWeaponLevel();
            }
        }
    }
}
