using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using PrisonerMod.Characters.Survivors.Prisoner.Misc;
using PrisonerMod.Survivors.Prisoner;
using RoR2;
using RoR2.Skills;
using RoR2.Stats;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


namespace PrisonerMod.Characters.Survivors.Prisoner.Components
{
    public class PrisonerController : MonoBehaviour
    {
        public PrisonerSpellDef spellDef { get; private set; }
        public PrisonerSpellDef defaultWeaponDef { get; private set; }

        public float chargeValue;
        private CharacterBody characterBody;
        private ModelSkinController skinController;
        private ChildLocator childLocator;
        private CharacterModel characterModel;
        private Animator animator;
        private SkillLocator skillLocator;

        private float comboDecay = 1f;

        public SkinnedMeshRenderer weaponRenderer;

        public readonly float upForce = 9f;
        public readonly float backForce = 2.4f;

        // ooooAAAAAUGHHHHHGAHEM,67TKM
        private SkillDef[] spellSkillOverrides;

        public GameObject crosshairPrefab;

        private PrisonerSpellDef lastWeaponDef;

        public PrisonerSpellDef[] spellSlots = new PrisonerSpellDef[4];

        private void Awake()
        {

            this.characterBody = this.GetComponent<CharacterBody>();
            this.skillLocator = this.GetComponent<SkillLocator>();

            for (int i = 0; i < spellSlots.Length; i++)
            {
                this.spellSlots[i] = PrisonerSpellCatalog.EmptySpell;
            }

            this.GetSkillOverrides();

        }


        private void GetSkillOverrides()
        {
            List<SkillDef> spells = new List<SkillDef>();

            for (int i = 0; i < PrisonerSpellCatalog.spellDefs.Length; i++)
            {
                if (PrisonerSpellCatalog.spellDefs[i])
                {
                    if (PrisonerSpellCatalog.spellDefs[i].spellSkillDef) spells.Add(PrisonerSpellCatalog.spellDefs[i].spellSkillDef);
                }
            }

            this.spellSkillOverrides = spells.ToArray();
        }

        public void DrawSpell(GenericSkill skillslot, int skillNumber)
        {
            var spell = PrisonerSpellCatalog.GetRandomSpell();
            spellSlots[skillNumber] = spell;

            // skillslot.SetSkillOverride(skillslot, spell.spellSkillDef, GenericSkill.SkillOverridePriority.Contextual); 

            //for (int i = 0; i < this.spellSkillOverrides.Length; i++)
            //{
            //    if (this.spellSkillOverrides[i])
            //    {
            //        skillslot.UnsetSkillOverride(skillslot, this.spellSkillOverrides[i], GenericSkill.SkillOverridePriority.Contextual);
            //    }

            //}
        }

        public void SetSkills()
        {
            for (int i = 0; i < spellSlots.Length; i++)
            {
                if (spellSlots[i] != PrisonerSpellCatalog.EmptySpell)
                {
                    this.skillLocator.allSkills[i].SetSkillOverride(this, spellSlots[i].spellSkillDef, GenericSkill.SkillOverridePriority.Network);
                }
                else
                {
                    this.skillLocator.allSkills[i].SetSkillOverride(this, PrisonerSurvivor.emptySpellSkillDef, GenericSkill.SkillOverridePriority.Network);
                }
            }
        }

        public void UnsetSkills()
        {
            for (int i = 0; i < spellSlots.Length; i++)
            {
                if (spellSlots[i] != PrisonerSpellCatalog.EmptySpell)
                {
                    this.skillLocator.allSkills[i].UnsetSkillOverride(this, spellSlots[i].spellSkillDef, GenericSkill.SkillOverridePriority.Network);
                }
                else
                {
                    this.skillLocator.allSkills[i].UnsetSkillOverride(this, PrisonerSurvivor.emptySpellSkillDef, GenericSkill.SkillOverridePriority.Network);
                }
            }
        }
    }
}
