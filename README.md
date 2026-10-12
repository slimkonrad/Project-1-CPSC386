# Rockfall Run

**CPSC 386 – Project 1** · Konner Rigby
**Repository:** https://github.com/slimkonrad/Project-1-CPSC386
**Engine:** Unity 6 (Universal 2D template, URP 2D Renderer)

A single-screen 2D platformer. You play as a pigeon climbing a cave-like level to reach a glowing finish flag while pooled rocks rain down from above. Getting hit restarts the run. Your best completion time is saved and shown on the Main Menu.

## Controls

| Action | Keys |
|---|---|
| Move left / right | A / D |
| Jump | Space or W (only while standing on ground) |
| Fast-fall (mid-air) | S |
| Pause / resume | Escape |

## How to play

1. Click **Start** on the Main Menu.
2. Climb the three platforms to the top, then drop down the right side of the pillar to the flag.
3. Avoid the falling rocks. One hit restarts the level.
4. Reaching the flag shows your time. A new record is saved as your best time.

---

## Scripts

All scripts are in `Assets/Scripts`.

| Script | Attached to | What it does |
|---|---|---|
| `MainMenu.cs` | MenuController (MainMenu scene) | Loads Level1 from the Start button, quits the app from the Quit button, and displays the saved best time from PlayerPrefs. Resets `Time.timeScale` to 1 on load as a safety net. |
| `PlayerController.cs` | Player | Reads the Move and Jump actions from the `PlayerControls` Input Actions asset and moves the player by setting `Rigidbody2D.linearVelocity` in `FixedUpdate`. Jumping is allowed only when touching Ground from above (contact normal check), which prevents wall-jumping. Holding Down in mid-air fast-falls. Flips the sprite to face the movement direction. Ignores input while the game is paused. |
| `PlayerTriggerHandler.cs` | Player | `OnTriggerEnter2D` + `CompareTag()`: touching a `Hazard` calls `LevelManager.PlayerDied()`, and touching `Finish` calls `LevelManager.LevelComplete()`. |
| `SimplePool.cs` | RockPool | Object pool reused from Assignment 3. Pre-creates a set number of rock prefabs in `Awake`, deactivated and parented under the pool. `GetFromPool(position)` reactivates an inactive instance at the given position. It only instantiates a new rock if every pooled rock is already in use. |
| `RockSpawner.cs` | RockSpawner | Every `spawnInterval` seconds, takes a rock from the pool and places it at a random x just above the top of the camera view. The spawn area is calculated from the camera so rocks never spawn inside the side walls. |
| `FallingRock.cs` | Rock prefab | Resets the rock's velocity each time it is reused (`OnEnable`). When it touches Ground (`OnTriggerEnter2D` + `CompareTag("Ground")`), or falls below the level, it deactivates itself, which returns it to the pool. |
| `LevelManager.cs` | LevelManager | Runs the run timer UI, reloads the level when the player dies, and on finishing freezes the game, compares the time against the saved best, saves a new best with `PlayerPrefs.SetFloat` / `PlayerPrefs.Save`, and shows the win panel. Handles the Play Again and Main Menu buttons. |
| `PauseMenu.cs` | LevelManager | Toggles the pause panel on the Pause action (Escape). Pausing sets `Time.timeScale = 0`, freezing physics, rocks, and the timer. The Resume button unpauses, and Quit to Menu restores time scale and loads the Main Menu. Pausing is disabled once the win panel is showing. |

## Scenes and key GameObjects

### MainMenu (build index 0)
- **Canvas** (Scale With Screen Size, 1920×1080): title text, **BestTimeText**, **StartButton**, **QuitButton**.
- **MenuController**: holds `MainMenu.cs`. Both buttons call its methods through On Click.
- **EventSystem**: required for UI clicks.

