using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour {
  [SerializeField] private string statName;

  [SerializeField] private StatType statType;
  [SerializeField] private TextMeshProUGUI statValueText;
  [SerializeField] private TextMeshProUGUI statNameText;

  private void OnValidate() {
    gameObject.name = "Stat - " + statName;

    if (statNameText)
      statNameText.text = statName;
  }

  void Start() {
    UpdateStatValueUI();
  }

  public void UpdateStatValueUI() {
    PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

    if (playerStats) {
      statValueText.text = playerStats.GetStat(statType).GetValue().ToString();
    }
  }
}