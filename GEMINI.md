# GEMINI.md: Project Cambrian Magic

## Project Overview

This is a Unity game project for a top-down, physics-based action roguelite tentatively titled "Deep Evolution". The core concept revolves around the modular evolution of a sea creature controlled by the player. The gameplay is inspired by a combination of Spore's creature editor and Hades' fast-paced combat.

The project is built using **Unity 6000.2.13f1** and the **Universal Render Pipeline (URP)**. It utilizes the new **Input System** for player controls.

The core gameplay loop involves:
1.  Controlling a primordial core creature in a top-down environment.
2.  Collecting "biomass" from the environment and defeated enemies.
3.  Using the collected resources to evolve the creature in real-time by attaching modular parts (e.g., claws, fins, shells) to a socket-based body.
4.  Adapting the creature's form to switch between different combat builds, such as a fast "Hunter" or a stationary "Bunker".

The game design emphasizes a strong sense of physical feedback, with features like hit-stop, screen shake, and physics-based recoil.

## Building and Running

**TODO:** This section needs to be updated with build and run instructions. As a standard Unity project, the following workflows apply:

*   **Running in the Editor:**
    1.  Open the project in the Unity Editor (version 6000.2.13f1 or compatible).
    2.  Open the main scene file (likely located at `Assets/Scenes/SampleScene.unity`).
    3.  Press the "Play" button in the editor toolbar.

*   **Building for PC:**
    1.  Go to `File > Build Settings`.
    2.  Select "PC, Mac & Linux Standalone" as the target platform.
    3.  Click "Build" and choose a destination for the executable.

## Development Conventions

**TODO:** This section should be expanded as the project develops.

*   **Scripting:** All scripts are to be written in C#. Based on the Game Design Document, the following scripts are planned:
    *   `PlayerController.cs`: Manages player movement and input.
    *   `SocketManager.cs`: Handles the attachment and detachment of modular parts.
    *   `EnemyAI.cs`: Basic AI for enemy behavior.
    *   `GameFeelManager.cs`: A singleton to manage game feel effects like hit-stop.
    *   `Projectile/Hitbox.cs`: For collision and damage detection.
*   **Version Control:** The project is under Git version control.
*   **Testing:** The `com.unity.test-framework` is included, so unit and integration tests should be created.
*   **Asset Naming:** A clear and consistent naming convention should be established for assets.
*   **Scene Structure:** Game objects in scenes should be organized logically, possibly using empty GameObjects as folders.
