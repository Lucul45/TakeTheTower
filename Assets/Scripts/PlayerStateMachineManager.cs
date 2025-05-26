using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerState
{
    IDLE,
    MOVE,
    MELEE,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    NONE
}

public class PlayerStateMachineManager : MonoBehaviour
{
    [Header("States")]
    private Dictionary<EPlayerState, APlayerState> _states = null;
    private EPlayerState _currentState;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    private float _fixedTime = 0f;
    private float _pressTime = 0f;

    [SerializeField] private float _playerSpeed = 10f;

    private Vector2 _movementInput = Vector2.zero;
    private bool _attack = false;
    private bool _hold = false;

    public APlayerState CurrentState
    {
        get
        {
            return _states[_currentState];
        }
    }
    public Vector2 MovementInput
    {
        get { return _movementInput; }
    }
    public bool Attack
    {
        get { return _attack; }
    }
    public float FixedTime
    {
        get { return _fixedTime; }
        set { _fixedTime = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _states = new Dictionary<EPlayerState, APlayerState>();
        _states.Add(EPlayerState.IDLE, new IdleState());
        _states.Add(EPlayerState.MOVE, new MoveState());
        _states.Add(EPlayerState.MELEE, new MeleeBaseState());
        _states.Add(EPlayerState.ATTACK1, new Attack1State());
        _states.Add(EPlayerState.ATTACK2, new Attack2State());
        _states.Add(EPlayerState.ATTACK3, new Attack3State());
        foreach (KeyValuePair<EPlayerState, APlayerState> state in _states)
        {
            state.Value.Init(this, _animator);
        }
        _currentState = EPlayerState.IDLE;
        CurrentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        FixedTime += Time.deltaTime;
        if (_hold)
        {
            _pressTime += Time.deltaTime;
        }
        CurrentState.Update();
        _animator.SetFloat("Speed", _rb.velocity.x);
        if (_hold && _pressTime < 1f)
        {
            _attack = true;
        }
        else
        {
            _attack = false;
            _pressTime = 0;
        }
        Debug.Log(Attack);
    }

    public void ChangeState(EPlayerState nextState)
    {
        Debug.Log("Transition from " + CurrentState + " To " + nextState);
        CurrentState.Exit();
        _currentState = nextState;
        CurrentState.Enter();
    }

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void GetAttackInput(InputAction.CallbackContext context)
    {
        _attack = context.action.triggered;
    }

    public void Move(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _playerSpeed, 0);
    }
}
