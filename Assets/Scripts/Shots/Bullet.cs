using System;
using Pool;
using UnityEngine;

/// <summary>
/// Represents a bullet that is shot and can deal dmaage.
/// </summary>
public class Bullet : PoolableObject
{
    // fields
    public float speed = 20f;

    public Vector2 dir;

    public Shot owner; // the Shot the shoots the bullet
    public Affiliation affiliation; // Enemy or Player

    [SerializeField] private PhysicsBody pb;

    
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

    private void Update()
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
