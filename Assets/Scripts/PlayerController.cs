using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player ID")]
    [SerializeField] private int _playerID;

    [Header("Refs")]
    [SerializeField] private GameObject _capsule;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private AttackData[] _attacksData;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 7f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _dashForce = 20f;
    [SerializeField] private float _turnaroundForce = 10f;
    [SerializeField] private float _fallMultiplier = 10f;
    [SerializeField] private float _airDashForce = 15f;
    private Vector2 _movementInput = Vector2.zero;
    /// <summary>
    /// value that store a movement input to be later compared with the current movement input
    /// </summary>
    private Vector2 _tempMovementInput = Vector2.zero;

    [Header("Air Physics (Smash Style)")]
    [SerializeField] private float _playerAirForce = 30f; // acceleration in the air
    [SerializeField] private float _maxAirSpeed = 8f;     // max horizontal air speed
    [SerializeField] private float _airFriction = 5f;     // Friction when no input

    [Header("Directional Influence")]
    [SerializeField] private float _maxDIAngle = 18f; // max deviation in degrees

    [Header("Wall Bounce Settings")]
    [SerializeField] private float _minBounceVelocity = 5f; // Vitesse minimum pour rebondir
    [SerializeField] private float _bounciness = 0.8f;      // 1 = garde toute sa vitesse, 0.5 = perd la moitié
    [SerializeField] private LayerMask _wallLayer;          // Pour être sûr de ne rebondir que sur les murs
    private bool _isNextToWall = false;

    [Header("Jump Settings")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _shortHopForce = 5f;
    [SerializeField] private float _fullHopForce = 10f;
    [SerializeField] private float _doubleJumpForce = 7f;
    private bool _canJump = true;
    private bool _isFullHop = false;
    private uint _jumpPressedOnFrame = 0;
    private uint _jumpReleasedOnFrame = 0;
    [SerializeField] private bool _canDoubleJump = true;

    [Header("FastFall Settings")]
    private bool _isFastFalling = false;
    private bool _fastFallInput = false;

    private bool _canAirDash = false;

    private AttackData _currentAttack = null;
    private bool _canAttack = true;
    private int _attackIndex = 1;
    private bool _shouldCombo = false;

    private bool _isHitting = false;

    private int _deathCooldown = 5;

    public int PlayerID
    {
        get { return _playerID; }
    }
    public Animator Animator
    {
        get { return _animator; }
    }
    public Rigidbody2D Rb
    {
        get { return _rb; }
    }
    public SpriteRenderer SpriteRenderer
    {
        get { return _spriteRenderer; }
    }
    public PlayerHealth PlayerHealth
    {
        get { return _playerHealth; }
    }
    public AttackData[] AttacksData
    {
        get
        {
            return _attacksData;
        }
    }
    public Vector2 MovementInput
    {
        get { return _movementInput; }
    }
    public Vector2 TempMovementInput
    {
        get { return _tempMovementInput; }
        set { _tempMovementInput = value; }
    }
    public float ShortHopForce
    {
        get { return _shortHopForce; }
    }
    public float FullHopForce
    {
        get { return _fullHopForce; }
    }
    public float DoubleJumpForce
    {
        get { return _doubleJumpForce; }
    }
    public bool CanJump
    {
        get { return _canJump; }
        set { _canJump = value; }
    }
    public bool IsFullHop
    {
        get { return _isFullHop; }
        set { _isFullHop = value; }
    }
    public bool CanDoubleJump
    {
        get { return _canDoubleJump; }
        set { _canDoubleJump = value; }
    }
    public bool IsFastFalling
    {
        get { return _isFastFalling; }
        set { _isFastFalling = value; }
    }
    public bool CanAirDash
    {
        get { return _canAirDash; }
        set { _canAirDash = value; }
    }
    public AttackData CurrentAttack
    {
        get { return _currentAttack; }
        set
        {
            _currentAttack = value;
        }
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
    public bool IsHitting
    {
        get { return _isHitting; }
        set { _isHitting = value; }
    }
    public int DeathCooldown
    {
        get { return Mathf.Clamp(_deathCooldown, 0, 100); }
        set { _deathCooldown = value; }
    }

    #region Events
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

    private event Action _jumpPressed = null;
    public event Action JumpPressed
    {
        add
        {
            _jumpPressed -= value;
            _jumpPressed += value;
        }
        remove
        {
            _jumpPressed -= value;
        }
    }

    private event Action _airDashPressed = null;
    public event Action AirDashPressed
    {
        add
        {
            _airDashPressed -= value;
            _airDashPressed += value;
        }
        remove { _airDashPressed -= value; }
    }
    #endregion Events

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void GetJumpInput(InputAction.CallbackContext context)
    {
        if (_jumpPressed != null && context.started)
        {
            _jumpPressed();
            _jumpPressedOnFrame = FrameManager.Instance.ElapsedFrames;
            _jumpReleasedOnFrame = 0;
        }
        if (context.canceled)
        {
            _jumpReleasedOnFrame = FrameManager.Instance.ElapsedFrames;
        }
    }

    public void GetAttackInput(InputAction.CallbackContext context)
    {
        if (_attackPressed != null && context.started)
        {
            _attackPressed();
        }
    }

    public void GetFastFallInput(InputAction.CallbackContext context)
    {
        EPlayerState currentState = EPlayerState.NONE;
        if (context.started)
        {
            _fastFallInput = true;
        }
        else
        {
            _fastFallInput = false;
        }
        if (PlayerID == 1)
        {
            currentState = PlayerStateMachineManager.Instance.EnumCurrentStateP1;
        }
        else if (PlayerID == 2)
        {
            currentState = PlayerStateMachineManager.Instance.EnumCurrentStateP2;
        }
        if (!IsGrounded() && _rb.velocity.y <= 0.5f && _fastFallInput && currentState != EPlayerState.HURT && currentState != EPlayerState.DEAD)
        {
            _isFastFalling = true;
        }
    }

    public void GetAirDashInput(InputAction.CallbackContext context)
    {
        if (_airDashPressed != null && context.started)
        {
            _airDashPressed();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // checks if you are next to a wall and makes sure you bounce only once if you are next to one
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            _isNextToWall = true;
        }
        EPlayerState currentState = EPlayerState.NONE;
        if (PlayerID == 1) 
            currentState = PlayerStateMachineManager.Instance.EnumCurrentStateP1;
        else if (PlayerID == 2) 
            currentState = PlayerStateMachineManager.Instance.EnumCurrentStateP2;

        // 1. Vérification du Layer et de l'état
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0 && currentState == EPlayerState.HURT)
        {
            // 2. On utilise relativeVelocity pour avoir la vitesse réelle de l'impact
            // On prend la magnitude (longueur du vecteur) pour tester la force
            float impactSpeed = collision.relativeVelocity.magnitude;

            if (impactSpeed > _minBounceVelocity)
            {
                // On récupère la normale pour le rebond
                Vector2 normal = collision.contacts[0].normal;

                // On passe la vitesse d'impact à Bounce pour reconstruire le vecteur
                Bounce(normal, collision.relativeVelocity);
                _isNextToWall = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // checks if you are next to a wall and makes sure you bounce only once if you are next to one
        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {
            _isNextToWall = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // checks if you are next to a wall and makes sure you bounce only once if you are next to one
        if (_isNextToWall)
        {
            EPlayerState currentState = EPlayerState.NONE;
            if (PlayerID == 1)
                currentState = PlayerStateMachineManager.Instance.EnumCurrentStateP1;
            else if (PlayerID == 2)
                currentState = PlayerStateMachineManager.Instance.EnumCurrentStateP2;

            // 1. Vérification du Layer et de l'état
            if (((1 << collision.gameObject.layer) & _wallLayer) != 0 && currentState == EPlayerState.HURT)
            {
                // 2. On utilise relativeVelocity pour avoir la vitesse réelle de l'impact
                // On prend la magnitude (longueur du vecteur) pour tester la force
                float impactSpeed = collision.relativeVelocity.magnitude;

                if (impactSpeed > _minBounceVelocity)
                {
                    // On récupère la normale pour le rebond
                    Vector2 normal = collision.contacts[0].normal;

                    // On passe la vitesse d'impact à Bounce pour reconstruire le vecteur
                    Bounce(normal, collision.relativeVelocity);
                    _isNextToWall = false;
                }
            }
        }
        
    }

    private void FaceTheDirection(Vector2 dir)
    {
        if (dir.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (dir.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void ReverseRotation()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    public bool IsFacingRight()
    {
        // transform.right.x vaut 1 si le perso regarde à droite, et -1 s'il regarde à gauche.
        // Ça marchera toujours, peu importe si l'angle est 0, 360 ou 720.
        return transform.right.x > 0;
    }

    /// <summary>
    /// Move the character based on a vector2 input. Also makes the character faced the right direction
    /// </summary>
    /// <param name="dir"></param>
    public void Walk(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _walkSpeed, _rb.velocity.y);
        FaceTheDirection(dir);
    }

    public void Run(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _runSpeed, _rb.velocity.y);
        FaceTheDirection(dir);
    }

    public void Dash(Vector2 dir)
    {
        _rb.AddForce(new Vector2(Mathf.Sign(dir.x) * _dashForce, 0f), ForceMode2D.Impulse);
        FaceTheDirection(dir);
    }

    public void Turnaround(Vector2 dir)
    {
        //_rb.AddForce(new Vector2(Mathf.Sign(dir.x) * _turnaroundForce, 0f), ForceMode2D.Force);
        _rb.velocity = new Vector2(Mathf.Sign(dir.x) * _turnaroundForce, _rb.velocity.y);
    }

    public void AirMove(Vector2 dir)
    {
        _rb.AddForce(new Vector2(dir.x * _playerAirForce, 0), ForceMode2D.Force);

        // Limits the horizontal max speed
        if (Mathf.Abs(_rb.velocity.x) > _maxAirSpeed)
        {
            float cappedX = Mathf.Sign(_rb.velocity.x) * _maxAirSpeed;
            _rb.velocity = new Vector2(cappedX, _rb.velocity.y);
        }
    }

    /// <summary>
    /// Apply friction on the player
    /// </summary>
    public void AirFriction()
    {
        float frictionX = Mathf.MoveTowards(_rb.velocity.x, 0, _airFriction * Time.deltaTime);
        _rb.velocity = new Vector2(frictionX, _rb.velocity.y);
    }

    public void Jump(float jumpForce)
    {
        _rb.AddForce(new Vector2(0, Vector2.up.y * jumpForce), ForceMode2D.Impulse);
    }

    public void DoubleJump(float jumpForce)
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(new Vector2(0, Vector2.up.y * jumpForce), ForceMode2D.Impulse);
    }

    public void FastFall()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -1 * _fallMultiplier);
    }

    public void AirDash(Vector2 dir)
    {
        _rb.velocity = Vector2.zero;
        _rb.AddForce(dir * _airDashForce, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Reset the combo counter
    /// </summary>
    public void ResetCombo()
    {
        _attackIndex = 1;
        _shouldCombo = false;
        _animator.SetBool("IsAttacking1", false);
        _animator.SetBool("IsAttacking2", false);
        _animator.SetBool("IsAttacking3", false);
    }

    /// <summary>
    /// Checks if the player is grounded
    /// </summary>
    /// <returns></returns>
    public bool IsGrounded()
    {
        Bounds bounds = _capsule.GetComponent<BoxCollider2D>().bounds;
        RaycastHit2D hit = Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, 0.2f, _groundLayer);
        // Debug
        Color rayColor = (hit.collider != null) ? Color.green : Color.red;
        Debug.DrawRay(bounds.center + new Vector3(bounds.extents.x, 0), Vector2.down * (bounds.extents.y + 0.2f), rayColor);
        Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x, 0), Vector2.down * (bounds.extents.y + 0.2f), rayColor);
        Debug.DrawRay(bounds.center - new Vector3(bounds.extents.x, bounds.extents.y + 0.2f), Vector2.right * (bounds.size.x), rayColor);
        return hit;
    }

    public bool IsFullHopping()
    {
        return _jumpReleasedOnFrame - _jumpPressedOnFrame >= 5;
    }

    public void ApplyKnockback(Vector2 baseKnockback, Vector2 currentInput)
    {
        // If there is no input, apply normal knockback
        if (currentInput.sqrMagnitude < 0.1f)
        {
            _rb.velocity = baseKnockback;
            return;
        }

        // We get the angles in degrees
        float baseAngle = Mathf.Atan2(baseKnockback.y, baseKnockback.x) * Mathf.Rad2Deg;
        float inputAngle = Mathf.Atan2(currentInput.y, currentInput.x) * Mathf.Rad2Deg;

        // We find the difference between the hit angle and the input angle
        float angleDifference = Mathf.DeltaAngle(baseAngle, inputAngle);
 
        // The sinus is at his max (1, -1) when the angles are perpendicular and at  0 we there are parallel
        float diMultiplier = Mathf.Sin(angleDifference * Mathf.Deg2Rad);

        float finalAngle = baseAngle + (_maxDIAngle * diMultiplier);

        // We recreate the vector (different angle, same force)
        float knockbackForce = baseKnockback.magnitude;
        Vector2 finalKnockback = new Vector2(Mathf.Cos(finalAngle * Mathf.Deg2Rad), Mathf.Sin(finalAngle * Mathf.Deg2Rad)) * knockbackForce;

        _rb.velocity = finalKnockback;
    }

    public void TakeHit(AttackData attackTaken, float attackerPosX, Vector2 currentInput)
    {
        _playerHealth.TakeDamage(attackTaken.AttackDamage);

        // Calculate the direction of the impact (mirrored or not)
        float hitDirection = (transform.position.x > attackerPosX) ? 1f : -1f;

        // We create the final direction, mirrored or not
        Vector2 finalDirection = new Vector2(attackTaken.KnockbackDirection.x * hitDirection, attackTaken.KnockbackDirection.y);

        // We multiply the direction by the force
        Vector2 finalKnockbackVector = finalDirection * attackTaken.KnockbackForce;

        ApplyKnockback(finalKnockbackVector, currentInput);

        IsFastFalling = false;
    }

    private void Bounce(Vector2 wallNormal, Vector2 incomingVelocity)
    {
        // Calcul de la réflexion basée sur la vitesse avant impact
        Vector2 reflectedVelocity = Vector2.Reflect(incomingVelocity, wallNormal);

        // On applique le rebond (on inverse car relativeVelocity est opposée à notre mouvement)
        _rb.velocity = -reflectedVelocity * _bounciness;

        ReverseRotation();
        Debug.Log("BOUNCE REUSSI !");
    }
}