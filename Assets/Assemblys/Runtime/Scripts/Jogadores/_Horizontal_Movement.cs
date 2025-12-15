using System.Collections;
using UnityEngine;

public class _Horizontal_Movement : MonoBehaviour
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
    public float horizontalInput;
    public float horizontalForcado;
    public float Direcao;

    public float velocityX;
    public Vector2 currentVelocity;
    private float pontoApice;

    public FloatGameEvent HorizontalVelocity;


    public void Input_LeftAnalog(Vector2 vector)
    {    
        horizontalInput = vector.x;
        StartCoroutine(HorizontalMovement());
    }

    public IEnumerator HorizontalMovement()
    {
        float targetVelocity;
        if (horizontalForcado != 0) { Direcao = horizontalForcado; } else { Direcao = horizontalInput; }
        if (Direcao > 0) { targetVelocity = 8; } else if (Direcao < 0) { targetVelocity = -8; } else { targetVelocity = 0; }

        while (Mathf.Abs(velocityX - targetVelocity) > 0.01f)
        {
            if (Mathf.Abs(Direcao) > _parameters.smallThreshold)
            {
                velocityX += Direcao * _parameters.acceleration * Time.fixedDeltaTime;
                float apiceHorizontalBonusSpeed = _parameters.apiceHorizontalBonus * pontoApice;
                velocityX += Direcao * apiceHorizontalBonusSpeed * Time.fixedDeltaTime;
            }
            else
            {
                velocityX = Mathf.MoveTowards(velocityX, 0, _parameters.desacelerar * Time.fixedDeltaTime);
            }

            velocityX = Mathf.Clamp(velocityX, -_parameters.moveClamp, _parameters.moveClamp);

            HorizontalVelocity.TriggerEvent(velocityX);

            // Espera o próximo FixedUpdate
            yield return new WaitForFixedUpdate();
        }
    // Garante que a velocidade final seja exatamente o target
    velocityX = targetVelocity;
    HorizontalVelocity.TriggerEvent(velocityX);
    }
}
