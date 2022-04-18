using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    
    private Animator _animator;
    private bool isActiveStone;
    private bool isActiveBridge;
    private bool isActiveShell;
    
    [SerializeField] private GameObject stones;
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject shell;
    [SerializeField] private GameObject triggerAttack;

    private PlayerController _playerController;

    public bool isMakingActions;
    //variables to melee attack
    
    public static PlayerPowers Instance { get; set; }
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _animator = GetComponent<Animator>();
        Instance = this;
    }

    void Update()
    {
        //Apretar E para mover rocas
        if (Input.GetKeyDown(KeyCode.E) && _playerController.isGrounded)
        {
            isMakingActions = true;
            moveRocks(stones);
        }
        //Apretar R para hacer aparecer puentes
        if (Input.GetKeyDown(KeyCode.R) && _playerController.isGrounded)
        {
            isMakingActions = true;
            constructBridge(bridge);
        }
        // Click derecho para el ataque basico
        if (Input.GetMouseButtonDown(1) && _playerController.isGrounded)
        {
            isMakingActions = true;
            triggerAttack.active = true;
            basicAttack();
        }

        if (Input.GetKeyDown(KeyCode.T) && _playerController.isGrounded)
        {
            Shell();
        }
    }
   /// <summary>
    /// Sin necesidad de orbes
    /// </summary>
    public void basicAttack()
   {
       Debug.Log("Ataque básico");
       _animator.SetBool("IsAttacking", true);
   }

    /// <summary>
    /// - 2 orbes se desactiva pasado 
    /// </summary>
    public void Shell()
    {
        if (!isActiveShell)
        {
            shell.SetActive(true);
            _playerController.DiscountOrbs(2);
            StartCoroutine(countDownShell());
        }   
       
    }

    /// <summary>
    /// - 4 orbes
    /// </summary>
    public void magicSwordAttack()
    {
        
    }

    /// <summary>
    ///  - 3 orbes
    /// </summary>
    public void magicArrowAttack()
    {
        
    }
    
    /// <summary>
    /// - 5 orbes
    /// </summary>
    public void magicMoonAttack()
    {
        
    }

    /// <summary>
    /// construccion -10 orbes
    /// </summary>
    public void constructBridge(GameObject bridges)
    {
        if (_playerController.counterOrbs >= 10 && isActiveBridge && !bridges.active)
        {
            //Faltará añadir la animación
            bridges.SetActive(true);
            _playerController.DiscountOrbs(10);
            isMakingActions = false;
        }
    }
    /// <summary>
    /// mover -10 orbes
    /// </summary>
    public void moveRocks(GameObject stones)
    {
        if (_playerController.counterOrbs >= 10 && isActiveStone && stones)
        {
            //Faltará añadir la animación
            Destroy(stones);
            _playerController.DiscountOrbs(10);
            isMakingActions = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "stones")
        {
            isActiveStone = true;
        }
        if (other.gameObject.tag == "bridge")
        {
            isActiveBridge = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isActiveStone = false;
        isActiveBridge = false;
    }

    private void resetAnimationAttack()
    {
        _animator.SetBool("IsAttacking", false);
        triggerAttack.active = false;
        isMakingActions = false;
    }


    IEnumerator countDownShell()
    {
        yield return new WaitForSeconds(2f);
        shell.SetActive(false);
        isMakingActions = false;
    }
}
