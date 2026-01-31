# Blast Game

A high-performance, grid-based puzzle game prototype developed in Unity. This project demonstrates a robust "Blast" (Match-2+) mechanic with dynamic visual feedback, advanced algorithms for board management, and mobile-responsive UI.

## Gameplay & Mechanics

The game features a dynamic grid where players tap to blast matching color groups. The system provides immediate visual feedback based on the strategic value of the move:

* **Standard Blocks:** Default square shape.
* **Tier 1 Combo (5-7 Blocks):** Blocks transform into **Triangles**.
* **Tier 2 Combo (8-9 Blocks):** Blocks transform into **Octagons**.
* **Tier 3 Combo (10+ Blocks):** Blocks transform into **Stars**.

## ⚙️ Technical Highlights

This project was built to showcase clean coding practices and algorithm implementation in Unity:

* **Flood Fill Algorithm:** Used a recursive search algorithm to efficiently detect connected blocks of the same color.
* **Deadlock Detection & Auto-Shuffle:** The board automatically detects when no valid moves are remaining and shuffles the grid to ensure continuous gameplay.
* **Mobile Optimization:**
    * Implemented a custom `CameraScalar` script to adapt the view for various mobile aspect ratios (responsive design).
    * Optimized `Update` loops and used component caching to minimize CPU usage.
* **Input System:** Raycast-based touch/click detection for precise interaction.

## Installation

1.  Clone this repository or download the ZIP file.
2.  Open **Unity Hub** and add the project folder.
3.  Open the project using a compatible Unity version (Recommended: 2021.3 LTS or later).
4.  Navigate to `Assets/Scenes` and open the **SampleScene**.
5.  Press **Play** to start the simulation.

---
**Developer:** Ahmet Furkan Çallı
**Engine:** Unity
**Language:** C#
