using UnityEngine;

public enum Colors
{
    white,
    red,
    green,
    blue,    
    black,
    gray,
    yellow,
    cyan,
    magenta,
    clear
}

public static class CorExtensions
{
    public static Color ToUnityColor(this Colors cor)
    {
        switch (cor)
        {
            case Colors.red: return Color.red;
            case Colors.green: return Color.green;
            case Colors.blue: return Color.blue;
            case Colors.white: return Color.white;
            case Colors.black: return Color.black;
            case Colors.gray: return Color.gray;
            case Colors.yellow: return Color.yellow;
            case Colors.cyan: return Color.cyan;
            case Colors.magenta: return Color.magenta;
            case Colors.clear: return Color.clear;
            default: return Color.white;
        }
    }
}