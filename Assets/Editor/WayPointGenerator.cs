using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WayPointGenerator : EditorWindow
{
    
    
   [MenuItem("Window/WayPointGenerator")]
   public static void ShowWindow()
    {
        GetWindow<WayPointGenerator>("WayPointGenerator");
    }
    private void OnGUI()
    {
        
        GUILayout.Label("Generate way points for selected road Object");
        
        if (GUILayout.Button("Generate Way Points")) {
            if (Selection.gameObjects.Length == 1) {

                GameObject dummy = new GameObject();              
                GameObject obj = Selection.gameObjects[0];
                Mesh mesh = obj.GetComponent<MeshFilter>().sharedMesh;
                Transform objTransform = obj.GetComponent<Transform>();
                int numberOfWayPoints = 0;
                for (int i = 0; i < mesh.vertices.Length-1; i+=2)
                {
                    Vector3 middlePosition = objTransform.localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[i]) + objTransform.localToWorldMatrix.MultiplyPoint3x4(mesh.vertices[i + 1]);
                    middlePosition /= 2f;
                    Instantiate(dummy,middlePosition , Quaternion.identity,objTransform).name="WayPoint"+numberOfWayPoints;
                    numberOfWayPoints++;
                }
                DestroyImmediate(dummy.gameObject);
            }
            {
                Debug.Log("Please Choose Only One GameObject");
            }
               
            }
        
    }
}
