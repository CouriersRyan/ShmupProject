using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingBullet : Bullet
{
    [SerializeField] private float searchRange;
    
    protected override void CalcSteeringForce()
    {
        target = PhysicsManager.Instance.SearchForNearest(searchRange, pb);
        
        if (target)
        {
            pb.ApplyForce(Seek(target.Position) * Time.deltaTime);
        }
        else if(pb.Velocity.magnitude < speed)
        {
            pb.ApplyForce(pb.Direction * (MaxForce * Time.deltaTime));
        }
    }
}
