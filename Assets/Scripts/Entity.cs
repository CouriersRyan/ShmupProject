using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An entity is anything that has HP among other stats
/// </summary>
public abstract class Entity : PoolableObject, IHealth
{
    [SerializeField] protected int health = 1;
    [SerializeField] protected int currHealth;

    void Start()
    {
        currHealth = health;
    }

    public int Health { get => currHealth; }
    public abstract void DealDamage(int damage);

    public abstract void DealDamage(DamageInfo damageInfo);
    
    public override void OnRecycle()
    {
        currHealth = health;
    }
}
