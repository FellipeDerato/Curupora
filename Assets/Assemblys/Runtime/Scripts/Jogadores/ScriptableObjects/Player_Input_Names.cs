using UnityEngine;

[CreateAssetMenu(fileName = "Player Input Names", menuName = "Player/Player Input Names")]
public class Player_Input_Names : ScriptableObject
{
    [Header("Left Analog Stick")]
    public string inputName_LeftAnalogX = "P1_LeftAnalog_Horizontal";
    public string inputName_LeftAnalogY = "P1_LeftAnalog_Vertical";
    public Vec2GameEvent leftAnalogGameEvent;

    [Header("Right Analog Stick")]
    public string inputName_RightAnalogX = "P1_RightAnalog_Horizontal";
    public string inputName_RightAnalogY = "P1_RightAnalog_Vertical";
    public Vec2GameEvent rightAnalogGameEvent;

    [Header("Jump")] // button A
    public string inputName_Jump = "Jump";
    public BoolGameEvent jumpPressed;

    [Header("Attack")] // button X
    public string inputName_Attack = "Fire1";
    public BoolGameEvent attackPressed;

    [Header("Magic")] // button B
    public string inputName_Magic = "Fire2";
    public BoolGameEvent magicPressed;

    [Header("Interact")] // button Y
    public string inputName_Interact = "Fire3";
    public BoolGameEvent interactPressed;

}
