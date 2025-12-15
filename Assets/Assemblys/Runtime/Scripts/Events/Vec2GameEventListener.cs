using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Vec2UnityEvent : UnityEvent<Vector2> { }

public class Vec2GameEventListener : GameEventListener<Vector2> { }
