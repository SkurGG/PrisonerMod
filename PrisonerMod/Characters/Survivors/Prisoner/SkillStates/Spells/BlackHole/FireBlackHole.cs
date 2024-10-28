using System;
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

namespace Spells.BlackHole
{
    // Token: 0x02000277 RID: 631
    public class BaseFireMine : BasePrisonerSkillState
    {
        // Token: 0x06000BA9 RID: 2985 RVA: 0x00030D74 File Offset: 0x0002EF74
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            float num = this.baseCrossfadeDuration / this.attackSpeedStat;
            Util.PlaySound(this.enterSoundString, base.gameObject);
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            if (base.GetModelAnimator())
            {
                base.PlayCrossfade(this.animationLayerName, this.animationStateName, this.animationPlaybackRateParam, this.duration, num);
            }
            if (this.muzzleEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleEffectPrefab, base.gameObject, this.muzzleName, false);
            }
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(this.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageStat * this.damageCoefficient, this.force, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
            }
        }

        // Token: 0x06000BAA RID: 2986 RVA: 0x00015B35 File Offset: 0x00013D35
        public override void OnExit()
        {
            base.OnExit();
        }

        // Token: 0x06000BAB RID: 2987 RVA: 0x00030E80 File Offset: 0x0002F080
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06000BAC RID: 2988 RVA: 0x00015CF7 File Offset: 0x00013EF7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public float baseDuration;
        public float baseCrossfadeDuration;

        public GameObject muzzleEffectPrefab;
        public GameObject projectilePrefab;

        public string muzzleName;
        public float damageCoefficient;
        public float force;
        public string enterSoundString;
        public string animationLayerName;
        public string animationStateName;
        public string animationPlaybackRateParam;
        private float duration;

    }
}
