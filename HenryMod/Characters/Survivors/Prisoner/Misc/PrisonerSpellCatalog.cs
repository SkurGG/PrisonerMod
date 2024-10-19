using EntityStates;
using PrisonerMod.Characters.Survivors.Prisoner;
using PrisonerMod.Modules;
using PrisonerMod.Characters.Survivors.Prisoner.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace PrisonerMod.Characters.Survivors.Prisoner.Misc
{
    public static class PrisonerSpellCatalog
    {
        public static Dictionary<string, PrisonerSpellDef> weaponDrops = new Dictionary<string, PrisonerSpellDef>();

        public static PrisonerSpellDef[] spellDefs = new PrisonerSpellDef[0];

        internal static PrisonerSpellDef EmptySpell; // we hatin'
        internal static PrisonerSpellDef Fireball; // arti's bomb but with less charge
        internal static PrisonerSpellDef Blink; //tp that resets
        internal static PrisonerSpellDef HollowPurple; // still charged attack. Gojo?
        internal static PrisonerSpellDef DamageBuff; //i dont know
        internal static PrisonerSpellDef Heal; //healing
        internal static PrisonerSpellDef DefenseDebuff; //armor shred
        internal static PrisonerSpellDef PersistentThing; //some kind of fire on floor or smth
        internal static PrisonerSpellDef AgileShoot; // like visions of heresy or som

        public static void AddSpell(PrisonerSpellDef spellDef)
        {
            Array.Resize(ref spellDefs, spellDefs.Length + 1);

            int index = spellDefs.Length - 1;
            spellDef.index = (ushort)index;

            spellDefs[index] = spellDef;
            spellDef.index = (ushort)index;

            Debug.Log("Added " + spellDef.nameToken + " to catalog with index: " + spellDef.index);
        }

        public static PrisonerSpellDef GetSpellFromIndex(int index)
        {
            return spellDefs.ElementAtOrDefault(index) ?? EmptySpell;
        }

        public static PrisonerSpellDef GetRandomSpell()
        {
            List<PrisonerSpellDef> validSpells = new List<PrisonerSpellDef>();

            for (int i = 0; i < spellDefs.Length; i++)
            {
                validSpells.Add(spellDefs[i]);
            }

            if (validSpells.Count <= 0) return EmptySpell; // emptySpell failsafe

            return validSpells[UnityEngine.Random.Range(0, validSpells.Count)];
        }
    }
}

