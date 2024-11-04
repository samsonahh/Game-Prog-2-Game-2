using DG.Tweening;
using UnityEngine;

public class PlayerMineState : PlayerGroundedState
{
    Quaternion startRotation;
    Quaternion backRotation;
    Quaternion targetRotation;

    public PlayerMineState(Player player) : base(player)
    {
        this.player = player;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.SetSpeedModifier(player.MiningSpeedModifier);

        startRotation = player.PickaxeTransform.localRotation;
        backRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, startRotation.eulerAngles.z + 45f);
        targetRotation = Quaternion.Euler(startRotation.eulerAngles.x, startRotation.eulerAngles.y, startRotation.eulerAngles.z - 80f);

        Sequence sequence = DOTween.Sequence().SetId("Mine");
        sequence.Append(player.PickaxeTransform.DOLocalRotateQuaternion(backRotation, player.CurrentMineDuration / 2).SetEase(Ease.InBack));
        sequence.Append(player.PickaxeTransform.DOLocalRotateQuaternion(targetRotation, player.CurrentMineDuration / 4).SetEase(Ease.InBack).OnComplete(()=> { player.Mine(); }));
        sequence.Append(player.PickaxeTransform.DOLocalRotateQuaternion(startRotation, player.CurrentMineDuration / 4).SetEase(Ease.OutBack).OnComplete(() => { player.ChangeState(player.DefaultState); }));
    }

    public override void OnExit()
    {
        DOTween.Kill("Mine");
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