using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public LayerMask enemyLayer;
    private Animator animator;

    private bool takeDamage;
    private bool die;
    internal bool grounded;


     
    [SerializeField][Range(0.0f, 70.0f)]private int hp = 100;        
    [SerializeField] private PlayerMovement playerMovement;        
    [SerializeField] private PlayerCollision playerCollision;        





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

        Debug.Log("Game Events Subscribed");
    }








    private void TakeDamage()
    {
        hp--;
        Debug.Log("HP = " + hp);
        CheckHp();
    }
    private void CheckHp()
    {
        if(hp < 1)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player Dead");
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




    private void OnDestroy()
    {
        GameEvents.current.onTakeDamage -= OnTakeDamage;
        GameEvents.current.onNoHp -= OnDie;
    }

}
