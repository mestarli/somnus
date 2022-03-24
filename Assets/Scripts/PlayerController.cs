using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float life;

    [SerializeField] private float counterOrbs = 0;
    
    [SerializeField] private float speed = 4.5f;

    [SerializeField] private float jumHeight = 1.0f;

    [SerializeField] private bool isJumping;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            counterOrbs += 2;
        }
    }

    private void OnColliderEnter(Collision other)
    {
        
    }
}
