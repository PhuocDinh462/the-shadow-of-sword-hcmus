using UnityEngine;

public class PlayerStats : CharacterStats {

  private Player player;
  protected override void Start() {
    base.Start();

    player = GetComponent<Player>();
  }

  public override void TakeDamage(int _damage) {
    base.TakeDamage(_damage);

    if (currentHealth - _damage > 0)
      AudioManager.instance.PlaySFX(Random.Range(31, 34));
  }


  protected override void Die() {
    base.Die();
    player.Die();

    AudioManager.instance.PlaySFX(34);
    GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
    PlayerManager.instance.currency = 0;

    GetComponent<PlayerItemDrop>()?.GenerateDrop();
  }

  protected override void DecreaseHealthBy(int _damage) {
    base.DecreaseHealthBy(_damage);
    if (isDead) return;

    if (_damage > GetMaxHealthValue() * .3f) {
      // player.SetupKnockbackPower(new Vector2(10, 6));

      int randomSound = Random.Range(34, 35);
      AudioManager.instance.PlaySFX(randomSound, null);
      AudioManager.instance.PlaySFX(randomSound, null);
    }

    ItemData_Equipment currentAmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

    currentAmor?.Effect(player.transform);
  }

  public override void OnEvasion() {

    player.skill.dodge.CreateMirageOnDodge();
  }

  public void CloneDoDamage(CharacterStats _targetStats, float _multiplier) {

    if (TargetCanAvoidAttack(_targetStats)) return;

    int totalDamage = damage.GetValue() + strength.GetValue();

    if (_multiplier > 0)
      totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);

    if (CanCrit()) {
      totalDamage = CalculateCriticalDamage(totalDamage);
    }

    totalDamage = CheckTargetArmor(_targetStats, totalDamage);
    _targetStats.TakeDamage(totalDamage);

    DoMagicalDamage(_targetStats); // remove if you dont want to apply magic hit on primary attack

  }
}
