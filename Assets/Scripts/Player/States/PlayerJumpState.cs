
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private float timer;

    public PlayerJumpState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        Debug.Log($"Entering {GetType().ToString()}");

        player.SetSpeedModifier(1f);

        player.FloatingCapsule.enabled = false;
        
        player.Jump();

        timer = 0f;
    }

    public override void OnExit()
    {
        player.FloatingCapsule.enabled = true;
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if(timer > player.JumpDurationToApex)
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
