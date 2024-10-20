using EntityStates;
using PrisonerMod.Characters.Survivors.Prisoner.Misc;
using PrisonerMod.Survivors.Prisoner;
using PrisonerMod.Characters.Survivors.Prisoner.Components;
using RoR2;
using UnityEngine;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates
{
    public class Draw : BasePrisonerSkillState
    {
        public static float baseDuration = 0.6f;
        public static float firePercentTime = 0.0f;
        public static float force = 800f;

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
            this.prisonerController.UnsetSkills();
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isAuthority || base.fixedAge < 0.5f) return;

            if ((this.inputBank.skill1.justPressed))
            {
                this.prisonerController.DrawSpell(skillLocator.primary, 0);
                this.outer.SetNextStateToMain();
                return;

            } else if ((this.inputBank.skill2.justPressed))
            {
                this.prisonerController.DrawSpell(skillLocator.secondary, 1);
                this.outer.SetNextStateToMain();
                return;

            } else if ((this.inputBank.skill3.justPressed))
            {
                this.prisonerController.DrawSpell(skillLocator.utility, 2);
                this.outer.SetNextStateToMain();
                return;

            } else if ((this.inputBank.skill4.justPressed))
            {
                this.prisonerController.DrawSpell(skillLocator.special, 3);
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}