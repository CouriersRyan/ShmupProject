using System;
using Pool;
using UnityEngine;

/// <summary>
/// Class representing a shmup enemy. Can spawn when on screen, shoot, move, and die.
/// </summary>
public class Enemy : Entity
{
    // Enemy should spawn, follow a certain path, shoot at either regular intervals, or at a fixed spot, then leave.
    
    // fields
    private Shot shot;
    
    [SerializeField] private PhysicsBody pb;
    [SerializeField] private float speed = 2f;

    //private GameObjectPool enemyPool;
    
    // methods
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        shot = GetComponent<Shot>();
        pb.DestroyRecycle += OnDestroyRecycle;
        
        // enemies are poolable objects, so create a pool if one isn't made.
        if (GameObjectPool.Pools.ContainsKey(poolID))
        {
            pool = GameObjectPool.Pools[poolID];
        }
        else
        {
            pool = GameObjectPool.Create(this, 10, 30);
        }
    }
    
    private void OnDestroy()
    {
        // unsubscribe from events when destroyed.
        pb.DestroyRecycle -= OnDestroyRecycle;
    }

    private void Update()
    {
        // shoot at the player
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
        pb.MovePosition(new Vector2(0, -speed * Time.deltaTime));
    }

    /// <summary>
    /// Take damage and resolve outcomes
    /// </summary>
    /// <param name="damage"></param>
    public override void DealDamage(int damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            pool.Recycle(this);
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
    
    /// <summary>
    /// Called when poolable object needs to be recycled.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDestroyRecycle(object sender, EventArgs e)
    {
        pool.Recycle(this);
    }

    public bool KillEnemy()
    {
        return pool.Recycle(this);
    }
}