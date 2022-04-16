using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }
}
