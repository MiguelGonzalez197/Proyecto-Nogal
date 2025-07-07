# 📁 Estructura del Proyecto

Este documento describe la estructura y organización del proyecto para facilitar su comprensión, mantenimiento y escalabilidad.

---

## 📑 Índice

1. [Arquitectura](#arquitectura)
2. [Tecnologías utilizadas](#tecnologías-utilizadas)
   - [Motor Grafico](#motor-grafico)
   - [Lenguajes/](#lenguajes)
3. [Carpetas](#carpetas)
   - [Docs/](#docs)
   - [Proyecto-Unity/](#proyecto-unity)

---

## Arquitectura

(Consulta el archivo [`Arquitectura.md`](./Arquitectura.md) para una descripción detallada del diseño del proyecto.)

---

## Tecnologías utilizadas

### Motor Grafico/

### Lenguajes/

Usamos exclusivamente **C#** para toda la lógica del juego en Unity.

| Extensión | Lenguaje | Uso                          |
|-----------|----------|-------------------------------|
| `.cs`     | C#       | Toda la lógica del juego      |

---

## Carpetas

### Docs/

Contiene documentación adicional como la arquitectura del sistema, estructura del proyecto, guías internas y notas de desarrollo.

### Proyecto-Unity/

Contiene todas las carpetas necesarias para abrir y ejecutar el proyecto correctamente desde Unity Hub.

```text
Assets/                  # Código fuente, escenas, prefabs, materiales, modelos, sonidos, animaciones, etc.
├── Editor/              # Scripts personalizados para herramientas del editor
├── Settings/            # Configuraciones personalizadas (puede incluir sistemas de entrada o settings de paquetes)
├── Audio/               # Efectos de sonido y música
├── Materials/           # Materiales organizados
├── Prefabs/             # Objetos preconfigurados del juego
├── Scenes/              # Escenas del juego
├── Scripts/             # Código fuente organizado por funcionalidad
│   ├── Player/          # Lógica del jugador
│   ├── Enums/           # Enumeraciones globales del juego
│   ├── UI/              # Scripts para la interfaz de usuario
│   ├── Systems/         # Controladores generales (GameManager, InputManager, etc.)
│   └── Interfaces/      # Interfaces para desacoplar sistemas (IPuzzle, IInteractable, etc.)
├── Animations/          # Controladores Animator y animaciones
├── UI/                  # Canvases, botones, íconos, fuentes
├── FX/                  # Partículas, efectos visuales y shaders
└── Resources/           # Assets cargados dinámicamente por scripts (usa con moderación)

Packages/                # Registra los paque
ProjectSettings/         # Configuraciones del proyecto: Input, Tags, Layers, Build Settings, Quality, etc.
UserSettings/            # Configuración específica del usuario 
