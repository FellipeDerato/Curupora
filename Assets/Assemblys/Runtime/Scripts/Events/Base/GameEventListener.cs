using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListener<T> : MonoBehaviour
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

    [Header("")]

    public GameEvent<T> gameEvent;
    public UnityEvent<T> onEventTriggered;

    private void OnEnable()
    {
        if (gameEvent != null)
            gameEvent.AddListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent != null)
            gameEvent.RemoveListener(this);
    }

    public void OnEventTriggered(T value)
    {
        onEventTriggered.Invoke(value);
    }


}

