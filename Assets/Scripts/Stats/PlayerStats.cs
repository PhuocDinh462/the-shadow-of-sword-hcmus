public class PlayerStats : CharacterStats {

  private Player player;
  protected override void Start() {
    base.Start();

    player = GetComponent<Player>();
  }

  public override void TakeDamage(int _damage) {
    base.TakeDamage(_damage);
  }


  protected override void Die() {
    base.Die();

    player.Die();

    GetComponent<PlayerItemDrop>()?.GenerateDrop();
  }

  protected override void DecreaseHealthBy(int _damage) {
    base.DecreaseHealthBy(_damage);

    ItemData_Equipment currentAmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

    currentAmor?.Effect(player.transform);
  }
}
