using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class VoidUnityEvent : UnityEvent<Void> { }

public class VoidGameEventListener : GameEventListener<Void> { }
