using System;
using Physics;
using Pool;
using UnityEngine;

/// <summary>
/// Represents a bullet that is shot and can deal dmaage.
/// </summary>
public class Bullet : Agent
{
    // fields
    public float speed = 20f;
    [SerializeField] private bool useLifespan;
    [SerializeField] private float lifespan;
    [SerializeField] private float currentLifespan = 0;
    public Vector2 Direction
    {
        get => pb.Direction;
        set => pb.Direction = value;
    }

    public Shot owner; // the Shot the shoots the bullet
    public Affiliation affiliation; // Enemy or Player

    public PhysicsBody target;

    
    // methods

    private void Start()
    {
        currentLifespan = lifespan;
        
        pb.DestroyRecycle += OnDestroyRecycle;
        pb.Collision += OnCollision;
    }

    private void OnDestroy()
    {
        pb.DestroyRecycle -= OnDestroyRecycle;
        pb.Collision -= OnCollision;
    }
    
    protected override void CalcSteeringForce()
    {
        // don't use forces by default and move be a fixed amount
        pb.MovePosition(Vector2.up * (speed));
    }

    protected override void Update()
    {
        base.Update();

        // decrement life if bullet has a finite lifespan
        if (useLifespan)
        {
            currentLifespan -= Time.deltaTime;

            if (currentLifespan <= 0)
            {
                pool.Recycle(this);
            }
        }
    }

    /// <summary>
    /// Resolves collisions against other PhysicsBodies.
    /// </summary>
    /// <param name="other"></param>
    private void OnCollision(PhysicsBody other)
    {
        // If enemy bullet hits the player, or player bullets hits an enemy, deal damage.
        if ((affiliation == Affiliation.Player && other.gameObject.CompareTag("Enemy")) ||
            affiliation == Affiliation.Enemy && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IHealth>().DealDamage(1);
            pool.Recycle(this);
        }
    }

    /// <summary>
    /// Resetting that happens when pool recycles this object.
    /// </summary>
    public override void OnRecycle()
    {
        currentLifespan = lifespan;
        owner = null;
    }

    /// <summary>
    /// Called when told to return this object to the pool.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDestroyRecycle(object sender, EventArgs e)
    {
        pool.Recycle(this);
    }
}

/// <summary>
/// Enum represents what affiliation the bullet has, being either shot by the player or the enemies.
/// </summary>
public enum Affiliation
{
    Player,
    Enemy
}
