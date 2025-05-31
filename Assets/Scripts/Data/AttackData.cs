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
    [SerializeField] private Sprite[] _canComboFrames;

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
    public Sprite[] CanComboFrames
    {
        get { return _canComboFrames; }
    }
}
