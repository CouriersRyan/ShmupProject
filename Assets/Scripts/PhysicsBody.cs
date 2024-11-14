using System;
using UnityEngine;

/// <summary>
/// Class that represents physical calculations for gameObjects such as movement and collisions
/// </summary>
public class PhysicsBody : MonoBehaviour
{
    [Header("Body")]
    private Vector3 movement; // Amount of movement for current frame.

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float radius = 1f;

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

    public Vector2 Center
    {
        get { return spriteRenderer.bounds.center; }
    }

    public float Radius { get => radius; }
    public Color SpriteColor
    {
        set { spriteRenderer.color = value; }
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main;

        // For now, only check active checkers against non-active checkers, in the future make so that actice can check against active.
        if (!isActivelyChecking)
        {
            PhysicsManager.Instance.AddPhysicsBody(this);
        }
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
        this.SpriteColor = Color.red;
    }
    
    /// <summary>
    /// Call at the start of every collision check, reset values for collision.
    /// </summary>
    public void ResetCollide()
    {
        this.SpriteColor = Color.white;
    }

    /// <summary>
    /// Resolve all physics.
    /// </summary>
    void FixedUpdate()
    {
        // Update position with queued movement
        transform.position += transform.rotation * movement;
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
        
        movement = Vector3.zero;
    }

    
    public void MovePosition(Vector3 movement)
    {
        this.movement += movement;
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

        // Apply positional changes if the object did hit the edge.
        if (hitEdge)
        {
            // Destroy/Pool
            DestroyRecycle?.Invoke(this, new EventArgs());
        }
    }
}

public enum EdgeBehavior
{
    Stop,
    Destroy
}
