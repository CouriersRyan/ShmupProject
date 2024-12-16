using System;
using UnityEngine;

// delegate for collision events
public delegate void CollisionHandler(PhysicsBody other);

/// <summary>
/// Class that represents physical calculations for gameObjects such as movement and collisions
/// </summary>
public class PhysicsBody : MonoBehaviour
{
    // fields
    [SerializeField] private PhysicsType type = PhysicsType.Dynamic;
    
    [Header("Body")]
    private Vector3 movement; // Amount of movement for current frame.

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float radius = 1f;
    private Vector3 position;
    private Vector3 direction;
    private Vector3 velocity;
    private Vector3 acceleration;
    [SerializeField] private float mass = 1;
    [SerializeField] private float maxSpeed = 500f;
    [SerializeField] private float maxForce = 50f;

    [Header("Collisions")]
    [SerializeField] private bool isActivelyChecking = false;
    [SerializeField] private LayerMask contactLayers;

    [SerializeField] private bool isBoundToCameraEdge = true;
    [SerializeField] private EdgeBehavior edgeBehavior = EdgeBehavior.Stop;
    // Amount to adjust the edge calculations by,
    // positive to expand the camera edges and negative to shrink it
    [SerializeField] private float edgeCorrection;
    
    private Camera cam;

    public event EventHandler DestroyRecycle;
    public event CollisionHandler Collision;

    // properties
    public Vector2 Center
    {
        get { return spriteRenderer.bounds.center; }
    }

    public float Radius { get => radius; }

    public LayerMask ContactLayers
    {
        get => contactLayers;
    }

    public Vector3 Position
    {
        get => position;
    }

    public Vector3 Velocity
    {
        get => velocity;
    }

    public float MaxSpeed
    {
        get => maxSpeed;
    }

    public float MaxForce
    {
        get => maxForce;
    }

    public Vector3 Direction
    {
        get => direction;
        set => direction = value;
    }
    
    // methods
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;
        
        DestroyRecycle += OnDestroyRecycle;

        // For now, only check active checkers against non-active checkers, in the future make so that active can check against active.
        PhysicsManager.Instance.AddPhysicsBody(this);
    }

    private void OnDestroy()
    {
        DestroyRecycle -= OnDestroyRecycle;
    }

    private void Update()
    {
        // Check for collisions if it is an body that wants to actively check.
        // This is done to prevent potentially O(n^2) checks.
        if (isActivelyChecking)
        {
            PhysicsManager.Instance.CheckCollisions(this);
        }
    }

    /// <summary>
    /// Call when there is a collision, get the other collider
    /// </summary>
    /// <param name="info"></param>
    public void OnCollide(PhysicsBody other)
    {
        Collision?.Invoke(other);
    }
    
    /// <summary>
    /// Call at the start of every collision check, reset values for collision.
    /// </summary>
    public void ResetCollide()
    {
    }

    /// <summary>
    /// Resolve all physics.
    /// </summary>
    void FixedUpdate()
    {
        position = transform.position;

        if (type == PhysicsType.Dynamic)
        {
            // Update velocity
            velocity += acceleration * Time.deltaTime;

            if(velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
            direction = velocity.normalized;

            // Update position
            position += velocity * Time.deltaTime;

            transform.position = position;

            // Reset Forces
            acceleration = Vector3.zero;
            
            // face movement direction
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Velocity);
        }
        else if (type == PhysicsType.Kinematic)
        {
            // Update position with queued movement
            transform.position += transform.rotation * (velocity);
            
            // face movement direction
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, movement);

            velocity = Vector3.zero;
            movement = Vector3.zero;
        }
        
        if (isBoundToCameraEdge)
        {
            if (edgeBehavior == EdgeBehavior.Stop)
            {
                BoundOnEdge();
            }
            else
            {
                DestroyOnEdge();
            }
        }
    }

    /// <summary>
    /// Adds movement to the total movement vector for the frame, to be resolved.
    /// </summary>
    /// <param name="movement"></param>
    public void MovePosition(Vector3 movement)
    {
        this.velocity += movement * Time.deltaTime;
    }
    
    /// <summary>
    /// Applies a generic force represented as a vector.
    /// </summary>
    /// <param name="force"></param>
    public void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    /// <summary>
    /// Cause the physics object to stop against the edges of the screen.
    /// </summary>
    private void BoundOnEdge()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        bool onEdge = false;

        // Check x-edges
        if(screenPos.x < 0 - edgeCorrection)
        {
            screenPos.x = 0 - edgeCorrection;
            onEdge = true;
        }
        else if(screenPos.x > cam.scaledPixelWidth + edgeCorrection)
        {
            screenPos.x = cam.scaledPixelWidth + edgeCorrection;
            onEdge = true;
        }

        // Check y-edges
        if (screenPos.y < 0 - edgeCorrection)
        {
            screenPos.y = 0 - edgeCorrection;
            onEdge = true;
        }
        else if (screenPos.y > cam.scaledPixelHeight + edgeCorrection)
        {
            screenPos.y = cam.scaledPixelHeight + edgeCorrection;
            onEdge = true;
        }

        // Apply positional changes if the object did stop.
        if (onEdge)
        {
            transform.position = cam.ScreenToWorldPoint(screenPos);
        }
    }
    
    /// <summary>
    /// Cause the physics object to be destroyed or pooled when it hits the edge.
    /// </summary>
    private void DestroyOnEdge()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        bool hitEdge = false;

        // Check x-edges
        if(screenPos.x < 0 - edgeCorrection)
        {
            screenPos.x = 0 - edgeCorrection;
            hitEdge = true;
        }
        else if(screenPos.x > cam.scaledPixelWidth + edgeCorrection)
        {
            screenPos.x = cam.scaledPixelWidth + edgeCorrection;
            hitEdge = true;
        }

        // Check y-edges
        if (screenPos.y < 0 - edgeCorrection)
        {
            screenPos.y = 0 - edgeCorrection;
            hitEdge = true;
        }
        else if (screenPos.y > cam.scaledPixelHeight + edgeCorrection)
        {
            screenPos.y = cam.scaledPixelHeight + edgeCorrection;
            hitEdge = true;
        }

        // Destroy if edge was hit.
        if (hitEdge)
        {
            // Destroy/Pool
            DestroyRecycle?.Invoke(this, new EventArgs());
        }
    }
    
    private void OnDestroyRecycle(object sender, EventArgs e)
    {
        // reset values when pooled
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        direction = Vector3.up;
    }
}

/// <summary>
/// Enum to handle PhysicsBody's behavior when colliding with the screen edge.
/// </summary>
public enum EdgeBehavior
{
    Stop,
    Destroy
}

/// <summary>
/// Enum representing whether a physics body is Dynamic (uses forces) or Kinematic (doesn't use forces).
/// </summary>
public enum PhysicsType
{
    Dynamic,
    Kinematic
}