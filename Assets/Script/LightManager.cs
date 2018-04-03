using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightManager : MonoBehaviour {

    public GameObject DirectLight;
    public GameObject InsideLight;

    public Button toNight;

    private void Start()
    {
      
        toNight.onClick.AddListener(ToNight);
    }

    void ToNight()
    {
        if (DirectLight.activeSelf)
        {
            DirectLight.SetActive(false);
            InsideLight.SetActive(true);
        }
        else
        {
            DirectLight.SetActive(true);
            InsideLight.SetActive(false);
        }
       
    }

}
