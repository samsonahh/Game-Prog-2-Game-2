using DG.Tweening;
using UnityEngine;

public class PlayerMineState : PlayerGroundedState
{
    Quaternion startRotation;
    Quaternion targetRotation;

    public PlayerMineState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.SetSpeedModifier(0f);

        startRotation = player.PickaxeTransform.localRotation;
        targetRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, 90f);

        player.PickaxeTransform.DOLocalRotateQuaternion(targetRotation, player.MineDuration/2).SetEase(Ease.InQuint).OnComplete(() => {
            player.Mine();
            player.PickaxeTransform.DOLocalRotateQuaternion(startRotation, player.MineDuration/2).SetEase(Ease.OutQuint).OnComplete(() => { player.ChangeState(player.DefaultState); });
        });
    }

    public override void OnExit()
    {
        DOTween.Kill(player.PickaxeTransform);
        player.PickaxeTransform.localRotation = startRotation;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
    }
}