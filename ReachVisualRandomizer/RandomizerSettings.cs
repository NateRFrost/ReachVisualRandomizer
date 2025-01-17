using System;
using System.Runtime.CompilerServices;

namespace ReachVisualRandomizer
{
    public class RandomizerSettings
    {
        public int Seed { get; set; } = 2;
        public string EkPath { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\HREK";
        public string MCCPath { get; set; } = @"C:\Program Files (x86)\Steam\steamapps\common\Halo The Master Chief Collection";
        
        
        public bool RandomizeCutscenes { get; set; } = true;
        public bool RandomizeWeapons { get; set; } = true;
        public bool RandomizeEquipments { get; set; } = true;
        public bool RandomizeVehicles { get; set; } = true;
        public bool RandomizeEnvironmentalObjects { get; set; } = true;
        public bool RandomizeWeaponStashTypes { get; set; } = false;

        public bool RandomizeStartingProfiles { get; set; } = true;
        public bool RandomizeSquads { get; set; } = true;
        public float GiveVehicleChance { get; set; } = 0.02f;
        public float MakeMuleChance { get; set; } = 0.01f;
        public float MakeEngineerChance { get; set; } = 0.01f;
        public float MakeHunterChance { get; set; } = 0.03f;
        public int CharactersPerCell { get; set; } = 2;
        public int WeaponsPerCell { get; set; } = 2;

        //public bool SquadCharactersFactionUnchanged { get; set; } = true;
        //public bool SquadWeaponsFactionUnchanged { get; set; } = true;
        //public bool SquadVehiclesFactionUnchanged { get; set; } = true;
    }
   
}