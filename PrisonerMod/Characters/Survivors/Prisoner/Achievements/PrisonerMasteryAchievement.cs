using RoR2;
using PrisonerMod.Modules.Achievements;

namespace PrisonerMod.Survivors.Prisoner.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class PrisonerMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = PrisonerSurvivor.PRISONER_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = PrisonerSurvivor.PRISONER_PREFIX + "masteryUnlockable";
        public override string RequiredCharacterBody => PrisonerSurvivor.instance.bodyName;
        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}