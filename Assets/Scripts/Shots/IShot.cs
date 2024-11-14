using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for interacting with shooting objects
/// </summary>
public interface IShot
{
    public List<ShotMod> Mods
    {
        get;
        set;
    }
    
    public void Shooting(Vector2 dir);
}
