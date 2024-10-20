using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using PrisonerMod.Modules;


[CreateAssetMenu(fileName = "spl", menuName = "ScriptableObjects/SpellDef", order = 1)]
public class PrisonerSpellDef : ScriptableObject
{
    public enum AnimationSet // i don't even know what an enum is
    {
        Default,
        TwoHanded,
        Dash
    }

    [Header("General")]
    public string nameToken = "";
    public string descriptionToken = "";
    public Texture icon = null;
    public GameObject crosshairPrefab = null;

    [Header("Skills")]
    public SkillDef spellSkillDef;

    [Header("Visuals")]
    public Mesh mesh;
    public Material material;
    public AnimationSet animationSet = AnimationSet.Default;
    public string calloutSoundString = "";

    [Header("Other")]
    public string configIdentifier = "";
    public float dropChance = 0f;

    [HideInInspector]
    public ushort index; // assigned at runtime
    [HideInInspector]
    public GameObject pickupPrefab; // same thing

    public static PrisonerSpellDef CreateSpellDefFromInfo(SpellDefInfo spellDefInfo)
    {
        PrisonerSpellDef spellDef = (PrisonerSpellDef)ScriptableObject.CreateInstance(typeof(PrisonerSpellDef));
        spellDef.name = spellDefInfo.nameToken;

        spellDef.nameToken = spellDefInfo.nameToken;
        spellDef.descriptionToken = spellDefInfo.descriptionToken;
        spellDef.icon = spellDefInfo.icon;
        spellDef.crosshairPrefab = spellDefInfo.crosshairPrefab;

        spellDef.spellSkillDef = spellDefInfo.spellSkillDef;

        spellDef.mesh = spellDefInfo.mesh;
        spellDef.material = spellDefInfo.material;
        spellDef.animationSet = spellDefInfo.animationSet;
        spellDef.calloutSoundString = spellDefInfo.calloutSoundString;

        spellDef.configIdentifier = spellDefInfo.configIdentifier;

        return spellDef;
    }
}

[System.Serializable]
public struct SpellDefInfo
{
    public string nameToken;
    public string descriptionToken;
    public Texture icon;
    public GameObject crosshairPrefab;

    public SkillDef spellSkillDef;

    public Mesh mesh;
    public Material material;
    public PrisonerSpellDef.AnimationSet animationSet;
    public string calloutSoundString;

    public string configIdentifier;
}


