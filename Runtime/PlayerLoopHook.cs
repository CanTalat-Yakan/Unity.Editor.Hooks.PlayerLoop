using UnityEngine.LowLevel;

namespace UnityEssentials
{
    public static class PlayerLoopHook
    {
        public static void AddToPlayerLoop<T>(PlayerLoopSystem.UpdateFunction updateDelegate) where T : struct
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

        public static void RemoveFromPlayerLoop<T>(PlayerLoopSystem.UpdateFunction updateDelegate) where T : struct
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