using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
  [Header("Player's drop")]
  [SerializeField] private float chanceToLooseItems;
  [SerializeField] private float chanceToLooseMaterials;

  public override void GenerateDrop() {
    base.GenerateDrop();

    Inventory inventory = Inventory.instance;

    List<InventoryItem> itemsToUnequips = new List<InventoryItem>();
    List<InventoryItem> materialsToLoose = new List<InventoryItem>();

    foreach (InventoryItem item in inventory.GetEquipmentList()) {

      if (Random.Range(0, 100) <= chanceToLooseItems) {
        DropItem(item.data);
        itemsToUnequips.Add(item);
      }
    }

    foreach (var item in itemsToUnequips) {

      inventory.UnequipItem(item.data as ItemData_Equipment);
    }

    foreach (InventoryItem item in inventory.GetStashList()) {

      if (Random.Range(0, 100) <= chanceToLooseMaterials) {
        DropItem(item.data);
        materialsToLoose.Add(item);
      }
    }

    foreach (var item in materialsToLoose) {

      inventory.RemoveItem(item.data);
    }
  }
}
