using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour {
  [SerializeField] private string sceneName = "MainScene";
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
    StartCoroutine(LoadSceneWithFadeEffect(1.5f));
  }

  public void NewGame() {
    AudioManager.instance.PlaySFX(7);
    SaveManager.instance.DeleteSavedData();
    StartCoroutine(LoadSceneWithFadeEffect(1.5f));
  }

  public void ExitGame() {
    AudioManager.instance.PlaySFX(7);
    Application.Quit();
  }

  IEnumerator LoadSceneWithFadeEffect(float _delay) {
    fadeScreen.FadeOut();

    yield return new WaitForSeconds(_delay);

    SceneManager.LoadScene(sceneName);
  }
}