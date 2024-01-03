using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Options : MonoBehaviour {
  [SerializeField] private string mainMenuSceneName = "MainMenu";
  [SerializeField] UI_FadeScreen fadeScreen;

  public void GoToMainMenu() {
    AudioManager.instance.PlaySFX(7);
    Time.timeScale = 1;
    GameObject.Find("SaveManager").GetComponent<SaveManager>().SaveGame();
    StartCoroutine(LoadSceneWithFadeEffect(5f));
  }

  IEnumerator LoadSceneWithFadeEffect(float _delay) {
    fadeScreen.FadeOut();
    yield return new WaitForSeconds(_delay);

    SceneManager.LoadScene(mainMenuSceneName);
  }
}
