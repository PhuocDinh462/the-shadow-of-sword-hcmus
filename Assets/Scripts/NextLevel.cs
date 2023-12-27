using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevel : MonoBehaviour {
  [SerializeField] private string nextSceneName;
  [SerializeField] UI_FadeScreen fadeScreen;

  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.GetComponent<Player>() != null) {
      GameObject.Find("SaveManager").GetComponent<SaveManager>().SaveGame();
      StartCoroutine(LoadSceneWithFadeEffect(5f));
      GameObject.Find("Canvas").GetComponent<UI>().SwitchOnNextScreen();
    }
  }

  IEnumerator LoadSceneWithFadeEffect(float _delay) {
    fadeScreen.FadeOut();

    yield return new WaitForSeconds(_delay);

    SceneManager.LoadScene(nextSceneName);
  }
}
