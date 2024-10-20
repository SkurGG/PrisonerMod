using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple
{
    public abstract class BaseThrowHollow : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / this.attackSpeedStat;
            PlayThrowAnimation();
            if (muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(muzzleflashEffectPrefab, base.gameObject, "MuzzleHollow", false);
            }
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
                    TrajectoryAimAssist.ApplyTrajectoryAimAssist(ref aimRay, ref fireProjectileInfo, 1f);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * (-selfForce * charge), false, false);
                }
            }
        }

        // Token: 0x06000ED0 RID: 3792 RVA: 0x00015CF7 File Offset: 0x00013EF7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        protected virtual void PlayThrowAnimation()
        {
            base.PlayAnimation("Gesture, Additive", BaseThrowHollow.FireNovaBombStateHash, BaseThrowHollow.FireNovaBombParamHash, duration);
        }

        // Token: 0x06000ED2 RID: 3794 RVA: 0x0000221D File Offset: 0x0000041D
        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {
        }

        // Token: 0x0400118F RID: 4495
        [SerializeField]
        public GameObject projectilePrefab;

        // Token: 0x04001190 RID: 4496
        [SerializeField]
        public GameObject muzzleflashEffectPrefab;

        // Token: 0x04001191 RID: 4497
        [SerializeField]
        public float baseDuration;

        // Token: 0x04001192 RID: 4498
        [SerializeField]
        public float minDamageCoefficient;

        // Token: 0x04001193 RID: 4499
        [SerializeField]
        public float maxDamageCoefficient;

        // Token: 0x04001194 RID: 4500
        [SerializeField]
        public float force;

        // Token: 0x04001195 RID: 4501
        [SerializeField]
        public float selfForce;

        // Token: 0x04001196 RID: 4502
        protected float duration;

        // Token: 0x04001197 RID: 4503
        public float charge;

        // Token: 0x04001198 RID: 4504
        private static int FireNovaBombStateHash = Animator.StringToHash("FireNovaBomb");

        // Token: 0x04001199 RID: 4505
        private static int FireNovaBombParamHash = Animator.StringToHash("FireNovaBomb.playbackRate");
    }
}
