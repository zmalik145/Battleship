# Battleship
 Battle Buildings is a strategic 2-player game where each player must strategically attack their opponent's buildings while defending their own. The game features a grid-based battlefield where players take turns attacking enemy cells and attempting to destroy buildings. The player who destroys all of their opponent's buildings first wins the game.
# Game Objective:
The objective of Battle Buildings is to strategically destroy all of the opponent's buildings before
they destroy yours. Each player takes turns attacking enemy cells, trying to find and destroy
buildings while defending their own.

# Gameplay Mechanics:
1. Grid-Based Battlefield: The game takes place on a grid-based battlefield where players can click on cells
to attack.
2. Player Turn: The game starts with the player's turn. The player clicks on enemy cells to attack. If the clicked cell contains a building, it is destroyed, and the player's score increases. If the cell does not contain a building, a missed text appears on the cell. After the player's turn ends, it becomes the enemy's turn.
3. Enemy Turn: During the enemy's turn, a button appears on the screen for the player to initiate
the enemy's attack. The enemy AI randomly selects a cell from the player's grid to attack.
○ If the selected cell contains a building, it is destroyed, and the enemy's score increases.
○ If the cell does not contain a building, a missed text appears on the cell.
○ After the enemy's turn ends, it becomes the player's turn again.
4. Building Destruction:
○ Each player has a fixed number of buildings, randomly distributed across their
grid.
○ Buildings are initially hidden for the enemy player, while the player's buildings are
visible.
○ When a building is destroyed, it becomes visibly damaged or marked as
destroyed on the grid.
5. Win Condition:
○ The game continues until one player destroys all of the opponent's buildings.
○ The player who destroys all enemy buildings first wins the game.
○ If both players destroy all buildings simultaneously, the game ends in a draw.
6. UI Elements:
○ The game displays the current turn (player or enemy) and each player's score.
○ A game status text updates to indicate whether it's the player's turn or if the game
has ended.

