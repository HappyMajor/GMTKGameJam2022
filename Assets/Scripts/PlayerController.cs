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
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float invulnerableAfterDamageTime = 0.25f;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private float cameraShakeOnDamageIntensityPerHp = 0.5f;
    [SerializeField] private float cameraShakeOnDamageTime = 0.1f;
    [SerializeField] private TMPro.TextMeshProUGUI goldText;
    [SerializeField] private TMPro.TextMeshProUGUI attackLevelText;
    [SerializeField] private CanvasRenderer goldCountCanvasRenderer;
    [SerializeField] private CanvasRenderer attackLevelCanvasRenderer;
    public float health = 3;
    public float maxHealth = 3;
    public float gold = 0;
    public float moveSpeed;
    public float autoAttackMovementDelay;
    public Slider healthBarSlider;
    public GameObject autoAttackPrefab;
    public GameObject replayMenu;
    public bool invulnerable = false;
    public bool isInShock = false;
    public float shockDuration = 0.15f;
    private bool isMovementBlockedByAttack = false;
    private Vector3 playerBounds;
    private Camera viewCamera;
    private float attackCooldown;
    private AudioSource audioSource;
    private Animator animator;
    private Rigidbody2D rigidBody;

    public void Start() {
        playerBounds = GetComponent<SpriteRenderer>().bounds.extents;
        viewCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
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
        // Update health
        this.health = Mathf.Max(value, 0);

        // Update health bar
        healthBarSlider.value = value / maxHealth;

        // Die if out of health
        if (value <= 0)
        {
            die();
        }
    }

    private void die() {
        // Play death sound
        OneShotAudio.Play("event:/sfx/player/death", transform, rigidBody);

        // Show replay menu
        replayMenu.SetActive(true);

        // Deactivate the player
        this.gameObject.SetActive(false);
    }

    public void SetGold(float value)
    {
        // Increased gold?
        if (value > gold) {
            // Bling flash the gold text
            goldText.GetComponent<TextFlash>().Flash();

            // Shake the gold display
            goldCountCanvasRenderer.GetComponent<HudElementShake>().Shake();
        }

        // Store the updated gold
        gold = value;

        // Update HUD text for gold count
        goldText.SetText(gold.ToString());
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
            SpawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, 10) * direction);
            SpawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, -10) * direction);
        }
        else if (attackLevel == AttackLevel.Four) {
            // Spawn multiple fireballs at angles
            SpawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, 15) * direction);
            SpawnAttack(stats: stats, direction: direction);
            SpawnAttack(stats: stats, direction: Quaternion.Euler(0, 0, -15) * direction);
        } else {
            SpawnAttack(stats: stats, direction: direction);
        }
    }
    
    private Vector3 getMouseDirection() {
        Vector3 mouseWorldPos = viewCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        
        return (mouseWorldPos - transform.position).normalized;
    }
    
    private void SpawnAttack(AttackLevelStats stats, Vector3 direction) {
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
        return !isMovementBlockedByAttack && !isInShock;
    }

    // Attack stats for the current weapon level
    public AttackLevelStats getAttackStats() {
        return weaponLevelStats[attackLevel];
    }

    private AttackLevel getNextAttackLevel() {
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

    private int getAttackLevelNum(AttackLevel attackLevel) {
        switch(attackLevel) {
            case AttackLevel.One:
                return 1;
            case AttackLevel.Two:
                return 2;
            case AttackLevel.Three:
                return 3;
            case AttackLevel.Four:
                return 4;
            default:
                throw new System.Exception("Invalid attack level: " + attackLevel);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            OnCollideWithMonster(collision.gameObject);
        }
       
    }

    public void MakeInvulnerableForSeconds(float seconds)
    {
        invulnerable = true;

        // NOTE: If we try to start the coroutine after play death it throws an error:
        // "Coroutine couldn't be started because the the game object 'Player' is inactive!"
        // So checking if still alive before scheduling the coroutine.
        if (health > 0) {
            StartCoroutine(Routines.DoLater(seconds, () =>
            {
                invulnerable = false;
            }));
        }
    }

    public void ApplyKnockback(Vector3 knockback)
    {
        isInShock = true;
        rigidBody.AddForce(knockback, ForceMode2D.Impulse);

        // NOTE: If we try to start the coroutine after play death it throws an error:
        // "Coroutine couldn't be started because the the game object 'Player' is inactive!"
        // So checking if still alive before scheduling the coroutine.
        if (health > 0) {
            StartCoroutine(Routines.DoLater(shockDuration, () =>
            {
                isInShock = false;
                rigidBody.velocity = new Vector3(0, 0, 0);
            }));
        }
    }


    public void ApplyDamage(int damage, Vector3 knockback) 
    {
        // Base behavior
        ApplyDamage(damage);

        // Knockback
        ApplyKnockback(knockback);
    }

    public void ApplyDamage(int damage)
    {
        // Lose one HP
        SetHealth(health - damage);

        // Temporarily invulnerable after taking damage
        MakeInvulnerableForSeconds(invulnerableAfterDamageTime);

        // Play hurt sound
        OneShotAudio.Play("event:/sfx/player/hurt", transform, rigidBody);

        // Hurt animation
        animator.SetTrigger("Hurt");

        // Shake camera when hit
        cameraShake.ShakeCamera(
            intensity: damage * cameraShakeOnDamageIntensityPerHp,
            time: cameraShakeOnDamageTime);
    }

    private void MonsterDamagesPlayer(GameObject monsterObj)
    {
        if (invulnerable) {
            return;
        }

        IMonster monster = monsterObj.GetComponent<IMonster>();
        // Knock back the monster
        monster.ApplyKnockback((monsterObj.transform.position - transform.position).normalized * 3);
        // Damage the monster
        monster.ApplyDamage(1f);

        // Damage the player
        var knockback = (transform.position- monsterObj.transform.position).normalized * 3;
        ApplyDamage(1, knockback);
    }

    private void OnCollideWithMonster(GameObject monster)
    {
        Skeleton skeleton = monster.GetComponent<Skeleton>();

        if (skeleton != null)
        {
            MonsterDamagesPlayer(monster);
        }
    }


    private void OnCollidWithConsumable(GameObject consumable)
    {
        if (consumable.GetComponent<Money>() != null)
        {
            var money = consumable.gameObject.GetComponent<Money>();
            consumable.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            consumable.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(consumable.gameObject);
            SetGold(gold + 1);
        }
        else if (consumable.gameObject.GetComponent<SpeedPotion>() != null)
        {
            var potion = consumable.gameObject.GetComponent<SpeedPotion>();
            consumable.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            consumable.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(consumable.gameObject);
            moveSpeed += 1f;
        }
        else if (consumable.gameObject.GetComponent<HealthPotion>() != null)
        {
            var potion = consumable.gameObject.GetComponent<HealthPotion>();
            consumable.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            consumable.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(consumable.gameObject);

            if (health + 1 >= maxHealth)
            {
                SetHealth(maxHealth);
            }
            else
            {
                SetHealth(health + 1);
            }
        }
        else if (consumable.gameObject.GetComponent<Powerup>() != null)
        {
            // Destroy the powerup
            Destroy(consumable.gameObject);

            // Level up player
            levelUp();
        }
    }

    void levelUp() {
        // Set new attack level
        attackLevel = getNextAttackLevel();

        // Update HUD for new level
        var text = GameObject.Find("Attack Level Text").GetComponent<TMPro.TMP_Text>();
        string newString = string.Format("Lvl {0}", getAttackLevelNum(attackLevel));
        text.SetText(newString);

        // Flash the level counter
        attackLevelText.GetComponent<TextFlash>().Flash();

        // Shake the level counter display
        attackLevelCanvasRenderer.GetComponent<HudElementShake>().Shake();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Consumable")
        {
            OnCollidWithConsumable(collision.gameObject);
        }
    }
}
