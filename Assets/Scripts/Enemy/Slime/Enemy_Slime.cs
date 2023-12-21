public class Enemy_Slime : Enemy {

  #region States
  public SlimeIdleState idleState { get; private set; }
  public SlimeMoveState moveState { get; private set; }
  public SlimeBattleState battleState { get; private set; }
  public SlimeAttackState attackState { get; private set; }
  public SlimeStunnedState stunState { get; private set; }
  public SlimeDeadState deadState { get; private set; }

  #endregion

  protected override void Awake() {
    base.Awake();

    SetupDefaultFacingDir(-1);

    idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
    moveState = new SlimeMoveState(this, stateMachine, "Move", this);
    battleState = new SlimeBattleState(this, stateMachine, "Move", this);
    attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
    stunState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
    deadState = new SlimeDeadState(this, stateMachine, "Idle", this);

  }

  protected override void Start() {
    base.Start();

    stateMachine.Initialize(idleState);

  }

  public override bool CanbeStunned() {
    if (base.CanbeStunned()) {
      stateMachine.ChangeState(stunState);
      return true;
    }
    return false;

  }

  public override void Die() {
    base.Die();
    stateMachine.ChangeState(deadState);
  }
}
