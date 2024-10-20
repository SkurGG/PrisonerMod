using PrisonerMod.Characters.Survivors.Prisoner.SkillStates;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells;

namespace PrisonerMod.Survivors.Prisoner
{
    public static class PrisonerStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Draw));
        }
    }
}
