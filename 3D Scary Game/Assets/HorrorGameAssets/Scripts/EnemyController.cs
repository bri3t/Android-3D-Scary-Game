using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] waypoints; // Array de puntos de ruta que el enemigo seguirá.
    public float idleTime = 2f; // Tiempo de espera en cada punto de ruta.
    public float walkSpeed = 2f; // Velocidad de caminata.
    public float chaseSpeed = 4f; // Velocidad de persecución.
    public float sightDistance = 10f; // Distancia de visión para detectar al jugador.
    public AudioClip idleSound; // Sonido cuando el enemigo está en estado idle.
    public AudioClip walkingSound; // Sonido cuando el enemigo está caminando.
    public AudioClip chasingSound; // Sonido cuando el enemigo está persiguiendo.

    private int currentWaypointIndex = 0; // Índice del punto de ruta actual.
    private NavMeshAgent agent; // Referencia al componente NavMeshAgent.
    private Animator animator; // Referencia al componente Animator.
    private float idleTimer = 0f; // Temporizador para el estado idle.
    private Transform player; // Referencia al jugador.
    private AudioSource audioSource; // Referencia al componente AudioSource.

    // Estados del enemigo.
    private enum EnemyState { Idle, Walk, Chase }
    private EnemyState currentState = EnemyState.Idle; // Estado actual del enemigo.

    private bool isChasingAnimation = false; // Indicador para la animación de persecución.

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // Obtiene el componente NavMeshAgent.
        animator = GetComponent<Animator>(); // Obtiene el componente Animator.
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador por la etiqueta "Player".
        audioSource = GetComponent<AudioSource>(); // Obtiene el componente AudioSource.

        // Configura el AudioSource para 3D sound
        audioSource.spatialBlend = 1.0f; // 3D sound
        audioSource.minDistance = 4.0f; // Distancia mínima para escuchar al volumen máximo
        audioSource.maxDistance = 20.0f; // Distancia máxima para escuchar el sonido
        audioSource.rolloffMode = AudioRolloffMode.Logarithmic; // Modo de disminución logarítmica

        SetDestinationToWaypoint(); // Establece el destino al primer punto de ruta.
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                idleTimer += Time.deltaTime; // Incrementa el temporizador.
                animator.SetBool("IsWalking", false); // Desactiva la animación de caminar.
                animator.SetBool("IsChasing", false); // Asegura que la animación de persecución esté desactivada.
                PlaySound(idleSound); // Reproduce el sonido de idle.

                if (idleTimer >= idleTime)
                {
                    NextWaypoint(); // Cambia al siguiente punto de ruta si se supera el tiempo de espera.
                }

                CheckForPlayerDetection(); // Verifica la detección del jugador.
                break;

            case EnemyState.Walk:
                idleTimer = 0f; // Resetea el temporizador.
                animator.SetBool("IsWalking", true); // Activa la animación de caminar.
                animator.SetBool("IsChasing", false); // Asegura que la animación de persecución esté desactivada.
                PlaySound(walkingSound); // Reproduce el sonido de caminar.

                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    currentState = EnemyState.Idle; // Cambia al estado idle si llega al destino.
                }

                CheckForPlayerDetection(); // Verifica la detección del jugador.
                break;

            case EnemyState.Chase:
                idleTimer = 0f; // Resetea el temporizador.
                agent.speed = chaseSpeed; // Establece la velocidad de persecución.
                agent.SetDestination(player.position); // Establece el destino al jugador.
                isChasingAnimation = true; // Activa la animación de persecución.
                animator.SetBool("IsChasing", true); // Activa la animación de persecución.

                PlaySound(chasingSound); // Reproduce el sonido de persecución.

                if (Vector3.Distance(transform.position, player.position) > sightDistance)
                {
                    currentState = EnemyState.Walk; // Cambia al estado walk si el jugador está fuera de la distancia de visión.
                    agent.speed = walkSpeed; // Restaura la velocidad de caminata.
                }
                break;
        }
    }

    private void CheckForPlayerDetection()
    {
        RaycastHit hit;
        Vector3 playerDirection = player.position - transform.position; // Calcula la dirección hacia el jugador.

        if (Physics.Raycast(transform.position, playerDirection.normalized, out hit, sightDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                currentState = EnemyState.Chase; // Cambia al estado chase si detecta al jugador.
            }
        }
    }

    private void PlaySound(AudioClip soundClip)
    {
        if (!audioSource.isPlaying || audioSource.clip != soundClip)
        {
            audioSource.clip = soundClip; // Cambia el clip de audio.
            audioSource.Play(); // Reproduce el sonido.
        }
    }

    private void NextWaypoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Cambia al siguiente punto de ruta.
        SetDestinationToWaypoint(); // Establece el destino al nuevo punto de ruta.
    }

    private void SetDestinationToWaypoint()
    {
        agent.SetDestination(waypoints[currentWaypointIndex].position); // Establece el destino al punto de ruta actual.
        currentState = EnemyState.Walk; // Cambia al estado walk.
        agent.speed = walkSpeed; // Establece la velocidad de caminata.
        animator.enabled = true; // Activa el componente Animator.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = currentState == EnemyState.Chase ? Color.red : Color.green; // Cambia el color del rayo según el estado.
        Gizmos.DrawLine(transform.position, player.position); // Dibuja una línea hacia el jugador.
    }
}
