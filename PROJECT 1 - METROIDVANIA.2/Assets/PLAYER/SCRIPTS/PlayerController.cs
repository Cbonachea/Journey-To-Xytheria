using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Transform attackPoint;
    private float attackRange = 0.5f;
    public LayerMask enemyLayer;
    private Animator animator;

    private bool isControlling;
    private bool attack;
    private bool canAttack;
    private bool isAttacking;
    private bool takeDamage;
    private bool die;
    internal bool grounded;


     
    [SerializeField][Range(0.0f, 70.0f)]private int hp = 100;        
    [SerializeField] private PlayerMovement playerMovement;        
    [SerializeField] private PlayerCollision playerCollision;        



    [SerializeField][Range(0.0f, 70.0f)]private float attackDamage = 1f;
    [SerializeField][Range(0.0f, 70.0f)]private float attackWindUp = .3f;
    [SerializeField][Range(0.0f, 70.0f)]private float attackCoolDown = .3f;

    void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        SubscribeGameEvents();
        animator = GetComponent<Animator>();
        isControlling = true;

        Debug.Log("Player Initialized - Systems Nominal");
    }
    private void SubscribeGameEvents()
    {

        GameEvents.current.onAttack_Input += OnAttack;
        GameEvents.current.onAttack_Input_Idle += OnAttack_Idle;

        GameEvents.current.onTakeDamage += OnTakeDamage;
        GameEvents.current.onNoHp += OnDie;

        Debug.Log("Game Events Subscribed");
    }



 





    private void CheckHp()
    {
        if(hp >= 1)
        {
            hp--;
            Debug.Log("HP = " + hp);
        }
        if(hp < 1)
        {
            Die();
        }
    }
    private void TakeDamage()
    {
        CheckHp();
    }
    private void Die()
    {
        isControlling = false;
        Debug.Log("Player Dead");
    }

    private void OnAttack()
    {
        if (attack == true) return;
        attack = true;
    }    
    private void OnAttack_Idle()
    {
        if (!attack) return;
        attack = false;
    }

    private void NoHp()
    {
        if (die == true) return;
        Die();
    }
    private void OnDie()
    {

    }    
    private void OnTakeDamage()
    {
        TakeDamage();
    }


    private IEnumerator Attack()
    {
        isControlling = false;
        canAttack = false;
        isAttacking = true;
        //enter wind up state animation trigger
        yield return new WaitForSeconds(attackWindUp);
        
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
        }
        
        //enter recovery state animation trigger
        yield return new WaitForSeconds(attackCoolDown);
        isControlling = true;
        canAttack = true;
    }

    private void OnDestroy()
    {

        GameEvents.current.onAttack_Input -= OnAttack;
        GameEvents.current.onAttack_Input_Idle -= OnAttack_Idle;
        GameEvents.current.onTakeDamage -= OnTakeDamage;
        GameEvents.current.onNoHp -= OnDie;
    }
}
