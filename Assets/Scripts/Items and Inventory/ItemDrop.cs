using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
  [SerializeField] private int possibleItemDrop;
  [SerializeField] private ItemData[] possibleDrop;
  private List<ItemData> dropList = new List<ItemData>();

  [SerializeField] private GameObject dropFrefab;

  public virtual void GenerateDrop() {
    for (int i = 0; i < possibleDrop.Length; i++) {
      if (Random.Range(0, 100) <= possibleDrop[i].dropChance) {
        dropList.Add(possibleDrop[i]);
      }
    }

    for (int i = 0; i < possibleItemDrop; i++) {
      int index = Random.Range(0, dropList.Count - 1);
      if (index < dropList.Count) {

        ItemData randomItem = dropList[index];

        dropList.Remove(randomItem);
        DropItem(randomItem);
      }
    }
  }

  protected void DropItem(ItemData _itemData) {
    GameObject newDrop = Instantiate(dropFrefab, transform.position, Quaternion.identity);

    Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 20));

    newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
  }

}