### Level1 (build index 1)
- **Grid → Ground**: the painted Tilemap (floor, side walls, three platforms, and the pillar). It has a Tilemap Collider 2D merged through a Composite Collider 2D (static Rigidbody2D) so the player doesn't catch on tile seams. Tagged **Ground**.
- **Player**: pigeon sprite with Rigidbody2D (gravity 3, continuous collision, rotation frozen), Box Collider 2D sized to the body, and a zero-friction Physics Material 2D (**NoFriction**) so the player doesn't stick to walls. Holds `PlayerController` and `PlayerTriggerHandler`. Child **Lantern** is a Spot Light 2D that follows the player.
- **RockPool**: holds `SimplePool` with the **Rock** prefab. All pooled rocks appear as its children.
- **RockSpawner**: holds `RockSpawner`, which references RockPool.
- **Rock prefab** (`Assets/Prefabs`): stone sprite, Rigidbody2D, trigger collider, `FallingRock`. Tag **Hazard**, layer **Rocks**.
- **Finish**: flag sprite with a trigger Box Collider 2D, tagged **Finish**. Child **Spot Light 2D** gives it a warm orange glow.
- **Global Light 2D**: intensity 0.4 to darken the level. Paired with a dark camera background, the flag and lantern lights stand out.
- **Canvas**: **TimerText**; **WinPanel** (WinText, Play Again, Main Menu); **PausePanel** (Paused text, Resume, Quit to Menu).
- **LevelManager**: holds `LevelManager` and `PauseMenu`.

### How they interact
RockSpawner pulls rocks from RockPool. Each rock falls and returns itself to the pool when it lands. If a rock (Hazard) touches the Player first, PlayerTriggerHandler tells LevelManager to restart. When the Player touches the Finish flag, LevelManager stops the timer, saves the best time, and shows the win panel. MainMenu reads that same saved value. PauseMenu freezes everything through `Time.timeScale`.

---

## Requirement checklist

1. **Scenes and scene management:** MainMenu's Start button loads Level1. Level1 returns to MainMenu through the pause menu's Quit to Menu button and the win panel's Main Menu button (`SceneManager.LoadScene`).
2. **Input System player control:** the Player moves using the `Assets/Settings/PlayerControls.inputactions` asset (Player map: Move, Jump, Pause), referenced through `InputActionReference` fields, with Rigidbody2D/Collider2D physics.
3. **Tilemap level layout:** the entire level (floor, walls, platforms, pillar) is the **Ground** Tilemap with a Tilemap Collider 2D (merged by a Composite Collider 2D).
4. **Prefab pooling:** the Rock prefab is pooled by `SimplePool` (Assignment 3 pattern). During play, RockPool's child rocks switch on and off while the count stays the same, so instances are reused rather than created and destroyed.
5. **Tags, layers, and triggers:** `OnTriggerEnter2D` + `CompareTag()` drive dying (Hazard), winning (Finish), and rock despawning (Ground). The Layer Collision Matrix has **Rocks vs. Rocks** disabled so rocks pass through each other.
6. **Pause menu:** a Canvas UI panel opened with Escape that sets `Time.timeScale = 0`, with working Resume and Quit to Menu buttons.
7. **PlayerPrefs persistence:** the best completion time is saved under the key `BestTime` and shown on the Main Menu. It persists after closing and relaunching the build.
8. **Lighting:** Global Light 2D dims the level, a Spot Light 2D on the finish flag highlights the goal, and a Spot Light 2D "lantern" follows the player.

---

## Third-party assets, packages, and assistance

- **Kenney – Pixel Platformer** (https://kenney.nl/assets/pixel-platformer), CC0 license. Used for all level tiles and the finish flag.
- **Player sprite:** custom sprite made from a photo of a friend (used with their permission), cut out from its background and resized.
- **Rock sprite:** custom 18×18 pixel-art stone created for this project.
- **Unity packages:** Input System, Universal Render Pipeline (2D Renderer and 2D Lights), 2D Tilemap Editor, TextMeshPro. All are standard Unity packages included with the Universal 2D template.
- **AI assistance:** Claude (Anthropic) was used as a step-by-step guide during development. It helped plan the game, draft and explain the scripts, walk through the Unity Editor setup, and create the stone and player sprites.

## Running the build

Unzip the archive and run `Rockfall Run.exe` (Windows). The Quit button on the Main Menu closes the game; it only works in the built version, not in the Unity Editor.
