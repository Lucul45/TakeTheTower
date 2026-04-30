using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using static FrameManager;

public enum EPlayerState
{
    IDLE,
    GROUNDSTART,
    WALK,
    RUN,
    DASH,
    TURNAROUND,
    JUMPSTART,
    JUMP,
    AIRBASE,
    AIRMOVE,
    AIRJUMP,
    AIRDASH,
    JAB,
    HURT,
    DEAD,
    NONE
}

public class PlayerStateMachineManager : Singleton<PlayerStateMachineManager>
{
    #region Attributs
    [Header("Refs")]
    [SerializeField] private PlayerController[] _players = new PlayerController[2];

    [Header("States P1")]
    private Dictionary<EPlayerState, APlayerState> _statesP1 = null;
    private EPlayerState _currentStateP1;
    private EPlayerState _lastStateP1;

    [Header("States P2")]
    private Dictionary<EPlayerState, APlayerState> _statesP2 = null;
    private EPlayerState _currentStateP2;
    private EPlayerState _lastStateP2;

    private uint _lastAttackToIdleFrameP1 = 0;
    private uint _lastHurtToIdleFrameP1 = 0;
    private uint _lastAttackToIdleFrameP2 = 0;
    private uint _lastHurtToIdleFrameP2 = 0;
    #endregion Attributs

    #region Properties
    public APlayerState CurrentStateP1
    {
        get
        {
            return _statesP1[_currentStateP1];
        }
    }
    public EPlayerState EnumCurrentStateP1
    {
        get
        {
            return _currentStateP1;
        }
    }
    public APlayerState CurrentStateP2
    {
        get
        {
            return _statesP2[_currentStateP2];
        }
    }
    public EPlayerState EnumCurrentStateP2
    {
        get
        {
            return _currentStateP2;
        }
    }
    public EPlayerState LastStateP1
    {
        get { return _lastStateP1; }
    }
    public EPlayerState LastStateP2
    {
        get { return _lastStateP2; }
    }
    public uint LastAttackToIdleFrameP1
    {
        get { return _lastAttackToIdleFrameP1; }
    }
    public uint LastHurtToIdleFrameP1
    {
        get { return _lastHurtToIdleFrameP1; }
    }
    public uint LastAttackToIdleFrameP2
    {
        get { return _lastAttackToIdleFrameP2; }
    }
    public uint LastHurtToIdleFrameP2
    {
        get { return _lastHurtToIdleFrameP2; }
    }
    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        // Plug the update on the frames
        FrameManager.Instance.FrameUpdate += UpdateOnFrame;
        // Initializing the state machine
        _statesP1 = new Dictionary<EPlayerState, APlayerState>();
        _statesP1.Add(EPlayerState.IDLE, new IdleState());
        _statesP1.Add(EPlayerState.GROUNDSTART, new GroundMoveStartState());
        _statesP1.Add(EPlayerState.WALK, new WalkState());
        _statesP1.Add(EPlayerState.RUN, new RunState());
        _statesP1.Add(EPlayerState.DASH, new DashState());
        _statesP1.Add(EPlayerState.TURNAROUND, new TurnaroundState());
        _statesP1.Add(EPlayerState.JUMPSTART, new JumpStartState());
        _statesP1.Add(EPlayerState.JUMP, new JumpState());
        _statesP1.Add(EPlayerState.AIRBASE, new AirBaseState());
        _statesP1.Add(EPlayerState.AIRMOVE, new AirMoveState());
        _statesP1.Add(EPlayerState.AIRJUMP, new AirJumpState());
        _statesP1.Add(EPlayerState.AIRDASH, new AirDashState());
        _statesP1.Add(EPlayerState.JAB, new JabState());
        _statesP1.Add(EPlayerState.HURT, new HurtState());
        _statesP1.Add(EPlayerState.DEAD, new DeadState());

        _statesP2 = new Dictionary<EPlayerState, APlayerState>();
        _statesP2.Add(EPlayerState.IDLE, new IdleState());
        _statesP2.Add(EPlayerState.GROUNDSTART, new GroundMoveStartState());
        _statesP2.Add(EPlayerState.WALK, new WalkState());
        _statesP2.Add(EPlayerState.RUN, new RunState());
        _statesP2.Add(EPlayerState.DASH, new DashState());
        _statesP2.Add(EPlayerState.TURNAROUND, new TurnaroundState());
        _statesP2.Add(EPlayerState.JUMPSTART, new JumpStartState());
        _statesP2.Add(EPlayerState.JUMP, new JumpState());
        _statesP2.Add(EPlayerState.AIRBASE, new AirBaseState());
        _statesP2.Add(EPlayerState.AIRMOVE, new AirMoveState());
        _statesP2.Add(EPlayerState.AIRJUMP, new AirJumpState());
        _statesP2.Add(EPlayerState.AIRDASH, new  AirDashState());
        _statesP2.Add(EPlayerState.JAB, new JabState());
        _statesP2.Add(EPlayerState.HURT, new HurtState());
        _statesP2.Add(EPlayerState.DEAD, new DeadState());

