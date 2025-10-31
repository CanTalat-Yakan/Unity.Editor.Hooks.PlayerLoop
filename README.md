# Unity Essentials

This module is part of the Unity Essentials ecosystem and follows the same lightweight, editor-first approach.
Unity Essentials is a lightweight, modular set of editor utilities and helpers that streamline Unity development. It focuses on clean, dependency-free tools that work well together.

All utilities are under the `UnityEssentials` namespace.

```csharp
using UnityEssentials;
```

## Installation

Install the Unity Essentials entry package via Unity's Package Manager, then install modules from the Tools menu.

- Add the entry package (via Git URL)
    - Window → Package Manager
    - "+" → "Add package from git URL…"
    - Paste: `https://github.com/CanTalat-Yakan/UnityEssentials.git`

- Install or update Unity Essentials packages
    - Tools → Install & Update UnityEssentials
    - Install all or select individual modules; run again anytime to update

---

# Player Loop Hook

> Quick overview: Tiny runtime helper to inject and remove managed delegates into top‑level Unity PlayerLoop phases like `Update`, `FixedUpdate`, `EarlyUpdate`, etc., via `PlayerLoop.Add<T>()` / `Remove<T>()`.

A minimal wrapper around `UnityEngine.LowLevel.PlayerLoop` that makes it easy to attach your own `UpdateFunction` delegates to a chosen PlayerLoop subsystem. Works in Editor and Player builds and keeps your logic decoupled from MonoBehaviour update methods.

![screenshot](Documentation/Screenshot.png)

## Features
- One‑line API
  - `PlayerLoopHook.Add<T>(PlayerLoopSystem.UpdateFunction)`
  - `PlayerLoopHook.Remove<T>(PlayerLoopSystem.UpdateFunction)`
- Targets top‑level PlayerLoop phases
  - Common phases include `Initialization`, `EarlyUpdate`, `FixedUpdate`, `PreUpdate`, `Update`, `PreLateUpdate`, `PostLateUpdate`, `TimeUpdate`
- Works in Editor and runtime builds
- No allocations during update; only modifies the loop when adding/removing
- Composable: attach multiple delegates to the same phase

## Requirements
- Unity 6000.0+ (Runtime + Editor)
- Namespaces used:
  - `using UnityEngine.LowLevel;`
  - `using UnityEngine.PlayerLoop;` (for phase marker structs like `Update`, `FixedUpdate`, etc.)

## Usage

Hook into `Update` and `FixedUpdate` with cached delegates so you can reliably remove them later.

```csharp
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;
using UnityEssentials;

public class PlayerLoopHookExample : MonoBehaviour
{
    // Cache delegates so Remove can use the same instance
    private static readonly PlayerLoopSystem.UpdateFunction OnUpdateFn = OnUpdate;
    private static readonly PlayerLoopSystem.UpdateFunction OnFixedFn  = OnFixed;

    private void OnEnable()
    {
        PlayerLoopHook.Add<Update>(OnUpdateFn);
        PlayerLoopHook.Add<FixedUpdate>(OnFixedFn);
    }

    private void OnDisable()
    {
        PlayerLoopHook.Remove<Update>(OnUpdateFn);
        PlayerLoopHook.Remove<FixedUpdate>(OnFixedFn);
    }

    private static void OnUpdate()
    {
        // Runs each frame in the Update phase
        // Debug.Log("PlayerLoop Update");
    }

    private static void OnFixed()
    {
        // Runs in the FixedUpdate phase
    }
}
```

### Choosing a phase
- Add the appropriate using and generic type:
  - `PlayerLoopHook.Add<EarlyUpdate>(fn);`
  - `PlayerLoopHook.Add<PreLateUpdate>(fn);`
  - `PlayerLoopHook.Add<PostLateUpdate>(fn);`
- The generic type `T` must be one of the top‑level marker structs in `UnityEngine.PlayerLoop`

### Removing delegates
- You must pass the same delegate instance you added; prefer caching static fields as shown
- Safe to call `Remove` multiple times; if not present, nothing happens

## Notes and Limitations
- Top‑level only: The helper searches only the top‑level `PlayerLoopSystem.subSystemList`. Nested subsystems are not modified
- Whole‑loop set: Each add/remove fetches the current loop and calls `PlayerLoop.SetPlayerLoop(loop)` after the change
- Don’t spam changes: Avoid frequent add/remove per frame; modify the loop on enable/disable or one‑time setup
- Multiple hooks: Order among multiple delegates attached to the same phase follows multicast delegate order
- Domain reloads: In the Editor, domain reload resets the loop to Unity’s default; re‑add hooks on enable/initialize

## Files in This Package
- `Runtime/PlayerLoopHook.cs` – Core implementation (Add/Remove delegates onto top‑level phases)
- `Runtime/UnityEssentials.PlayerLoopHook.asmdef` – Runtime assembly definition
- `package.json` – Package manifest metadata

## Tags
unity, unity-editor, runtime, playerloop, update, fixedupdate, earlyupdate, prelateupdate, postlateupdate, loop, hook, utility
