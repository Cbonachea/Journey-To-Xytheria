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
        ChangeAnimationState("isRunning", PlayerController.animStateType.Bool, bool.FalseString);
        Debug.Log("Player Animator Initialized");
    }
    private void SubscribeGameEvents()
    {
        GameEvents.current.onTakeDamage += OnTakeDamage;
        GameEvents.current.onNoHp += OnDie;
        Debug.Log("Game Events Subscribed");
    }


    public enum animStateType
    {
        Bool,
        Trigger,
        Int,
        Float,
    }

    public void ChangeAnimationState(string condition, animStateType type = animStateType.Trigger ,string value = null)
    {
        //this is basically a complicated if statement courtesy of Brent L.
        switch (type)
        {
            case animStateType.Trigger:
                animator.SetTrigger(condition);
                break;
            case animStateType.Bool:
                animator.SetBool(condition, bool.Parse(value));
                break;
            case animStateType.Int:
                animator.SetInteger(condition, int.Parse(value));
                break;
            case animStateType.Float:
                animator.SetFloat(condition, float.Parse(value));
                break;
        }
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
        ChangeAnimationState("isTakingdamage");
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

    private void OnDestroy()
    {
        GameEvents.current.onTakeDamage -= OnTakeDamage;
        GameEvents.current.onNoHp -= OnDie;
    }

}
