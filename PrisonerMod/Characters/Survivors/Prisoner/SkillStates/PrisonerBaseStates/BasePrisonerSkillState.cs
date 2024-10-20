using EntityStates;
using PrisonerMod.Characters.Survivors.Prisoner.Components;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState
{
    public class BasePrisonerSkillState : BaseSkillState
    {
        protected PrisonerController prisonerController;

        public override void OnEnter()
        {
            base.OnEnter();
            RefreshState();
        }

        public void RefreshState()
        {
            if (!this.prisonerController) this.prisonerController = this.GetComponent<PrisonerController>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
