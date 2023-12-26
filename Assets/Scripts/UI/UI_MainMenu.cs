using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour, ISaveManager {
  [SerializeField] private string sceneName = "Level 1";
  [SerializeField] private GameObject nextLevelText;
  [SerializeField] private GameObject continueButton;
  [SerializeField] UI_FadeScreen fadeScreen;
  [SerializeField] int bgmIndex = 5;

  private void Start() {
    if (!SaveManager.instance.HasSavedData())
      continueButton.SetActive(false);
    AudioManager.instance.PlayBGM(bgmIndex);
  }

  public void ContinueGame() {
    AudioManager.instance.PlaySFX(7);
    StartCoroutine(LoadSceneWithFadeEffect(5f));
  }

  public void NewGame() {
    AudioManager.instance.PlaySFX(7);
    SaveManager.instance.DeleteSavedData();
    sceneName = "Level 1";
    StartCoroutine(LoadSceneWithFadeEffect(5f));
  }

  public void ExitGame() {
    AudioManager.instance.PlaySFX(7);
    Application.Quit();
  }

  IEnumerator LoadSceneWithFadeEffect(float _delay) {
    fadeScreen.FadeOut();
    StartCoroutine(NextScreenCoroutine());
    yield return new WaitForSeconds(_delay);

    SceneManager.LoadScene(sceneName);
  }

  IEnumerator NextScreenCoroutine() {
    yield return new WaitForSeconds(1);
    nextLevelText.GetComponent<TextMeshProUGUI>().text = sceneName;
    nextLevelText.SetActive(true);
  }

  public void LoadData(GameData _data) {
    if (_data.level != null)
      sceneName = _data.level;
  }

  public void SaveData(ref GameData _data) { }
}