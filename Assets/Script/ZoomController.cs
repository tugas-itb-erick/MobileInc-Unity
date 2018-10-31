using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour {

    public UIManager UM;
    private static int counter = 0;

    public void OnZoomInClick()
    {
        if (counter < 3)
        {
            counter++;
            UM.camera.transform.position = new Vector3(UM.camera.transform.position.x,
                UM.camera.transform.position.y - 2, UM.camera.transform.position.z + 2);
        }
    }

    public void OnZoomOutClick()
    {
        if (counter > -3)
        {
            counter--;
            UM.camera.transform.position = new Vector3(UM.camera.transform.position.x,
                UM.camera.transform.position.y + 2, UM.camera.transform.position.z - 2);
        }
    }
}
