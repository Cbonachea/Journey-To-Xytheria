using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private string currentState;

    private bool takeDamage;
    private bool die;
    internal bool grounded;


     
    [SerializeField][Range(0, 100)]private int hp = 100;        
    [SerializeField][Range(0.0f, 250.0f)]private float heat = 100f;        
    [SerializeField] private PlayerMovement playerMovement;        
    [SerializeField] private PlayerCollision playerCollision;        
    [SerializeField] private PlayerAttack playerAttack;        





    void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        SubscribeGameEvents();
        animator = GetComponent<Animator>();
        Debug.Log("Player Animator Initialized");
    }
    private void SubscribeGameEvents()
    {
        GameEvents.current.onTakeDamage += OnTakeDamage;
        GameEvents.current.onNoHp += OnDie;

    /*    GameEvents.current.onAttack_Input += OnAttack;
        GameEvents.current.onAttack_Input_Idle += OnAttack_Idle;

        GameEvents.current.onJump_Input += OnJump;
        GameEvents.current.onJump_Input_Idle += OnJump_Idle;
    */
        GameEvents.current.onRun_R_Input += OnRun_R;
      /*
        GameEvents.current.onRun_R_Input_Idle += OnRun_R_Idle;
        GameEvents.current.onRun_L_Input += OnRun_L;
        GameEvents.current.onRun_L_Input_Idle += OnRun_L_Idle;
        GameEvents.current.onDash_Input += OnDash;
        GameEvents.current.onDash_Input_Idle += OnDash_Idle;

        */

        Debug.Log("Game Events Subscribed");
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play("newState");
        currentState = newState;
    }


    private void NoHp()
    {
        if (die == true) return;
        Die();
    }
    private void OnDie()
    {
        Die();
    }
    private void Die()
    {
        Debug.Log("Player Dead");
        Destroy(this);
    }
    private void OnTakeDamage()
    {
        TakeDamage();
    }
    private void TakeDamage()
    {
        hp--;
        Debug.Log("HP = " + hp);
        CheckHp();
    }
    private void CheckHp()
    {
        if (hp < 1)
        {
            Die();
        }
    }
    private void OnRun_R()
    {
        ChangeAnimationState(playerRun);
    }



    private void OnDestroy()
    {
        GameEvents.current.onTakeDamage -= OnTakeDamage;
        GameEvents.current.onNoHp -= OnDie;
    }

}
