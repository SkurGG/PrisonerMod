﻿using BepInEx.Configuration;
using PrisonerMod.Characters.Survivors.Prisoner.Components;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells;
using PrisonerMod.Characters.Survivors.Prisoner.SkillStates.Spells.HollowPurple;
using PrisonerMod.Modules;
using PrisonerMod.Modules.Characters;
using PrisonerMod.Survivors.Prisoner.Components;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ShaderSwapper;

namespace PrisonerMod.Survivors.Prisoner
{
    public class PrisonerSurvivor : SurvivorBase<PrisonerSurvivor>
    {
        public const string PRISONER_PREFIX = "_PRISONER_";

        public override string assetBundleName => "prisonerbundle";
        public override string bodyName => "PrisonerBody"; 
        public override string masterName => "PrisonerMonsterMaster"; 
        public override string modelPrefabName => "mdlPrisoner";
        public override string displayPrefabName => "PrisonerDisplay";


        //skill or
        internal static SkillDef drawSkillDef;
        internal static SkillDef castSkillDef;
        internal static SkillDef deleteSkillDef;
        internal static SkillDef emptySpellSkillDef;

        //spelldefs
        internal static SkillDef fireballSkillDef;
        internal static SkillDef hollowPurpleSkillDef;
        internal static SkillDef healSkillDef;




        public override string survivorTokenPrefix => PRISONER_PREFIX;
        
        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = PRISONER_PREFIX + "NAME",
            subtitleNameToken = PRISONER_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("texHenryIcon"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 110f,
            healthRegen = 1.5f,
            armor = 0f,

            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "MeshA",
                    material = assetBundle.LoadMaterial("matPrisoner_Cape"),
                },
                new CustomRendererInfo
                {
                    childName = "MeshB",
                    material = assetBundle.LoadMaterial("matPrisoner_Body"),
                },
                new CustomRendererInfo
                {
                    childName = "MeshC",
                    material = assetBundle.LoadMaterial("matPrisoner_Body"),
                }
        };

        public override UnlockableDef characterUnlockableDef => PrisonerUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new PrisonerItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }
        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            PrisonerUnlockables.Init();

            base.InitializeCharacter();

            PrisonerConfig.Init();
            PrisonerStates.Init();
            PrisonerTokens.Init();

            PrisonerAssets.Init(assetBundle);
            PrisonerBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<PrisonerWeaponComponents>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            bodyPrefab.AddComponent<PrisonerController>();
            Skills.ClearGenericSkills(bodyPrefab);
            Modules.Skills.CreateSkillFamilies(bodyPrefab);

            CreateSkills(bodyPrefab);
            Skills.AddPrimarySkills(bodyPrefab, castSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, deleteSkillDef);
            Skills.AddUtilitySkills(bodyPrefab, emptySpellSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, drawSkillDef);

        }

        private void CreateSkills(GameObject prefab)
        {

            SkillLocator skillLocator = prefab.GetComponent<SkillLocator>();

            #region Misc
            PrisonerSurvivor.drawSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_DRAW_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_DRAW_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_DRAW_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Draw)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            PrisonerSurvivor.deleteSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_DELETE_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_DELETE_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_DELETE_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Delete)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            PrisonerSurvivor.castSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_CAST_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_CAST_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_CAST_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texCastIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Cast)),
                activationStateMachineName = "Weapon", interruptPriority = EntityStates.InterruptPriority.Any,
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });

            PrisonerSurvivor.emptySpellSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_EMPTYSPELL_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_EMPTYSPELL_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_EMPTYSPELL_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texCancelIcon"),
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Any,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
            });
            #endregion Misc

            #region Spells

            PrisonerSurvivor.fireballSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_FIREBALL_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_FIREBALL_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_FIREBALL_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ThrowHollow)),
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,

            });

            PrisonerSurvivor.hollowPurpleSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_HOLLOWPURPLE_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_HOLLOWPURPLE_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_HOLLOWPURPLE_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texBazookaIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(ChargeHollow)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,

            });

            PrisonerSurvivor.healSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = PRISONER_PREFIX + "_PRISONER_BODY_HEAL_NAME",
                skillNameToken = PRISONER_PREFIX + "_PRISONER_BODY_HEAL_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "_PRISONER_BODY_HEAL_DESC",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(Heal)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,
                baseMaxStock = 1,
                baseRechargeInterval = 0f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,

            });

            #endregion

            PrisonerMod.Assets.InitSpellDefs();

        }
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = PRISONER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_STUNNING",
                icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryPassive",
                skillNameToken = PRISONER_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }
        //private void AddPrimarySkills()
        //{
        //    Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

        //    //the primary skill is created using a constructor for a typical primary
        //    //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
        //    SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
        //        (
        //            "Cast",
        //            PRISONER_PREFIX + "PRIMARY_SLASH_NAME",
        //            PRISONER_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
        //            assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
        //            new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)),
        //            "Weapon",
        //            true
        //        ));
        //    //custom Skilldefs can have additional fields that you can set manually
        //    primarySkillDef1.stepCount = 2;
        //    primarySkillDef1.stepGraceDuration = 0.5f;

        //    Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);
        //}
        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Delete",
                skillNameToken = PRISONER_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Draw)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }
        //private void AddUtiitySkills()
        //{
        //    Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

        //    //here's a skilldef of a typical movement skill.
        //    SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
        //    {
        //        skillName = "HenryRoll",
        //        skillNameToken = PRISONER_PREFIX + "UTILITY_ROLL_NAME",
        //        skillDescriptionToken = PRISONER_PREFIX + "UTILITY_ROLL_DESCRIPTION",
        //        skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

        //        activationState = new EntityStates.SerializableEntityStateType(typeof(Roll)),
        //        activationStateMachineName = "Body",
        //        interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

        //        baseRechargeInterval = 4f,
        //        baseMaxStock = 1,

        //        rechargeStock = 1,
        //        requiredStock = 1,
        //        stockToConsume = 1,

        //        resetCooldownTimerOnUse = false,
        //        fullRestockOnAssign = true,
        //        dontAllowPastMaxStocks = false,
        //        mustKeyPress = false,
        //        beginSkillCooldownOnSkillEnd = false,

        //        isCombatSkill = false,
        //        canceledFromSprinting = false,
        //        cancelSprintingOnActivation = false,
        //        forceSprintDuringState = true,
        //    });

        //    Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);
        //}
        private void AddSpecialSkills()
        {

            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "Draw",
                skillNameToken = PRISONER_PREFIX + "SPECIAL_DRAW_NAME",
                skillDescriptionToken = PRISONER_PREFIX + "SPECIAL_DRAW_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(Draw)),
                activationStateMachineName = "Weapon", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
        }
        #endregion skills
        
        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
                //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
                //uncomment this when you have another skin
            //defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySword",
            //    "meshHenryGun",
            //    "meshHenry");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            
            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(HENRY_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);
            
            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            PrisonerAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(PrisonerBuffs.armorBuff))
            {
                args.armorAdd += 300;
            }
        }
    }
}