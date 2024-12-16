using Pool;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void EntityHealthChangedHandler(int newHealth);

/// <summary>
/// An entity is anything that has HP among other stats
/// </summary>
public abstract class Entity : PoolableObject, IHealth
{
    // fields
    [FormerlySerializedAs("health")] [SerializeField] protected int maxHealth = 1;
    [SerializeField] private int currHealth;
    
    public event EntityHealthChangedHandler OnEntityHealthChanged;

    // properties
    public int Health
    {
        get => currHealth;
        protected set
        {
            currHealth = value;
            // Invoke health changed event.
            if (OnEntityHealthChanged != null) OnEntityHealthChanged.Invoke(currHealth);
        }
    }

    public int MaxHealth
    {
        get => maxHealth;
    }
    
    // methods
    protected virtual void Start()
    {
        currHealth = maxHealth;
    }
    
    public override void OnRecycle()
    {
        currHealth = maxHealth;
    }
    
    // abstract methods
    public abstract void DealDamage(int damage);

    public abstract void DealDamage(DamageInfo damageInfo);
}
