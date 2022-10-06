using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool isControlling;    
    private Rigidbody2D rb_player;

    private bool attack;
    private bool jump;
    private bool canJump = true;
    private bool dash;
    private bool isDashing;
    private bool canDash = true;
    private bool grounded = true;
    private bool run_R;
    private bool run_L;
    private bool isFacingRight;
    private bool takeDamage;
    private bool die;
    private bool jumpTimerSet = false;
    private float newJumpTimer;


     
    [SerializeField][Range(0.0f, 70.0f)]private int hp = 100;        
    [SerializeField][Range(0.0f, 70.0f)]private float runSpeed = 5f;      
    [SerializeField][Range(0.0f, 1000.0f)]private float dashPower = 250f;      
    [SerializeField][Range(0.0f, 1000.0f)]private float dashCoolDown = .5f;      
    [SerializeField][Range(0.0f, 70.0f)]private float maxRunSpeed = 5f;    
    [SerializeField][Range(0.0f, 70.0f)]private float jumpHeight = 25f;       
    [SerializeField][Range(0.0f, 70.0f)]private float maxFallSpeed = 25f;    
    [SerializeField][Range(0.0f, 70.0f)]private float gravity = 3f;    
    [SerializeField][Range(0.0f, 70.0f)]private float fallGravity = 4f;    
    [SerializeField][Range(0.0f, 70.0f)]private float stopGravity = 10f;    
    [SerializeField][Range(0.0f, 70.0f)]private float jumpTimer = .2f;
    [SerializeField][Range(0.0f, 70.0f)]private float dashPeriod = .2f;

    void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        SubscribeGameEvents();
        rb_player = GetComponent<Rigidbody2D>();
        isControlling = true;
        rb_player.gravityScale = gravity;
        dashPeriod = .5f;
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
        if (!grounded && !jump) rb_player.gravityScale = fallGravity;
        if (!run_R && !run_L && !jump && grounded) rb_player.gravityScale = stopGravity;
        if (rb_player.velocity.x > maxRunSpeed)
            rb_player.velocity = new Vector2(maxRunSpeed, rb_player.velocity.y);
        if (rb_player.velocity.y < -maxFallSpeed)
            rb_player.velocity = new Vector2(rb_player.velocity.x, -maxFallSpeed);
        Flip();
        if (jumpTimerSet)
        {
            canJump = false;
            if (newJumpTimer > 0)
            {
                newJumpTimer -= Time.deltaTime;
            }
            else
            {
                canJump = true;
                jumpTimerSet = false;
            }
        }        
/*        
 *        if (dashTimerSet)
        {
            canDash = false;
            rb_player.gravityScale = 0;
            if (newDashTimer > 0)
            {
                newDashTimer -= Time.deltaTime;
            }
            else
            {
                canDash = true;
                dashTimerSet = false;
                rb_player.gravityScale = gravity;
            }
        }
*/
    }
    void FixedUpdate()
    {
        if (!isControlling) return;
        if (jump && grounded) Jump();
        if (canDash && dash) StartCoroutine(Dash());
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

    private void Jump()
    {
        if (canJump)
        {
            newJumpTimer = jumpTimer;
            jumpTimerSet = true;
            grounded = false;
            rb_player.gravityScale = gravity;
            rb_player.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        }

    }    

/*    
 *    private void Dash()
    {
        if (canDash)
        {
            newDashTimer = dashTimer;
            dashTimerSet = true;
            Debug.Log("Dash");
            rb_player.gravityScale = gravity - gravity;
            rb_player.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
            canDash = false;
        }

    }
*/
    private void Run_R()
    {
        rb_player.AddForce(transform.right * runSpeed, ForceMode2D.Impulse);
    }
    private void Run_L()
    {
        rb_player.AddForce(transform.right * -runSpeed, ForceMode2D.Impulse);
    }
    private void Flip()
    {
        if(isFacingRight && run_L || !isFacingRight && run_R)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
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
    private void Attack()
    {

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
        Attack();
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
    }
    private void OnRun_R_Idle()
    {
        if (!run_R) return;
        run_R = false;
    }
    private void OnRun_L()
    {
        if (run_L == true) return;
        run_L = true;
    }    
    private void OnRun_L_Idle()
    {
        if (!run_L) return;
        run_L = false;
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
