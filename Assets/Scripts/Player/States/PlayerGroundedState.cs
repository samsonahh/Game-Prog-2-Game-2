
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (!player.FloatingCapsule.IsGrounded)
        {
            player.ChangeState(player.PlayerFallState);
            return;
        }

        player.HandleMineInput();
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
