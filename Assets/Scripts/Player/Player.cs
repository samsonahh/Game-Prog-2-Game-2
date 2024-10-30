using DG.Tweening;
using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] private Rigidbody rigidBody;
    [field: SerializeField, Self] public FloatingCapsule FloatingCapsule { get; private set; }
    [field: SerializeField, Anywhere] public Transform PickaxeTransform { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 3f;
    private float speedModifier = 1f;
    private float moveSpeed => speedModifier * baseSpeed;
    public Vector3 MoveDirection { get; private set; }

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 3f;
    private float instantJumpVelocity => Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
    public float JumpDurationToApex => Mathf.Abs(instantJumpVelocity / Physics.gravity.y);

    [field: Header("Mine Settings")]
    [field: SerializeField] public float MineDuration { get; private set; } = 0.25f;
    [SerializeField] private float mineRange = 2f;
    [SerializeField] private float mineDamage = 10f;

    #region States
    public BaseState CurrentState { get; private set; }
    public BaseState DefaultState { get; private set; }
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerWalkState PlayerWalkState { get; private set; }
    public PlayerJumpState PlayerJumpState { get; private set; }
    public PlayerFallState PlayerFallState { get; private set; }
    public PlayerMineState PlayerMineState { get; private set; }
    #endregion

    private void OnValidate()
    {
        this.ValidateRefs();
    }

    private void Awake()
    {
        InitializeStates();
    }

    private void Start()
    {
        SetDefaultState(PlayerIdleState);
        SetStartState(PlayerIdleState);
    }

    private void Update()
    {
        CurrentState?.OnUpdate();

        // Handle inputs
        HandleMovementInput();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        CurrentState?.OnFixedUpdate();

        RotateWithCamera();
    }

    #region StateFunctions
    public void ChangeState(BaseState newState)
    {
        if (CurrentState == newState) return;

        CurrentState?.OnExit();
        CurrentState = newState;
        CurrentState?.OnEnter();
    }

    private void SetDefaultState(BaseState defaultState)
    {
        DefaultState = defaultState;
    }

    private void SetStartState(BaseState startState)
    {
        ChangeState(startState);
    }

    private void InitializeStates()
    {
        PlayerIdleState = new PlayerIdleState(this);
        PlayerWalkState = new PlayerWalkState(this);
        PlayerJumpState = new PlayerJumpState(this);
        PlayerFallState = new PlayerFallState(this);
        PlayerMineState = new PlayerMineState(this);
    }
    #endregion

    #region HandleInputFunctions
    private void HandleMovementInput()
    {
        MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void HandleJumpInput()
    {
        if(CurrentState == PlayerJumpState) return;
        if (CurrentState == PlayerFallState) return;
        if (CurrentState == PlayerMineState) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlayerJumpState);
        }
    }

    public void HandleMineInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if(CurrentState == PlayerMineState) return;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            ChangeState(PlayerMineState);
        }
    }
    #endregion

    public void Move()
    {
        float forwardAngleBasedOnCamera = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.y;
        Quaternion targetForwardRotation = Quaternion.Euler(0, forwardAngleBasedOnCamera, 0);
        Vector3 targetForwardDirection = targetForwardRotation * Vector3.forward;

        if (MoveDirection == Vector3.zero)
        {
            targetForwardRotation = transform.rotation;
            targetForwardDirection = Vector3.zero;
        }

        Vector3 velocityWithNoY = new Vector3(rigidBody.velocity.x, 0, rigidBody.velocity.z);

        rigidBody.AddForce(moveSpeed * targetForwardDirection - velocityWithNoY, ForceMode.VelocityChange);
    }

    public void Jump()
    {
        Vector3 vel = rigidBody.velocity;
        vel.y = instantJumpVelocity;

        rigidBody.velocity = vel;
    }

    public void Mine()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        bool didHit = Physics.Raycast(mouseRay, out hit, mineRange, LayerMask.GetMask("Ore"));

        if (!didHit) return;

        Ore ore = hit.collider.GetComponent<Ore>();

        ore.TakeDamage(mineDamage);
    }

    private void RotateWithCamera()
    {
        Quaternion targetRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        rigidBody.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.fixedDeltaTime));
    }

    public void SetSpeedModifier(float newSpeed)
    {
        speedModifier = newSpeed;
    }
}
