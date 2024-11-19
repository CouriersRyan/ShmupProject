using System;
using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

/// <summary>
/// An entity is anything that has HP among other stats
/// </summary>
public abstract class Entity : PoolableObject, IHealth
{
    // fields
    [SerializeField] protected int health = 1;
    [SerializeField] protected int currHealth;

    public int Health { get => currHealth; }
    
    // methods
    void Start()
    {
        currHealth = health;
    }
    
    public override void OnRecycle()
    {
        currHealth = health;
    }
    
    // abstract methods
    public abstract void DealDamage(int damage);

    public abstract void DealDamage(DamageInfo damageInfo);
}
