using PrisonerMod.Characters.Survivors.Prisoner.SkillStates;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple;

namespace PrisonerMod.Survivors.Prisoner
{
    public static class PrisonerStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(Draw));
            Modules.Content.AddEntityState(typeof(ChargeHollow));
            Modules.Content.AddEntityState(typeof(ThrowHollow));
            Modules.Content.AddEntityState(typeof(BaseChargeHollowState));
            Modules.Content.AddEntityState(typeof(BaseThrowHollow));


        }
    }
}
