using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class QuitOnCancel : MonoBehaviour
{
    void Update ()
    {
        if(CrossPlatformInputManager.GetButtonDown("Cancel"))
        {
            Application.Quit ();
        }
    }
}

