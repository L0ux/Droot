using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderScene : MonoBehaviour
{
    public void Start()
    {

    }
    public void loadNextLevel()
    {
        Debug.Log("Miamoumiam");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }

}
