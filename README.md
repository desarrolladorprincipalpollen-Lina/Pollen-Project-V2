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

```mermaid
graph TD
    A[TutorialSequencer] -->|Carga Escena| B[LevelCreator]
    B --> C[InstructionBuilder]
    C --> D[Player Interaction]
    D --> E[ContainerHandler]
    E -->|Validación| F[Scoreboard]
    F -->|Progreso| B
    B -->|Meta Alcanzada| G[LevelTransition]
    G -->|Reset Lógico| B
