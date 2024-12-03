using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child class of bullet that uses autonomous agent Seeking to home on targets.
/// </summary>
public class HomingBullet : Bullet
{
    [SerializeField] private float searchRange;
    
    protected override void CalcSteeringForce()
    {
        // find the nearest valid target based on the physics body layers
        target = PhysicsManager.Instance.SearchForNearest(searchRange, pb);
        
        // seek if there is a target in range
        if (target)
        {
            pb.ApplyForce(Seek(target.Position) * Time.deltaTime);
        }
        // move in the current direction otherwise
        else if(pb.Velocity.magnitude < speed)
        {
            pb.ApplyForce(pb.Direction * (MaxForce * Time.deltaTime));
        }
    }
}
