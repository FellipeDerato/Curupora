using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

public class StringGameEventListener : GameEventListener<string> { }
