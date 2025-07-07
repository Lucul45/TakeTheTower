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
    THROW,
    PARRY,
    DASH,
    HURT,
    DEAD,
    NONE
}

public class PlayerStateMachineManager : MonoBehaviour
{
    [Header("States")]
    private Dictionary<EPlayerState, APlayerState> _states = null;
    private EPlayerState _currentState;
    private EPlayerState _lastState;

    [Header("Refs")]
    [SerializeField] private PlayerDamageManager _playerDamageManager;
    [SerializeField] private WinMenuManager _winManager;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AttackData[] _attacksData;
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private DashBar _dashBar;
    [SerializeField] private Sprite _perfectParryFrame;
    [SerializeField] private GameObject _otherPlayer;

    private float _fixedTime = 0f;

    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private float _clankForce = 10f;

    private Vector2 _movementInput = Vector2.zero;

    private bool _canMove = true;

    private AttackData _currentAttack = null;
    private bool _canAttack = true;
    private int _attackIndex = 1;
    private bool _shouldCombo = false;

    private bool _canParry = true;
    private bool _isParrying = false;
    private bool _perfectParry = false;

    private bool _isStunned = false;
    private bool _canClank = false;

    [SerializeField] private float _dashForce = 10f;
    [SerializeField] private float _dashCooldown = 1f;
    [SerializeField] private float _dashTime = 0.2f;
    private float _cooldown = 0;
    private bool _canDash = true;

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
    public EPlayerState LastState
    {
        get { return _lastState; }
    }
    public PlayerDamageManager PlayerDamageManager
    {
        get { return _playerDamageManager; }
    }
    public AttackData[] AttacksData
    {
        get
        {
            return _attacksData;
        }
    }
    public GameObject Hitbox
    {
        get { return _hitbox; }
    }
    public Sprite PerfectParryFrame
    {
        get { return _perfectParryFrame; }
    }
    public GameObject OtherPlayer
    {
        get { return _otherPlayer; }
        set { _otherPlayer = value; }
    }
    public Vector2 MovementInput
    {
        get { return _movementInput; }
    }
    public bool CanMove
    {
        get { return _canMove; }
    }
    public AttackData CurrentAttack
    {
        get { return _currentAttack; }
        set { _currentAttack = value; }
    }
    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }
    public int AttackIndex
    {
        get { return _attackIndex; }
        set { _attackIndex = value; }
    }
    public bool ShouldCombo
    {
        get { return _shouldCombo; }
        set { _shouldCombo = value; }
    }
    public bool CanParry
    {
        get { return _canParry; }
        set { _canParry = value; }
    }

    public bool IsParrying
    {
        get { return _isParrying; }
        set { _isParrying = value; }
    }
    public bool PerfectParry
    {
        get { return _perfectParry; }
        set { _perfectParry = value; }
    }
    public bool IsStunned
    {
        get { return _isStunned; }
    }
    public bool CanClank
    {
        get { return _canClank; }
        set { _canClank = value; }
    }
    public float DashTime
    {
        get { return _dashTime; }
    }
    public float DashCooldown
    {
        get { return _dashCooldown; }
    }
    public bool CanDash
    {
        get { return _canDash; }
    }
    public float FixedTime
    {
        get { return _fixedTime; }
        set { _fixedTime = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        FrameManager.Instance.FrameUpdate += UpdateOnFrame; 
        _cooldown = _dashCooldown + _dashTime;
        _states = new Dictionary<EPlayerState, APlayerState>();
        _states.Add(EPlayerState.IDLE, new IdleState());
        _states.Add(EPlayerState.MOVE, new MoveState());
        _states.Add(EPlayerState.THROW, new ThrowState());
        _states.Add(EPlayerState.MELEE, new MeleeBaseState());
        _states.Add(EPlayerState.PARRY, new ParryState());
        _states.Add(EPlayerState.DASH, new DashState());
        _states.Add(EPlayerState.HURT, new HurtState());
        _states.Add(EPlayerState.DEAD, new DeadState());
        foreach (KeyValuePair<EPlayerState, APlayerState> state in _states)
        {
            state.Value.Init(this, _animator, _spriteRenderer, _rb, _winManager);
        }
        _currentState = EPlayerState.IDLE;
        CurrentState.Enter();
    }

    // UpdateFrame is called once per frame
    public void UpdateOnFrame()
    {
        FixedTime += Time.deltaTime;
        SetDashCooldown();
        CurrentState.Update();
        _animator.SetFloat("Speed", _rb.velocity.x);
    }

    public void ChangeState(EPlayerState nextState)
    {
        //Debug.Log("Transition from " + CurrentState + " To " + nextState);
        CurrentState.Exit();
        _lastState = _currentState;
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
            _attackPressed();
        }
        
    }

    public void GetParryInput(InputAction.CallbackContext context)
    {
        if (_parryPressed != null && context.started)
        {
            _parryPressed();
        }
    }

    public void GetDashInput(InputAction.CallbackContext context)
    {
        //Debug.Log("input");
        if (_dashPressed != null && context.started)
        {
            //Debug.Log("DASH");
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

    public IEnumerator Dash()
    {
        _canDash = false;
        if (RecordInput().normalized.x == 0)
        {
            _rb.velocity = transform.right * _dashForce;
        }
        else
        {
            _rb.velocity = new Vector2(RecordInput().normalized.x * _dashForce, 0);
        }
        _cooldown = 0;
        yield return new WaitForSeconds(_dashTime);
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    public IEnumerator AttackCooldown(float cooldown)
    {
        _animator.speed = 0f;
        CanAttack = false;
        yield return new WaitForSeconds(cooldown);
        _animator.speed = 1f;
        CanAttack = true;
    }

    public IEnumerator HitStun(float time)
    {
        _isStunned = true;
        _animator.speed = 0f;

        yield return new WaitForSeconds(time);

        _animator.speed = 1f;
        _isStunned = false;
    }

    public void Knockback(float force, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            _rb.velocity = -transform.right * force;
        }
    }

    private void SetDashCooldown()
    {
        if (_cooldown < _dashCooldown + _dashTime)
        {
            _dashBar.SetDashTimeValue(_cooldown);
            _cooldown = Mathf.Clamp(_cooldown + Time.deltaTime, 0, _dashTime + _dashCooldown);
        }
    }

    public IEnumerator Clank()
    {
        _canMove = false;
        StartCoroutine(_playerDamageManager.Freeze());
        ChangeState(EPlayerState.IDLE);
        _rb.AddForce(-transform.right * _clankForce);
        yield return new WaitForSeconds(0.5f);
        _canMove = true;
    }

    public void ResetCombo()
    {
        _attackIndex = 1;
        _shouldCombo = false;
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
        _animator.SetBool("IsAttackingDash", false);
    }
}
