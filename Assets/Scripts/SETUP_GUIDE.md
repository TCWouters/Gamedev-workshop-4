# Enemy AI State Machine - Setup Guide

## Overzicht
Dit is een uitgebreide state machine voor enemy AI met Idle → Alerted → Attack states.

## Setup Stappen

### 1. **Enemy GameObject Setup**
- Maak een nieuwe 3D GameObject voor je enemy
- Voeg volgende componenten toe:
  - **NavMeshAgent** (voor navigatie)
  - **Collider** (Box of Capsule)
  - **EnemyAI script** (de controller)

### 2. **EnemyAI Component Configureren**
Stel in de Inspector volgende parameters in:

- **Waypoints**: Array van Transform points waar enemy tussen patrouilleert
  - Sleep Empty GameObjects in de scene
  - Sleep ze in de Waypoints array
  
- **Sight Range**: 15 (hoe ver kan enemy zien)
- **Sight Angle**: 90 (FOV in graden - 90 = 90 graden totaal)
- **Hearing Range**: 10 (hoe ver kan enemy horen)
- **Capture Range**: 1 (hoe dicht moet speler zijn om gepakt te worden)

- **Patrol Speed**: 2 (snelheid in patrol/idle)
- **Attack Speed**: 5 (snelheid bij aanval)
- **Stopping Distance**: 0.5 (hoe dicht bij waypoint stoppen)

### 3. **Player GameObject Setup**
- Voeg **Player script** toe aan je Player GameObject
- Zorg dat Player GameObject tag "Player" heeft:
  - Select Player in Hierarchy
  - In Inspector: Tag dropdown → "Player"
  - Of maak een nieuwe Player tag aan

### 4. **GameManager Setup**
- Maak een lege GameObject in de scene
- Noem het "GameManager"
- Voeg **GameManager script** toe

### 5. **NavMesh Setup**
- **BELANGRIJK**: Bake een NavMesh in je scene!
  - Ga naar: Window → AI → Navigation
  - Select je level geometry
  - Mark als "Walkable"
  - Click "Bake"

### 6. **Tags Controleren**
- Player moet tag "Player" hebben
- Enemy kan elke tag hebben

## State Transitions

```
IDLE
  ├─ ziet speler → ALERTED
  └─ hoort speler → ALERTED

ALERTED
  ├─ ziet speler → ATTACK
  ├─ niets meer gehoord → IDLE
  └─ timeout (5 sec) → IDLE

ATTACK
  ├─ speler uit zicht EN niet gehoord → IDLE
  ├─ speler niet gezien MAAR gehoord → ALERTED
  └─ speler in capture range → GAME OVER
```

## Debugging

### Gizmos bekijken
- Selecteer Enemy in Scene view
- Gizmos tonen:
  - **Yellow cirkel** = Sight Range
  - **Blue cirkel** = Hearing Range
  - **Red cirkel** = Capture Range
  - **Green lijnen** = Waypoint paden

### Console Debugging
- Debug logs tonen state changes:
  - "Enemy is idle."
  - "Enemy is alerted!"
  - "Enemy is attacking!"
  - "Player captured! Game Over!"

## Tips

1. **Performance**: Gebruik `Physics.Raycast` voor line-of-sight. Als traag, optimize Colliders.

2. **Speler ontsnappen**: 
   - Sprint-geluid is luider (15) dan walk (5)
   - Lopen is stiller dan sprinten
   - Uit zicht gaan helpt altijd

3. **Multiple Enemies**:
   - Duplicate je enemy GameObject
   - Geef elk andere waypoints
   - Alles werkt automatisch

4. **Tweaken**:
   - Verlaag `Sight Range` voor minder aggressive enemies
   - Verhoog `Capture Range` voor makkelijkere/moeilijkere game
   - Verander `Patrol Speed` / `Attack Speed` naar voorkeur

## Troubleshooting

| Issue | Oplossing |
|-------|----------|
| Enemy beweegt niet | Zorg NavMesh gebaked is |
| Speler zichtbaar maar geen attack | Check "Player" tag |
| Game freeze na capture | Check GameManager script |
| Enemy ziet overal dwars door muren | Controleer line-of-sight raycast in EnemyAI |

## Script Files

- `EnemyAI.cs` - Main controller
- `IdleState.cs` - Patrol behavior
- `AlertedState.cs` - Search behavior
- `AttackState.cs` - Chase behavior
- `IEnemyState.cs` - State interface
- `Player.cs` - Player controller met noise
- `GameManager.cs` - Game over handling
