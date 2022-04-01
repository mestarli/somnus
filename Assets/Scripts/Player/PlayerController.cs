using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float life = 100f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float speed = 8.5f;
    [SerializeField] private float speedRun = 15.5f;
    private float initialSpeed;
    
    [SerializeField] private float jumHeight = 1.0f;

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping;
    
    private CharacterController _characterController;
    private Animator _animator;
    
    //For check if its toching ground
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    
    [SerializeField] private float counterOrbs = 0;


    void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        initialSpeed = speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // comprobamos que esta tocando suelo, si no es así, es que está saltando
        isGrounded  = Physics.CheckSphere(groundCheck.position,0.15f,groundLayer);
        _animator.SetBool("IsGrounded", isGrounded);

    }

    void FixedUpdate()
    {
        // Llamamos funcionalidades para moverse, correr, saltar...
        
        _animator.SetBool("IsRunning", false);
        Movement();
        
    }
    private void Movement()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); 
        float zMove = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("Walking", Mathf.Abs(xMove));
        _animator.SetFloat("Walking", Mathf.Abs(zMove));
        
        Vector3 move = new Vector3(xMove, 0, zMove);
        
        _characterController.Move( move * Time.deltaTime * speed);
        Run();
    }
    private void Jump()
    {
        Vector3 direction = new Vector3();
        if (isGrounded && gameObject.transform.position.y < 0)
        {
            direction.y = 0f;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            direction.y += Mathf.Sqrt(jumHeight * -3.0f *  gravity);

            direction.y +=  gravity * Time.deltaTime;
            _characterController.Move(direction * Time.deltaTime);
        }
    }

    /// <summary>
    /// Método para correr
    /// </summary>
    private void Run()
    {
        bool isShiftKeyDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        if(isShiftKeyDown)
        {
            speed = speedRun;
            _animator.SetBool("IsRunning", isShiftKeyDown);
           
        }
        else
        {
            speed = initialSpeed;
        }
    }

    /// <summary>
    /// Método que descuenta el daño, según el enemigo
    /// </summary>
    ///  /// <param name="enemy"></param>
    private void Damage(GameObject enemy)
    {
        if(enemy.tag == "simple_orb")
        {
            counterOrbs += 1;
        }
        if (enemy.tag == "extra_orb")
        {
            counterOrbs += 2;
        }
    }

    /// <summary>
    /// Método para recolectar orbes que subirán nuestra energia
    /// </summary>
    /// <param name="orb"></param>
    private void recollectOrb(GameObject orb)
    {
        if(orb.tag == "simple_orb")
        {
            counterOrbs += 1;
        }
        if (orb.tag == "extra_orb")
        {
            counterOrbs += 3;
        }
        Destroy(orb);
    }
    
    /// <summary>
    /// Detectamos colisiones para llamar a una función determinada
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Contains("orb") )
        {
            recollectOrb(other.gameObject);
        }
    }
}
