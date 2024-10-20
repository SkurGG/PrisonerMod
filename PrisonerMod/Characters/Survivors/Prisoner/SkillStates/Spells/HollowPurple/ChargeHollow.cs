using System;
using System.Collections.Generic;
using System.Text;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple
{
    public class ChargeHollow : BaseChargeHollowState
    {
        protected override BaseThrowHollow GetNextState()
        {
            return new ThrowHollow();
        }
    }
}
