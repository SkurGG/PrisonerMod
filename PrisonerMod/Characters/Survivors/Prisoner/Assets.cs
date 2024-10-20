using System.Reflection;
using R2API;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using System.IO;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using TMPro;
using RoR2.UI;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using PrisonerMod.Characters.Survivors.Prisoner.Misc;
using PrisonerMod.Characters.Survivors;
using PrisonerMod.Survivors.Prisoner;

namespace PrisonerMod
{
    public static class Assets
    {
        public static AssetBundle mainAssetBundle;

        internal static Shader hopoo = Resources.Load<Shader>("Shaders/Deferred/HGStandard");

        public static GameObject defaultCrosshairPrefab;

        internal static PrisonerSpellDef fireballSpellDef;
        internal static PrisonerSpellDef hollowPurpleSpellDef;

        internal static void PopulateAssets()
        {
            using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(""))
            {
                mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
            }
            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream(""))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
            

            defaultCrosshairPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageCrosshair.prefab").WaitForCompletion().InstantiateClone("PrisonerDefaultCrosshair", false);
            //if (!Modules.Config.enableCrosshairDot.Value) defaultCrosshairPrefab.GetComponent<RawImage>().enabled = false;
            //if (dynamicCrosshair) defaultCrosshairPrefab.AddComponent<DynamicCrosshair>();
        }
        

        internal static void InitSpellDefs()
        {
            fireballSpellDef = PrisonerSpellDef.CreateSpellDefFromInfo(new SpellDefInfo
            {
                nameToken = "PRISONER_FIREBALL_NAME",
                descriptionToken = "PRISONER_FIREBALL_DESC",
                //icon = Assets.pistolWeaponIcon,
                crosshairPrefab = Assets.defaultCrosshairPrefab,
                spellSkillDef = PrisonerSurvivor.fireballSkillDef,
                // mesh = Assets.pistolMesh,
                // material = Assets.pistolMat,
                // animationSet = DriverWeaponDef.AnimationSet.Default
            });
            PrisonerSpellCatalog.AddSpell(fireballSpellDef);
            PrisonerSpellCatalog.Fireball = fireballSpellDef;

            hollowPurpleSpellDef = PrisonerSpellDef.CreateSpellDefFromInfo(new SpellDefInfo
            {
                nameToken = "PRISONER_HOLLOWPURPLE_NAME",
                descriptionToken = "PRISONER_HOLLOWPURPLE_DESC",
                //icon = Assets.pistolWeaponIcon,
                crosshairPrefab = Assets.defaultCrosshairPrefab,
                spellSkillDef = PrisonerSurvivor.hollowPurpleSkillDef,
                // mesh = Assets.pistolMesh,
                // material = Assets.pistolMat,
                // animationSet = DriverWeaponDef.AnimationSet.Default
            });
            PrisonerSpellCatalog.AddSpell(hollowPurpleSpellDef);
            PrisonerSpellCatalog.Fireball = hollowPurpleSpellDef;
        }
        
    }
}
