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

    [SerializeField] private bool isGrounded;
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
    
    
    //for steps
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 2f;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        initialSpeed = speed;
        _rigidbody = GetComponent<Rigidbody>();
        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
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
        Jump();
    }

    void FixedUpdate()
    {
        // Llamamos funcionalidades para moverse, correr, saltar...
        
        _animator.SetBool("IsRunning", false);
        Movement();
       
        
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
    
    void stepClimb()
    {
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                _rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLower45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f,0,1), out hitLower45, 0.1f))
        {

            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f,0,1), out hitUpper45, 0.2f))
            {
                _rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }

        RaycastHit hitLowerMinus45;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f,0,1), out hitLowerMinus45, 0.1f))
        {

            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f,0,1), out hitUpperMinus45, 0.2f))
            {
                _rigidbody.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
            }
        }
    }
}
