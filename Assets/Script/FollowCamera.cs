using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject maincamera;

    private void Start()
    {
        maincamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        //transform.rotation = maincamera.transform.rotation;

        transform.eulerAngles = new Vector3(0, maincamera.transform.eulerAngles.y, 0);
    }

}
