using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Input : MonoBehaviour
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

    [Header("Input Names")]
    public Player_Input_Names Inputs;   

    public void OnLeftStick(InputAction.CallbackContext context)
    {
        float horizontalLeftInput = context.ReadValue<Vector2>().x;
        float verticalLeftInput = context.ReadValue<Vector2>().y;
        
        if (Inputs.leftAnalogGameEvent != null)
            Inputs.leftAnalogGameEvent.TriggerEvent
            (new Vector2(horizontalLeftInput, verticalLeftInput));
    }

    public void OnRightStick(InputAction.CallbackContext context)
    {
        float horizontalRightInput = context.ReadValue<Vector2>().x;
        float verticalRightInput = context.ReadValue<Vector2>().y;

        if (Inputs.rightAnalogGameEvent != null)
            Inputs.rightAnalogGameEvent.TriggerEvent
            (new Vector2(horizontalRightInput, verticalRightInput));
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (Inputs.jumpPressed != null)
            Inputs.jumpPressed.TriggerEvent(context.ReadValueAsButton());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (Inputs.attackPressed != null)
            Inputs.attackPressed.TriggerEvent(context.ReadValueAsButton());
    }

    public void OnMagic(InputAction.CallbackContext context)
    {
        if (Inputs.magicPressed != null)
            Inputs.magicPressed.TriggerEvent(context.ReadValueAsButton());
    }

/*
    private void Update()
    {
        float horizontalLeftInput = Input.GetAxisRaw(Inputs.inputName_LeftAnalogX);
        float verticalLeftInput = Input.GetAxisRaw(Inputs.inputName_LeftAnalogY);


        if (Inputs.leftAnalogGameEvent)
            Inputs.leftAnalogGameEvent.TriggerEvent
            (new Vector2(horizontalLeftInput, verticalLeftInput));

        if (Inputs.jumpPressed != null && Input.GetButtonDown(Inputs.inputName_Jump))
            Inputs.jumpPressed.TriggerEvent(true);

        if (Inputs.jumpPressed != null && Input.GetButtonUp(Inputs.inputName_Jump))
            Inputs.jumpPressed.TriggerEvent(false);

        if (Inputs.attackPressed != null && Input.GetButtonDown(Inputs.inputName_Attack))
            Inputs.attackPressed.TriggerEvent(true);

        if (Inputs.attackPressed != null && Input.GetButtonUp(Inputs.inputName_Attack))
            Inputs.attackPressed.TriggerEvent(false);

        if (Inputs.magicPressed != null && Input.GetButtonDown(Inputs.inputName_Magic))
            Inputs.magicPressed.TriggerEvent(true);

        if (Inputs.magicPressed != null && Input.GetButtonUp(Inputs.inputName_Magic))
            Inputs.magicPressed.TriggerEvent(false);
    }    

    */
}
