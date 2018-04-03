using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MouseHit : MonoBehaviour
{
    public event Action<GameObject> MoveToObj;
    public event Action<GameObject> ExplodeDlgV;
    public event Action<GameObject> ExplodeDlgU;
    public event Action<GameObject> CameraDlgOrbit;
    public event Action<GameObject> CameraDlgLinear;
    public static MouseHit mouseHit;

    private void Awake()
    {
        mouseHit = this;
    }


    private void OnMouseDown()
    {
        if(gameObject.tag == "DirPoint")
        {
            MoveToObj(gameObject);
        }
        if (gameObject.tag == "ExplodeDlg_V")
        {
            ExplodeDlgV(gameObject);
        }
        if (gameObject.tag == "ExplodeDlg_U")
        {
            ExplodeDlgU(gameObject);
        }
        if (gameObject.tag == "CameraDlg_Orbit")
        {
            CameraDlgOrbit(gameObject);
        }
        if (gameObject.tag == "CameraDlg_Linear")
        {
            CameraDlgLinear(gameObject);
        }
    }
}
