﻿using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.PrisonerBaseState;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using PrisonerMod.Modules;
using PrisonerMod.Survivors.Prisoner;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple
{
    public class ThrowHollow : BasePrisonerSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / this.attackSpeedStat;
            //PlayThrowAnimation();
            //if (muzzleflashEffectPrefab)
            //{
            //    EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, base.gameObject, "MuzzleHollow", false);
            //}
            Chat.AddMessage("Before fire");
            Fire();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= duration)
            {
                this.outer.SetNextStateToMain();
            }
        }
        public override void OnExit()
        {
            base.OnExit();
        }
        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                if (projectilePrefab != null)
                {
                    float num = Util.Remap(charge, 0f, 1f, minDamageCoefficient, maxDamageCoefficient);
                    float num2 = charge * force;
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = projectilePrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = this.damageStat * num,
                        force = num2,
                        crit = base.RollCrit()
                    };
                    ModifyProjectile(ref fireProjectileInfo);
                    Chat.AddMessage("projectile created");
                    TrajectoryAimAssist.ApplyTrajectoryAimAssist(ref aimRay, ref fireProjectileInfo, 1f);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * (-selfForce * charge), false, false);
                }
            }
        }

        public ThrowHollow() : base() { } 
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        protected virtual void PlayThrowAnimation()
        {
            //base.PlayAnimation("Gesture, Additive", BaseThrowHollow.FireNovaBombStateHash, BaseThrowHollow.FireNovaBombParamHash, duration);
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {
        }


        public static GameObject projectilePrefab = PrisonerAssets.hollowPurpleProjectilePrefab;
        public static GameObject muzzleflashEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MuzzleflashMageLightningLarge.prefab").WaitForCompletion();
        public static float baseDuration = 2f;
        public static float minDamageCoefficient = 200f;
        public static float maxDamageCoefficient = 800f;
        public static float force = 30f;
        public static float selfForce = 30f;
        protected static float duration = 2f;
        public float charge = 1f;

        private static int FireNovaBombStateHash = Animator.StringToHash("FireNovaBomb");
        private static int FireNovaBombParamHash = Animator.StringToHash("FireNovaBomb.playbackRate");
    }
}
