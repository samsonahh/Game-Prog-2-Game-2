using DG.Tweening;
using KBCore.Refs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Self] private Rigidbody rigidBody;
    [SerializeField, Self] private FloatingCapsule floatingCapsule;

    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 3f;
    private float speedModifier = 1f;
    private float moveSpeed => speedModifier * baseSpeed;
    public Vector3 MoveDirection { get; private set; }

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 3f;

    #region Flags
    [HideInInspector] public bool IsGrounded;
    #endregion

    #region States
    public BaseState CurrentState { get; private set; }
    public BaseState DefaultState { get; private set; }
    public PlayerIdleState PlayerIdleState { get; private set; }
    public PlayerWalkState PlayerWalkState { get; private set; }
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

        CheckGrounded();
    }

    private void FixedUpdate()
    {
        CurrentState?.OnFixedUpdate();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + (0.25f) * floatingCapsule.Radius * transform.up, floatingCapsule.Radius);   
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
    }
    #endregion

    #region HandleInputFunctions
    public void HandleMovementInput()
    {
        MoveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    public void HandleJumpInput()
    {
        if (!IsGrounded) return;

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }
    #endregion

    private void CheckGrounded()
    {
        IsGrounded = Physics.CheckSphere(transform.position + (0.25f) * floatingCapsule.Radius * transform.up, floatingCapsule.Radius, LayerMask.GetMask("Ground"));
    }

    public void Move()
    {
        float forwardAngleBasedOnCamera = Mathf.Atan2(MoveDirection.x, MoveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.rotation.eulerAngles.y;
        Quaternion targetForwardRotation = Quaternion.Euler(0, forwardAngleBasedOnCamera, 0);
        Vector3 targetForwardDirection = targetForwardRotation * Vector3.forward;

        rigidBody.MovePosition(transform.position + moveSpeed * Time.fixedDeltaTime * targetForwardDirection);
    }

    public void Jump()
    {
        float newYVel = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        float timeToApex = Mathf.Abs(newYVel / Physics.gravity.y);

        TemporarilyDisableFloatingCapsule(timeToApex);

        Vector3 vel = rigidBody.velocity;
        vel.y = newYVel;

        rigidBody.velocity = vel;
    }

    private void TemporarilyDisableFloatingCapsule(float duration)
    {
        floatingCapsule.enabled = false;
        DOTween.Kill("DisableFloatingCapsule");
        DOVirtual.DelayedCall(duration, () => floatingCapsule.enabled = true).SetId("DisableFloatingCapsule");
    }

    public void SetSpeedModifier(float newSpeed)
    {
        speedModifier = newSpeed;
    }
}
