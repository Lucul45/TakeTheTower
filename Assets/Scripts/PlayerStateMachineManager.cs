using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EPlayerState
{
    IDLE,
    MOVE,
    MELEEENTRY,
    MELEE,
    PARRY,
    DASH,
    NONE
}

public class PlayerStateMachineManager : MonoBehaviour
{
    [Header("States")]
    private Dictionary<EPlayerState, APlayerState> _states = null;
    private EPlayerState _currentState;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _attack1CanCombo;
    [SerializeField] private Sprite[] _attack2CanCombo;
    private float _fixedTime = 0f;

    [SerializeField] private float _playerSpeed = 10f;

    private Vector2 _movementInput = Vector2.zero;
    private bool _attack = false;
    private bool _canAttack = true;
    private bool _canParry = true;
    private PlayerControls _controls;

    private event Action _attackPressed = null;
    public event Action AttackPressed
    {
        add 
        {
            _attackPressed -= value;
            _attackPressed += value; 
        }
        remove { _attackPressed -= value; }
    }

    private event Action _parryPressed = null;
    public event Action ParryPressed
    {
        add
        {
            _parryPressed -= value;
            _parryPressed += value;
        }
        remove { _parryPressed -= value; }
    }

    private event Action _dashPressed = null;
    public event Action DashPressed
    {
        add
        {
            _dashPressed -= value;
            _dashPressed += value;
        }
        remove { _dashPressed -= value; }
    }

    public APlayerState CurrentState
    {
        get
        {
            return _states[_currentState];
        }
    }
    public Sprite[] Attack1CanCombo
    {
        get
        {
            return _attack1CanCombo;
        }
    }
    public Sprite[] Attack2CanCombo
    {
        get
        {
            return _attack2CanCombo;
        }
    }
    public Vector2 MovementInput
    {
        get { return _movementInput; }
    }
    public bool Attack
    {
        get { return _attack; }
        set { _attack = value; }
    }
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    public bool CanParry
    {
        get { return _canParry; }
        set { _canParry = value; }
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
        _states.Add(EPlayerState.MELEEENTRY, new MeleeEntryState());
        _states.Add(EPlayerState.MELEE, new MeleeBaseState());
        _states.Add(EPlayerState.PARRY, new ParryState());
        _states.Add(EPlayerState.DASH, new DashState());
        foreach (KeyValuePair<EPlayerState, APlayerState> state in _states)
        {
            state.Value.Init(this, _animator, _spriteRenderer, _rb);
        }
        _currentState = EPlayerState.IDLE;
        CurrentState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        FixedTime += Time.deltaTime;
        CurrentState.Update();
        _animator.SetFloat("Speed", _rb.velocity.x);
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
        if (_attackPressed != null && context.started)
        {
            Debug.Log("ATK");
            _attackPressed();
        }
        
    }

    public void GetParryInput(InputAction.CallbackContext context)
    {
        if (_parryPressed != null && context.started)
        {
            Debug.Log("PARRY");
            _parryPressed();
        }
    }

    public void GetDashInput(InputAction.CallbackContext context)
    {
        if (_dashPressed != null && context.started)
        {
            Debug.Log("DASH");
            _dashPressed();
        }
    }

    public void Move(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _playerSpeed, 0);
    }

    public Vector2 RecordInput()
    {
        Vector2 recorded = _rb.velocity;    
        return recorded;
    }

    public void StartDash()
    {
        //StartCoroutine(_states[EPlayerState.DASH]);
    }
}
