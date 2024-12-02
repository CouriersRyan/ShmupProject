using System;
using Pool;
using UnityEngine;

/// <summary>
/// Represents a bullet that is shot and can deal dmaage.
/// </summary>
public class Bullet : Agent
{
    // fields
    public float speed = 20f;

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
        pb.MovePosition(Vector2.up * (speed * Time.deltaTime));
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

    public override void OnRecycle()
    {
        owner = null;
    }

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
