using System;
using UnityEngine;

public class Bullet : PoolableObject
{
    public float speed = 20f;

    public Vector2 dir;

    public Shot owner;
    public Affiliation affiliation;

    [SerializeField] private PhysicsBody pb;

    private void Start()
    {
        pb.DestroyRecycle += OnDestroyRecycle;
    }

    private void OnDestroy()
    {
        pb.DestroyRecycle -= OnDestroyRecycle;
    }

    private void Update()
    {
        pb.MovePosition(Vector2.up * (speed * Time.deltaTime));
    }

    private void OnCollision(Collision2D other)
    {
        if (other.gameObject.tag.Equals("BulletBound"))
        {
            pool.Recycle(this);
        }

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


public enum Affiliation
{
    Player,
    Enemy
}
