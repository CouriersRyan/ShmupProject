using UnityEngine;

/// <summary>
/// Class that represents physical calculations for gameObjects such as movement and collisions
/// </summary>
public class PhysicsBody : MonoBehaviour
{
    [Header("Body")]
    private Vector3 position;

    private Vector3 movement; // Amount of movement for current frame.

    public Vector3 Position
    {
        get => position;
    }
    
    [SerializeField] private bool isActivelyChecking = false;
    [SerializeField] private bool isBoundToCamera = true;
    [SerializeField] private EdgeBehavior edgeBehavior = EdgeBehavior.Stop;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private float radius = 1f;
    // Amount to adjust the edge calculations by,
    // positive to expand the camera edges and negative to shrink it
    [SerializeField] private float edgeCorrection;
    
    private Camera cam;
    
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
        position = transform.position;
    }

    private void Update()
    {
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
        position += movement;
        BoundOnEdge();

        transform.position = position;

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
        Vector3 screenPos = cam.WorldToScreenPoint(position);
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
            position = cam.ScreenToWorldPoint(screenPos);
        }
    }
    
    /// <summary>
    /// Cause the physics object to be destroyed or pooled when it hits the edge.
    /// </summary>
    private void DestroyOnEdge()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(position);
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
        }
    }
}

public enum EdgeBehavior
{
    Stop,
    Destroy
}
