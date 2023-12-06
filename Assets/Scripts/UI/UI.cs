using UnityEngine;

public class UI : MonoBehaviour {
  [SerializeField] private GameObject characterUI;

  public UI_ItemToolTip itemToolTip;
  public UI_StatToolTip statToolTip;

  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public void SwitchTo(GameObject _menu) {
    for (int i = 0; i < transform.childCount; i++)
      transform.GetChild(i).gameObject.SetActive(false);

    if (_menu)
      _menu.SetActive(true);
  }
}