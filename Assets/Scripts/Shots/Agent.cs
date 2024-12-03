using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Class representing agents that steers forces based on a target.
/// </summary>
[RequireComponent(typeof(PhysicsBody))]
public abstract class Agent : PoolableObject
{
    // fields
    [SerializeField] public PhysicsBody pb;
    [SerializeField] public float boundsScalar = 2f;

    protected Vector3 ultimaForce;
    protected Vector3 wanderPoint;

    // properties
    public float MaxSpeed
    {
        get => pb.MaxSpeed;
    }

    public float MaxForce
    {
        get => pb.MaxForce;
    }

    // methods
    /// <summary>
    /// Defines behavior of the steering force for the agent class.
    /// </summary>
    protected abstract void CalcSteeringForce();

    /// <summary>
    /// Initialize steering Agent values.
    /// </summary>
    protected virtual void Init()
    {

    }


    void Start()
    {
        Init();
    }

    // run every frame for every agent.
    protected virtual void Update()
    {
        ultimaForce = Vector3.zero;

        CalcSteeringForce();
    }

    /// <summary>
    /// Returns a steering force to have agent seek out the target.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public Vector3 Seek(Agent target)
    {
        Vector3 desiredVelocity = target.pb.Position - pb.Position;
        desiredVelocity = desiredVelocity.normalized * MaxSpeed;

        Vector3 seekingForce = desiredVelocity - pb.Velocity;
        seekingForce = seekingForce.normalized * MaxForce;

        return seekingForce;
    }

    /// <summary>
    /// Returns a steering force to have agent seek out the target.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 Seek(Vector3 position)
    {
        Vector3 desiredVelocity = position - pb.Position;
        desiredVelocity = desiredVelocity.normalized * MaxSpeed;

        Vector3 seekingForce = desiredVelocity - pb.Velocity;
        seekingForce = seekingForce.normalized * MaxForce;

        return seekingForce;
    }

    /// <summary>
    /// Returns a steering force to have agent flee form the target.
    /// </summary>
    /// <param name="runAwayFrom"></param>
    /// <returns></returns>
    public Vector3 Flee(Agent runAwayFrom)
    {
        Vector3 desiredVelocity = pb.Position - runAwayFrom.pb.Position;
        desiredVelocity = desiredVelocity.normalized * MaxSpeed;

        Vector3 fleeingForce = desiredVelocity - pb.Velocity;
        fleeingForce = fleeingForce.normalized * MaxForce;

        return fleeingForce;
    }

    /// <summary>
    /// Get the projected position of the agent after a certain about of time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public Vector3 GetFuturePosition(float time)
    {
        return pb.Position + pb.Velocity * (time);
    }

    /// <summary>
    /// Get a wandering force for the agent based on previous wandering force and future posiiton.
    /// </summary>
    /// <param name="currentWanderAngle"></param>
    /// <param name="wanderRange"></param>
    /// <param name="maxWanderAngle"></param>
    /// <param name="time"></param>
    /// <param name="wanderRadius"></param>
    /// <returns></returns>
    public Vector3 Wander(ref float currentWanderAngle, float wanderRange, float maxWanderAngle, float time, float wanderRadius)
    {
        // get future position
        Vector3 lookAheadPoint = GetFuturePosition(time);

        // get the wander angle based on previous wander angle and a maximum
        currentWanderAngle += Random.Range(-wanderRange, wanderRange) * Time.deltaTime;
        currentWanderAngle = Mathf.Clamp(currentWanderAngle, -maxWanderAngle, maxWanderAngle);

        // rotate wander angle by heading direction
        float newWanderAngle = Vector3.Angle(Vector3.up, pb.Direction) + currentWanderAngle;

        // calculate the point to seek based on wander angle
        Vector3 wanderPoint = new Vector3(lookAheadPoint.x + (Mathf.Cos(newWanderAngle) * wanderRadius), lookAheadPoint.y + (Mathf.Sin(newWanderAngle) * wanderRadius), lookAheadPoint.z);
        this.wanderPoint = wanderPoint;
        
        return Seek(wanderPoint);
    }

    /// <summary>
    /// Create a force that makes the agent go back to the center when future position is out of bounds.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="padding"></param>
    /// <returns></returns>
    public Vector3 StayInBounds(float time, float padding)
    {
        Camera cam = Camera.main;
        Vector3 lookAheadPoint = GetFuturePosition(time);
        Vector2 screenPos = cam.WorldToScreenPoint(lookAheadPoint);
        bool isGoingOutOfBounds = false;

        // Check x-edges
        if (screenPos.x < 0 + padding)
        {
            isGoingOutOfBounds = true;
        }
        else if (screenPos.x > cam.scaledPixelWidth - padding)
        {
            isGoingOutOfBounds = true;
        }

        // Check y-edges
        if (screenPos.y < 0 + padding)
        {
            isGoingOutOfBounds = true;
        }
        else if (screenPos.y > cam.scaledPixelHeight - padding)
        {
            isGoingOutOfBounds = true;
        }

        if (isGoingOutOfBounds)
        {
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }
}
