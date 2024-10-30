
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.SetSpeedModifier(0f);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.MoveDirection.magnitude > 0)
        {
            player.ChangeState(player.PlayerWalkState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}