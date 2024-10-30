
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        player.SetSpeedModifier(1f);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (player.FloatingCapsule.IsGrounded)
        {
            player.ChangeState(player.DefaultState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
