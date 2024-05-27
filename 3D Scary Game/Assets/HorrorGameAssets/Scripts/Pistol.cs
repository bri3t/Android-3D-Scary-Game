using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pistol : MonoBehaviour
{
    public int maxAmmoInMag = 10;       // Capacidad máxima de munición en el cargador
    public int maxAmmoInStorage = 30;   // Capacidad máxima de munición en el almacenamiento
    public float shootCooldown = 0.5f;  // Tiempo de enfriamiento entre disparos
    public float reloadCooldown = 0.5f;  // Tiempo de enfriamiento para recargar
    private float switchCooldown = 0.5f;  // Cooldown entre disparos
    public float shootRange = 100f;     // Alcance del raycast (rayo)

    public ParticleSystem impactEffect; // Efecto de partículas para el impacto

    public int currentAmmoInMag;       // Munición actual en el cargador
    public int currentAmmoInStorage;   // Munición actual en el almacenamiento
    public int damager;                // Daño infligido por el arma
    public bool canShoot = true;       // Bandera para comprobar si se permite disparar
    public bool canSwitch = true;      // Bandera para comprobar si se permite cambiar
    private bool isReloading = false;   // Bandera para comprobar si se está recargando
    private float shootTimer;           // Temporizador para el enfriamiento del disparo

    public Transform cartridgeEjectionPoint; // Punto de expulsión del cartucho
    public GameObject cartridgePrefab;       // Prefab del cartucho
    public float cartridgeEjectionForce = 5f; // Fuerza aplicada al cartucho al expulsarlo

    public Animator gun;                   // Animador del arma
    public ParticleSystem muzzleFlash;     // Efecto de destello en la boca del cañón
    public GameObject muzzleFlashLight;    // Luz del destello en la boca del cañón
    public AudioSource shoot;              // Sonido del disparo

    private PlayerInputActions playerInputActions;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Jugador.Shoot.performed += OnShoot; // Vincula la acción de disparar al método OnShoot
    }

    void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Jugador.Shoot.performed -= OnShoot; // Desvincula la acción de disparar del método OnShoot
    }

    void Start()
    {
        currentAmmoInMag = maxAmmoInMag;
        currentAmmoInStorage = maxAmmoInStorage;
        canSwitch = true;
        muzzleFlashLight.SetActive(false);
    }

    void Update()
    {
        // Actualiza la cantidad de munición actual
        currentAmmoInMag = Mathf.Clamp(currentAmmoInMag, 0, maxAmmoInMag);
        currentAmmoInStorage = Mathf.Clamp(currentAmmoInStorage, 0, maxAmmoInStorage);

        // Actualiza el temporizador de disparo
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        if (canShoot && !isReloading)
        {
            switchCooldown = shootCooldown;
            Shoot();
        }
    }

    void Shoot()
    {
        // Verifica si hay munición en el cargador
        if (currentAmmoInMag > 0 && shootTimer <= 0f)
        {
            canSwitch = false;
            shoot.Play();                 // Reproduce el sonido de disparo
            muzzleFlash.Play();           // Activa el efecto de destello
            muzzleFlashLight.SetActive(true); // Activa la luz del destello
            gun.SetBool("shoot", true);   // Activa la animación de disparo

            // Realiza la acción de disparo
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
            {
                // Verifica si el objeto impactado tiene la etiqueta "Enemy"
                if (hit.collider.CompareTag("Enemy"))
                {
                    // Obtiene el componente EnemyHealth del objeto impactado
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

                    // Verifica si el enemigo tiene el componente EnemyHealth
                    if (enemyHealth != null)
                    {
                        // Aplica daño al enemigo
                        enemyHealth.TakeDamage(damager); // Reemplaza 'damager' con el valor de daño real
                    }
                }

                // Instancia el efecto de impacto en el punto de impacto
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // Instancia el cartucho vacío
            GameObject cartridge = Instantiate(cartridgePrefab, cartridgeEjectionPoint.position, cartridgeEjectionPoint.rotation);
            Rigidbody cartridgeRigidbody = cartridge.GetComponent<Rigidbody>();

            // Aplica fuerza para expulsar el cartucho
            cartridgeRigidbody.AddForce(cartridgeEjectionPoint.right * cartridgeEjectionForce, ForceMode.Impulse);

            StartCoroutine(endAnimations()); // Inicia la corrutina para finalizar la animación de disparo
            StartCoroutine(endLight());      // Inicia la corrutina para apagar la luz del destello
            StartCoroutine(canswitchshoot()); // Inicia la corrutina para permitir cambiar y disparar

            switchCooldown -= Time.deltaTime;

            // Reduce la cantidad de munición
            currentAmmoInMag--;

            // Inicia el enfriamiento del disparo
            shootTimer = shootCooldown;
        }
        else
        {
            // Sin munición en el cargador o disparo en enfriamiento
            Debug.Log("No se puede disparar");
        }
    }

    IEnumerator ReloadCooldown()
    {
        isReloading = true;
        canShoot = false;
        canSwitch = false;

        yield return new WaitForSeconds(reloadCooldown);

        isReloading = false;
        canShoot = true;
        canSwitch = true;
    }

    IEnumerator endAnimations()
    {
        yield return new WaitForSeconds(.1f);
        gun.SetBool("shoot", false);
    }

    IEnumerator endLight()
    {
        yield return new WaitForSeconds(.1f);
        muzzleFlashLight.SetActive(false);
    }

    IEnumerator canswitchshoot()
    {
        yield return new WaitForSeconds(shootCooldown);
        canSwitch = true;
    }
}
