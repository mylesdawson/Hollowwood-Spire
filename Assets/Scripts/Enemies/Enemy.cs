


using UnityEngine;

[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(Healthable))]
[RequireComponent(typeof(Bouncable))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[DisallowMultipleComponent]
public class Enemy : MonoBehaviour
{
    LootRarity lootRarity = LootRarity.standard;
    [SerializeField] ParticleSystem hitEffect;
    [SerializeField] FlashWhite flashWhite;
    [HideInInspector] public Collider2D enemyCollider;
    [SerializeField] LayerMask terrainLayer;
    [HideInInspector] public Rigidbody2D rb;
    Attackable attackable;
    Healthable healthable;
    Bouncable bouncable;

    [SerializeField] float speed = 3f;
    [SerializeField] float defaultSpeed = 3f;

    // Knockback / stun
    [SerializeField] float knockbackStrength = 10f;
    [SerializeField] float knockbackDuration = 0.2f;

    bool stunned = false;
    float stunTimer = 0f;

    void Awake()
    {
        attackable = GetComponent<Attackable>();
        healthable = GetComponent<Healthable>();
        bouncable = GetComponent<Bouncable>();
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();

    }

    void Start()
    {
        defaultSpeed = speed;
    }

    void OnEnable()
    {
        attackable.onAttacked.AddListener(HandleOnAttacked);
        healthable.onDie.AddListener(OnDie);
    }

    void OnDisable()
    {
        attackable.onAttacked.RemoveListener(HandleOnAttacked);
        healthable.onDie.RemoveListener(OnDie);
    }

    void Update()
    {
        // Handle stun timer first
        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                stunned = false;
            }
            // while stunned, let physics (velocity) handle movement; skip patrol logic
            return;
        }

        float wallCheckDistance = .1f;
        Vector2 origin = enemyCollider.bounds.center;
        Vector2 size = enemyCollider.bounds.extents;
        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, size.x + wallCheckDistance, terrainLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, Vector2.left, size.x + wallCheckDistance, terrainLayer);

        if (hitRight.collider != null)
        {
            speed = -Mathf.Abs(speed);
        }
        else if (hitLeft.collider != null)
        {
            speed = Mathf.Abs(speed);
        }

        // Apply horizontal patrol velocity; preserve current vertical velocity
        rb.linearVelocityX = speed;
    }

    // Call this to apply a knockback to the enemy. direction should be non-zero and will be normalized.
    public void OnHit(Vector2 direction, float strength = -1f, float duration = -1f)
    {
        if (strength <= 0f) strength = knockbackStrength;
        if (duration <= 0f) duration = knockbackDuration;

        stunned = true;
        stunTimer = duration;

        // Apply an immediate velocity change (overrides current velocity)
        if (direction == Vector2.zero) direction = Vector2.up;
        rb.linearVelocityX = direction.normalized.x * strength;
        rb.linearVelocityY = direction.normalized.y * strength;

        if(flashWhite) flashWhite.Flash();
        SpawnDamageParticles(direction);
        // SoundEffectsManager.Instance.PlayEffect("EnemyHit");
    }

    private void SpawnDamageParticles(Vector2 direction)
    {
        if (hitEffect != null)
        {
            var dir = Quaternion.FromToRotation(Vector2.right, direction);  

            ParticleSystem effect = Instantiate(hitEffect, transform.position, dir);
            effect.Play();
            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }

    void HandleOnAttacked(float damage, Vector2 direction, float knockbackStrength, float hitStop = 0f)
    {
        Debug.Log("enemy took: " + damage + " damage");
        var strength = knockbackStrength;
        var duration = .1f;

        stunned = true;
        stunTimer = duration;

        // Apply an immediate velocity change (overrides current velocity)
        if (direction == Vector2.zero) direction = Vector2.up;
        rb.linearVelocityX = direction.normalized.x * strength;
        rb.linearVelocityY = direction.normalized.y * strength;


        // Hit stop here???
        if(flashWhite) flashWhite.Flash();
        SpawnDamageParticles(direction);
        // SoundEffectsManager.Instance.PlayEffect("EnemyHit");

        healthable.LoseHealth(damage);
    }

    void OnDie()
    {
        EventBus.Instance.onEnemyDeath?.Invoke(this.gameObject);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.TryGetComponent<Attackable>(out var attackable))
            {
                attackable.TakeDamage(10, Vector2.zero, 0);
            }
        }
    }
}