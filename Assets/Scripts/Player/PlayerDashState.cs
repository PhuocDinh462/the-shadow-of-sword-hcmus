using UnityEngine;

public class PlayerDashState : PlayerState {
  public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
  }

  public override void Enter() {
    base.Enter();

    player.skill.clone.CreateCloneOnDashStart();

    stateTimer = player.dashDuration;
  }

  public override void Exit() {

    player.skill.clone.CreateCloneOnDashOver();

    base.Exit();
    player.SetVelocity(0, rb.velocity.y);
  }

  public override void Update() {
    base.Update();

    if (!player.isGroundDetected() && player.isWallDetected())
      stateMachine.ChangeState(player.wallSlide);

    player.SetVelocity(player.dashSpeed * player.dashDir, 0);

    if (stateTimer < 0)
      stateMachine.ChangeState(player.idleState);
  }
}