        foreach (KeyValuePair<EPlayerState, APlayerState> state in _statesP1)
        {
            state.Value.Init(_players[1], this, _players[0].Animator, _players[0].SpriteRenderer, _players[0].Rb, _players[0], _players[0].PlayerHealth);
        }
        foreach (KeyValuePair<EPlayerState, APlayerState> state in _statesP2)
        {
            state.Value.Init(_players[0], this, _players[1].Animator, _players[1].SpriteRenderer, _players[1].Rb, _players[1], _players[1].PlayerHealth);
        }
        _currentStateP1 = EPlayerState.IDLE;
        _currentStateP2 = EPlayerState.IDLE;
        CurrentStateP1.Enter();
        CurrentStateP2.Enter();
    }

    // UpdateOnFrame is called once per frame
    public void UpdateOnFrame()
    {
        // Registering the data on each frame
        FrameActionData dataP1 = new FrameActionData()
        {
            PlayerID = _players[0].PlayerID,
            PlayerState = _currentStateP1,
            StateFrame = CurrentStateP1.StateFrame,
            IsHitting = _players[0].IsHitting
        };
        FrameActionData dataP2 = new FrameActionData()
        {
            PlayerID = _players[1].PlayerID,
            PlayerState = _currentStateP2,
            StateFrame = CurrentStateP2.StateFrame,
            IsHitting = _players[1].IsHitting
        };
        CurrentStateP1.Update();
        CurrentStateP2.Update();
        _players[0].Animator.SetFloat("MovementInput", _players[0].MovementInput.x);
        _players[1].Animator.SetFloat("MovementInput", _players[1].MovementInput.x);
        // Add a new frame data to the dictionary
        FrameManager.Instance.AddActionFrameData(dataP1);
        FrameManager.Instance.AddActionFrameData(dataP2);
        // Remove the earliest frame data of the dictionary
        FrameManager.Instance.RemoveActionFrameData();
    }

    // Change the state of the state machine and store on which frame it does
    public void ChangeState(int playerIndex, EPlayerState nextState)
    {
        if (playerIndex == _players[0].PlayerID)
        {
            UnityEngine.Debug.Log("Transition from " + CurrentStateP1 + " To " + nextState);
            CurrentStateP1.Exit();
            _lastStateP1 = _currentStateP1;

            // NOUVEAU : On nettoie les anciennes frames au moment où P1 lance une nouvelle attaque
            if (nextState == EPlayerState.JAB)
            {
                _lastAttackToIdleFrameP1 = 0;
                _lastHurtToIdleFrameP2 = 0;
            }

            if (_currentStateP1 == EPlayerState.JAB && nextState == EPlayerState.IDLE)
            {
                _lastAttackToIdleFrameP1 = FrameManager.Instance.ElapsedFrames;
                UnityEngine.Debug.Log($"Joueur {gameObject.name} (Attack) -> IDLE à la frame : {_lastAttackToIdleFrameP1}");
            }
            if (_currentStateP1 == EPlayerState.HURT && nextState == EPlayerState.IDLE)
            {
                _lastHurtToIdleFrameP1 = FrameManager.Instance.ElapsedFrames;
                UnityEngine.Debug.Log($"Joueur {gameObject.name} (Hurt) -> IDLE à la frame : {_lastHurtToIdleFrameP1}");
            }
            _currentStateP1 = nextState;
            CurrentStateP1.Enter();
        }
        else if (playerIndex == _players[1].PlayerID)
        {
            UnityEngine.Debug.Log("Transition from " + CurrentStateP2 + " To " + nextState);
            CurrentStateP2.Exit();
            _lastStateP2 = _currentStateP2;

            // NOUVEAU : Pareil pour P2 s'il attaque
            if (nextState == EPlayerState.JAB)
            {
                _lastAttackToIdleFrameP2 = 0;
                _lastHurtToIdleFrameP1 = 0;
            }

            if (_currentStateP2 == EPlayerState.JAB && nextState == EPlayerState.IDLE)
            {
                _lastAttackToIdleFrameP2 = FrameManager.Instance.ElapsedFrames;
                UnityEngine.Debug.Log($"Joueur {gameObject.name} (Attack) -> IDLE à la frame : {_lastAttackToIdleFrameP2}");
            }
            if (_currentStateP2 == EPlayerState.HURT && nextState == EPlayerState.IDLE)
            {
                _lastHurtToIdleFrameP2 = FrameManager.Instance.ElapsedFrames;
                UnityEngine.Debug.Log($"Joueur {gameObject.name} (Hurt) -> IDLE à la frame : {_lastHurtToIdleFrameP2}");
            }
            _currentStateP2 = nextState;
            CurrentStateP2.Enter();
        }
    }

    public void ResetLastAttackToIdleFrameP1()
    {
        _lastAttackToIdleFrameP1 = 0;
    }

    public void ResetLastHurtToIdleFrameP1()
    {
        _lastHurtToIdleFrameP1 = 0;
    }
    public void ResetLastAttackToIdleFrameP2()
    {
        _lastAttackToIdleFrameP2 = 0;
    }

    public void ResetLastHurtToIdleFrameP2()
    {
        _lastHurtToIdleFrameP2 = 0;
    }
}
