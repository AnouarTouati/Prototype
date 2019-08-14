using UnityEngine;

public class CameraMainMenu : MonoBehaviour {

 
    float OldPosition;
    float InitialPositionX;
    float InitialPositionY;
    public float Speed = 0.1f;
    Vector2 OldMousePosition;
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            InitialPositionX = Input.mousePosition.x;
            InitialPositionY = Input.mousePosition.y;
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        if (Input.GetKey(KeyCode.Mouse0) && OldMousePosition.x>Input.mousePosition.x)
        {
           transform.localEulerAngles += new Vector3(0, -Mathf.Abs((InitialPositionX - Input.mousePosition.x)) * Speed, 0);
        }
        if (Input.GetKey(KeyCode.Mouse0) && OldMousePosition.x < Input.mousePosition.x)
        {
              transform.localEulerAngles += new Vector3(0, Mathf.Abs((InitialPositionX - Input.mousePosition.x)) * Speed, 0);
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
     /*   if (Input.GetKey(KeyCode.Mouse0) && OldMousePosition.y > Input.mousePosition.y && transform.localEulerAngles.x<70 && transform.localEulerAngles.x >0)
        {
            transform.localEulerAngles += new Vector3(Mathf.Abs((InitialPositionY - Input.mousePosition.y)) * Speed, 0, 0);
        }
        if (Input.GetKey(KeyCode.Mouse0) && OldMousePosition.y < Input.mousePosition.y && transform.localEulerAngles.x < 70 && transform.localEulerAngles.x > 0)
        {
            transform.localEulerAngles += new Vector3(-Mathf.Abs((InitialPositionY - Input.mousePosition.y)) * Speed, 0, 0);
        }*/
     
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        OldMousePosition = Input.mousePosition;

            }
}
