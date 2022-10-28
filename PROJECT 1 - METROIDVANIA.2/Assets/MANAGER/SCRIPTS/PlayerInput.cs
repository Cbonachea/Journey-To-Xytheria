using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float xInput;
    private float yInput;
    private float subWeaponInput;
    private float grappleInput;
    private float dashInput;
    private float slideInput;
    private float sipInput;
    private float switchDrinkInput;
    private float menuInput;
    private float mapInput;

    private bool jumpInput;
    private bool attackInput;


    private void Update()
    {
        xInput = (Input.GetAxis("Horizontal"));
        yInput = (Input.GetAxis("Vertical"));
        jumpInput = (Input.GetButton("Jump"));
        attackInput = (Input.GetButton("Attack"));
        subWeaponInput = (Input.GetAxis("SubWeapon"));
        grappleInput = (Input.GetAxis("Grapple"));
        dashInput = (Input.GetAxis("Dash"));
        slideInput = (Input.GetAxis("Slide"));
        sipInput = (Input.GetAxis("Sip"));
        switchDrinkInput = (Input.GetAxis("SwitchDrink"));
        menuInput = (Input.GetAxis("Menu"));
        mapInput = (Input.GetAxis("Map"));

        //    if (xInput == .2) GameEvents.current.Walk_R_Input();
        //    if (xInput == 0) GameEvents.current.Walk_R_Input_Idle();

        //    if (xInput == -.2) GameEvents.current.Walk_L_Input();
        //    if (xInput == 0) GameEvents.current.Walk_L_Input_Idle();

        if (jumpInput) GameEvents.current.Jump_Input();
        if (!jumpInput) GameEvents.current.Jump_Input_Idle();

        if (xInput == 1) GameEvents.current.Run_R_Input();
        if (xInput <= 0.8) GameEvents.current.Run_R_Input_Idle();
                
        if (xInput == -1) GameEvents.current.Run_L_Input();
        if (xInput >= -0.8) GameEvents.current.Run_L_Input_Idle();

        if (yInput == -1) GameEvents.current.Crouch_Input();
        if (yInput == 0) GameEvents.current.Crouch_Input_Idle();

        if (attackInput) GameEvents.current.Attack_Input();
        if (!attackInput) GameEvents.current.Attack_Input_Idle();

        if (subWeaponInput == 1) GameEvents.current.SubWeapon_Input();
        if (subWeaponInput == 0) GameEvents.current.SubWeapon_Input_Idle();

        if (grappleInput ==1) GameEvents.current.Grapple_Input();
        if (grappleInput == 0) GameEvents.current.Grapple_Input_Idle();

        if (dashInput >= .5f) GameEvents.current.Dash_Input();
        if (dashInput <= .5f) GameEvents.current.Dash_Input_Idle();

        if (slideInput == 1) GameEvents.current.Slide_Input();
        if (slideInput == 0) GameEvents.current.Slide_Input_Idle();

        if (sipInput == 1) GameEvents.current.Sip_Input();
        if (sipInput == 0) GameEvents.current.Sip_Input_Idle();

        if (switchDrinkInput == 1) GameEvents.current.SwitchDrink_Input();
        if (switchDrinkInput == 0) GameEvents.current.SwitchDrink_Input_Idle();

        if (menuInput == 1) GameEvents.current.Menu_Input();
        if (menuInput == 0) GameEvents.current.Menu_Input_Idle();

        if (mapInput == 1) GameEvents.current.Map_Input();
        if (mapInput == 0) GameEvents.current.Map_Input_Idle();


    }
}