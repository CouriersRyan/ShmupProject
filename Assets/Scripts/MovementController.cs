using UnityEngine;

[RequireComponent(typeof(PhysicsBody))]
public class MovementController : MonoBehaviour
{
    [SerializeField] private float focusedSpeed = 5f;
    [SerializeField] private float unfocusedSpeed = 10f;
    private float speed;
    private Vector2 moveVector;
    private readonly Vector2 ShootVector = Vector2.up;
    
    private bool _isShooting;
    private bool _focused = false;

    private PhysicsBody pb;
    
    [SerializeField] private Shot _focusedShot;
    [SerializeField] private Shot _unfocusedshot;

    public bool Focus
    {
        get => _focused;
        set
        {
            _focused = value;
            if (_focused)
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
        get => _isShooting;
        set => _isShooting = value;
    }

    void Start()
    {
        if (_unfocusedshot == null) _unfocusedshot = GetComponent<Shot>();
        if (_focusedShot == null) _focusedShot = GetComponent<Shot>();

        pb = GetComponent<PhysicsBody>();

        speed = unfocusedSpeed;

    }

    private void FixedUpdate()
    {
        pb.MovePosition(moveVector * (speed * Time.deltaTime));
        
        // Shoot
        if (_isShooting)
        {
            if (_focused)
            {
                _focusedShot.Shooting(ShootVector);
            }
            else
            {
                _unfocusedshot.Shooting(ShootVector);
            }
        }
    }
}
