using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb_player;
    private float verticleSpeed;
    private float horizontalSpeed;
    private Vector3 playerScale;
    private Transform attackPoint;
    private float attackRange = 0.5f;
    public LayerMask enemyLayer;
    private Animator animator;

    private bool isControlling;    
    private bool isFacingRight = true;
    private bool attack;
    private bool canAttack;
    private bool isAttacking;
    private bool jump;
    private bool canJump = true;
    private bool isJumping;
    private bool dash;
    private bool canDash = true;
    private bool isDashing;
    private bool grounded = true;
    private bool run_R;
    private bool run_L;
    private bool takeDamage;
    private bool die;



     
    [SerializeField][Range(0.0f, 70.0f)]private int hp = 100;        

    [SerializeField][Range(0.0f, 70.0f)]private float gravity = 3f;    
    [SerializeField][Range(0.0f, 70.0f)]private float fallGravity = 4f;    
    [SerializeField][Range(0.0f, 70.0f)]private float stopGravity = 10f;    
    [SerializeField][Range(0.0f, 70.0f)]private float maxFallSpeed = 25f;    

    [SerializeField][Range(0.0f, 70.0f)]private float jumpHeight = 25f;       
    [SerializeField][Range(0.0f, 70.0f)]private float jumpCoolDown = .3f;

    [SerializeField][Range(0.0f, 70.0f)]private float runSpeed = 9f;      
    [SerializeField][Range(0.0f, 70.0f)]private float maxRunSpeed = 5f;    

    [SerializeField][Range(0.0f, 1000.0f)]private float dashPower = 250f;      
    [SerializeField][Range(0.0f, 1000.0f)]private float dashCoolDown = .2f;      
    [SerializeField][Range(0.0f, 70.0f)]private float dashPeriod = .1f;

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
        rb_player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isControlling = true;
        rb_player.gravityScale = gravity;
        Debug.Log("Player Initialized - Systems Nominal");
    }
    private void SubscribeGameEvents()
    {
        GameEvents.current.onJump_Input += OnJump;
        GameEvents.current.onJump_Input_Idle += OnJump_Idle;
        GameEvents.current.onRun_R_Input += OnRun_R;
        GameEvents.current.onRun_R_Input_Idle += OnRun_R_Idle;        
        GameEvents.current.onRun_L_Input += OnRun_L;
        GameEvents.current.onRun_L_Input_Idle += OnRun_L_Idle;
        GameEvents.current.onAttack_Input += OnAttack;
        GameEvents.current.onAttack_Input_Idle += OnAttack_Idle;
        GameEvents.current.onDash_Input += OnDash;
        GameEvents.current.onDash_Input_Idle += OnDash_Idle;
        GameEvents.current.onTakeDamage += OnTakeDamage;
        GameEvents.current.onNoHp += OnDie;

        Debug.Log("Game Events Subscribed");
    }

    void Update()
    {
        
        if (!isControlling) return;
        if (isDashing) return;
        if (run_R || run_L) rb_player.gravityScale = gravity;
        if (!grounded && jump) rb_player.gravityScale = gravity;
        if (!grounded && !jump)
        {
            rb_player.gravityScale = fallGravity;
        }
        if (!run_R && !run_L && !jump && grounded)
        {
            rb_player.gravityScale = stopGravity;
        }
        if (rb_player.velocity.x > maxRunSpeed)
            rb_player.velocity = new Vector2(maxRunSpeed, rb_player.velocity.y);        
        if (rb_player.velocity.x < -maxRunSpeed)
            rb_player.velocity = new Vector2(-maxRunSpeed, rb_player.velocity.y);
        if (rb_player.velocity.y < -maxFallSpeed)
            rb_player.velocity = new Vector2(rb_player.velocity.x, -maxFallSpeed);
        verticleSpeed = rb_player.velocity.y;
        horizontalSpeed = rb_player.velocity.x;
        if (verticleSpeed < -.2f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else if (verticleSpeed > .2f) animator.SetBool("isJumping", true);
        if (Mathf.Abs(horizontalSpeed) >= .2f && grounded) animator.SetBool("isRunning", true);
        else if (Mathf.Abs(horizontalSpeed) <= .2f && grounded)
        {
            animator.SetBool("isStopping", false);
            animator.SetBool("isIdle", true);
        }
        Flip();


    }
    void FixedUpdate()
    {
        if (!isControlling) return;
        if (canJump && jump && grounded) StartCoroutine(Jump());
        if (canDash && dash) StartCoroutine(Dash());
        if (canAttack && attack) StartCoroutine(Attack());
        if (run_R == true) Run_R();
        if (run_L == true) Run_L();
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground") grounded = false;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground") grounded = true;
    }    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground") animator.SetBool("isFalling", false);
    }


    private void Run_R()
    {
        if (grounded) animator.SetBool("isRunning", true);
        rb_player.AddForce(transform.right * runSpeed, ForceMode2D.Impulse);
    }
    private void Run_L()
    {
        if (grounded) animator.SetBool("isRunning", true);
        rb_player.AddForce(transform.right * -runSpeed, ForceMode2D.Impulse);
    }
    private void Flip()
    {
        if (!isControlling) return;
        if(isFacingRight && run_L || !isFacingRight && run_R)
        {
            playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
            isFacingRight = !isFacingRight;
            Debug.Log(isFacingRight);
        }
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
        rb_player.angularDrag = 0f;
        rb_player.gravityScale = -0.4f;
        Debug.Log("Player Dead");
    }
    private void OnJump()
    {
        if (jump) return;
        if (!grounded) return;
        jump = true;
    }       
    private void OnJump_Idle()
    {
        if (!jump) return;
        jump = false;
    }
    private void OnDash()
    {
        if (dash) return;
        dash = true;
    }       
    private void OnDash_Idle()
    {
        if (!dash) return;
        dash = false;
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
    private void OnRun_R()
    {
        if (run_R == true) return;;
        run_R = true;
        animator.SetBool("isRunning", true);
    }
    private void OnRun_R_Idle()
    {
        if (!run_R) return;
        run_R = false;
        animator.SetBool("isRunning", false);
        animator.SetBool("isStopping", true);
    }
    private void OnRun_L()
    {
        if (run_L == true) return;
        run_L = true;
        animator.SetBool("isRunning", true);
    }
    private void OnRun_L_Idle()
    {
        if (!run_L) return;
        run_L = false;
        animator.SetBool("isRunning", false);
        animator.SetBool("isStopping", true);
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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb_player.gravityScale;
        float tempMaxRunSpeed = maxRunSpeed; 
        maxRunSpeed = dashPower;
        rb_player.gravityScale = 0f;
        rb_player.velocity = new Vector2(transform.localScale.x * dashPower, 0f);
        yield return new WaitForSeconds(dashPeriod);
        rb_player.gravityScale = originalGravity;
        isDashing = false;
        maxRunSpeed = tempMaxRunSpeed;
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }        
    private IEnumerator Jump()
    {
        canJump = false;
        //    isJumping = true;
        rb_player.gravityScale = 3f;
        rb_player.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpCoolDown);
    //    isJumping = false;
        canJump = true;
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
        GameEvents.current.onJump_Input -= OnJump;
        GameEvents.current.onJump_Input_Idle -= OnJump_Idle;
        GameEvents.current.onRun_R_Input -= OnRun_R;
        GameEvents.current.onRun_R_Input_Idle -= OnRun_R_Idle;        
        GameEvents.current.onRun_L_Input -= OnRun_L;
        GameEvents.current.onRun_L_Input_Idle -= OnRun_L_Idle;
        GameEvents.current.onAttack_Input -= OnAttack;
        GameEvents.current.onAttack_Input_Idle -= OnAttack_Idle;
        GameEvents.current.onTakeDamage -= OnTakeDamage;
        GameEvents.current.onNoHp -= OnDie;
    }
}
