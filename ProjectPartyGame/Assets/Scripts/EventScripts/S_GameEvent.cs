using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a game event for use in the game.
/// </summary>
[CreateAssetMenu(menuName = "GameEvent")]
public class S_GameEvent : ScriptableObject
{
    public List<S_GameEventListener> listeners = new List<S_GameEventListener>();

    public void Raise(object data = null)
    {
        foreach (var listener in listeners)
        {
            listener.OnEventRaised(data);
        }
    }

    public void RegisterListener(S_GameEventListener listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(S_GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
