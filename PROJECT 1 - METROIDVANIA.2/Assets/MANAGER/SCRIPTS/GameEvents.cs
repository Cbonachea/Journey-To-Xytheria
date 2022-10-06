using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action onJump_Input;
    public event Action onJump_Input_Idle;
    public event Action onRun_R_Input;
    public event Action onRun_R_Input_Idle;    
    public event Action onRun_L_Input;
    public event Action onRun_L_Input_Idle;
    public event Action onWalk_R_Input;
    public event Action onWalk_R_Input_Idle;    
    public event Action onWalk_L_Input;
    public event Action onWalk_L_Input_Idle;
    public event Action onCrouch_Input;
    public event Action onCrouch_Input_Idle;
    public event Action onAttack_Input;
    public event Action onAttack_Input_Idle;
    public event Action onSubWeapon_Input;
    public event Action onSubWeapon_Input_Idle;
    public event Action onGrapple_Input;
    public event Action onGrapple_Input_Idle;
    public event Action onDash_Input;
    public event Action onDash_Input_Idle;
    public event Action onSlide_Input;
    public event Action onSlide_Input_Idle;
    public event Action onSip_Input;
    public event Action onSip_Input_Idle;
    public event Action onSwitchDrink_Input;
    public event Action onSwitchDrink_Input_Idle;
    public event Action onMenu_Input;
    public event Action onMenu_Input_Idle;
    public event Action onMap_Input;
    public event Action onMap_Input_Idle;
    public event Action onTakeDamage;
    public event Action onNoHp;

    public void Jump_Input()
    {
        if (onJump_Input != null)
        {
            onJump_Input();
        }
    }       
    public void Jump_Input_Idle()
    {
        if (onJump_Input_Idle != null)
        {
            onJump_Input_Idle();
        }
    }    
    public void Run_R_Input()
    {
        if (onRun_R_Input != null)
        {
            onRun_R_Input();
        }
    }     
    public void Run_R_Input_Idle()
    {
        if (onRun_R_Input_Idle != null)
        {
            onRun_R_Input_Idle();
        }
    }    
    public void Run_L_Input()
    {
        if (onRun_L_Input != null)
        {
            onRun_L_Input();
        }
    }     
    public void Run_L_Input_Idle()
    {
        if (onRun_L_Input_Idle != null)
        {
            onRun_L_Input_Idle();
        }
    }
    public void Walk_R_Input()
    {
        if (onWalk_R_Input != null)
        {
            onWalk_R_Input();
        }
    }
    public void Walk_R_Input_Idle()
    {
        if (onWalk_R_Input_Idle != null)
        {
            onWalk_R_Input_Idle();
        }
    }    
    public void Walk_L_Input()
    {
        if (onWalk_L_Input != null)
        {
            onWalk_L_Input();
        }
    }
    public void Walk_L_Input_Idle()
    {
        if (onWalk_L_Input_Idle != null)
        {
            onWalk_L_Input_Idle();
        }
    }
    public void Crouch_Input()
    {
        if (onCrouch_Input != null)
        {
            onCrouch_Input();
        }
    }
    public void Crouch_Input_Idle()
    {
        if (onCrouch_Input_Idle != null)
        {
            onCrouch_Input_Idle();
        }
    }
    public void Attack_Input()
    {
        if (onAttack_Input != null)
        {
            onAttack_Input();
        }
    }       
    public void Attack_Input_Idle()
    {
        if (onAttack_Input_Idle != null)
        {
            onAttack_Input_Idle();
        }
    }
    public void SubWeapon_Input()
    {
        if (onSubWeapon_Input != null)
        {
            onSubWeapon_Input();
        }
    }
    public void SubWeapon_Input_Idle()
    {
        if (onSubWeapon_Input_Idle != null)
        {
            onSubWeapon_Input_Idle();
        }
    }
    public void Grapple_Input()
    {
        if (onGrapple_Input != null)
        {
            onGrapple_Input();
        }
    }
    public void Grapple_Input_Idle()
    {
        if (onGrapple_Input_Idle != null)
        {
            onGrapple_Input_Idle();
        }
    }
    public void Dash_Input()
    {
        if (onDash_Input != null)
        {
            onDash_Input();
        }
    }
    public void Dash_Input_Idle()
    {
        if (onDash_Input_Idle != null)
        {
            onDash_Input_Idle();
        }
    }
    public void Slide_Input()
    {
        if (onSlide_Input != null)
        {
            onSlide_Input();
        }
    }
    public void Slide_Input_Idle()
    {
        if (onSlide_Input_Idle != null)
        {
            onSlide_Input_Idle();
        }
    }
    public void Sip_Input()
    {
        if (onSip_Input != null)
        {
            onSip_Input();
        }
    }
    public void Sip_Input_Idle()
    {
        if (onSip_Input_Idle != null)
        {
            onSip_Input_Idle();
        }
    }
    public void SwitchDrink_Input()
    {
        if (onSwitchDrink_Input != null)
        {
            onSwitchDrink_Input();
        }
    }
    public void SwitchDrink_Input_Idle()
    {
        if (onSwitchDrink_Input_Idle != null)
        {
            onSwitchDrink_Input_Idle();
        }
    }
    public void Menu_Input()
    {
        if (onMenu_Input != null)
        {
            onMenu_Input();
        }
    }
    public void Menu_Input_Idle()
    {
        if (onMenu_Input_Idle != null)
        {
            onMenu_Input_Idle();
        }
    }
    public void Map_Input()
    {
        if (onMap_Input != null)
        {
            onMap_Input();
        }
    }
    public void Map_Input_Idle()
    {
        if (onMap_Input_Idle != null)
        {
            onMap_Input_Idle();
        }
    }
    public void TakeDamage()
    {
        if (onTakeDamage != null)
        {
            onTakeDamage();
        }
    }    
    public void Die()
    {
        if (onNoHp != null)
        {
            onNoHp();
        }
    }

}