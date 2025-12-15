using UnityEngine;

public class _Anim_Controller : MonoBehaviour
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

    public StringGameEvent NextAnimationEvent;
    public Transform playerTransform;


    bool basicAttack;
    bool jump;
    bool grounded;
    bool freeFall;
    string next;
    float Direciton;

    [Header("Stopping Bools")]
    public bool stopAttacks;
    public bool walkingStop;
    public bool TotalStop;
    public bool stopJump;


    public void MainAnimations()
    {
        if (TotalStop) return;

        if (Direciton > 0 && !basicAttack)
        {
            playerTransform.rotation = Quaternion.Euler(0, 180, 0); // Looking to the left
        }
        
        else if (Direciton < 0 && !basicAttack)
        {
            playerTransform.rotation = Quaternion.Euler(0, 0, 0);   // Looking to the right
        }

        if (Mathf.Abs(Direciton) > 0.01f && !walkingStop)
        {
            next = "Walking";
        }
        else
        {
            next = "Idle";
        }        
        if (freeFall && !grounded && !basicAttack)
        {
            next = "FreeFall";
        }
        if (jump && !grounded && !stopJump && !basicAttack)
        {
            next = "Jump";
        }
        if (basicAttack && !stopAttacks)
        {
            next = "BasicAttack";
        }

        NextAnimationEvent.TriggerEvent(next);
    }

    public void Direction(float state)
    {
        Direciton = state;
        MainAnimations();
    }

    public void BasicAttackButton(bool state)
    {
        basicAttack = state;
        MainAnimations();
    }

    public void JumpButton(bool state)
    {
        jump = state;
        MainAnimations();
    }

    public void IsGrounded(bool state)
    {
        grounded = state;
        MainAnimations();
    }

    public void FreeFall(bool state)
    {
        freeFall = state;
        MainAnimations();
    }

}