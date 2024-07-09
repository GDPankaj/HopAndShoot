using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWithGunState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.GetComponent<SpriteRenderer>().sprite = player.GetWithGunSprite();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.IsShooting()) return;
        if (Input.GetMouseButtonDown(0))
        {
            player.Shoot();
        }
        if (player.GetComponent<PlayerMovement>().IsJumpingAndNotGrounded())
        {
            player.PlayAnimation(player.GetJumpWithGun());
        }
        else if(player.GetComponent<PlayerMovement>().GetInputFactor() != 0) 
        {
            player.PlayAnimation(player.GetMoveWithGun());
        }
        else
        {
            player.PlayAnimation(player.GetIdleWithGun());
        }
    }
    
    public override void ExitState(PlayerStateManager player)
    {

    }

    public override void HandleInput(PlayerStateManager player)
    {
        throw new System.NotImplementedException();
    }
}
