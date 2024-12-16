using Physics;
using UnityEngine;

/// <summary>
/// Class that handles character movement logic and shooting.
/// </summary>
[RequireComponent(typeof(PhysicsBody))]
public class MovementController : MonoBehaviour
{
    // fields
    [SerializeField] private float focusedSpeed = 5f;
    [SerializeField] private float unfocusedSpeed = 10f;
    private float speed;
    private Vector2 moveVector;
    private readonly Vector2 ShootVector = Vector2.up;
    
    private bool isShooting;
    private bool focused = false;

    private PhysicsBody pb;
    
    [SerializeField] private Shot focusedShot;
    [SerializeField] private Shot unfocusedshot;
    
    // properties
    public bool Focus
    {
        get => focused;
        set
        {
            focused = value;
            if (focused)
            {
                speed = focusedSpeed;
            }
            else
            {
                speed = unfocusedSpeed;
            }
        }
    }

    public Vector2 MoveVector
    {
        get => moveVector;
        set => moveVector = value;
    }

    public bool Shoot
    {
        get => isShooting;
        set => isShooting = value;
    }

    
    //methods
    void Start()
    {
        // Shots should be set in spector, but if not find a Shot component.
        if (unfocusedshot == null) unfocusedshot = GetComponent<Shot>();
        if (focusedShot == null) focusedShot = GetComponent<Shot>();

        pb = GetComponent<PhysicsBody>();

        speed = unfocusedSpeed;

    }

    private void FixedUpdate()
    {
        // resolve movement
        pb.MovePosition(moveVector * (speed));
        
        // Shoot
        if (isShooting)
        {
            if (focused)
            {
                focusedShot.Shooting(ShootVector);
            }
            else
            {
                unfocusedshot.Shooting(ShootVector);
            }
        }
    }
}
