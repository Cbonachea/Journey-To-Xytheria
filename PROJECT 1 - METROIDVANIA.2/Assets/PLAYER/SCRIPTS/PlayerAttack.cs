using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private Transform attackPoint;
    private float attackRange = 0.5f;

    private bool attack;
    private bool canAttack = true;
    private bool isAttacking = false;

    [SerializeField] PlayerController playerController;
    [SerializeField] [Range(0.0f, 70.0f)] private float attackDamage = 1f;
    [SerializeField] [Range(0.0f, 70.0f)] private float attackWindUp = .3f;
    [SerializeField] [Range(0.0f, 70.0f)] private float attackCoolDown = .3f;
    private void Start()
    {
        SubscribeAttackEvents();
    }
    private void SubscribeAttackEvents()
    {
        GameEvents.current.onAttack_Input += OnAttack;
        GameEvents.current.onAttack_Input_Idle += OnAttack_Idle;
        Debug.Log("Attack Events Subscribed");
    }

    void Update()
    {
        if (attack && canAttack)
        {
            StartCoroutine(Attack());
            canAttack = false;
        }
    }

    private void OnAttack()
    {
        if (attack) return;
        attack = true;
    }
    private void OnAttack_Idle()
    {
        if (!attack) return;
        attack = false;
    }
    private IEnumerator Attack()
    {
        playerController.ChangeAnimationState("isAttacking", PlayerController.animStateType.Bool, bool.TrueString);
        isAttacking = true;
        //enter wind up state animation trigger
        yield return new WaitForSeconds(attackWindUp);

    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        Debug.Log("Hit" + enemy.name);
    //    }

        //enter recovery state animation trigger

        playerController.ChangeAnimationState("isAttacking", PlayerController.animStateType.Bool, bool.FalseString);
        isAttacking = false;
        yield return new WaitForSeconds(attackCoolDown);
        canAttack = true;
    }
    private void OnDestroy()
    {
        GameEvents.current.onAttack_Input -= OnAttack;
        GameEvents.current.onAttack_Input_Idle -= OnAttack_Idle;
    }
}
