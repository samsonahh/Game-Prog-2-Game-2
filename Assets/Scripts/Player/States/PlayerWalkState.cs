
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log($"Entering {GetType().ToString()}");

        player.SetSpeedModifier(1f);
    }

    public override void OnExit()
    {

    }

    public override void OnUpdate()
    {
        if(player.MoveDirection.magnitude == 0)
        {
            player.ChangeState(player.PlayerIdleState);
            return;
        }
    }

    public override void OnFixedUpdate()
    {
        player.Move();
    }
}
