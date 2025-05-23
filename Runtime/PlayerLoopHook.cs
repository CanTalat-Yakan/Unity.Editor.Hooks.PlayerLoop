using UnityEngine.LowLevel;

namespace UnityEssentials
{
    /// <summary>
    /// Provides utility methods for adding and removing custom update delegates to the Unity player loop system.
    /// </summary>
    /// <remarks>The <see cref="PlayerLoopHook"/> class allows developers to inject or remove custom update
    /// logic into specific subsystems of the Unity player loop. This can be useful for extending or modifying the
    /// behavior of the game loop without directly modifying Unity's internal systems.</remarks>
    public static class PlayerLoopHook
    {
        public static void Add<T>(PlayerLoopSystem.UpdateFunction updateDelegate) where T : struct
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();

            for (int i = 0; i < loop.subSystemList.Length; i++)
                if (loop.subSystemList[i].type == typeof(T))
                {
                    var system = loop.subSystemList[i];
                    system.updateDelegate += updateDelegate;
                    loop.subSystemList[i] = system;
                    break;
                }

            PlayerLoop.SetPlayerLoop(loop);
        }

        public static void Remove<T>(PlayerLoopSystem.UpdateFunction updateDelegate) where T : struct
        {
            var loop = PlayerLoop.GetCurrentPlayerLoop();

            for (int i = 0; i < loop.subSystemList.Length; i++)
                if (loop.subSystemList[i].type == typeof(T))
                {
                    var system = loop.subSystemList[i];
                    system.updateDelegate -= updateDelegate;
                    loop.subSystemList[i] = system;
                    break;
                }

            PlayerLoop.SetPlayerLoop(loop);
        }
    }
}