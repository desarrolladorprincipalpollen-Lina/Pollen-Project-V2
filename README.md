# Pollen_V2 - Unity Modular Games

Este repositorio contiene una batería de juegos modulares desarrollados en **Unity 6**. El proyecto está estructurado para permitir la expansión de múltiples juegos (módulos) compartiendo una base lógica escalable.

## 🏗️ Estructura del Proyecto

El proyecto se organiza bajo una arquitectura de módulos:
* **Assets/Modules/Shared**: Recursos, scripts y prefabs comunes.
* **Assets/Modules/Game_1_Pollen**: Juego de clasificación y lógica de instrucciones.
    * `/Scripts`: Lógica central (LevelCreator, InstructionBuilder, etc.).
    * `/Prefabs`: Objetos interactuables y contenedores.

## 🎮 Game 1: Pollen - Lógica de Juego

Pollen es un juego de destreza y seguimiento de instrucciones donde el jugador debe clasificar objetos en contenedores específicos bajo presión de tiempo.

### Flujo de Trabajo (Game Loop)
El juego utiliza un sistema de **Cambio Lógico** en lugar de recarga de escenas. Todo ocurre en una única escena donde el `LevelCreator` gestiona la dificultad.

1.  **Tutorial**: `TutorialSequencer` gestiona el flujo narrativo inicial y carga la escena principal.
2.  **Generación**: `InstructionBuilder` crea un objetivo aleatorio (Objeto + Contenedor).
3.  **Interacción**: El jugador usa un sistema de Raycast (`PCControllers1`) para recoger y soltar objetos (`PickableHandler`).
4.  **Validación**: `ContainerHandler` verifica si el objeto entregado coincide con la instrucción activa.
5.  **Progresión**: Al alcanzar el número de objetivos definidos en `LevelInfo`, se dispara una transición visual (`LevelTransition`) y se incrementa el nivel.

### Diagrama de Arquitectura (UML)

https://miro.com/welcomeonboard/WjFINDNZdklEREN3eHZDWEt3OU1qclVPS0paMUY0c2tmbjlqZzhSb0Qrek9VczdJR21kYzBZL3dxZkp3MmFNQzZvU200amVxMFdBQXVnQmJqc1crNHJaSjZXWnl1NS9WTU93L2hJajAyeklKWkpudm1kaHJ0cXExQnFPODgrQ3VhWWluRVAxeXRuUUgwWDl3Mk1qRGVRPT0hdjE=?share_link_id=363341240544
![Pollen Diagramas](https://github.com/user-attachments/assets/7647a6d7-4d48-465c-a7a3-e60e9c000ea7)

