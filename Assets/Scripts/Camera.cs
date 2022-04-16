using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject FadeOut;
    public void StartGame()
    {
        FadeOut.SetActive(true);
        StartCoroutine(startNewGame());
    }

    IEnumerator startNewGame()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("MainLevel");
    }
}
