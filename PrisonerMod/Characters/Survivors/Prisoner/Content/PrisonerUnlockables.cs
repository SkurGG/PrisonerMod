using PrisonerMod.Survivors.Prisoner.Achievements;
using RoR2;
using UnityEngine;

namespace PrisonerMod.Survivors.Prisoner
{
    public static class PrisonerUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                PrisonerMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(PrisonerMasteryAchievement.identifier),
                PrisonerSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
