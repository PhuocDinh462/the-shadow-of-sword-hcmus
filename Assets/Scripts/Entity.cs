using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour {

  #region Components
  public Animator anim { get; private set; }
  public Rigidbody2D rb { get; private set; }
  public EntityFX fx { get; private set; }

  public SpriteRenderer sr { get; private set; }
  public CharacterStats stats { get; private set; }
  public CapsuleCollider2D cd { get; private set; }
  #endregion


  [Header("KnockBack info")]
  [SerializeField] protected Vector2 knockBackDirection;
  [SerializeField] protected float knockBackDuration;
  protected bool isKnocked;

  [Header("Collision info")]
  public Transform attackCheck;
  public float attackCheckRadius;
  [SerializeField] protected Transform groundCheck;
  [SerializeField] protected float groundCheckDistance;
  [SerializeField] protected Transform wallCheck;
  [SerializeField] protected float wallCheckDistance;
  [SerializeField] protected LayerMask whatIsGround;

  public int facingDir { get; private set; } = 1;
  protected bool facingRight = true;

  public System.Action onFlipped;

  protected virtual void Awake() {

  }

  protected virtual void Start() {
    sr = GetComponentInChildren<SpriteRenderer>();
    anim = GetComponentInChildren<Animator>();
    rb = GetComponent<Rigidbody2D>();
    fx = GetComponent<EntityFX>();
    stats = GetComponent<CharacterStats>();
    cd = GetComponent<CapsuleCollider2D>();
  }

  protected virtual void Update() { }

  public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration) {

  }

  protected virtual void ReturnDefaultSpeed() {
    anim.speed = 1;
  }
  public virtual void DamageEffect() {
    fx.StartCoroutine("FlashFX");
    StartCoroutine("HitKnockBack");
    // Debug.Log(gameObject.name + " was damaged");
  }

  protected virtual IEnumerator HitKnockBack() {
    isKnocked = true;

    rb.velocity = new Vector2(knockBackDirection.x * -facingDir, knockBackDirection.y);

    yield return new WaitForSeconds(knockBackDuration);

    isKnocked = false;
  }

  #region Collision
  public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

  public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

  protected virtual void OnDrawGizmos() {
    Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
  }
  #endregion
  #region Velocity
  public void SetZeroVelocity() {
    if (isKnocked)
      return;
    rb.velocity = new Vector2(0, 0);
  }
  public void SetVelocity(float _xVelocity, float _yVelocity) {
    if (isKnocked)
      return;
    rb.velocity = new Vector2(_xVelocity, _yVelocity);
    FlipController(_xVelocity);
  }
  #endregion
  #region Flip
  public virtual void FlipController(float _x) {
    if ((_x > 0 && !facingRight) || (_x < 0 && facingRight))
      Flip();
  }

  public virtual void Flip() {
    facingDir *= -1;
    facingRight = !facingRight;
    transform.Rotate(0, 180, 0);

    if (onFlipped != null) {
      onFlipped();
    }
  }
  #endregion

  public void MakeTransparent(bool _transparent) {
    if (_transparent) {
      sr.color = Color.clear;
    }
    else {
      sr.color = Color.white;
    }
  }

  public virtual void Die() {

  }
}