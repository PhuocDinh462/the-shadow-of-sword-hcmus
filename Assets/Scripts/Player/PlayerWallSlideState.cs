using UnityEngine;

public class PlayerWallSlideState : PlayerState {
  public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
  }

  public override void Enter() {
    base.Enter();
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();
    if(player.isWallDetected() == false) {
      stateMachine.ChangeState(player.airState);
    }
    if (Input.GetKeyDown(KeyCode.Space)) {
      stateMachine.ChangeState(player.wallJump);
      return;
    }

    if ((xInput != 0 && xInput != player.facingDir))
      stateMachine.ChangeState(player.idleState);

    if (yInput < 0)
      player.SetVelocity(0, rb.velocity.y);
    else
      player.SetVelocity(0, rb.velocity.y * .7f);

    if (player.isGroundDetected())
      stateMachine.ChangeState(player.idleState);
  }
}
