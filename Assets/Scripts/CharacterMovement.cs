using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;

    [SerializeField] private float _playerSpeed = 10f;

    private Vector2 _movementInput = Vector2.zero;
    private bool _attack = false;
    private bool _isAttacking = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move(_movementInput);

        if (_attack && !_isAttacking)
        {
            _isAttacking = true;
            Debug.Log("ATTACK");
            _isAttacking = false;
        }
    }

    private void Move(Vector2 dir)
    {
        _rb.velocity = new Vector2(dir.x * _playerSpeed, 0);
    }

    public void GetMovementInput(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void GetAttackInput(InputAction.CallbackContext context)
    {
        _attack = context.action.IsPressed();
    }
}
