using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGLDraw : MonoBehaviour {
    public Material mat;
    public List<Vector3> spts = new List<Vector3>();
    // Use this for initialization
    public GameObject obj;

    private void OnRenderObject()
    {
        spts.Clear();
        spts.Add(screenToWorld(new Vector2(150, 400)));
        spts.Add(screenToWorld(new Vector2(300, 400)));

        //spts.Add(new Vector3(0, 0, 0));
        spts.Add(obj.transform.position);

        GLDraw.Polyline(spts,Color.white);
    }

    public Vector3 screenToWorld(Vector3 v)
    {
        Ray r = Camera.main.ScreenPointToRay(v);
        return r.GetPoint(1);
    }
   
}
