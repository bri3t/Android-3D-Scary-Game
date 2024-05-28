# Android-3D-Scary-Game


El juego consiste en escapar de la mazmorra. Una vez entras, no sales. Tienes que encontrar el cofre que contiene la llave para abrir la puerta (cubo blanco) y así terminar el juego. También puedes ganar matando al zombie. Hay varios cofres repartidos por el mapa donde podrás encontrar diferentes utilidades. ¡Pero ten cuidado, que no te pille el zombie!

## Funcionamiento General del Juego y Cómo Jugar

Inicio del juego: Ejecuta el juego y selecciona 'Start' en el menú principal.

  - **Movimiento**: Te mueves con el joystick y controlas la dirección de la cámara con el panel táctil de la parte derecha.
  - **Disparo**: Al recolectar la pistola se habilita el botón para disparar.
  - **Vidas**: Si el zombie te toca, mueres. Puedes matarlo con 5 disparos de bala.

## Puzzle
Para poder pasarte el juego, deberás encontrar el cofre con la llave y abrir la puerta con el botón de acción.


<br>
<div>
  <img src="https://github.com/bri3t/Android-3D-Scary-Game/assets/120582826/e39e1cca-4258-4839-8c6a-1952051a4522" alt="puzzle" width="400" height="350">
  <img src="https://github.com/bri3t/Android-3D-Scary-Game/assets/120582826/fe285a06-65af-428b-940f-dfe4fcadc7b4" alt="puzzle" width="600" height="300">
</div>

## Animaciones
-&gt; HorrorGameAssets/Animations
- **Zombie**: Idle, caminar, correr, morir. 
- **Cofre**: al abrirse.
- **Lampara**: idle (balanceo)
- **Pistola**: Al disparar (tiene retroceso)

## Físicas:
- En el mapa, la parte del barro ubicada en el suelo, hace resbalar y rebotar ligeramente al jugador.
- La lampara y la pistola, tienen un script "Sway", que tiene un script que crea un efecto de balanceo en un objeto (en la pistola) basado en el movimiento de la camara, dando la apariencia de que el objeto se balancea ligeramente mientras el jugador mueve la cámara. El swayAmount controla cuánto se balancea el objeto y el smoothFactor controla la suavidad del movimiento. El balanceo se calcula en el método Update y se aplica interpolando entre la rotación actual y la rotación objetivo con Quaternion.Slerp.

## Uso del rayCast
El script del enemigo (EnemyController) utiliza un Raycast para detectar al jugador dentro del rango de visión del enemigo. En el método CheckForPlayerDetection, se calcula un vector desde la posición del enemigo hacia la posición del jugador. Luego, se realiza un Raycast desde la posición del enemigo en esa dirección hasta una distancia definida por sightDistance. Si el Raycast impacta con un objeto que tiene la etiqueta "Player", se cambia el estado del enemigo a Chase, indicando que ha detectado al jugador y debe comenzar a perseguirlo.

## Comportamiento enemigo (IA)
La IA del enemigo está gestionada por un NavMeshAgent y se organiza en tres estados: Idle, Walk, y Chase. En el estado Idle, el enemigo espera en su posición actual y verifica si el jugador está en su rango de visión. Si el temporizador de inactividad expira, cambia al estado Walk y se mueve al siguiente punto de ruta. Si detecta al jugador en cualquier momento mediante el Raycast, cambia al estado Chase, aumentando su velocidad y moviéndose hacia la posición del jugador. Si pierde de vista al jugador, regresa al estado Walk y sigue patrullando entre los puntos de ruta. Durante estos estados, el enemigo reproduce diferentes sonidos y anima sus movimientos apropiadamente.


## Cómo Instalar y Ejecutar

### Descargar el Código Fuente
Para obtener el código fuente del juego, primero necesitarás clonar el repositorio de GitHub. Puedes hacer esto usando Git. Si no tienes Git instalado en tu ordenador, puedes descargarlo desde [git-scm.com](https://git-scm.com/downloads).

#### (Windows)
Abre una terminal y ejecuta el siguiente comando para clonar el repositorio:
1. Clona el repositorio a tu máquina local usando:
```bash
   git clone https://github.com/bri3t/Android-3D-Scary-Game.git
   cd Android-3D-Scary-Game
```
## Abre el Proyecto en Unity

Una vez que tengas el código fuente en tu máquina local, abre Unity Hub. Si no lo tienes, puedes descargarlo desde [Unity Download Page](https://unity.com/download).

En Unity Hub, ve a la pestaña 'Projects', luego haz clic en 'ADD' y selecciona la carpeta donde clonaste el proyecto. Esto añadirá el proyecto a tu lista de proyectos de Unity. Haz clic en el nombre del proyecto para abrirlo en el editor de Unity.
Construir y Ejecutar el Juego

Para desplegar y ejecutar el juego en tu dispositivo móvil, sigue estos pasos en el editor de Unity:
1. Conecta tu dispositivo móvil al ordenador a través de USB. Asegúrate de que esté configurado para permitir la depuración por USB.
2. En Unity, ve a File &gt; Build Settings.
3. Selecciona la plataforma de destino (Android) en la lista y haz clic en Switch Platform.
4. Para Android, asegúrate de que Google Android Project esté marcado si quieres editar el proyecto con Android Studio.
5. Haz clic en Build. Esto compilará el juego y generará un fichero apk que podrás instalar en un dispositivo android.
6. ¡Jugar!

Para ejecutar el juego desde el ordenador, simplemente ejecutalo desde unity. Podrás jugarlo desde la pestaña "Game" usando los controles del ordenador, o desde el simulador, para poder probar la pantalla tactil y jugabilidad en móvil.

## Compatibilidad
Para hacer este proyecto he usado:
- Unity: 2022.3.21f1
- Microsoft Visual Studio: 2022 17.9.6
- Java: "17.0.9" 2023-10-17 LTS
- Gradle 8.2.0
- SDK 33

## Lenguaje de programación usado
- C#


