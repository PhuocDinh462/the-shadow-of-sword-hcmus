using UnityEngine;


[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/Thunder strike")]

public class ThunderStrike_Effect : ItemEffect {
  [SerializeField] private GameObject thunderStrikePrefab;

  public override void ExecuteEffect(Transform _enemyPosition) {
    AudioManager.instance.PlaySFX(29);
    GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _enemyPosition.position, Quaternion.identity);
    Destroy(newThunderStrike, 1);
  }
}
