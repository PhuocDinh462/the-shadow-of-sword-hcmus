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
  [SerializeField] private float dashCoolDown;

  void Start() {
      if(playerStats != null) {
        playerStats.onHealthChanged += UpdateHealthUI;
      }
    dashCoolDown = SkillManager.instance.dash.cooldown;
  }

  private void Update() {
    if (Input.GetKeyDown(KeyCode.LeftShift)) {
      SetCoolDownOf(dashImage);
    }

    CheckCoolDownOf(dashImage, dashCoolDown);
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
    if (_image.fillAmount <= 0)
      _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
  }
}
