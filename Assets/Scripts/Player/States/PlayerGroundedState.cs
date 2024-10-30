
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log($"Entering {GetType().ToString()}");
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (!player.IsGrounded)
        {
            player.ChangeState(player.PlayerFallState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
