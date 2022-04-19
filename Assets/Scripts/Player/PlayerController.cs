using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float life = 100f;
    [SerializeField] private float maxLife = 100f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float speed = 8.5f;
    [SerializeField] private float speedRun = 15.5f;
    private float initialSpeed;
    private Rigidbody _rigidbody;
    
    [SerializeField] private float jumHeight = 3.5f;

    public bool isGrounded;
    [SerializeField] private bool isJumping;
    
    private Animator _animator;
    
    //For check if its toching ground
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    
    
    public float counterOrbs = 100f;

    // Para la camara
    private float rotXCamera;
    [SerializeField] private Transform playerCamera;
    private Vector2 inputRot;
    [SerializeField] private float sensibilityMouse = 2f;
    

    void Awake()
    {
        _animator = GetComponent<Animator>();
        initialSpeed = speed;
        _rigidbody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
     
        // Get vertical camera rotation
        rotXCamera = playerCamera.eulerAngles.x;
        
        // Inicializamos UI
        UIManager.Instance.UpdateOrbs(counterOrbs.ToString());
        UIManager.Instance.UpdateLife(life, maxLife);

    }

    // Update is called once per frame
    void Update()
    {
        // comprobamos que esta tocando suelo, si no es así, es que está saltando
        isGrounded  = Physics.CheckSphere(groundCheck.position,0.15f,groundLayer);
        if (!PlayerPowers.Instance.isMakingActions)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Llamamos funcionalidades para moverse, correr, saltar...
        
        _animator.SetBool("IsRunning", false);
        if (!PlayerPowers.Instance.isMakingActions)
        {
            Movement();
        }
       
        
        //Get Player rotation from Mouse
        inputRot.x = Input.GetAxis("Mouse X") * sensibilityMouse;
        //inputRot.y = Input.GetAxis("Mouse Y") * sensibilityMouse;


        //Method to update camera rotation
        MovePlayerCamera();
        
        
    }
    private void Movement()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); 
        float zMove = Input.GetAxisRaw("Vertical");
        _animator.SetFloat("Walking", Mathf.Abs(xMove));
        _animator.SetFloat("Walking", Mathf.Abs(zMove));
        
        //Movement of player
        _rigidbody.velocity = transform.forward * speed *  zMove // Forward, Backward movement of player
                              + transform.right * speed * xMove   // Left, Right Movement of player
                              + new Vector3(0, _rigidbody.velocity.y, 0);
        Run();
    }
    private void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            
            _rigidbody.AddForce(jumHeight * Vector3.up, ForceMode.VelocityChange);
            _animator.SetTrigger("IsJumping");
        }
        //_animator.SetBool("IsJumping", isGrounded);
        
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
        UIManager.Instance.UpdateOrbs(counterOrbs.ToString());
        Destroy(orb);
    }
    
    /// <summary>
    /// Detectamos colisiones para llamar a una función determinada
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag.Contains("orb"))
        {
            recollectOrb(other.gameObject);
        }

        if (other.gameObject.tag == "infinito")
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "infinito")
        {
            SceneManager.LoadScene("GameOver");
        } 
        if (other.gameObject.tag == "exit")
        {
            SceneManager.LoadScene("ToBeContinued");
        }

        //El ataque melee quita 5
        if (other.gameObject.tag == "enemy_melee_attack")
        {
            restarVida(5f);
        }
        
        //El ataque magico quita 10
        if (other.gameObject.tag == "enemy_magic_attack")
        {
            restarVida(10f);
        }
        
        //El chocarse con enemigos quita 1
        if (other.gameObject.tag == "enemy")
        {
            restarVida(1f);
        }
        
    }

    public void restarVida(float restar_vida)
    {
        life -= restar_vida;
        if (life <= 0) ;
        {
            //SceneManager.LoadScene("GameOver");
        }
    }
    
    /// <summary>
    /// Metodo para la rotacion de la camara
    /// </summary>
    public void MovePlayerCamera()
    {
        rotXCamera -= inputRot.y;

        // Set limits to vertical rotation
        rotXCamera = Mathf.Clamp(rotXCamera,10,15);
        // Horizontal rotation camera
        transform.Rotate(0, inputRot.x * sensibilityMouse,0f);
        // Vertical rotation camera
        //playerCamera.transform.localRotation = Quaternion.Euler(rotXCamera, 0f, 0f);
    }

    public void DiscountOrbs(float orbs)
    {
        counterOrbs -= orbs;
        UIManager.Instance.UpdateOrbs(counterOrbs.ToString());
    }
    
}
