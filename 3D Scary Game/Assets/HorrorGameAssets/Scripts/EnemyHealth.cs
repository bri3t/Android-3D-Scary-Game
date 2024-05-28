using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Salud máxima del enemigo.
    public int maxHealth = 100;

    // Salud actual del enemigo.
    private int currentHealth;

    // Se llama cuando se inicializa el enemigo.
    private void Start()
    {
        // Establece la salud inicial al valor máximo de salud.
        currentHealth = maxHealth;
    }

    // Función para aplicar daño al enemigo.
    public void TakeDamage(int damageAmount)
    {
        // Reduce la salud actual en la cantidad de daño recibido.
        currentHealth -= damageAmount;

        // Verifica si la salud del enemigo ha llegado a cero o menos.
        if (currentHealth <= 0)
        {
            // Llama a la función para manejar la muerte del enemigo.
            Die();
        }
    }

    // Función para manejar la muerte del enemigo.
    private void Die()
    {
        // Realiza cualquier acción relacionada con la muerte aquí, como reproducir animaciones de muerte, generar efectos o eliminar al enemigo de la escena.
        // Puedes personalizar este método según los requisitos de tu juego.

        // Por ejemplo, podrías destruir el GameObject del enemigo:
        gameObject.GetComponent<Animator>().SetBool("Death", true);
    }
}
