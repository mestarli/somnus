using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float life = 100f;

    [SerializeField] private float counterOrbs = 0;
    
    [SerializeField] private float speed = 4.5f;

    [SerializeField] private float jumHeight = 1.0f;

    [SerializeField] private bool isGrounded;
    
    private Rigidbody _rigidbody; 

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        float xMove = Input.GetAxisRaw("Horizontal"); 
        float zMove = Input.GetAxisRaw("Vertical");
        _rigidbody.velocity = new Vector3(xMove, _rigidbody.velocity.y, zMove) * speed; 
    }
    private void Jump()
    {
        
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
