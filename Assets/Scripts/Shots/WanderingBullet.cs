using UnityEngine;

/// <summary>
/// Child of Bullet that uses autonomous agent Wandering
/// </summary>
public class WanderingBullet : Bullet
{
    // fields
    [SerializeField] private float wanderTime = 1f;
    [SerializeField] private float wanderRadius = 1f;
    [SerializeField] private float padding = 20f;

    private float currWanderAngle = 0;

    //methods

    /// <summary>
    /// Override of Agent's Init, sets start steering force.
    /// </summary>
    protected override void Init()
    {
        transform.rotation = Quaternion.Euler(0, 0,Random.Range(-180, 180));
    }

    /// <summary>
    /// Calculates total steering force as a sum of Wander and StayInBounds forces.
    /// </summary>
    protected override void CalcSteeringForce()
    {
        ultimaForce += Wander(ref currWanderAngle, Mathf.PI/36, Mathf.PI/4, wanderTime, wanderRadius);
        ultimaForce += StayInBounds(wanderTime, padding) * boundsScalar;

        Vector3.ClampMagnitude(ultimaForce, MaxForce);
        pb.ApplyForce(ultimaForce);
    }
    
    /// <summary>
    /// Visualizes ultima force.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, GetFuturePosition(wanderTime));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, wanderPoint);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(GetFuturePosition(wanderTime), wanderRadius);
    }
}
