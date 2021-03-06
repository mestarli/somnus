using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text countOrbs;
    [SerializeField] private Image countLife;
    [SerializeField] private GameObject _camera;
    [SerializeField] private GameObject UIPanelStart;
    public static UIManager Instance { get; set; }
    
    void Awake()
    {
        Instance = this;
        Screen.SetResolution(1920, 1080, true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartNewGame()
    {
        Animation anim = _camera.GetComponent<Animation>();
        anim.Play("StartGame");
        UIPanelStart.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void UpdateOrbs( string orbs)
    {
        countOrbs.text = orbs;
    }

    public void UpdateLife(float life,float maxlife)
    {
        countLife.fillAmount = life / maxlife;
    }

    public void OnClickConstructBridge(GameObject bridges)
    {
        PlayerPowers.Instance.constructBridge(bridges);
    }
    
    public void  OnClickMoveRocks(GameObject stones)
    {
        PlayerPowers.Instance.moveRocks(stones);
    }

    public void ShowInstructions(GameObject panel)
    {
        panel.active = !panel.active;
    }
}
