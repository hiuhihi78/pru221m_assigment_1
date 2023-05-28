using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public new Camera camera;
    public void OpenSelectMap()
    {
        Vector3 selectMapCameraPostion = new Vector3(75, 0, -10);
        camera.transform.Translate(selectMapCameraPostion);
        LoadMap();
    }

    

    public void QuitGame()
    {

    }

    public void BackMenu()
    {
        Vector3 menuCameraPosition = new Vector3(0, 0, -10);
        camera.transform.position = menuCameraPosition;
    }

    public void StartGameDefaultMap1()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void LoadMap()
    {

    }

}
