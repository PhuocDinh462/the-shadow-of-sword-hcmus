using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public static GameManager instance;

  private void Awake() {
    if (instance)
      Destroy(instance.gameObject);
    else
      instance = this;
  }

  public void RestartScene() {
    Scene scene = SceneManager.GetActiveScene();

    SceneManager.LoadScene(scene.name);
  }
}