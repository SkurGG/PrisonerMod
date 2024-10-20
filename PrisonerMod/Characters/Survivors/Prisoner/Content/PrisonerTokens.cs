using System;
using PrisonerMod.Modules;
using PrisonerMod.Survivors.Prisoner.Achievements;

namespace PrisonerMod.Survivors.Prisoner
{
    public static class PrisonerTokens
    {
        public static void Init()
        {
            AddHenryTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddHenryTokens()
        {
            string prefix = PrisonerSurvivor.PRISONER_PREFIX;

            string desc = "The Prisoner is a chaotic force of nature, sending uncontrollable energy at the rythm of the heart of the planet that watched them born.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > " + Environment.NewLine + Environment.NewLine
             + "< ! > " + Environment.NewLine + Environment.NewLine
             + "< ! > " + Environment.NewLine + Environment.NewLine
             + "< ! > " + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, hoping to fulfill the desires of his Father.";
            string outroFailure = "..and so he vanished, forever just a remnant of the vengeance they sought.";

            Language.Add(prefix + "NAME", "Prisoner");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The True Son");
            Language.Add(prefix + "LORE", "");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Unchained");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Concentration");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Prisoner has to concentrate to get energy through his prison. Every ability procs a Concentration event. Every time you get a Critical Concentration, gain 1 stack of concentration, giving a <style=cIsDamage> 10% damage boost</style> to the next attack you do." +
                "If you fail a Concentration event, all your abilities enter in a X second cooldown.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "C001: Create");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Cast a spell from your spellslots.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "C002: Destroy");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Delete a spell from your spellslots.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "C003: Recover");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Invoke a Concentration event, and if you get Critical Conncentration, you enter into <style=cIsUtility>Flow</style> state.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "C004: Channel");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Channel the will of Petrichor V, and fill your Soul with power. Draw a spell into a spellslot.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(PrisonerMasteryAchievement.identifier), "Prisoner: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(PrisonerMasteryAchievement.identifier), "As Prisoner, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
