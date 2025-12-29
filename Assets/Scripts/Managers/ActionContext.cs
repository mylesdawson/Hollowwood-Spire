using System;
using System.Collections.Generic;
using UnityEngine;

// Context object gets passed to all possible actions
public class ActionContext
{
    public Transform Transform { get; set; }
    public Rigidbody2D Rigidbody { get; set; }
    public SpriteRenderer SpriteRenderer { get; set;}
    public WalledStatus WalledStatus { get; set; }
    public WalledStatus LeftWallAs { get; set; }
    public List<Collider2D> CollisionsWithEnemies { get; set; }
    public bool IsGrounded { get; set; }
    public BaseState CurrentState { get; set; }
    public BaseState PreviousState { get; set; }

    public bool DidDashThisFrame { get; set; }
    public bool DidAttackThisFrame { get; set; }
    public bool DidJumpThisFrame { get; set; }
    public bool IsCurrentlyJumping { get; set; }
    public bool DidReleaseJumpThisFrame { get; set; }
    
    public Vector2 MovementInput { get; set; }
    public int CurrentDirection { get; set; }
    private Vector2 velocity;
    public float VelocityX
    {
        get => velocity.x;
        set => velocity.x = value;
    }
    public float VelocityY
    {
        get => velocity.y;
        set => velocity.y = value;
    }
    public Vector2 Velocity
    {
        get => velocity;
        set => velocity = value;
    }

    public GravityConfig GravityConfig { get; set; }

    public bool IsDashLocked { get; set; }
    public bool IsDashing { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsHit { get; set; }
    public bool IsAttackLocked { get; set; }
    public bool IsWallJumping { get; set; }
    public Action ResetMovementAbilities { get; set; }
}