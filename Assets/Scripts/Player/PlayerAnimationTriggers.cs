using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour {
  private Player player => GetComponentInParent<Player>();

  private void AnimationTrigger() {
    player.AnimationTrigger();
  }

  private void AttackTrigger() {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
    bool haveEnemies = false;

    foreach (var hit in colliders) {
      if (hit.GetComponent<Enemy>() != null) {
        haveEnemies = true;
        EnemyStats _target = hit.GetComponent<EnemyStats>();

        if (_target != null)
          player.stats.DoDamage(_target);


        ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

        if (weaponData != null)
          weaponData.Effect(_target.transform);

      }
    }

    if (haveEnemies)
      AudioManager.instance.PlaySFX(0);
    else
      AudioManager.instance.PlaySFX(2);
  }

  private void ThrowSword() {
    SkillManager.instance.sword.CreateSword();
  }
}
