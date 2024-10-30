
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.SetSpeedModifier(1f);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if(player.MoveDirection.magnitude == 0)
        {
            player.ChangeState(player.PlayerIdleState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}

public class PlayerSlideState : PlayerGroundedState
{
    public PlayerSlideState(Player player) : base(player)
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

        if (player.MoveDirection.magnitude == 0)
        {
            player.ChangeState(player.PlayerIdleState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}
