using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Entity
{
    [SerializeField] private PlayerInput playerInput;
    private InputAction _playerMove;
    private InputAction _playerShoot;
    private InputAction _playerFocus;
    private InputAction _playerBomb;

    private MovementController _movementController;

    public static PlayerController Player
    {
        get;
        private set;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Player = this;
        _movementController = GetComponent<MovementController>();
        
        playerInput.SwitchCurrentActionMap("PlayableCharacter");
        
        // Bind Functions to Action Map
        _playerMove = playerInput.actions["Move"];
        _playerShoot = playerInput.actions["Shoot"];
        _playerFocus = playerInput.actions["Focus"];
        _playerBomb = playerInput.actions["Bomb"];

        _playerShoot.started += OnShootStart;
        _playerShoot.canceled += OnShootEnd;
        _playerMove.performed += OnMoving;
        _playerMove.started += OnMoving;
        _playerMove.canceled += OnMoving;
        _playerFocus.started += OnFocusStart;
        _playerFocus.canceled += OnFocusEnd;
        
    }

    // Input Events
    private void OnMoving(InputAction.CallbackContext context)
    {
        _movementController.MoveVector = context.ReadValue<Vector2>();
    }
    
    private void OnShootStart(InputAction.CallbackContext context)
    {
        _movementController.Shoot = true;
    }
    private void OnShootEnd(InputAction.CallbackContext context)
    {
        _movementController.Shoot = false;
    }

    private void OnFocusStart(InputAction.CallbackContext context)
    {
        _movementController.Focus = true;
    }
    
    private void OnFocusEnd(InputAction.CallbackContext context)
    {
        _movementController.Focus = false;
    }

    private void OnBombPerformed(InputAction.CallbackContext context)
    {
        // TODO: Screen Clear
    }

    public override void DealDamage(int damage)
    {
        health--;
    }

    public override void DealDamage(DamageInfo damageInfo)
    {
        throw new System.NotImplementedException();
    }
}
