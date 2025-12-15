using UnityEngine;

public class _Jump : MonoBehaviour
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

    [Header("Player Parameters")]
    public Player_Parameters _parameters;

    [Header("Dynamic Script Parameters")]
    public bool isGrounded;
    public bool jumpCut;
    public bool jumpKey;

    public Vector2 velocity;

    private float lastGroundedTime;
    private float lastJumpPressedTime;

    public FloatGameEvent JumpForce;
    public BoolGameEvent JumpCutEvent;


    public void IsGrounded(bool state)
    {
        isGrounded = state;
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            jumpCut = false;
            JumpCutEvent.TriggerEvent(false);
        }
    }

    public void JumpKey(bool pressed)
    {
        jumpKey = pressed;
        if (pressed)
        {
            lastJumpPressedTime = Time.time;
            CanJump();
        }
        else
        {
            jumpCut = true;
            JumpCutEvent.TriggerEvent(true);
            velocity.y = 0;
        }
        
    }

    public void Jump()
    {
        float jumpSpeed = Mathf.Sqrt(2f * Mathf.Abs(Physics2D.gravity.y) * _parameters.jumpHeight);
        JumpForce.TriggerEvent(jumpSpeed); // manda só o valor
        jumpCut = false;
        JumpCutEvent.TriggerEvent(false);
        lastJumpPressedTime = -999f;
        lastGroundedTime = -999f;
    }


    public void CanJump()
    {
        if (Time.time - lastGroundedTime <= _parameters.coyoteTimeThreshold && Time.time - lastJumpPressedTime <= _parameters.jumpBuffer) // && !stagger_Porta            
        { 
            Jump(); 
        }
    }
}
