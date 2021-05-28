using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using BepInEx.Configuration;

namespace Hitbox_Fix
{

    [BepInPlugin("hitbox.fix", "Hitbox Fix", "1.3.0")]
    [BepInProcess("valheim.exe")]
    public class HitboxFix : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("hitbox.fix");
        private static ConfigEntry<float> offset, height;
        private static ConfigEntry<int> nexusID;

        private void Awake()
        {
            nexusID = Config.Bind<int>("General", "NexusID", 727, "Nexus mod ID for updates");
            offset = Config.Bind<float>("Modify offset", "offset", 0f, "Offset");
            height = Config.Bind<float>("Modify height (0.6 default, 1 mod default)", "height", 1f, "Height");
            harmony.PatchAll();
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }

        [HarmonyPatch(typeof(Attack), "DoMeleeAttack")]
        class Hitbox_Patch
        {
            static void Prefix(ref float ___m_maxYAngle, ref float ___m_attackOffset, ref float ___m_attackHeight)
            {
                    ___m_maxYAngle = 180f;
                    ___m_attackOffset = offset.Value;
                    ___m_attackHeight = height.Value;
            }
        }
    }
}
