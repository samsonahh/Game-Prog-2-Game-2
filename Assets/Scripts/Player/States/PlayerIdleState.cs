
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log($"Entering {GetType().ToString()}");

        player.SetSpeedModifier(0f);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if (player.MoveDirection.magnitude > 0)
        {
            player.ChangeState(player.PlayerWalkState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {

    }
}