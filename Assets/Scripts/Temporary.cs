using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temporary : MonoBehaviour
{
    public void OnButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "QuitButton":
                SceneManager.LoadScene("GameTitle");
                break;
            case "AgainButton":
                SceneManager.LoadScene("Stage1");
                break;
        }
    }
}
