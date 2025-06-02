using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "AttackData")]
public class AttackData : ScriptableObject
{
    [SerializeField] private int _attackID;
    [SerializeField] private string _animationName;
    [SerializeField] private string _animatorCondition;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _hurtTime;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private Sprite[] _canComboFrames;
    [SerializeField] private Sprite _endFrame;

    public int AttackID
    {
        get { return _attackID; }
    }
    public string AnimationName
    {
        get { return _animationName; }
    }
    public string AnimatorCondition
    {
        get { return _animatorCondition; }
    }
    public float AttackCooldown
    {
        get { return _attackCooldown; }
    }
    public float HurtTime
    {
        get { return _hurtTime; }
    }
    public float KnockbackForce
    {
        get { return _knockbackForce; }
    }
    public Sprite[] CanComboFrames
    {
        get { return _canComboFrames; }
    }
    public Sprite EndFrame
    {
        get { return _endFrame; }
    }
}
