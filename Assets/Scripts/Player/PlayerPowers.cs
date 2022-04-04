using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    
    private Animator _animator;
    private bool isActiveStone;
    private bool isActiveBridge;
    private PlayerController _playerController;
    public static PlayerPowers Instance { get; set; }
    void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        Instance = this;
    }
   /// <summary>
    /// Sin necesidad de orbes
    /// </summary>
    public void basicAttack()
    {
        
    }

    /// <summary>
    /// - 2 orbes
    /// </summary>
    public void Shell()
    {
        
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
        if (_playerController.counterOrbs >= 10 && isActiveStone)
        {
            //Faltará añadir la animación
            bridges.SetActive(true);
            _playerController.DiscountOrbs(10);
        }
    }
    /// <summary>
    /// mover -10 orbes
    /// </summary>
    public void moveRocks(GameObject stones)
    {
        if (_playerController.counterOrbs >= 10 && isActiveStone)
        {
            //Faltará añadir la animación
            Destroy(stones);
            _playerController.DiscountOrbs(10);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "stones")
        {
            isActiveStone = true;
        }
        if (other.gameObject.tag == "bridges")
        {
            isActiveBridge = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isActiveStone = false;
        isActiveBridge = false;
    }

    
}
