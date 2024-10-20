using EntityStates;
using PrisonerMod.Characters.Survivors.Prisoner.Misc;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState;
using PrisonerMod.Survivors.Prisoner;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates
{
    internal class Cast : BasePrisonerSkillState
    {
        public static float baseDuration = 0.6f;
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static int skillpressed;

        private float duration;
        private float fireTime;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            if (this.prisonerController)
            {
                this.prisonerController.SetSkills();
            }
        }

        public override void OnExit()
        {
            this.prisonerController.spellSlots[skillpressed] = PrisonerSpellCatalog.EmptySpell;
            this.prisonerController.UnsetSkills();
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isAuthority || base.fixedAge < 0.5f) return;

            if ((this.inputBank.skill1.justPressed))
            {
                skillpressed = 0;
                return;

            }
            else if ((this.inputBank.skill2.justPressed))
            {
                skillpressed = 1;
                return;

            }
            else if ((this.inputBank.skill3.justPressed))
            {
                skillpressed = 2;
                return;

            }
            else if ((this.inputBank.skill4.justPressed))
            {
                skillpressed = 3;
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Any;
        }
    }
}
