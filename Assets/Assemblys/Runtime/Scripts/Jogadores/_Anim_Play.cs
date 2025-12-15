using UnityEngine;

public class _Anim_Play : MonoBehaviour
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

    public Animator animator;
    public string currentAnimation;
    public string nextAnimation;
    public float transitionTime = 0.1f;

    [Header("Currtent Animation Event Signal")]
    public StringGameEvent CurrentAnimationEvent;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public void NextAnim(string nextAnimation)
    {
        if (nextAnimation != currentAnimation)
        {
            currentAnimation = nextAnimation;
            CurrentAnimationEvent?.TriggerEvent(currentAnimation);
            animator.CrossFade(currentAnimation, transitionTime);
        }
    }



    /* Old Code walking and stoping animation
        public void WalkingAndStoping(float Direciton)
        {
            if (Mathf.Abs(Direciton) > 0.01f)
            {
                if (!basicAttackButton)
                {
                    animator.Play("Walking");
                }
                else
                {
                    animator.Play("Walking+BasicAttack");
                }
                // Rotaciona para esquerda ou direita usando EulerAngles
                if (Direciton > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0); // olhando pra esquerda
                }
                else if (Direciton < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);   // olhando pra direita
                }
            }
            else
            {
                if (!basicAttackButton)
                {
                    animator.Play("Idle");
                }
                else
                {
                    animator.Play("BasicAttack");
                }

            }
        }
    */

}
