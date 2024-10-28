using RoR2;
using UnityEngine;
using PrisonerMod.Modules;
using System;
using ShaderSwapper;
using RoR2.Projectile;

namespace PrisonerMod.Survivors.Prisoner
{
    public static class PrisonerAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;
        public static GameObject hollowPurpleProjectilePrefab;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();

        }


        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");
        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects


        #region projectiles
        private static void CreateProjectiles()
        {
            AddAbsorbProjectile();
            CreateBombProjectile();
            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(hollowPurpleProjectilePrefab);
        }

        private static void CreateBombProjectile()
        {
            hollowPurpleProjectilePrefab = _assetBundle.LoadAsset<GameObject>("HollowPurpleProjectile");
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            
            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");
            
            bombController.startSound = "";
        }

        private static void AddAbsorbProjectile()
        {
            hollowPurpleProjectilePrefab = _assetBundle.LoadAsset<GameObject>("HollowPurpleProjectile");
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want

            //remove their ProjectileImpactExplosion component and start from default values

            hollowPurpleProjectilePrefab.AddComponent<PrisonerMod.AbsorbProjectileComponent.SlowDownProjectiles>();

            ProjectileController hpController = hollowPurpleProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HollowPurpleGhost") != null)
                hpController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HollowPurpleGhost");

            hpController.startSound = "";
        }
        #endregion projectiles
    }
}
