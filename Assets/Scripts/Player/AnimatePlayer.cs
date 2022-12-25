using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        // Load components
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        // Subscribe to movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;

        // Subscribe to movement to position event
        player.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        // Subscribe to idle event
        player.idleEvent.OnIdle += IdleEvent_OnIdle;

        // Subscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        // Unsubscribe to movement by velocity event
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;

        // Unsubscribe to movement to position event
        player.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        // Unsubscribe to idle event
        player.idleEvent.OnIdle -= IdleEvent_OnIdle;

        // Unsubscribe to weapon aim event
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    /// <summary>
    /// On movement by velocity event handler
    /// </summary>
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent, MovementByVelocityArgs movementByVelocityArgs)
    {
        InitializeRollAnimationParameters();
        SetMovementAnimationParameters();
    }

    /// <summary>
    /// On movement to position event handler
    /// </summary>
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        InitializeAimAnimationParameters();
        InitializeRollAnimationParameters();
        SetMovementToPositionAnimationParameters(movementToPositionArgs);
    }

    /// <summary>
    /// On idle event handler
    /// </summary>
    private void IdleEvent_OnIdle(IdleEvent idleEvent)
    {
        InitializeRollAnimationParameters();
        SetIdleAnimationParameters();
    }

    /// <summary>
    /// On weapon aim event handler
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();
        InitializeRollAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// Initialize aim animation parameters
    /// </summary>
    private void InitializeAimAnimationParameters()
    {
        player.anim.SetBool(Settings.aimUp, false);
        player.anim.SetBool(Settings.aimUpRight, false);
        player.anim.SetBool(Settings.aimUpLeft, false);
        player.anim.SetBool(Settings.aimRight, false);
        player.anim.SetBool(Settings.aimLeft, false);
        player.anim.SetBool(Settings.aimDown, false);
    }

    /// <summary>
    /// Initialize roll animation parameters
    /// </summary>
    private void InitializeRollAnimationParameters()
    {
        player.anim.SetBool(Settings.rollDown, false);
        player.anim.SetBool(Settings.rollRight, false);
        player.anim.SetBool(Settings.rollLeft, false);
        player.anim.SetBool(Settings.rollUp, false);
    }

    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        player.anim.SetBool(Settings.isMoving, true);
        player.anim.SetBool(Settings.isIdle, false);
    }

    /// <summary>
    /// Set movement to position animation parameters
    /// </summary>
    private void SetMovementToPositionAnimationParameters(MovementToPositionArgs movementToPositionArgs)
    {
        // Animate roll
        if (movementToPositionArgs.isRolling)
        {
            if (movementToPositionArgs.moveDirection.x > 0f)
            {
                player.anim.SetBool(Settings.rollRight, true);
            }
            else if (movementToPositionArgs.moveDirection.x < 0f)
            {
                player.anim.SetBool(Settings.rollLeft, true);
            }
            else if(movementToPositionArgs.moveDirection.y > 0f)
            {
                player.anim.SetBool(Settings.rollUp, true);
            }
            else if(movementToPositionArgs.moveDirection.y < 0f)
            {
                player.anim.SetBool(Settings.rollDown, true);
            }
        }
    }

    /// <summary>
    /// Set movement animation parameters
    /// </summary>
    private void SetIdleAnimationParameters()
    {
        player.anim.SetBool(Settings.isMoving, false);
        player.anim.SetBool(Settings.isIdle, true);
    }

    /// <summary>
    /// Set aim animation parameters
    /// </summary>
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        // Set aim direction
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.anim.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.UpRight:
                player.anim.SetBool(Settings.aimUpRight, true);
                break;

            case AimDirection.UpLeft:
                player.anim.SetBool(Settings.aimUpLeft, true);
                break;

            case AimDirection.Right:
                player.anim.SetBool(Settings.aimRight, true);
                break;

            case AimDirection.Left:
                player.anim.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Down:
                player.anim.SetBool(Settings.aimDown, true);
                break;
        }
    }
}