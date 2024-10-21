using EntityStates;
using RoR2;
using RoR2.UI;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple
{
    public class BaseChargeHollowState : BasePrisonerSkillState
    {
        private protected float duration;
        private protected ChildLocator childLocator;
        public float baseDuration = 1.5f;
        public float minBloomRadius;
        public float maxBloomRadius;
        public float minChargeDuration = 0.5f;
        public string chargeSoundString;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            this.childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                Transform transform = childLocator.FindChild("MuzzleHollow") ?? characterBody.coreTransform;
                if (transform && this.chargeEffectPrefab)
                {
                    chargeEffectInstance = UnityEngine.Object.Instantiate(this.chargeEffectPrefab, transform.position, transform.rotation);
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
            this.PlayChargeAnimation();
            this.loopSoundInstanceId = Util.PlayAttackSpeedSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
            //if (crosshairOverridePrefab)
            //{
            //    crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(characterBody, crosshairOverridePrefab, CrosshairUtils.OverridePriority.Skill);
            //}
            base.StartAimMode(duration + 2f, false);
        }

        public override void OnExit()
        {
            CrosshairUtils.OverrideRequest overrideRequest = this.crosshairOverrideRequest;
            //if (overrideRequest != null)
            //{
            //    overrideRequest.Dispose();
            //}
            AkSoundEngine.StopPlayingID(this.loopSoundInstanceId);
            if (!this.outer.destroying)
            {
                //PlayAnimation("Gesture, Additive", EmptyStateHash);
            }
            EntityState.Destroy(this.chargeEffectInstance);
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            float num = this.CalcCharge();
            if (isAuthority && (!base.IsKeyDownAuthority() && fixedAge >= minChargeDuration || fixedAge >= duration))
            {
                Chat.AddMessage("Start nextstate");
                BaseThrowHollow nextState = this.GetNextState();
                nextState.charge = num;
                this.outer.SetNextState(nextState);
                Chat.AddMessage("End nextstate");
            }
        }
        protected float CalcCharge()
        {
            return Mathf.Clamp01(base.fixedAge / this.duration);
        }

        public BaseChargeHollowState() : base() { }

        public override void Update()
        {
            base.Update();
            characterBody.SetSpreadBloom(Util.Remap(CalcCharge(), 0f, 1f, minBloomRadius, maxBloomRadius), true);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        protected virtual BaseThrowHollow GetNextState()
        {
            return new BaseThrowHollow();
        }
        protected virtual void PlayChargeAnimation()
        {
            //base.PlayAnimation("Gesture, Additive", BaseChargeHollowState.ChargeNovaBombStateHash, BaseChargeHollowState.ChargeNovaBombParamHash, duration);
        }

        public GameObject chargeEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/ChargeMageLightningBomb.prefab").WaitForCompletion();
        private protected GameObject chargeEffectInstance;

        public GameObject crosshairOverridePrefab;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;

        private uint loopSoundInstanceId;
        private static int EmptyStateHash = Animator.StringToHash("Empty");
        private static int ChargeNovaBombStateHash = Animator.StringToHash("ChargeNovaBomb");
        private static int ChargeNovaBombParamHash = Animator.StringToHash("ChargeNovaBomb.playbackRate");

    }
}
