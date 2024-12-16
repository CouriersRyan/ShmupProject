using System;
using Pool;
using UnityEngine;

/// <summary>
/// Class representing a shmup enemy. Can spawn when on screen, shoot, move, and die.
/// </summary>
public class Enemy : Entity, IScorable
{
    // Enemy should spawn, follow a certain path, shoot at either regular intervals, or at a fixed spot, then leave.
    
    // fields
    protected Shot shot;
    private SpriteRenderer sr;
    
    [SerializeField] protected PhysicsBody pb;
    [SerializeField] protected float speed = 2f;
    [SerializeField] private int pointValue;
    [SerializeField] private float damagedTimerMax = 0.2f;
    private float damageTimer;
    
    //p properties
    public int Score { get => pointValue; }
    
    // methods
    protected override void Start()
    {
        base.Start();
        Init();
    }

    /// <summary>
    /// Enemy initialization.
    /// </summary>
    public void Init()
    {
        shot = GetComponent<Shot>();
        sr = GetComponent<SpriteRenderer>();
        pb.DestroyRecycle += OnDestroyRecycle;
        damageTimer = 0;
    }
    
    private void OnDestroy()
    {
        // unsubscribe from events when destroyed.
        pb.DestroyRecycle -= OnDestroyRecycle;
    }

    protected virtual void Update()
    {
        // shoot at the player and move as the basic behaviors
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
        pb.MovePosition(new Vector2(0, -speed * Time.deltaTime));
        
        // enemies turn red when hit and taking damage.
        if (damageTimer > 0)
        {
            sr.color = Color.red;
            damageTimer -= Time.deltaTime;
        }
        else
        {
            sr.color = Color.white;
        }
    }
    
    /// <summary>
    /// Called when poolable object needs to be recycled.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDestroyRecycle(object sender, EventArgs e)
    {
        pool.Recycle(this);
    }

    /// <summary>
    /// Return enemy to its pool when it is killed.
    /// </summary>
    /// <returns></returns>
    public virtual bool KillEnemy()
    {
        // update score for killing enemy
        ((IScorable)this).AddScore();
        return pool.Recycle(this);
    }

    /// <summary>
    /// Take damage and resolve outcomes
    /// </summary>
    /// <param name="damage"></param>
    public override void DealDamage(int damage)
    {
        Health -= damage;
        damageTimer = damagedTimerMax;
        if (Health <= 0)
        {
            KillEnemy();
        }
    }

    /// <summary>
    /// Take damage from secondary effects.
    /// </summary>
    /// <param name="damageInfo"></param>
    /// <exception cref="NotImplementedException"></exception>
    public override void DealDamage(DamageInfo damageInfo)
    {
        throw new NotImplementedException();
    }

    public override void OnRecycle()
    {
        // make sure the Shot is reset as well.
        base.OnRecycle();
        damageTimer = 0;
    }
}