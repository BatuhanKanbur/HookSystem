# Unity Hook System 🪝

A high-performance, reflection-based variable tracking and event polling system for Unity.  
Detect changes in **fields** or **properties** automatically without writing manual dirty checks or setter logic.

![Unity Version](https://img.shields.io/badge/Unity-2021.3%2B-blue)
![License](https://img.shields.io/badge/License-MIT-green)

## 🚀 Key Features

* **Zero Boilerplate:** Just add `[HookVar]` attribute to any variable.
* **Dual Support:** Works on both `MonoBehaviour` components and **Plain C# Classes (POCO)**.
* **Optimized Performance:** Uses **Reflection Caching** at initialization. No garbage allocation during runtime checks.
* **Memory Safe:** Uses `WeakReference` to prevent memory leaks.
* **Scene Persistent:** Automatically handles scene transitions and `DontDestroyOnLoad` objects.
* **Configurable:** Includes a custom Editor Window to manage global tick rates and debug settings.

---

## 📦 Installation

1.  Download the package or clone this repository.
2.  Drop the `HookSystem` folder into your project's `Assets/Scripts` directory.
3.  You are ready to go! No initialization required.

---

## 📖 Quick Start

### 1. Tracking a MonoBehaviour Variable
Inherit from `HookableMono`. The system handles registration automatically.

```csharp
using UnityEngine;
public class PlayerHealth : HookableMono 
{
    // Define the callback method name in the attribute
    [HookVar("OnHealthChanged")]
    [SerializeField] private int _currentHealth = 100;

    // Callback must accept (object oldValue, object newValue)
    private void OnHealthChanged(object oldVal, object newVal)
    {
        Debug.Log($"Health dropped from {oldVal} to {newVal}!");
    }

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount; // Hook triggers automatically!
    }
}
```
2. Tracking a Plain Class (POCO) Variable
Inherit from HookableObject. Perfect for Inventory systems, Stats, or Data classes.

```csharp
public class InventoryItem : HookableObject
{
    [HookVar("OnCountUpdated")]
    public int ItemCount;

    // Optional: Set custom update interval in constructor
    public InventoryItem() : base(updateInterval: 0.5f) { }

    private void OnCountUpdated(object oldVal, object newVal)
    {
        Debug.Log($"Item count updated: {newVal}");
    }
}
```
⚙️ Configuration
Go to Tools > Hook System > Settings to open the configuration window.

Global Tick Rate: Controls how often the system checks for changes (default: 0.1s).

Debug Logs: Enables console logs for registration events.

Run In Background: Auto-fix button to ensure hooks run when the window is unfocused.

🔧 Advanced Usage
Custom Update Intervals
You can override the global tick rate for specific critical objects.

```csharp
// Checks every frame (approx)
HookSystem.Register(this, 0.01f); 

// Checks once per second
HookSystem.Register(this, 1.0f);

```
Manual Registration
If you cannot inherit from HookableMono or HookableObject, you can manually register any object.

```csharp
public class CustomManager : MonoBehaviour
{
    [HookVar("OnStateChange")]
    private string _state;

    void Awake() => HookSystem.Register(this);
    void OnDestroy() => HookSystem.Unregister(this);
}

```
🛠 Architecture Notes
The HookRunner: A singleton [HookSystem_Global] is automatically created at runtime. It survives scene loads (DontDestroyOnLoad) and manages all active trackers.

Zombie Protection: The system includes safety checks to prevent "Object spawned during OnDestroy" errors when quitting the application.

Reflection: Reflection is only used once during registration to cache MemberInfo. The update loop uses these cached references for maximum performance.

🛠️ License

© 2025 Batuhan Kanbur.
All rights reserved.
This plugin may be used in both personal and commercial Unity projects.

🌟 Credits

Developed by Batuhan Kanbur

📄 License
This project is licensed under the MIT License - see the LICENSE file for details.
