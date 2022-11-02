using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (playerController.grounded == false) return;
        else if (collision.gameObject.tag == "ground") playerController.grounded = false;
        playerController.ChangeAnimationState("isGrounded", PlayerController.animStateType.Bool, bool.FalseString);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerController.grounded == true) return;
        else if (collision.gameObject.tag == "ground") playerController.grounded = true;
        playerController.ChangeAnimationState("isFalling", PlayerController.animStateType.Bool, bool.FalseString);
        playerController.ChangeAnimationState("isGrounded", PlayerController.animStateType.Bool, bool.TrueString);
    }
}
