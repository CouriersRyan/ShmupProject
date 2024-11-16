using System;
using Pool;
using UnityEngine;

public class Enemy : Entity
{
    // Enemy should spawn, follow a certain path, shoot at either regular intervals, or at a fixed spot, then leave.
    private Shot shot;
    
    [SerializeField] private PhysicsBody pb;

    private GameObjectPool enemyPool;
    
    private void Start()
    {
        shot = GetComponent<Shot>();
        pb.DestroyRecycle += OnDestroyRecycle;
        
        if (GameObjectPool.Pools.ContainsKey(this.name))
        {
            enemyPool = GameObjectPool.Pools[this.name];
        }
        else
        {
            enemyPool = GameObjectPool.Create(this, 10, 30);
        }
    }
    
    private void OnDestroy()
    {
        pb.DestroyRecycle -= OnDestroyRecycle;
    }

    private void Update()
    {
        shot.Shooting(PlayerController.Player.transform.position - transform.position);
    }

    public override void DealDamage(int damage)
    {
        currHealth -= damage;
        if (currHealth <= 0)
        {
            enemyPool.Recycle(this);
        }
    }

    public override void DealDamage(DamageInfo damageInfo)
    {
        throw new NotImplementedException();
    }
    
    private void OnDestroyRecycle(object sender, EventArgs e)
    {
        pool.Recycle(this);
    }
}