using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An entity is anything that has HP among other stats
/// </summary>
public abstract class Entity : MonoBehaviour, IHealth
{
    [SerializeField] protected int health = 1;
    public int Health { get => health; }
    public abstract void DealDamage(int damage);

    public abstract void DealDamage(DamageInfo damageInfo);
}
