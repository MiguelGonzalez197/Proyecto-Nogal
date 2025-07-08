# EcoMisión: Simulador de Recolección de Basura

## 1. Justificación de la necesidad
Este videojuego tiene como objetivo promover la educación ambiental, específicamente la correcta separación de residuos, mediante una experiencia interactiva y lúdica. El proyecto ha sido desarrollado con el fin de participar en la convocatoria “Premios Fundación El Nogal 2025”.

## 2. Equipo del Proyecto
- Juan David Urrego Fonseca
- Miguel Ángel González Contreras
- Helen Viviana Gamboa Fonseca
- Juan Pablo Rodríguez Rodríguez

## 3. Requerimientos Funcionales

| ID | Nombre | Actor | Relacionados | Casos de Uso |
|----|--------|-------|--------------|--------------|
| RF-001 | Generación aleatoria de bolsas | Sistema | RF-002, RF-003, RF-014 | CUS-001, CUS-004 |
| RF-002 | Apertura de bolsas | Jugador | RF-001, RF-003 | CUS-001 |
| RF-003 | Visualización de ítems | Jugador | RF-001, RF-002, RF-005 | CUS-001 |
| RF-004 | Movimiento entre zonas | Jugador | RF-005 | CUS-003 |
| RF-005 | Clasificación de ítems | Jugador | RF-003, RF-006, RF-011 | CUS-001 |
| RF-006 | Evaluación y puntaje | Sistema | RF-005, RF-013, RF-014 | CUS-005, CUS-006 |
| RF-007 | Textos educativos | Sistema | RF-014 | CUS-002, CUS-007 |
| RF-008 | Temporizador | Sistema | RF-005, RF-006 | CUS-001 |
| RF-009 | Rareza de ítems | Sistema | RF-001, RF-006 | CUS-004 |
| RF-010 | Feedback visual/sonoro | Sistema | RF-006 | CUS-005, CUS-008 |
| RF-011 | Canecas con detección | Sistema | RF-005, RF-006 | CUS-001 |
| RF-012 | Indicador de progreso | Sistema | RF-005, RF-013 | CUS-006 |
| RF-013 | Resumen por bolsa | Sistema | RF-006, RF-012 | CUS-006 |
| RF-014 | Clasificación por color | Jugador | RF-001, RF-006, RF-007 | CUS-001, CUS-006 |

## 4. Requerimientos No Funcionales

| ID | Nombre | Actor | Casos de Uso |
|----|--------|-------|--------------|
| RNF-001 | Escalabilidad modular | Programador | CUS-004, CUS-010 |
| RNF-002 | Rendimiento | Sistema | CUS-005, CUS-006 |
| RNF-003 | Compatibilidad | Sistema | CUS-010 |
| RNF-004 | Accesibilidad visual | UX/UI | CUS-002, CUS-007 |
| RNF-005 | Guardado automático | Sistema | CUS-010 |
| RNF-006 | Tiempos de carga | Sistema | CUS-003 |
| RNF-007 | Seguridad del progreso | Sistema | CUS-010 |
| RNF-008 | Tolerancia al error | Sistema | CUS-007 |
| RNF-009 | Control de sonido | Jugador | CUS-008 |

## 5. Casos de Uso

### CUS-001: Clasificar ítems reciclables
El jugador abre una bolsa, ve los ítems y los clasifica en las canecas. Si la bolsa es del color de la caneca, puede verterla completa.

### CUS-002: Ver textos educativos
El sistema muestra textos para ayudar al jugador a clasificar correctamente.

### CUS-003: Cambiar entre zonas
El jugador cambia entre la zona de apertura y de clasificación.

### CUS-004: Generar bolsas aleatorias
El sistema genera bolsas con ítems de diferentes rarezas.

### CUS-005: Evaluar clasificación
Se suma o resta puntaje según los aciertos o errores.

### CUS-006: Ver resumen final
Al terminar una bolsa, se muestra un resumen con desempeño.

### CUS-007: Usar ayuda educativa
El jugador accede a una guía explicativa en cualquier momento.

### CUS-008: Controlar sonido
Permite activar o desactivar música y efectos.

### CUS-010: Guardar y continuar
El sistema guarda el progreso para continuar después.

## 6. Observaciones Finales
Este proyecto fue desarrollado con fines de impacto social, y está inscrito en los Premios Fundación El Nogal 2025.

---
Autor: Equipo EcoMisión
