using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefaultState : PlayerBaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        player.GetComponent<SpriteRenderer>().sprite = player.GetDefaultSprite();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if (player.GetComponent<PlayerMovement>().IsJumpingAndNotGrounded())
        {
            player.PlayAnimation(player.GetJump());
        }
        else if (player.GetComponent<PlayerMovement>().GetInputFactor() != 0)
        {
            player.PlayAnimation(player.GetMove());
        }
        else
        {
            player.PlayAnimation(player.GetIdle());
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