# Technical Details:
● Grid System: The game takes place on a grid with a set size (defined by gridSizeX and
gridSizeY in GridManager script).
● Cell: Each cell on the grid represents a location where a building can be placed. Cells
are created by the GridManager script.
● Building: Buildings are placed randomly on the player's side of the grid at the start of
the game (handled by GameManager's RandomlySelectBuildings function). They are
hidden from the opponent until attacked.
● Attacking: Players attack by clicking on their opponent's cells. This triggers a raycast to
check if there's a building present (handled by PlayerController script for player attack
and EnemyAI script for enemy attack).
● Health: Buildings have health points (defined by maxHealth in BattleBuilding script).
Taking damage reduces their health.
● Turn System: Players take turns attacking. The GameManager script keeps track of
whose turn it is and swaps turns after each attack.
● Scoring: Destroying a building increases the attacker's score by 1. The game is won by
reaching a score equal to the total number of buildings placed at the start (handled by
GameManager's BuildingHit function).

# Scripts Details:
1. GridManager.cs
The GridManager class is responsible for creating the grid layout for the game. It
instantiates cells (game objects) based on the specified dimensions (gridSizeX and
gridSizeY) and spacing (cellSpacing). It creates two grids, one for the player and one for
the enemy, by calling the CreateGrid method twice with different starting positions and
cell lists.
Inside the CreateGrid method, it iterates over the grid dimensions and instantiates cells
at calculated positions. Each cell is given a unique name and tagged based on whether
it belongs to the player or the enemy. The instantiated cells are added to the respective
cell lists (playerCells or enemyCells) for further use. Finally, it invokes the SelectBuilding
event, triggering the selection of buildings for both players.
2. BattleBuilding.cs
a. Properties:
i. HasBuilding: Indicates whether this cell has a building placed on it.
ii. IsDead: Indicates whether the building on this cell has been destroyed.
iii. IsShaking: Used for a shaking animation effect.
b. State Management:
i. Tracks the building's current health (currentHealth).
ii. Stores the initial position of the cell (originalPosition).
iii. Manages a timer for the shaking animation (shakeTimer).
c. Methods:
i. Start: Initializes the cell's health, health text, and stores the original
position.
ii. Update: Handles the shaking animation if triggered.
iii. Shake: Starts the shaking animation.
iv. TakeDamage: Reduces the building's health, updates health text, and
triggers the death effect if health reaches zero.
v. UpdateHealthText: Updates the displayed health value.
vi. DeathEffect: Shows the building image, a "cross" sprite (presumably
signifying destruction), and sets the IsDead flag.
vii. HitMissed: Shows a "missed" text when an attack misses a building on
this cell.
viii. BuildingImage: Controls the visibility of the building image based on the
provided state.
3. PlayerController.cs And EnemyAI.cs
The PlayerController And EnemyAI script handles clicks during the player's turn. It
raycasts to check if other player building is hit. If it hits a building, it deals damage, plays
a hit sound, and shakes the cell. If it misses, it plays a miss sound and shows a "missed"
effect. Finally, it triggers a turn switch.
4. GameManager.cs
1. Singleton Instance: It ensures only one GameManager instance exists by using
a static property Instance.
2. Game Data:
a. It stores the maximum number of buildings allowed (maxBuilding).
b. It references UI elements for displaying game status (gameStatus),
player/enemy scores (playerScoreText, enemyScoreText), and the player
attack button (playerAttackButton).
c. It tracks internal variables for player turn (isPlayerTurn), game over state
(isGameOver), player/enemy scores (playerScore, enemyScore), and
building placement on the grid (playerCells, enemyCells, playerBuildings,
enemyBuildings).
3. Event Management:
a. It defines static delegate actions:
i. SelectBuilding: Likely used to trigger initial building placement on
the grid (implemented in Awake).
ii. SwapTurn: Handles logic when switching between player and
enemy turns (implemented in Awake).
4. Initialization (Awake):
a. It sets itself as the singleton instance if none exists, otherwise destroys
itself to prevent duplicates.
b. It subscribes the RandomlySelectBuildings function to the SelectBuilding
action.
c. It subscribes the OnSwapTurn function to the SwapTurn action.
5. Game Start (Start):
a. It calls OnSwapTurn, likely to initialize the turn and update UI.
6. OnDisable:
a. It unregisters functions from the delegate actions when the game object is
disabled.
7. Building Placement (RandomlySelectBuildings):
a. It determines the total number of buildings (totalBuilding) randomly within
a range.
b. It calls GetRandomCells twice to randomly select cells for player and
enemy buildings from their respective cell lists (playerCells, enemyCells)
and stores them in building lists (playerBuildings, enemyBuildings).
c. It activates the building visuals (ActivateBuildings) for player and enemy
buildings with appropriate states (showing or hiding the building image).
8. Activate Buildings:
a. It iterates through a building list and activates their building image based
on the provided flag and sets the HasBuilding property to true.
9. GetRandomCells:
a. It shuffles the original cell list using a generic Shuffle function.
b. It selects the first n elements (limited by the original list size) and adds
them to the building list.
10. Shuffle (Generic)
● It implements the Fisher-Yates shuffle algorithm to randomize the order of
elements in a generic list.
11. Update UI:
● It updates the game status text to indicate whose turn it is.
● It updates the player and enemy score text displays.
12. Swap Turns (OnSwapTurn):
● It toggles the isPlayerTurn flag.
● It activates/deactivates the player attack button based on the current turn.
● It calls UpdateUI to reflect changes on the UI.
13. Building Hit:
● It increments the appropriate score (player or enemy) based on the current turn.
● It checks the win condition (CheckWinCondition) if the score reaches the total
building count.
14. Win Condition Check:
● It sets the isGameOver flag to true.
● It starts a coroutine (GameStatus) to display the win/lose message after a delay.
15. Game Status Coroutine:
● It waits for a second.
● It deactivates the player attack button.
● It plays the win or lose sound effect based on the provided flag (win).
● It displays the win/lose message on the game status text and prints a message to
the console.

# User Interface (UI)
● Text displays the current player's turn ("Player Turn" or "Enemy Turn"). (handled by
GameManager's UpdateUI function)
● Text displays the score for each player ("Player Score: HitBuilding/TotalBuilding" and
"Enemy Score: X/Y"). (handled by GameManager's UpdateUI function)
● A button (only visible during the enemy’s turn) allows the enemy to initiate an attack.
(handled by GameManager's OnSwapTurn function)

# Sound Design
● Sound effects for attacks hitting and missing targets. (handled by PlayerController,
EnemyAI and BattleBuilding scripts)
● Sound effects for building destruction. (handled by BattleBuilding script)
● Sound effects or music for winning/losing the game. (handled by GameManager script)


