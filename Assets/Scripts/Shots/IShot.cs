using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for interacting with shooting objects
/// </summary>
public interface IShot
{
    /// <summary>
    /// List of modifiers that can change the behavior of the shot.
    /// </summary>
    public List<ShotMod> Mods
    {
        get;
        set;
    }
    
    /// <summary>
    /// Tells component to start handling shooting.
    /// </summary>
    /// <param name="dir"></param>
    public void Shooting(Vector2 dir);
}
