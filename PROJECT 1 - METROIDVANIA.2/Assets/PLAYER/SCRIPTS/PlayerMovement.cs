using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Vector3 playerScale;
    private Rigidbody2D rb_player;
    [SerializeField] PlayerController playerController;


    private bool jump;
    private bool canJump = true;
    private bool isJumping;
    private bool dash;
    private bool canDash = true;
    private bool isDashing;

    private bool run_R;
    private bool run_L;
    private bool isFacingRight = true;
    private bool isControlling = true;

    [SerializeField] [Range(0.0f, 70.0f)] private float gravity = 3f;
    [SerializeField] [Range(0.0f, 70.0f)] private float fallGravity = 4f;
    [SerializeField] [Range(0.0f, 70.0f)] private float stopGravity = 10f;
    [SerializeField] [Range(0.0f, 70.0f)] private float maxFallSpeed = 25f;

    [SerializeField] [Range(0.0f, 70.0f)] private float jumpHeight = 25f;
    [SerializeField] [Range(0.0f, 70.0f)] private float jumpCoolDown = .3f;

    [SerializeField] [Range(0.0f, 70.0f)] private float runSpeed = 9f;
    [SerializeField] [Range(0.0f, 70.0f)] private float maxRunSpeed = 5f;

    [SerializeField] [Range(0.0f, 1000.0f)] private float dashPower = 250f;
    [SerializeField] [Range(0.0f, 1000.0f)] private float dashCoolDown = .2f;
    [SerializeField] [Range(0.0f, 70.0f)] private float dashPeriod = .1f;

    private void Start()
    {
        rb_player = GetComponent<Rigidbody2D>();
        rb_player.gravityScale = gravity;
        SubscribeMovementEvents();
    }

    private void SubscribeMovementEvents()
    {
        GameEvents.current.onJump_Input += OnJump;
        GameEvents.current.onJump_Input_Idle += OnJump_Idle;
        GameEvents.current.onRun_R_Input += OnRun_R;
        GameEvents.current.onRun_R_Input_Idle += OnRun_R_Idle;
        GameEvents.current.onRun_L_Input += OnRun_L;
        GameEvents.current.onRun_L_Input_Idle += OnRun_L_Idle; 
        GameEvents.current.onDash_Input += OnDash;
        GameEvents.current.onDash_Input_Idle += OnDash_Idle;
        Debug.Log("Movement Events Subscribed");
    }
    void Update()
    {
        if (!isControlling) return;
        if (run_R || run_L)
        {
            playerController.ChangeAnimationState("isRunning", PlayerController.animStateType.Bool, bool.TrueString);
            rb_player.gravityScale = gravity;
        }
        if (!playerController.grounded && jump) rb_player.gravityScale = gravity;
        if (!playerController.grounded && !jump)
        {
            rb_player.gravityScale = fallGravity;
        }
        if (!run_R && !run_L && !jump && playerController.grounded)
        {
            rb_player.gravityScale = stopGravity;
        }

        if (rb_player.velocity.x > maxRunSpeed)
            rb_player.velocity = new Vector2(maxRunSpeed, rb_player.velocity.y);
        if (rb_player.velocity.x < -maxRunSpeed)
            rb_player.velocity = new Vector2(-maxRunSpeed, rb_player.velocity.y);
        if (rb_player.velocity.y < -maxFallSpeed)
            rb_player.velocity = new Vector2(rb_player.velocity.x, -maxFallSpeed);
        Flip();


    }


    private void FixedUpdate()
    {
        if (!isControlling) return;
        if (canJump && jump && playerController.grounded) StartCoroutine(Jump());
        if (canDash && dash) StartCoroutine(Dash());
        if (playerController.grounded == false && !jump)
        {
            playerController.ChangeAnimationState("isFalling", PlayerController.animStateType.Bool, bool.TrueString);
        }
        if (run_R == true) Run_R();
        if (run_L == true) Run_L();
        else playerController.ChangeAnimationState("isRunning", PlayerController.animStateType.Bool, bool.FalseString);
    }


    private void OnJump()
    {
        if (jump) return;
        if (!playerController.grounded) return;
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
 
    private void OnRun_R()
    {
        if (run_R == true) return;
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
  
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        isControlling = false;
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
        isControlling = true;
        canDash = true;
    }
    private IEnumerator Jump()
    {
        playerController.ChangeAnimationState("isJumping", PlayerController.animStateType.Bool, bool.TrueString);
        canJump = false;
        isJumping = true;
        rb_player.gravityScale = 3f;
        rb_player.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
        yield return new WaitForSeconds(jumpCoolDown);
        isJumping = false;
        canJump = true;
        playerController.ChangeAnimationState("isJumping", PlayerController.animStateType.Bool, bool.FalseString);
    }

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
        if (!isControlling) return;
        if (isFacingRight && run_L || !isFacingRight && run_R)
        {
            playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
            isFacingRight = !isFacingRight;
            Debug.Log(isFacingRight);
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onJump_Input -= OnJump;
        GameEvents.current.onJump_Input_Idle -= OnJump_Idle;
        GameEvents.current.onRun_R_Input -= OnRun_R;
        GameEvents.current.onRun_R_Input_Idle -= OnRun_R_Idle;
        GameEvents.current.onRun_L_Input -= OnRun_L;
        GameEvents.current.onRun_L_Input_Idle -= OnRun_L_Idle;
    }
}
