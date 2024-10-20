using EntityStates;
using RoR2;
using RoR2.UI;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple
{
    public abstract class BaseChargeHollowState : BasePrisonerSkillState
    {
        private protected float duration;
        private protected ChildLocator childLocator;
        private protected GameObject chargeEffectInstance;
        public float baseDuration = 1.5f;
        public float minBloomRadius;
        public float maxBloomRadius;
        public float minChargeDuration = 0.5f;
        public string chargeSoundString;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            childLocator = GetModelChildLocator();
            if (childLocator)
            {
                Transform transform = childLocator.FindChild("MuzzleHollow") ?? characterBody.coreTransform;
                if (transform && chargeEffectPrefab)
                {
                    chargeEffectInstance = UnityEngine.Object.Instantiate(chargeEffectPrefab, transform.position, transform.rotation);
                    chargeEffectInstance.transform.parent = transform;
                    ScaleParticleSystemDuration component = chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    ObjectScaleCurve component2 = chargeEffectInstance.GetComponent<ObjectScaleCurve>();
                    if (component)
                    {
                        component.newDuration = duration;
                    }
                    if (component2)
                    {
                        component2.timeMax = duration;
                    }
                }
            }
            PlayChargeAnimation();
            loopSoundInstanceId = Util.PlayAttackSpeedSound(chargeSoundString, gameObject, attackSpeedStat);
            if (crosshairOverridePrefab)
            {
                crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(characterBody, crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
            }
            StartAimMode(duration + 2f, false);
        }

        public override void OnExit()
        {
            CrosshairUtils.OverrideRequest overrideRequest = crosshairOverrideRequest;
            if (overrideRequest != null)
            {
                overrideRequest.Dispose();
            }
            AkSoundEngine.StopPlayingID(loopSoundInstanceId);
            if (!outer.destroying)
            {
                PlayAnimation("Gesture, Additive", EmptyStateHash);
            }
            Destroy(chargeEffectInstance);
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            float num = CalcCharge();
            if (isAuthority && (!IsKeyDownAuthority() && fixedAge >= minChargeDuration || fixedAge >= duration))
            {
                BaseThrowHollow nextState = GetNextState();
                nextState.charge = num;
                outer.SetNextState(nextState);
            }
        }
        protected float CalcCharge()
        {
            return Mathf.Clamp01(fixedAge / duration);
        }
        public override void Update()
        {
            base.Update();
            characterBody.SetSpreadBloom(Util.Remap(CalcCharge(), 0f, 1f, minBloomRadius, maxBloomRadius), true);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        protected abstract BaseThrowHollow GetNextState();
        protected virtual void PlayChargeAnimation()
        {
            base.PlayAnimation("Gesture, Additive", BaseChargeHollowState.ChargeNovaBombStateHash, BaseChargeHollowState.ChargeNovaBombParamHash, duration);
        }

        public GameObject chargeEffectPrefab;
        public GameObject crosshairOverridePrefab;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;

        private uint loopSoundInstanceId;
        private static int EmptyStateHash = Animator.StringToHash("Empty");
        private static int ChargeNovaBombStateHash = Animator.StringToHash("ChargeNovaBomb");
        private static int ChargeNovaBombParamHash = Animator.StringToHash("ChargeNovaBomb.playbackRate");

    }
}
