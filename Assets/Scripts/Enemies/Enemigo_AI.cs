using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo_AI : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float life = 20f;
    [SerializeField] private float runSpeed;
    [SerializeField] private LayerMask player_mask;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private Transform player;
    
    // Patrulla
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex;
    
    //Ataque
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttaked;
    private bool isTakingDamage;
    [SerializeField] private GameObject colliderAttack;
    
    //Estados
    [SerializeField] public float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    
    //Drop de bola
    [SerializeField] private GameObject orbeSpawn;


    private void Awake()
    {
        //Recuperamos los componentes NavMeshAgent y Animator del Enemy
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        // buscamos al player dentro de la escena a prtir de su tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Start()
    {
        
        //Asignamos un punto de ruta al enemy par que se mueva hacia el
        _navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        
        //Revisamos si muenstro enemigo esta chocando con la layer que contiene el player, mirando su posición actual, el rango de visión o de ataque

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, player_mask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, player_mask);
        
        
        //Dependiendo si el player esta en rango de visión o ataque realizaremos patrulla, perseguir o atacar
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrullar();
        }
        
        if (playerInSightRange && !playerInAttackRange)
        {
            Perseguir();
        }
        
        if (playerInSightRange && playerInAttackRange)
        {
           Atacar();
        }
        
    }
    
    // Método que asigna un punto de ruta al enemigo y le dice donde ir, en el momento que llega o cierta distancia
    // pasa al siguiente punto de la lista

    private void Patrullar()
    {
        // Decimos donde tiene que ir el enemigo
        _navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        
        // Asignamos de nuestro animator que animación tiene que reproducir el enemigo
        _animator.SetBool("isPatrolling", true);
        _animator.SetBool("isChassing", false);
        
        // Asignamos la velocidad correspondiente al estado de patrulla
        _navMeshAgent.speed = this.speed;
        
        // Revisamos si estamosa menos de X distancia del punto de ruta al que nos dirigimos
        if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
        {
            //Indicamos el siguiente punto de ruta al que dirigirnos
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            _navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
    
    
    // Método que indica al enemigo que tiene que setguir al jugador, ya que esta dentro de su zona de visión

    private void Perseguir()
    {
        //Asignamos la animación del Animator que tiene que reproducir
        _animator.SetBool("isPatrolling", false);
        _animator.SetBool("isChassing", true);
        
        //Indicamos donde está el player para que le persiga
        _navMeshAgent.SetDestination(player.position);
        
        //Asignamos la velocidad correspondiente al estado de perseguir
        _navMeshAgent.speed = this.runSpeed;
    }

    
    
    // Método que ejecuta una animación de ataque del enemigo si se encuentra dentro del rango de ataque asignado
    
    private void Atacar()
    {
        _navMeshAgent.SetDestination(player.position);
        
        //Miramos haciea el player mientras atacamos
        transform.LookAt(player);

        if (!alreadyAttaked && !isTakingDamage)
        {
            alreadyAttaked = true;
            colliderAttack.active = true;
            //Invocamos el trigger para reproducir la animación de ataque
            _animator.SetTrigger("isAttacking");
            _animator.SetBool("isChassing", false);
            // Volvemos a dejar atacar al enemigo despues del tiempo definido
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }
    
    //Método que resetea la variable que permite al enemigo atacar

    private void ResetAttack()
    {
        alreadyAttaked = false;
        colliderAttack.active = false;
    }

    public void ResetChassing()
    {
        _animator.SetBool("isChassing", true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // el ataque básico quita 2 de vida
        if (other.gameObject.tag == "weapon")
        {
            restarVida(2f);
        }
        // los ataques mágicos 5
        if (other.gameObject.tag == "magic_attack")
        {
            restarVida(5f);
        }
    }
    public void restarVida(float restar_vida)
    {
        life -= restar_vida;
        if (life <= 0) 
        {
            //Animacion de morir
            isTakingDamage = true;
            _animator.SetTrigger("Die");
            //Destroy(gameObject);
        }else
        {
            isTakingDamage = true;
            _animator.SetBool("isChassing", false);
            _animator.SetTrigger("Damage");
        }
    }

    /// <summary>
    /// Método para el trigger de cuando acaba la animación de morrir
    /// </summary>
    public void destroyEnemy()
    {
        Instantiate(orbeSpawn, transform.position, orbeSpawn.transform.rotation);
        
        //Si toca un número random entre 0 y 50, creamos otro orbe
        int rand = Random.Range(0, 100);
        if (rand <= 50)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += 1.5f;
            Instantiate(orbeSpawn, newPosition , orbeSpawn.transform.rotation);

        }
        Destroy(gameObject);
        
    }

    public void resetDamage()
    {
        isTakingDamage = false;
    }
}
