using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Ui_InGame : MonoBehaviour
{
  [SerializeField] private PlayerStats playerStats;
  [SerializeField] private Slider slider;

  [SerializeField] private Image dashImage;
  [SerializeField] private Image parryImage;
  [SerializeField] private Image crystalImage;
  [SerializeField] private Image swordImage;
  [SerializeField] private Image blackHoleImage;
  [SerializeField] private Image flaskImage;


  private SkillManager skills;

  void Start() {
      if(playerStats != null) {
        playerStats.onHealthChanged += UpdateHealthUI;
      }
    skills = SkillManager.instance;
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.dashUnlocked) {
      SetCoolDownOf(dashImage);
    }
    if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked) {
      SetCoolDownOf(parryImage);
    }
    if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked) {
      SetCoolDownOf(crystalImage);
    }
    if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked) {
      SetCoolDownOf(swordImage);
    }
    if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked) {
      SetCoolDownOf(blackHoleImage);
    }
    if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null) {
      SetCoolDownOf(flaskImage);
    }

    CheckCoolDownOf(dashImage, skills.dash.cooldown);
    CheckCoolDownOf(parryImage, skills.parry.cooldown);
    CheckCoolDownOf(crystalImage, skills.crystal.cooldown);
    CheckCoolDownOf(swordImage, skills.sword.cooldown);
    CheckCoolDownOf(blackHoleImage, skills.blackhole.cooldown);

    CheckCoolDownOf(flaskImage, Inventory.instance.flaskCooldown);
  }

  private void UpdateHealthUI() {
    slider.maxValue = playerStats.GetMaxHealthValue();
    slider.value = playerStats.currentHealth;
  }


  private void SetCoolDownOf(Image _image) {
    if (_image.fillAmount <= 0)
      _image.fillAmount = 1;
  }

  private void CheckCoolDownOf(Image _image, float _cooldown) {
    if (_image.fillAmount > 0)
      _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
  }
}
