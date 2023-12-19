using UnityEngine;

public class PlayerGroundedState : PlayerState {
  public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) {
  }

  public override void Enter() {
    base.Enter();
  }

  public override void Exit() {
    base.Exit();
  }

  public override void Update() {
    base.Update();

    if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked && player.skill.blackhole.cooldownTimer <= 0) {
      if (player.skill.blackhole.cooldownTimer > 0) {
        player.fx.CreatePopUpText("Cooldown");
        return;
      }
      stateMachine.ChangeState(player.blackhole);
    }

    if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.sword.swordUnlocked)
      stateMachine.ChangeState(player.aimSword);

    if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
      stateMachine.ChangeState(player.counterAttack);
    if (Input.GetKey(KeyCode.Mouse0))
      stateMachine.ChangeState(player.primaryAttack);

    if (!player.isGroundDetected())
      stateMachine.ChangeState(player.airState);

    if (Input.GetKeyDown(KeyCode.Space) && player.isGroundDetected())
      stateMachine.ChangeState(player.jumpState);
  }

  private bool HasNoSword() {
    if (!player.sword) return true;

    player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
    return false;
  }
}