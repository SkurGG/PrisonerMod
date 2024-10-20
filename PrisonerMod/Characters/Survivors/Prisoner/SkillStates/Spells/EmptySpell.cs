using PrisonerMod.Characters.Survivors.Prisoner.Components;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells
{
    public class EmptySpell : BasePrisonerSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            prisonerController.UnsetSkills();
            base.OnExit();
        }
    }
}
