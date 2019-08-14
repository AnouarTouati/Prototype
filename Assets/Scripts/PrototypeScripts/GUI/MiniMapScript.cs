using UnityEngine;

public class MiniMapScript : MonoBehaviour {

    Transform Car;

    public float Offset;

    void Start () {
        if (GameObject.FindGameObjectWithTag("Player"))
        Car = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Car == null)
        {
            if (GameObject.FindGameObjectWithTag("Player"))
            Car = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        else
        {
            GetComponent<Transform>().position = new Vector3(Car.position.x, Offset, Car.position.z);
            GetComponent<Transform>().rotation = Quaternion.Euler(new Vector3(90, Car.transform.rotation.eulerAngles.y, 0));
        }
	}
}
