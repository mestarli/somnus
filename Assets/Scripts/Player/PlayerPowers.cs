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
    
    //Para los iconos
    [SerializeField] private GameObject espadaUI;
    [SerializeField] private GameObject EscudoUI;
    [SerializeField] private GameObject LunaUI;
    [SerializeField] private GameObject PuenteUI;
    [SerializeField] private GameObject RocasUI;

    private PlayerController _playerController;

    [SerializeField] private Material SphereMaterial_Stone;
    [SerializeField] private Material SphereMaterial_Stone_02;
    public bool isMakingActions;
    //variables to melee attack
    
    
    //Para los materiales de disolver
    private bool isDissolving;

    private float _dissolveValue;
    private float _valueStart = 1;
    private float _valueEnd = 0;
    [SerializeField] private float _dissolveSpeed;

    [SerializeField] private GameObject baston;
    [SerializeField] private GameObject espada;
    
    
    //Para los ataques mágicos de la luna
    [SerializeField] private float bulletForce;
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject spawnProjectile;
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
            moveRocks(stones);
        }
        //Apretar R para hacer aparecer puentes
        if (Input.GetKeyDown(KeyCode.R) && _playerController.isGrounded)
        {
            constructBridge(bridge);
        }
        // Click derecho para el ataque basico
        if (Input.GetMouseButtonDown(1) && _playerController.isGrounded)
        {
            isMakingActions = true;
            triggerAttack.active = true;
            basicAttack();
        }
        
        if (Input.GetMouseButtonDown(0) && _playerController.isGrounded)
        {
            magicSwordAttack();
        }


        if (Input.GetKeyDown(KeyCode.T) && _playerController.isGrounded)
        {
            Shell();
        }
        if (Input.GetKeyDown(KeyCode.F) && _playerController.isGrounded)
        {
            magicMoonAttack();
        }
    }
   /// <summary>
    /// Sin necesidad de orbes
    /// </summary>
    public void basicAttack()
   {
       _animator.SetBool("IsAttacking", true);
   }

    /// <summary>
    /// - 2 orbes se desactiva pasado 
    /// </summary>
    public void Shell()
    {
        if (!isActiveShell && _playerController.counterOrbs >=2)
        {
            isMakingActions = true;
            shell.SetActive(true);
            callCoroutineUI(EscudoUI);
            _playerController.DiscountOrbs(2);
            StartCoroutine(countDownShell());
        }   
       
    }

    /// <summary>
    /// - 4 orbes
    /// </summary>
    public void magicSwordAttack()
    {
        if(_playerController.counterOrbs >=4)
        {
            baston.active = false;
            espada.active = true;
            isMakingActions = true;
            callCoroutineUI(espadaUI);
            _animator.SetBool("IsMagicAttack", true);
            _playerController.DiscountOrbs(4);
        }
    }
    
    public void resetMagicSword()
    {
        baston.active = true;
        espada.active = false;
        isMakingActions = false;
        _animator.SetBool("IsMagicAttack", false);
    }

    /// <summary>
    /// - 4 orbes
    /// </summary>
    public void magicMoonAttack()
    {
        if(_playerController.counterOrbs >=4)
        {
            isMakingActions = true;
            callCoroutineUI(LunaUI);
            _animator.SetTrigger("IsSpelling");
            StartCoroutine(spellMoon());
        }
    }

    IEnumerator spellMoon()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject newBullet = Instantiate(projectile, spawnProjectile.transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(spawnProjectile.transform.forward * 600);
        Destroy(newBullet, 2.0f);
        _playerController.DiscountOrbs(4);
    }
    /// <summary>
    /// construccion -10 orbes
    /// </summary>
    public void constructBridge(GameObject bridges)
    {
        if (_playerController.counterOrbs >= 2 && isActiveBridge && !bridges.active)
        {
            //Faltará añadir la animación
            isMakingActions = true;
            callCoroutineUI(PuenteUI);
            _animator.SetTrigger("IsSpelling");
            bridges.SetActive(true);
            _playerController.DiscountOrbs(2);
        }
    }
    /// <summary>
    /// mover -10 orbes
    /// </summary>
    public void moveRocks(GameObject stones)
    {
        if (_playerController.counterOrbs >= 2 && isActiveStone && stones)
        {
            isMakingActions = true;
            //Faltará añadir la animación
            _animator.SetTrigger("IsSpelling");
            callCoroutineUI(RocasUI);
            
            MeshRenderer meshRenderer = stones.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            // Set the new material on the GameObject
            meshRenderer.material = SphereMaterial_Stone;
            MeshRenderer meshRenderer2 = stones.gameObject.transform.GetChild(1).GetComponent<MeshRenderer>();
            // Set the new material on the GameObject
            meshRenderer2.material = SphereMaterial_Stone;
            MeshRenderer meshRenderer3 = stones.gameObject.transform.GetChild(2).GetComponent<MeshRenderer>();
            // Set the new material on the GameObject
            meshRenderer3.material = SphereMaterial_Stone_02;
            MeshRenderer meshRenderer4 = stones.gameObject.transform.GetChild(3).GetComponent<MeshRenderer>();
            // Set the new material on the GameObject
            meshRenderer4.material = SphereMaterial_Stone_02;
            
            MeshRenderer meshRenderer5 = stones.gameObject.transform.GetChild(4).GetComponent<MeshRenderer>();
            // Set the new material on the GameObject
            meshRenderer5.material = SphereMaterial_Stone;
            
            StartCoroutine(DissolveRocks());
            StartCoroutine(DissolveRocks02());
            _playerController.DiscountOrbs(2);
        }
    }

    private IEnumerator DissolveRocks()
    {
            isDissolving = true;

            _dissolveValue = _valueStart;

            while(_dissolveValue > _valueEnd)
            {
                _dissolveValue -= Time.deltaTime * _dissolveSpeed;
                SphereMaterial_Stone.SetFloat("_Dissolve", _dissolveValue);

                yield return null;
            }

            yield return new WaitForSeconds(1f);
            

            isDissolving = false;
    }
    private IEnumerator DissolveRocks02()
    {
        isDissolving = true;

        _dissolveValue = _valueStart;

        while(_dissolveValue > _valueEnd)
        {
            _dissolveValue -= Time.deltaTime * _dissolveSpeed;
            SphereMaterial_Stone_02.SetFloat("_Dissolve", _dissolveValue);

            yield return null;
        }

        yield return new WaitForSeconds(1f);
            

        isDissolving = false;
        Destroy(stones);
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
        yield return new WaitForSeconds(3f);
        shell.SetActive(false);
        isMakingActions = false;
    }

    public void callCoroutineUI(GameObject UIButton)
    {
        StartCoroutine(countActiveHabilidadUI(UIButton));
    }
    IEnumerator countActiveHabilidadUI(GameObject UIButton)
    {
        UIButton.gameObject.transform.GetChild(1).gameObject.active = true;
        yield return new WaitForSeconds(1.5f);
        UIButton.gameObject.transform.GetChild(1).gameObject.active = false;
    }
}
