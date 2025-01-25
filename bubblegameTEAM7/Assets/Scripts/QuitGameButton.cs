using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script does not do anything by itself. Instead, a UI Button should be used to call its QuitApplication functions.
public class QuitGameButton : MonoBehaviour
{
    //This function will not work inside of Editor - Application refers to the a build of the game when its launched from .exe or the like.
    public void QuitApplication()
    {
        Application.Quit();
        //Since it does not work in the Editor, a message is logged instead to show that function was called successfully.
        Debug.Log("Exited the application");
    }
}
