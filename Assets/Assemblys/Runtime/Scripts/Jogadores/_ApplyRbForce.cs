using NUnit.Framework.Internal.Builders;
using UnityEngine;

public class _ApplyRbForce : MonoBehaviour
{
    #region Smart Inspector Attributes
    [Header("Smart Inspector Attributes")]

    public string CustomName;
    public Texture2D CustomIcon;
    public Colors CustomCollor;
    public enum Colors
    {
        white, red, green, blue, black, gray, yellow, cyan, magenta, clear
    }
    #endregion

    [Header("Parameters")]
    public Player_Parameters _parameters;

    [Header("GRAVITY")]
    public Vector2 velocity;
    public bool jumpCut;
    bool jumped;
    public int lastDirection;
    public bool isGrounded;
    public Rigidbody2D rb;
    public float apexPoint;

    public float HorizontalMovementForce;
    public float JumpForce;
    public float knockbackForce;
    public bool verticalKnockbackDone;
    public bool horizontalKnockbackDone;
    float previousJumpForce;
    public float GravityForce;

    [Header("Game Events")]
    public Vec2GameEvent RBLinearVelocity;


    public BoolGameEvent IsGroundedEvent;
    public BoolGameEvent FreeFallEvent;

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        Ground_Box();
    }

    private void FixedUpdate()
    {
        // Sends the LinearVelocity Data
        RBLinearVelocity.TriggerEvent(rb.linearVelocity);


        velocity = rb.linearVelocity;

        ApplyGravity();
        CalculateJumpApex();
        CalculateNextVelocity();

        if (velocity.x > 8) { Debug.Log(velocity.x);}
        
        rb.linearVelocity = velocity;
    }

    void CalculateNextVelocity()
    {
        velocity.x = HorizontalMovementForce;
        if (horizontalKnockbackDone && !isGrounded)
        {
            velocity.x += knockbackForce;
            horizontalKnockbackDone = false;
        }
        

        if (JumpForce != 0)
        {
            velocity.y = JumpForce;
            previousJumpForce = JumpForce;
            JumpForce = 0;
        }
        if (knockbackForce != 0 && verticalKnockbackDone) // da glicth
        {
            velocity.y = knockbackForce;
            knockbackForce = 0;
        }
    }


    public void X_Force(float force)
    {
        HorizontalMovementForce = force;
    }

    public void ApplyJumpForce(float force)
    {
        JumpForce = force;
        jumped = true;
    }

    public void SelfKnockback(Vector2 knock) // x = direction, y = force
    {
        int dir = Mathf.RoundToInt(knock.x);

        // Bloqueia reaplicação de knock horizontal se já houver knock ativo
        if ((dir == 1 || dir == 2) && (verticalKnockbackDone || horizontalKnockbackDone))
            return;

        // Se estivermos no chão, ignoramos knock vertical (não faz sentido pogo quando já no chão)
        if ((dir == 3 || dir == 4) && isGrounded)
            return;

        switch (dir) // horizontalknockbackdone sempre on, e quando bate e vira de um lado pro outro rapido ta bugado
        {
            case 4:
                knockbackForce = knock.y;
                verticalKnockbackDone = true;
                break;
            case 3:
                knockbackForce = -knock.y;
                verticalKnockbackDone = true;
                break;
            case 2:
                knockbackForce = -knock.y;
                horizontalKnockbackDone = true;
                break;
            case 1:
                knockbackForce = knock.y;
                horizontalKnockbackDone = true;
                break;
        }
    }

    public void JumpCut(bool state)
    {
        jumpCut = state;
        if (state)
        {
            FreeFallEvent.TriggerEvent(true);
        }
    }

    private void CalculateJumpApex()
    {
        if (!isGrounded)
            apexPoint = Mathf.InverseLerp(_parameters.jumpApexThreshold, 0, Mathf.Abs(rb.linearVelocity.y));
        else
            apexPoint = 0;

        if (apexPoint > 0.9f)
            FreeFallEvent.TriggerEvent(true);

        if (previousJumpForce > 0 && velocity.y <= 0)
            FreeFallEvent.TriggerEvent(true);
    }

    private void ApplyGravity()
    {
        float gravityScale = 1f; // Default gravity scale

        if (jumpCut) // Early jump cut
        {
            gravityScale = _parameters.jumpEndEarlyGravityModifier;
        }

        if (verticalKnockbackDone) // Reset gravity after knockback, cancel jump cut
        {
            gravityScale = 1f;
        }

        if (velocity.y < 0) // Falling
        {
            gravityScale = _parameters.fallGravityMultiplier;
        }

        if (!isGrounded) // Apply gravity if not grounded
        {
            velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;
        }
        else // Grounded, reset states
        {
            verticalKnockbackDone = false;
            jumped = false;
        }

        // Clamp the speed to a maximum value
        velocity.y = Mathf.Clamp(velocity.y, _parameters.fallClamp, _parameters.maxFallSpeed);
    }
    
    
    #region Ground Check
    private void Ground_Box()
    {
        Vector2 origin = (Vector2)transform.position + _parameters.groundBoxCenter;
        Vector2 size = _parameters.groundBoxSize;
        isGrounded = Check_Collision_Box(origin, size, Vector2.down, _parameters.groundLayer);
        IsGroundedEvent.TriggerEvent(isGrounded);
        GroundedParameters();
    }

    private bool Check_Collision_Box(Vector2 origin, Vector2 size, Vector2 direction, LayerMask _layer)
    {
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, direction, 0f, _layer);
        return hit.collider != null;
    }
    public void GroundedParameters()
    {
        if (isGrounded)
        {
            FreeFallEvent.TriggerEvent(false);
            previousJumpForce = 0;
            jumped = false;
        }
        else
        {
            if (!jumped)
            {
                FreeFallEvent.TriggerEvent(true);
            }
        }        
    }
    #endregion

}
