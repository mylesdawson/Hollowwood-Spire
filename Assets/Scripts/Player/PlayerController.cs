using System;
using System.Collections.Generic;
using UnityEngine;

public enum WalledStatus
{
    None,
    Left,
    Right
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(JumpManager))]
[RequireComponent(typeof(GravityManager))]
[RequireComponent(typeof(MoveManager))]
[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(DashManager))]
[RequireComponent(typeof(HitManager))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public StateManager stateManager;
    [HideInInspector] public JumpManager jumpManager;
    public GravityConfig gravityConfig = new();
    GravityManager gravityManager;
    [HideInInspector] public MoveManager moveManager;
    [HideInInspector] public AttackManager attackManager;
    [HideInInspector] public HitManager hitManager;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] LayerMask terrainLayer;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [SerializeField] public CapsuleCollider2D bodyCollider;
    // [SerializeField] CapsuleCollider2D feetCollider;
    public CharacterController playerInputActions;
    [HideInInspector] public WalledStatus leftWallAs = WalledStatus.None;
    [HideInInspector] public DashManager dashManager;
    public ActionContext actionContext;
    PlayerConfig playerConfig;

    public Action<Collision2D> OnCollideWithEnemy;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        stateManager = GetComponent<StateManager>();
        jumpManager = GetComponent<JumpManager>();
        gravityManager = GetComponent<GravityManager>();
        moveManager = GetComponent<MoveManager>();
        attackManager = GetComponent<AttackManager>();  
        playerInputActions = new CharacterController();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // abilitiesManager = GetComponent<AbilitiesManager>();
        dashManager = GetComponent<DashManager>();
        hitManager = GetComponent<HitManager>();

        actionContext = new ActionContext();


        var attackPrefab = (GameObject)Resources.Load("Attack");
        playerConfig = new PlayerConfig(
            playerName: PlayerNames.TheChosen,
            moveConfig: new MoveConfig(),
            jumpAbility: new RegularJump(jumpManager.jumpStatMutations),
            gravityConfig: new GravityConfig(),
            dashAbility: new RegularDash(),
            attackAbility: new RegularAttack(attackPrefab.transform)
        );

        dashManager.Initialize(playerConfig.DashAbility);
        attackManager.Initialize(playerConfig.AttackAbility);
        gravityManager.Initialize(playerConfig.GravityConfig);
        moveManager.Initialize(playerConfig.MoveConfig);
        jumpManager.Initialize(playerConfig.JumpAbility);
    }

    void Start()
    {
        GatherActionContext();
        stateManager.StartMachine(actionContext);
    }

    void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        Vector2 movementInput = playerInputActions.Player.Movement.ReadValue<Vector2>();
        var xAxis = movementInput.x;

        GatherActionContext();

        stateManager.UpdateState(actionContext);

        moveManager.OnUpdate(actionContext);
        attackManager.OnUpdate(actionContext);
        jumpManager.OnUpdate(actionContext);
        dashManager.OnUpdate(actionContext);
        gravityManager.OnUpdate(actionContext, Time.deltaTime);
        hitManager.OnUpdate(actionContext);

        // Animations
        // if(stateManager.currentState is Airborne)
        // {
        //     if (rb.linearVelocity.y > 0)
        //     {
        //         animator.SetBool("isJumping", true);
        //     }
        //     else
        //     {
        //         animator.SetBool("isFalling", true);
        //     }
        // }
        // else
        // {
        //     animator.SetBool("isJumping", false);
        //     animator.SetBool("isFalling", false);
        // }

        // if(stateManager.currentState is Grounded)
        // {
        //     if (xAxis > 0)
        //     {
        //         animator.SetBool("isRunning", true);
        //     }
        //     else if (xAxis < 0)
        //     {
        //         animator.SetBool("isRunning", true);
        //     }
        //     else
        //     {
        //         animator.SetBool("isRunning", false);
        //     }
        // } else
        // {
        //     animator.SetBool("isRunning", false);
        // }

        // Flip sprite direction
        if (xAxis > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (xAxis < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    // use fixed update for physics updates on rigidbody
    void FixedUpdate()
    {
        rb.linearVelocity = actionContext.Velocity;
    }

    public bool IsGrounded()
    {
        float groundCheckDistance = .2f;
        Vector2 origin = new Vector2(bodyCollider.bounds.center.x, bodyCollider.bounds.min.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, terrainLayer);
        return hit.collider != null;
    }

    public WalledStatus GetWalledStatus()
    {
        float wallCheckDistance = .3f;
        Vector2 origin = bodyCollider.bounds.center;
        Vector2 size = bodyCollider.bounds.extents;
        RaycastHit2D hitRight = Physics2D.Raycast(origin, Vector2.right, size.x + wallCheckDistance, terrainLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, Vector2.left, size.x + wallCheckDistance, terrainLayer);

        var lWalled = hitLeft ? true : false;
        var rWalled = hitRight ? true : false;

        if (lWalled) return WalledStatus.Left;
        if (rWalled) return WalledStatus.Right;
        return WalledStatus.None;
    }

    public void ResetMovementAbilities()
    {
        dashManager.dashAbility?.dashConfig.ResetRemainingDashes(dashManager.dashStatMutations);
        jumpManager.jump.numJumps = (int)jumpManager.jump.config.GetStat(AbilityStat.maxJumps, jumpManager.jumpStatMutations);
    }

    public int GetDirectionalityBasedOnSpriteFlip()
    {
        return spriteRenderer.flipX ? -1 : 1;
    }

    public (bool didCollide, List<Collider2D> results) CollidedWithEnemy()
    {
        if(gameObject.layer == LayerMask.NameToLayer("InvinciblePlayer")) return (false, new List<Collider2D>());

        ContactFilter2D filter = new();
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;
        var bodyResult = new List<Collider2D>();
        // var footResult = new List<Collider2D>();
        int bodyCount = bodyCollider.Overlap(filter, bodyResult);
        // int footCount = feetCollider.Overlap(filter, footResult);

        var didCollide = bodyCount > 0;
        var combinedResults = new List<Collider2D>();
        combinedResults.AddRange(bodyResult);
        // combinedResults.AddRange(footResult);

        return (didCollide, combinedResults);
    }

    public void GatherActionContext()
    {
        actionContext.DidDashThisFrame = playerInputActions.Player.Dash.WasPressedThisFrame();
        actionContext.DidAttackThisFrame = playerInputActions.Player.Attack.WasPressedThisFrame();
        actionContext.DidJumpThisFrame = playerInputActions.Player.Jump.WasPressedThisFrame();
        actionContext.IsCurrentlyJumping = playerInputActions.Player.Jump.IsPressed();
        actionContext.MovementInput = playerInputActions.Player.Movement.ReadValue<Vector2>();
        actionContext.CurrentDirection = GetDirectionalityBasedOnSpriteFlip();
        actionContext.GravityConfig = gravityManager.config;
        actionContext.Rigidbody = this.rb;
        actionContext.Transform = this.transform;
        actionContext.SpriteRenderer = this.spriteRenderer;
        actionContext.IsDashLocked = actionContext.IsDashLocked;
        actionContext.CurrentState = stateManager.currentState;
        actionContext.PreviousState = stateManager.previousState;
        actionContext.WalledStatus = GetWalledStatus();
        actionContext.IsGrounded = IsGrounded();
        actionContext.ResetMovementAbilities = ResetMovementAbilities;
        var (_, Collisions) = CollidedWithEnemy();
        actionContext.CollisionsWithEnemies = Collisions;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            OnCollideWithEnemy?.Invoke(collision);
        }
    }
}
