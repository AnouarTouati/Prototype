using UnityEngine;
using UnityEngine.UI;
public class FrameCounter : MonoBehaviour {

	void Start()
    {
        DontDestroyOnLoad(this);
    }
	void Update () {
        GetComponentInChildren<Text>().text = "" + Mathf.FloorToInt(1 / Time.deltaTime);
	}
}
