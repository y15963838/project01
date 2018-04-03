using System.Collections.Generic;
using UnityEngine;

public class GLDraw
{
    private static Material _lineMaterial;
    public static Material lineMaterial
    {
        get
        {
            if (!_lineMaterial)
            {
                Material nlineMaterial;
                // Unity has a built-in shader that is useful for drawing
                // simple colored things.
                Shader shader = Shader.Find("Hidden/Internal-Colored");
                nlineMaterial = new Material(shader);
                nlineMaterial.hideFlags = HideFlags.HideAndDontSave;
                // Turn on alpha blending
                nlineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                nlineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                // Turn backface culling off
                nlineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
                // Turn off depth writes
                nlineMaterial.SetInt("_ZWrite", 0);
                _lineMaterial = nlineMaterial;
            }

            

            return _lineMaterial;
        }
    }

    public static void PolylineOnScreen(IEnumerable<Vector3> pts, Color? color = null)
    {
        if (!color.HasValue) color = Color.black;
        Material mat = lineMaterial;
        mat.SetPass(0);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(color.Value);
        foreach (Vector2 v in pts)
        {
            //Vector3 wv = Camera.main.ScreenToWorldPoint(v);
            Ray r=Camera.main.ScreenPointToRay(v);
            Vector3 wv = r.GetPoint(1);
            Debug.Log(wv);
            GL.Vertex(wv);
        }
        GL.End();
    }

    public static void Polyline(IEnumerable<Vector3> pts, Color? color=null)
    {
        if (!color.HasValue) color = Color.black;
        Material mat = lineMaterial;
        mat.SetPass(0);
        GL.Begin(GL.LINE_STRIP);
        GL.Color(color.Value);
        foreach (Vector3 v in pts) GL.Vertex(v);
        GL.End();
    }
    
}
