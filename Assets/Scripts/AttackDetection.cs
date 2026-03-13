using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<PlayerController>().IsHitting = true;
        if (collision.tag == "Player2")
        {
            PlayerStateMachineManager.Instance.ChangeStateP2(EPlayerState.HURT);
        }
        else if (collision.tag == "Player1")
        {
            PlayerStateMachineManager.Instance.ChangeStateP1(EPlayerState.HURT);
        }
        else if (collision.tag == "Tower")
        {
            collision.GetComponentInParent<TowerBehavior>().GetDamaged(GetComponentInParent<PlayerController>().CurrentAttack.AttackDamage);
        }
    }
}
