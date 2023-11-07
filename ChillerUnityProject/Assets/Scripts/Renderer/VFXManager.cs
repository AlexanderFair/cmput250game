using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public GameObject clickObject;

    // Update is called once per frame
    void Update()
    {
        if(Settings.Controls.Click.GetKeyDown(true, false))
        {
            Instantiate(clickObject).GetComponent<Clicker>().Setup(Util.GetMouseWorldPoint());
        }
    }
}
