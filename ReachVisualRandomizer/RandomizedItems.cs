using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ReachVisualRandomizer
{

    public static class RandomizedItems
    {

        public class Level
        {
            public required string Name { get; set; }
            public string FancyName { get; set; } = "";
            public bool CutsceneOnly { get; set; } = false;
            public bool Randomize { get; set; } = true;
        }

        public class RandomizedObjectList
        {
            public string Name { get; set; } = "";
            public List<RandomizedObjectDetails> List { get; set; } = new List<RandomizedObjectDetails>();
            public string Type { get; set; } = "";
            public string ResourceFileType { get; set; } = "";
            public string ResourceFileExtension { get; set; } = "";
            public string PaletteName { get; set; } = "";

            public RandomizedObjectList FilterName(string name)
            {
                RandomizedObjectList new_list = (RandomizedObjectList)this.MemberwiseClone();
                new_list.List = this.List.Where(o => o.Name.Contains(name)).ToList();
                return new_list;
            }


            public RandomizedObjectList DeepCopy()
            {
                RandomizedObjectList new_list = (RandomizedObjectList)this.MemberwiseClone();
                new_list.List = new List<RandomizedObjectDetails>();
                foreach (RandomizedObjectDetails item in List)
                {
                    new_list.List.Add(item.ShallowCopy());
                }
                return new_list;
            }

            public void ResetIndexes()
            {
                foreach (var object_type in List)
                {
                    object_type.PaletteIndex = -1;
                }
            }

            public RandomizedObjectDetails? GetRandomObjectWeighted(Random rand, List<SubCategory>? subcategories = null, List<Faction>? factions = null, List<string>? names = null, List<string>? compatible_character_names = null, bool require_palette_index = false)
            {
                var filtered_list = List.ToList();
                if (subcategories != null)
                {
                    filtered_list = filtered_list.Where(o => subcategories.Contains(o.SubCategory)).ToList();
                }
                if (factions != null)
                {
                    filtered_list = filtered_list.Where(o => factions.Contains(o.Faction)).ToList();
                }
                if (names != null)
                {
                    filtered_list = filtered_list.Where(o => names.Any(o2 => o2.Contains(o.Name))).ToList();
                }
                if (compatible_character_names != null)
                {
                    filtered_list = filtered_list.Where(weapon => compatible_character_names.All(character_name => weapon.CompatibleEnemies.List.Any(character => character_name == character.Name))).ToList();
                }
                if (require_palette_index)
                {
                    filtered_list = filtered_list.Where(o => o.PaletteIndex != -1).ToList();
                }



                int total_weight = 0;
                foreach (RandomizedObjectDetails item in filtered_list)
                {
                    total_weight += item.Weight;
                }
                int random_number = rand.Next(0, total_weight);
                foreach (RandomizedObjectDetails item in filtered_list)
                {
                    if (random_number <= item.Weight)
                    {
                        return item;
                    }
                    random_number -= item.Weight;
                }
                return null;
            }
        }

        public enum SubCategory
        {
            None,
            Air,
            Land,
            Space,
            Turret,
            Civilian,
            WeaponStash,
            Weak,
            Medium,
            Strong,
            Specialist,
            Grenade,
            ArmorAbility,

        }

        public enum Faction
        {
            None,
            Human,
            Covenant,
            Forerunner,
            Flood,
            Promethean,
            Mule,
        }

        public class RandomizedObjectDetails
        {
            public string Name { get; set; } = "";
            public string Path { get; set; } = @"";
            public int AmmoMag { get; set; } = 0;
            public int AmmoTotal { get; set; } = 0;
            public int Seats { get; set; } = 1;
            public string Subtitle { get; set; } = "";

            public int PaletteIndex { get; set; } = -1;
            public int Weight { get; set; } = 10;
            public SubCategory SubCategory { get; set; }
            public Faction Faction { get; set; }

            public RandomizedObjectList CompatibleEnemies { get; set; } = new RandomizedObjectList();
            public RandomizedObjectList Variants { get; set; } = new RandomizedObjectList();

            public RandomizedObjectDetails ShallowCopy()
            {
                return (RandomizedObjectDetails)this.MemberwiseClone();
            }

        }


        public static List<Level> Levels = new List<Level>()
        {
            new Level{Name = "m05", FancyName = "Noble Actual", CutsceneOnly = true},
            new Level{Name = "m10", FancyName = "Winter Contingency"},
            new Level{Name = "m20", FancyName = "Oni: Sword Base"},
            new Level{Name = "m30", FancyName = "Nightfall"},
            new Level{Name = "m35", FancyName = "On the Tip of the Spear"},
            new Level{Name = "m45", FancyName = "Long Night of Solace"},
            new Level{Name = "m50", FancyName = "Exodus"},
            new Level{Name = "m52", FancyName = "New Alexandria"},
            new Level{Name = "m60", FancyName = "The Package"},
            new Level{Name = "m70", FancyName = "The Pillar of Autumn"},
            new Level{Name = "m70_a", FancyName = "Epilogue", CutsceneOnly = true },
            new Level{Name = "m70_bonus", FancyName = "Lone Wolf"},
        };

        public static RandomizedObjectList characters = new RandomizedObjectList()
        {
            Name = "characters",
            Type = "char",
            PaletteName = "character palette",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails{Name = "mule", Path =  @"objects\characters\mule\ai\mule", Faction = Faction.Mule, Weight = 1},
                new RandomizedObjectDetails{Name = "brute", Path =  @"objects\characters\brute\ai\brute", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "brute_captain", Path =   @"objects\characters\brute\ai\brute_captain", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "brute_chieftain_armor",  Path =  @"objects\characters\brute\ai\brute_chieftain_armor", Weight = 1, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "brute_chieftain_weapon", Path =   @"objects\characters\brute\ai\brute_chieftain_weapon", Weight = 1, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "grunt", Path = @"objects\characters\grunt\ai\grunt", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "grunt_heavy", Path =   @"objects\characters\grunt\ai\grunt_heavy", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "grunt_major", Path =   @"objects\characters\grunt\ai\grunt_major", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "grunt_specops", Path =   @"objects\characters\grunt\ai\grunt_specops", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "grunt_ultra", Path = @"objects\characters\grunt\ai\grunt_ultra", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "jackal_sniper", Path =  @"objects\characters\jackal\ai\jackal_sniper", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "jackal", Path =   @"objects\characters\jackal\ai\jackal", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "jackal_major", Path =   @"objects\characters\jackal\ai\jackal_major", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "skirmisher", Path =   @"objects\characters\jackal\ai\skirmisher", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "skirmisher_champion", Path = @"objects\characters\jackal\ai\skirmisher_champion", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "skirmisher_commando", Path = @"objects\characters\jackal\ai\skirmisher_commando", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "skirmisher_major", Path = @"objects\characters\jackal\ai\skirmisher_major", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "skirmisher_murmillone", Path = @"objects\characters\jackal\ai\skirmisher_murmillone", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite", Path = @"objects\characters\elite\ai\elite", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_general", Path =   @"objects\characters\elite\ai\elite_general", Weight = 2, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_jetpack",  Path =  @"objects\characters\elite\ai\elite_jetpack", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_officer", Path =   @"objects\characters\elite\ai\elite_officer", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_specops", Path =   @"objects\characters\elite\ai\elite_specops", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_story", Path =   @"objects\characters\elite\ai\elite_story", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_story_unique", Path =   @"objects\characters\elite\ai\elite_story_unique", Weight = 2, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_bob", Path =   @"objects\characters\elite\ai\named\elite_bob", Weight = 1, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "elite_ultra", Path =   @"objects\characters\elite\ai\elite_ultra", Weight = 5, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "hunter", Path =   @"objects\characters\hunter\ai\hunter", Weight = 1, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "engineer",  Path =  @"objects\characters\engineer\ai\engineer", Weight = 2, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "bugger",  Path =  @"objects\characters\bugger\ai\bugger", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "bugger_captain", Path =   @"objects\characters\bugger\ai\bugger_captain", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "bugger_captain_major", Path =   @"objects\characters\bugger\ai\bugger_captain_major", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "bugger_major",  Path =  @"objects\characters\bugger\ai\bugger_major", Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "bugger_ultra",  Path =  @"objects\characters\bugger\ai\bugger_ultra", Faction = Faction.Covenant},
            }
        };

        public static RandomizedObjectList weapons = new RandomizedObjectList()
        {
            Name = "weapons",
            Type = "weap",
            ResourceFileType = "*eap",
            ResourceFileExtension = "scenario_weapons_resource",
            PaletteName = "weapon palette",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails{Name = "energy_sword", Path = @"objects\weapons\melee\energy_sword\energy_sword", CompatibleEnemies = characters.FilterName("elite") , Faction = Faction.Covenant, Weight = 5},
                new RandomizedObjectDetails{Name = "gravity_hammer", Path = @"objects\weapons\melee\gravity_hammer\gravity_hammer", CompatibleEnemies = characters.FilterName("brute"), Faction = Faction.Covenant, Weight = 5},
                //new RandomizedObjectDetails{Name = "jackal_shield", Path = @"objects\weapons\melee\jackal_shield\jackal_shield" },
                //new RandomizedObjectDetails{Name = "skirmisher_bracers", Path = @"objects\weapons\melee\skirmisher_bracers\skirmisher_bracers" },
                //new RandomizedObjectDetails{Name = "unarmed", Path = @"objects\weapons\melee\unarmed\unarmed", CompatibleEnemies = characters.List.Where(o => o.Name.Contains("engineer")).ToList() },
                new RandomizedObjectDetails{Name = "magnum", Path = @"objects\weapons\pistol\magnum\magnum", AmmoMag = 8, AmmoTotal = 24, CompatibleEnemies = characters, Faction = Faction.Human, Weight = 20},
                new RandomizedObjectDetails{Name = "needler", Path = @"objects\weapons\pistol\needler\needler", AmmoMag = 24, AmmoTotal = 72, CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "plasma_pistol", Path = @"objects\weapons\pistol\plasma_pistol\plasma_pistol", CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "assault_rifle", Path = @"objects\weapons\rifle\assault_rifle\assault_rifle", AmmoMag = 32, AmmoTotal = 160, CompatibleEnemies = characters, Faction = Faction.Human, Weight = 20},
                new RandomizedObjectDetails{Name = "concussion_rifle", Path = @"objects\weapons\rifle\concussion_rifle\concussion_rifle", AmmoMag = 6, AmmoTotal = 18, CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "dmr", Path = @"objects\weapons\rifle\dmr\dmr", AmmoMag = 15, AmmoTotal = 45, CompatibleEnemies = characters, Weight = 14, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "focus_rifle", Path = @"objects\weapons\rifle\focus_rifle\focus_rifle", CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "grenade_launcher", Path = @"objects\weapons\rifle\grenade_launcher\grenade_launcher", AmmoMag = 1, AmmoTotal = 16, CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "needle_rifle", Path = @"objects\weapons\rifle\needle_rifle\needle_rifle", AmmoMag = 21, AmmoTotal = 63, CompatibleEnemies = characters, Weight = 14, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "plasma_repeater", Path = @"objects\weapons\rifle\plasma_repeater\plasma_repeater", CompatibleEnemies = characters, Weight = 20, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "plasma_rifle", Path = @"objects\weapons\rifle\plasma_rifle\plasma_rifle", CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "shotgun", Path = @"objects\weapons\rifle\shotgun\shotgun", AmmoMag = 6, AmmoTotal = 18, CompatibleEnemies = characters, Faction = Faction.Human, Weight = 20},
                new RandomizedObjectDetails{Name = "sniper_rifle", Path = @"objects\weapons\rifle\sniper_rifle\sniper_rifle", AmmoMag = 4, AmmoTotal = 12, CompatibleEnemies = characters, Weight = 8, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "spike_rifle", Path = @"objects\weapons\rifle\spike_rifle\spike_rifle", AmmoMag = 40, AmmoTotal = 120, CompatibleEnemies = characters, Faction = Faction.Covenant, Weight = 20},
                new RandomizedObjectDetails{Name = "flak_cannon", Path = @"objects\weapons\support_high\flak_cannon\flak_cannon", AmmoMag = 5, AmmoTotal = 20, CompatibleEnemies = characters, Weight = 8, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "plasma_launcher", Path = @"objects\weapons\support_high\plasma_launcher\plasma_launcher", CompatibleEnemies = characters, Weight = 8, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "rocket_launcher", Path = @"objects\weapons\support_high\rocket_launcher\rocket_launcher", AmmoMag = 2, AmmoTotal = 8, CompatibleEnemies = characters, Weight = 8, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "spartan_laser", Path = @"objects\weapons\support_high\spartan_laser\spartan_laser", CompatibleEnemies = characters, Weight = 8, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "machinegun_turret_jorge", Path = @"objects\weapons\turret\machinegun_turret_jorge\machinegun_turret_jorge", AmmoMag = 200, AmmoTotal = 200, CompatibleEnemies = characters, Weight = 4, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "plasma_turret", Path = @"objects\vehicles\covenant\turrets\plasma_turret\weapon\plasma_turret\plasma_turret", AmmoMag = 200, AmmoTotal = 200, CompatibleEnemies = characters, Weight = 4, Faction = Faction.Covenant},
                new RandomizedObjectDetails{Name = "machinegun_turret", Path = @"objects\vehicles\human\turrets\machinegun\weapon\machinegun_turret\machinegun_turret", AmmoMag = 200, AmmoTotal = 200, CompatibleEnemies = characters, Weight = 4, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "target_laser", Path = @"objects\weapons\pistol\target_laser\target_laser", CompatibleEnemies = characters, Weight = 1, Faction = Faction.Human},
                new RandomizedObjectDetails{Name = "flag", Path = @"objects\weapons\multiplayer\flag\flag", CompatibleEnemies = characters, Weight = 1},
                new RandomizedObjectDetails{Name = "skull", Path = @"objects\weapons\multiplayer\skull\skull", CompatibleEnemies = characters, Weight = 1},
                new RandomizedObjectDetails{Name = "golf_club", Path = @"objects\levels\shared\golf_club\golf_club", CompatibleEnemies = characters.FilterName("brute"), Weight = 1},
            }
        };

        public static RandomizedObjectList equipments = new RandomizedObjectList()
        {
            Name = "equipments",
            Type = "eqip",
            ResourceFileType = "*qip",
            ResourceFileExtension = "scenario_equipment_resource",
            PaletteName = "equipment palette",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails{Name = "active_camouflage", Path = @"objects\equipment\active_camouflage\active_camouflage", SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "armor_lockup", Path = @"objects\equipment\armor_lockup\armor_lockup", SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "drop_shield", Path = @"objects\equipment\drop_shield\drop_shield", SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "evade", Path = @"objects\equipment\evade\evade", SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "hologram", Path = @"objects\equipment\hologram\hologram", SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "jet_pack", Path = @"objects\equipment\jet_pack\jet_pack", Weight = 5, SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "sprint", Path = @"objects\equipment\sprint\sprint", SubCategory = SubCategory.ArmorAbility},
                new RandomizedObjectDetails{Name = "frag_grenade", Path = @"objects\weapons\grenade\frag_grenade\frag_grenade", SubCategory = SubCategory.Grenade, Weight = 70},
                new RandomizedObjectDetails{Name = "plasma_grenade", Path = @"objects\weapons\grenade\plasma_grenade\plasma_grenade", SubCategory = SubCategory.Grenade, Weight = 70},
                new RandomizedObjectDetails{Name = "rocket_launcher_rocket", Path = @"objects\weapons\support_high\rocket_launcher\projectiles\rocket_launcher_rocket", Weight = 3},
                new RandomizedObjectDetails{Name = "rocket_launcher_ammo", Path = @"objects\gear\human\military\rocket_launcher_ammo\rocket_launcher_ammo", Weight = 3},
                new RandomizedObjectDetails{Name = "sniper_rifle_ammo", Path = @"objects\gear\human\military\sniper_rifle_ammo\sniper_rifle_ammo", Weight = 3},
                //new RandomizedObjectDetails{Name = "health_pack", Path = @"objects\equipment\health_pack\health_pack"},
                //new RandomizedObjectDetails{Name = "powerup_blue", Path = @"objects\multi\powerups\powerup_blue\powerup_blue", Weight = 1},
                //new RandomizedObjectDetails{Name = "powerup_red", Path = @"objects\multi\powerups\powerup_red\powerup_red", Weight = 1},
                //new RandomizedObjectDetails{Name = "powerup_yellow", Path = @"objects\multi\powerups\powerup_yellow\powerup_yellow", Weight = 1},
                new RandomizedObjectDetails{Name = "ammo_box", Path = @"objects\gear\human\military\ammo_box\ammo_box", Weight = 1},
            }
        };

        public static RandomizedObjectList vehicles = new RandomizedObjectList()
        {
            Name = "vehicles",
            Type = "vehi",
            ResourceFileType = "*ehi",
            ResourceFileExtension = "scenario_vehicles_resource",
            PaletteName = "vehicle palette",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails{Name = "banshee", Path =  @"objects\vehicles\covenant\banshee\banshee", Weight = 4, Faction = Faction.Covenant, SubCategory = SubCategory.Air},
                new RandomizedObjectDetails{Name = "seraph", Path =  @"objects\vehicles\covenant\seraph\seraph", Weight = 1, Faction = Faction.Covenant, SubCategory = SubCategory.Space},
                new RandomizedObjectDetails{Name = "space_banshee", Path =  @"objects\vehicles\covenant\space_banshee\space_banshee", Weight = 1, Faction = Faction.Covenant, SubCategory = SubCategory.Space},
                //new RandomizedObjectDetails{Name = "space_phantom", Path =  @"objects\vehicles\covenant\space_phantom\space_phantom", Weight = 2, Faction = Faction.Covenant, SubCategory = SubCategory.Space, Seats = 5},
                new RandomizedObjectDetails{Name = "sabre", Path =  @"objects\vehicles\human\sabre\sabre", Weight = 1, Faction = Faction.Human, SubCategory = SubCategory.Space},
                new RandomizedObjectDetails{Name = "ghost",  Path = @"objects\vehicles\covenant\ghost\ghost" , Faction = Faction.Covenant, SubCategory = SubCategory.Land},
                new RandomizedObjectDetails{Name = "plasma_turret_mounted",  Path = @"objects\vehicles\covenant\turrets\plasma_turret\plasma_turret_mounted" , Faction = Faction.Covenant, SubCategory = SubCategory.Turret},
                new RandomizedObjectDetails{Name = "machinegun",  Path = @"objects\vehicles\human\turrets\machinegun\machinegun" , Faction = Faction.Human, SubCategory = SubCategory.Turret},
                new RandomizedObjectDetails{Name = "revenant", Path =  @"objects\vehicles\covenant\revenant\revenant", Seats = 2, Faction = Faction.Covenant, SubCategory = SubCategory.Land},
                new RandomizedObjectDetails{Name = "wraith",  Path = @"objects\vehicles\covenant\wraith\wraith", Seats = 2, Weight = 4, Faction = Faction.Covenant, SubCategory = SubCategory.Land},
                new RandomizedObjectDetails{Name = "mongoose",  Path = @"objects\vehicles\human\mongoose\mongoose" , Seats = 2, Faction = Faction.Human, SubCategory = SubCategory.Land},
                new RandomizedObjectDetails{Name = "scorpion",  Path = @"objects\vehicles\human\scorpion\scorpion",Seats = 2, Weight = 4, Faction = Faction.Human, SubCategory = SubCategory.Land},
                new RandomizedObjectDetails{Name = "cargo_truck",  Path = @"objects\vehicles\human\civilian\cargo_truck\cargo_truck" , Faction = Faction.Human, SubCategory = SubCategory.Civilian, Weight = 1},
                new RandomizedObjectDetails{Name = "cart_electric",  Path = @"objects\vehicles\human\civilian\cart_electric\cart_electric" , Faction = Faction.Human, SubCategory = SubCategory.Civilian, Weight = 1},
                new RandomizedObjectDetails{Name = "forklift",  Path = @"objects\vehicles\human\civilian\forklift\forklift" , Faction = Faction.Human, SubCategory = SubCategory.Civilian, Weight = 1},
                new RandomizedObjectDetails{Name = "oni_van",  Path = @"objects\vehicles\human\civilian\oni_van\oni_van" , Faction = Faction.Human, SubCategory = SubCategory.Civilian, Weight = 1},
                new RandomizedObjectDetails{Name = "pickup",  Path = @"objects\vehicles\human\civilian\pickup\pickup" , Faction = Faction.Human, SubCategory = SubCategory.Civilian, Weight = 1},
                new RandomizedObjectDetails
                {
                    Name = "military_truck",
                    Path = @"objects\vehicles\human\civilian\military_truck\military_truck" ,
                    Faction = Faction.Human,
                    SubCategory = SubCategory.Civilian,
                    Weight = 1,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "cargo"},
                            new RandomizedObjectDetails{Name = "container" },
                            new RandomizedObjectDetails{Name = "container_small" },
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "truck_cab_large",
                    Path = @"objects\vehicles\human\civilian\truck_cab_large\truck_cab_large" ,
                    Faction = Faction.Human,
                    SubCategory = SubCategory.Civilian,
                    Weight = 1,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "bed_long"},
                            new RandomizedObjectDetails{Name = "airdam_large" },
                            new RandomizedObjectDetails{Name = "airdam_small" },
                            new RandomizedObjectDetails{Name = "bed_long_container" },
                            new RandomizedObjectDetails{Name = "bed_long_tanker" },
                            new RandomizedObjectDetails{Name = "bed_small" },
                            new RandomizedObjectDetails{Name = "container_small" },
                            new RandomizedObjectDetails{Name = "tanker_small" },
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "warthog",
                    Path = @"objects\vehicles\human\warthog\warthog" ,
                    Seats = 3,
                    Faction = Faction.Human,
                    SubCategory = SubCategory.Land,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "gauss"},
                            new RandomizedObjectDetails{Name = "troop" },
                            new RandomizedObjectDetails{Name = "rocket" },
                        }
                    }
                },
                new RandomizedObjectDetails{
                    Name = "falcon",
                    Path = @"objects\vehicles\human\falcon\falcon",
                    Seats = 3,
                    Weight = 4,
                    Faction = Faction.Human,
                    SubCategory = SubCategory.Air,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "no_sideguns"},
                            new RandomizedObjectDetails{Name = "grenade" },
                            new RandomizedObjectDetails{Name = "multiplayer" },
                            new RandomizedObjectDetails{Name = "no_guns" },
                            new RandomizedObjectDetails{Name = "canopy_opaque"},
                            new RandomizedObjectDetails{Name = "no_sideguns_opaque" },
                            new RandomizedObjectDetails{Name = "grenade_opaque"},
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "shade",
                    Path = @"objects\vehicles\covenant\turrets\shade\shade" ,
                    Faction = Faction.Covenant,
                    SubCategory = SubCategory.Turret,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "plasma_cannon"},
                            new RandomizedObjectDetails{Name = "flak_cannon"},
                            new RandomizedObjectDetails{Name = "auto" },
                        }
                    }
                },
            }
        };

        public static RandomizedObjectList scenerys = new RandomizedObjectList()
        {
            Name = "scenerys",
            ResourceFileType = "*cen",
            ResourceFileExtension = "scenario_scenery_resource",
            PaletteName = "scenery palette",
            Type = "scen",
            List = new List<RandomizedObjectDetails>()
            {
                /*
                new RandomizedObjectDetails
                {
                    Name = "health_cabinet",
                    Path = @"objects\props\human\unsc\health_cabinet\health_cabinet",
                    Faction = Faction.Human,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "campaign"},
                        }
                    }
                },
                */
                new RandomizedObjectDetails
                {
                    Name = "weapon_rack",
                    Path = @"objects\gear\human\military\weapon_rack\weapon_rack",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "empty"},
                            new RandomizedObjectDetails{Name = "heavy_weapons"},
                            new RandomizedObjectDetails{Name = "armor_locks" },
                            new RandomizedObjectDetails{Name = "jetpack" },
                            new RandomizedObjectDetails{Name = "ar" },
                            new RandomizedObjectDetails{Name = "ar_frags"},
                            new RandomizedObjectDetails{Name = "br" },
                            new RandomizedObjectDetails{Name = "br_frags"},
                            new RandomizedObjectDetails{Name = "shotgun"},
                            new RandomizedObjectDetails{Name = "shotgun_frags"},
                            new RandomizedObjectDetails{Name = "drop_shields"},
                            new RandomizedObjectDetails{Name = "holograms"},
                            new RandomizedObjectDetails{Name = "jetpacks"},
                            new RandomizedObjectDetails{Name = "active_camo"},
                            new RandomizedObjectDetails{Name = "sprint"},
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "equipment_case",
                    Path = @"objects\gear\human\military\equipment_case\equipment_case",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "jetpack_2"},
                            new RandomizedObjectDetails{Name = "drop_lockup"},
                            new RandomizedObjectDetails{Name = "sprint_hologram"},
                            new RandomizedObjectDetails{Name = "drop_hologram" },
                            new RandomizedObjectDetails{Name = "sprint_lockup" },
                            new RandomizedObjectDetails{Name = "drop_camo" },
                            new RandomizedObjectDetails{Name = "lock_hologram" },
                            new RandomizedObjectDetails{Name = "empty" },
                            new RandomizedObjectDetails{Name = "lockup" },
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "armory_shelf",
                    Path = @"objects\gear\human\military\armory_shelf\armory_shelf",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "empty"},
                            new RandomizedObjectDetails{Name = "ar" },
                            new RandomizedObjectDetails{Name = "ar01" },
                            new RandomizedObjectDetails{Name = "ar02" },
                            new RandomizedObjectDetails{Name = "br" },
                            new RandomizedObjectDetails{Name = "br01" },
                            new RandomizedObjectDetails{Name = "br02" },
                            new RandomizedObjectDetails{Name = "shot" },
                            new RandomizedObjectDetails{Name = "shot01" },
                            new RandomizedObjectDetails{Name = "shot02" },
                            new RandomizedObjectDetails{Name = "closed" },
                            new RandomizedObjectDetails{Name = "shot_dmr_rl"},
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "armory_shelf_small",
                    Path = @"objects\gear\human\military\armory_shelf_small\armory_shelf_small",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "4ar_no_grenade" },
                            new RandomizedObjectDetails{Name = "4ar_grenade" },
                            new RandomizedObjectDetails{Name = "4pistol_no_grenade" },
                            new RandomizedObjectDetails{Name = "4pistol_grenade" },
                            new RandomizedObjectDetails{Name = "2ar_no_grenade" },
                            new RandomizedObjectDetails{Name = "2ar_grenade" },
                            new RandomizedObjectDetails{Name = "2pistol_no_grenade" },
                            new RandomizedObjectDetails{Name = "2pistol_grenade" },
                            new RandomizedObjectDetails{Name = "empty" },
                            new RandomizedObjectDetails{Name = "2br_grenade" },
                        }
                    }
                },

            }
        };
        public static RandomizedObjectList crates = new RandomizedObjectList()
        {
            Name = "crates",
            ResourceFileType = "*cen",
            ResourceFileExtension = "scenario_scenery_resource",
            PaletteName = "crate palette",
            Type = "bloc",
            List = new List<RandomizedObjectDetails>()
            {
                /*
                new RandomizedObjectDetails {Name =  "battery", Path = @"objects\props\covenant\battery\battery", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "cov_barrier", Path = @"objects\props\covenant\cov_barrier\cov_barrier", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "bulk_plasma_storage", Path = @"objects\props\covenant\bulk_plasma_storage\bulk_plasma_storage", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "crate_space_small", Path = @"objects\props\covenant\crate_space_small\crate_space_small", Faction = Faction.Covenant},
                //new RandomizedObjectDetails {Name =  "shield_door_small", Path = @"objects\props\covenant\shield_door_small\shield_door_small", Faction = Faction.Covenant},
                //new RandomizedObjectDetails {Name =  "shield_door_medium", Path = @"objects\props\covenant\shield_door_medium\shield_door_medium", Faction = Faction.Covenant},
                //new RandomizedObjectDetails {Name =  "shield_door_large", Path = @"objects\props\covenant\shield_door_large\shield_door_large", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "antennae_comm", Path = @"objects\props\covenant\antennae_comm\antennae_comm", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "cov_camp_stool", Path = @"objects\props\covenant\cov_camp_stool\cov_camp_stool", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "wraith_destroyed", Path = @"objects\props\covenant\destroyed_vehicles\wraith_destroyed\wraith_destroyed", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "revanent_destroyed", Path = @"objects\props\covenant\destroyed_vehicles\revanent_destroyed\revanent_destroyed", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "cex_damnation_grunt_crate", Path = @"levels\dlc\cex_damnation\scenery\cex_damnation_grunt_crate\cex_damnation_grunt_crate", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "antennae_comm", Path = @"objects\props\covenant\antennae_comm\antennae_comm", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "crate_packing", Path = @"objects\gear\human\military\crate_packing\crate_packing", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "warthog_destroyed", Path = @"objects\props\human\unsc\destroyed_vehicles\warthog_destroyed\warthog_destroyed", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "crate_packing_giant", Path = @"objects\gear\human\military\crate_packing_giant\crate_packing_giant", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "drum_55gal", Path = @"objects\gear\human\military\drum_55gal\drum_55gal", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "water_barrel", Path = @"objects\props\human\pioneer\water\water_barrel\water_barrel", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "water_barrel_pallette", Path = @"objects\props\human\pioneer\water\water_barrel_pallette\water_barrel_pallette", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "jersey_barrier_short", Path = @"objects\gear\human\industrial\jersey_barrier_short\jersey_barrier_short", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "pallet", Path = @"objects\gear\human\industrial\pallet\pallet", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "h_barrel_rusty", Path = @"objects\gear\human\industrial\h_barrel_rusty\h_barrel_rusty", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "jersey_barrier", Path = @"objects\gear\human\industrial\jersey_barrier\jersey_barrier", Faction = Faction.Human},
                //new RandomizedObjectDetails {Name =  "container_small", Path = @"objects\vehicles\human\civilian\truck_cab_large\attachments\container_small\container_small", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "box_wooden_small", Path = @"objects\gear\human\industrial\box_wooden_small\box_wooden_small", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "oni_crate_giant", Path = @"objects\props\human\oni\oni_crate_giant\oni_crate_giant", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "oni_crate_small", Path = @"objects\props\human\oni\oni_crate_small\oni_crate_small", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "fusion_coil", Path = @"objects\gear\human\military\fusion_coil\fusion_coil", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "cart_trailer", Path = @"objects\vehicles\human\civilian\cart_trailer\cart_trailer", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "fuel_canister", Path = @"objects\props\human\pioneer\furniture\fuel_canister\fuel_canister\fuel_canister", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "propane_tank_small", Path = @"objects\props\human\pioneer\garage\propane_tank_small\propane_tank_small", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "welding_station", Path = @"objects\props\human\pioneer\garage\welding_station\welding_station", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "crate_space", Path = @"objects\props\covenant\crate_space\crate_space", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "wire_spool", Path = @"objects\props\human\pioneer\garage\wire_spool\wire_spool", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "barricade_large", Path = @"objects\gear\human\military\barricade_large\barricade_large", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "barricade_small", Path = @"objects\gear\human\military\barricade_small\barricade_small", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "vehicle_fuel_storage", Path = @"objects\props\covenant\vehicle_fuel_storage\vehicle_fuel_storage", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "cov_fuel_cell_a", Path = @"objects\props\covenant\cov_fuel_cell_a\cov_fuel_cell_a", Faction = Faction.Covenant},
                new RandomizedObjectDetails {Name =  "space_crate", Path = @"objects\props\human\urban\space_crate\space_crate", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "backpack_teddy", Path = @"objects\props\human\urban\luggage\backpack_teddy\backpack_teddy", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "destroyed_falcon", Path = @"objects\props\human\unsc\destroyed_vehicles\destroyed_falcon\destroyed_falcon", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "street_cone", Path = @"objects\gear\human\industrial\street_cone\street_cone", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "cargo_truck_destroyed", Path = @"objects\props\human\unsc\destroyed_vehicles\cargo_truck_destroyed\cargo_truck_destroyed", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "truck_cab_large_destroyed", Path = @"objects\props\human\unsc\destroyed_vehicles\truck_cab_large_destroyed\truck_cab_large_destroyed", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "propane_tank", Path = @"objects\gear\human\industrial\propane_tank\propane_tank", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "pallet_large", Path = @"objects\gear\human\industrial\pallet_large\pallet_large", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "box_cardboard_large_open", Path = @"objects\props\human\urban\box_cardboard_large_open\box_cardboard_large_open", Faction = Faction.Human},
                new RandomizedObjectDetails {Name =  "land_mine", Path = @"objects\multi\land_mine\land_mine", Faction = Faction.Human},
                */
                new RandomizedObjectDetails
                {
                    Name = "crate_h_gun_rack_1",
                    Path = @"objects\gear\human\military\crate_h_gun_rack_1\crate_h_gun_rack_1",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "example" },
                            new RandomizedObjectDetails{Name = "shot_rock01" },
                            new RandomizedObjectDetails{Name = "br_rock01" },
                            new RandomizedObjectDetails{Name = "ar_shot" },
                            new RandomizedObjectDetails{Name = "br_shot" },
                            new RandomizedObjectDetails{Name = "ar_10" },
                            new RandomizedObjectDetails{Name = "br_5" },
                            new RandomizedObjectDetails{Name = "mag_shot" },
                            new RandomizedObjectDetails{Name = "empty" },
                            new RandomizedObjectDetails{Name = "mag_grenade" },
                            new RandomizedObjectDetails{Name = "mag_dmr_ar" }
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "crate_h_gun_rack_2",
                    Path = @"objects\gear\human\military\crate_h_gun_rack_2\crate_h_gun_rack_2",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "example" },
                            new RandomizedObjectDetails{Name = "ar_10" },
                            new RandomizedObjectDetails{Name = "empty" },
                            new RandomizedObjectDetails{Name = "shot_10" },
                            new RandomizedObjectDetails{Name = "shot_9" },
                            new RandomizedObjectDetails{Name = "br_10" },
                            new RandomizedObjectDetails{Name = "standard" },
                            new RandomizedObjectDetails{Name = "stand_rock" },
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "crate_space_a",
                    Path = @"objects\props\covenant\crate_space_a\crate_space_a",
                    Faction = Faction.Covenant,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "plasma_rifles0" },
                            new RandomizedObjectDetails{Name = "plasma_rifles1" },
                            new RandomizedObjectDetails{Name = "mixed0" },
                            new RandomizedObjectDetails{Name = "energy_blades0" },
                            new RandomizedObjectDetails{Name = "spike_rifle0" },
                            new RandomizedObjectDetails{Name = "spike_rifle1" },
                            new RandomizedObjectDetails{Name = "plasma_pistols0" },
                            new RandomizedObjectDetails{Name = "plasma_pistols1" },
                            new RandomizedObjectDetails{Name = "flak_cannons0" },
                            new RandomizedObjectDetails{Name = "needle_rifles0" },
                            new RandomizedObjectDetails{Name = "hologram" },
                            new RandomizedObjectDetails{Name = "active_camo" },
                            new RandomizedObjectDetails{Name = "armor_lockup" },
                            new RandomizedObjectDetails{Name = "jet_pack" },
                            new RandomizedObjectDetails{Name = "concussion_rifles0" },
                            new RandomizedObjectDetails{Name = "plasma_launchers0" },
                            new RandomizedObjectDetails{Name = "plasma_launchers1" },
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "crate_space_b",
                    Path = @"objects\gear\human\military\crate_space_b\crate_space_b",
                    Faction = Faction.Covenant,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default" },
                            new RandomizedObjectDetails{Name = "plasma_rifles0" },
                            new RandomizedObjectDetails{Name = "spike_rifle0" },
                            new RandomizedObjectDetails{Name = "needle_rifles0" },
                        }
                    }
                },
            }
        };


        public static RandomizedObjectList machines = new RandomizedObjectList()
        {
            Name = "machines",
            ResourceFileType = "srdgr*",
            ResourceFileExtension = "scenario_devices_resource",
            PaletteName = "machine palette",
            Type = "device_machine",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails
                {
                    Name = "pioneer_weapons_stash",
                    Path = @"objects\props\human\pioneer\pioneer_weapons_stash\pioneer_weapons_stash",
                    Faction = Faction.Human,
                    SubCategory = SubCategory.WeaponStash,
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default"},
                            new RandomizedObjectDetails{Name = "loaded"},
                            new RandomizedObjectDetails{Name = "ar_dmr" },
                            new RandomizedObjectDetails{Name = "sr_sr" },
                        }
                    }
                },
            }
        };

        public static RandomizedObjectList bipeds = new RandomizedObjectList()
        {
            Name = "bipeds",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails
                {
                    Name = "halsey",
                    Path = @"objects\characters\halsey\halsey",
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "sorvad" },
                            new RandomizedObjectDetails{Name = "halsey" },
                            new RandomizedObjectDetails{Name = "halsey_noeyelashes" }
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "spartans",
                    Path = @"objects\characters\spartans\spartans",
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "default" },
                            new RandomizedObjectDetails{Name = "carter" },
                            new RandomizedObjectDetails{Name = "jun" },
                            new RandomizedObjectDetails{Name = "female" },
                            new RandomizedObjectDetails{Name = "emile" },
                            new RandomizedObjectDetails{Name = "kat" },
                            new RandomizedObjectDetails{Name = "emile_knifeless" },
                            new RandomizedObjectDetails{Name = "player_skull" },
                            new RandomizedObjectDetails{Name = "comicon" },
                            new RandomizedObjectDetails{Name = "mcfarlane_mp2" },
                            new RandomizedObjectDetails{Name = "mcfarlane_exo" },
                            new RandomizedObjectDetails{Name = "male" },
                            new RandomizedObjectDetails{Name = "mcfarlane_militarypolice" },
                            new RandomizedObjectDetails{Name = "john117" },
                            new RandomizedObjectDetails{Name = "fp_cinematic" },
                            new RandomizedObjectDetails{Name = "default_cinematic" },
                            new RandomizedObjectDetails{Name = "dead_male" },
                            new RandomizedObjectDetails{Name = "dead_female" },
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "marine",
                    Path = @"objects\characters\marine\marine",
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "trooper_light" },
                            new RandomizedObjectDetails{Name = "odst_light" },
                            new RandomizedObjectDetails{Name = "odst_jetpack" },
                            new RandomizedObjectDetails{Name = "trooper_medic" },
                            new RandomizedObjectDetails{Name = "trooper_heavy" },
                            new RandomizedObjectDetails{Name = "trooper_radio" },
                            new RandomizedObjectDetails{Name = "badlands_medic" },
                            new RandomizedObjectDetails{Name = "badlands_light" },
                            new RandomizedObjectDetails{Name = "badlands_heavy" },
                            new RandomizedObjectDetails{Name = "badlands_radio" },
                            new RandomizedObjectDetails{Name = "space_marine_light" },
                            new RandomizedObjectDetails{Name = "pilot" },
                            new RandomizedObjectDetails{Name = "oni_officer" },
                            new RandomizedObjectDetails{Name = "trooper_duvall" },
                            new RandomizedObjectDetails{Name = "dead_kiva" }
                        }
                    }
                },
                new RandomizedObjectDetails
                {
                    Name = "elite",
                    Path = @"objects\characters\elite\elite",
                    Variants = new RandomizedObjectList
                    {
                        List = new List<RandomizedObjectDetails>()
                        {
                            new RandomizedObjectDetails{Name = "minor" },
                            new RandomizedObjectDetails{Name = "officer" },
                            new RandomizedObjectDetails{Name = "ultra" },
                            new RandomizedObjectDetails{Name = "space" },
                            new RandomizedObjectDetails{Name = "spec_ops" },
                            new RandomizedObjectDetails{Name = "general" },
                            new RandomizedObjectDetails{Name = "zealot" },
                            new RandomizedObjectDetails{Name = "mp" },
                            new RandomizedObjectDetails{Name = "bob" },
                            new RandomizedObjectDetails{Name = "jetpack" },
                            new RandomizedObjectDetails{Name = "story_elite" },
                        }
                    }
                }
            }
        };

        public static List<string> skipKeyWords = new List<string>()
        {
            "marine",
            "pelican",
            "space",
            "warthog",
            "falcon",
            "phantom",
            //"seraph",
            "spirit",
            "dropship",
            //"shade",
            "turret",
            "spartan",
            "trooper",
            "bfg",
            "aa",
            "hog",
            "carter",
            "emile",
            "jorge",
            "kat",
            "jun",
            //"shade",
            "allies",
            "ally",
            "civilian",
            "human",
            "witness",
            "fork",
            //"sq_court_cov_w1",
            "Human",
            "crv_sbr",
            "crv_sph",
            "crv_ph",
            "crv_cannon",
            "crv_bsh",
            "sabre",
            "waf_sbr",
            "waf_bsh",
            "waf_sph",
            "waf_ph",
            "sq_cov_bch_ds",
            "odst",
            "unsc",
            "sq_evac1_m1",
            "sq_evac0_m0",
        };

        public static List<string> forceNoSkipKeyWords = new List<string>()
        {
            "hog_cov",
            "falcon_ex",
            
        };


        public static List<string> skipSpecialEnemySquads = new List<string>()
        {
            "3kiva03_cov_drop01a",
            "3kiva03_cov_drop01b",
            "3kiva03_cov_drop01c",
            "3kiva03_cov_drop02a",
            "3kiva03_cov_drop02b",
            "3kiva03_cov_drop02c",
            "3kiva03_cov_drop02a_coop",
            "3kiva02_cov_drop01a",
            "3kiva02_cov_drop01b",
            "3kiva02_cov_drop01c",
            "3kiva02_cov_drop02a",
            "3kiva02_cov_drop02b",
            "3kiva02_cov_drop02c",
            "3kiva02_cov_drop02a_coop",
            "atrium_cov_counter_inf0",
            "atrium_cov_counter_inf1",
            "atrium_cov_counter_inf2",
            "atrium_cov_counter_inf3",
            "atrium_cov_concussion_inf0",
            "atrium_cov_rangers_inf0",
            "atrium_cov_rangers_inf1",
            "atrium_cov_captain0",
            "atrium_cov_captain1",
            "sq_platform_w0_1",
            "sq_platform_w0_2",
            "sq_platform_w0_3",
            "sq_platform_w0_4",
            "sq_platform_w1_1",
            "sq_platform_w1_2",
            "sq_platform_w1_3",
            "sq_platform_w1_4a",
            "sq_platform_w1_4b",
            "sq_platform_w2_1",
            "sq_platform_w2_2",
            "sq_platform_w2_3",
            "sq_roof_banshee",
            "waf_ph",
            "waf_bsh",
            "waf_sph",
            "trophy_cov_shade",

        };

        public static List<string> SquadTemplates = new List<string>()
        {
            @"ai\squad_templates\sq_camp_banshee_1",
            @"ai\squad_templates\sq_camp_banshee_2",
            @"ai\squad_templates\sq_camp_banshee_3",
            @"ai\squad_templates\sq_camp_banshee_air_1",
            @"ai\squad_templates\sq_camp_banshee_air_2",
            @"ai\squad_templates\sq_camp_banshee_air_3",
            @"ai\squad_templates\sq_camp_brute_1",
            @"ai\squad_templates\sq_camp_brute_2",
            @"ai\squad_templates\sq_camp_brute_3",
            @"ai\squad_templates\sq_camp_brute_captain_1",
            @"ai\squad_templates\sq_camp_brute_captain_2",
            @"ai\squad_templates\sq_camp_brute_captain_3",
            @"ai\squad_templates\sq_camp_bugger_1",
            @"ai\squad_templates\sq_camp_bugger_4",
            @"ai\squad_templates\sq_camp_cov_bc1_g3",
            @"ai\squad_templates\sq_camp_cov_bc1_j3",
            @"ai\squad_templates\sq_camp_cov_e1_g2",
            @"ai\squad_templates\sq_camp_cov_e1_g3",
            @"ai\squad_templates\sq_camp_cov_e1_g3_j2",
            @"ai\squad_templates\sq_camp_cov_e1_g4",
            @"ai\squad_templates\sq_camp_cov_e1_j2",
            @"ai\squad_templates\sq_camp_cov_e1_j3",
            @"ai\squad_templates\sq_camp_cov_e1_s2",
            @"ai\squad_templates\sq_camp_cov_e1_s3",
            @"ai\squad_templates\sq_camp_cov_e1_s4",
            @"ai\squad_templates\sq_camp_cov_e2_j2",
            @"ai\squad_templates\sq_camp_cov_e3_g3",
            @"ai\squad_templates\sq_camp_cov_g2_j1",
            @"ai\squad_templates\sq_camp_cov_g3_j1",
            @"ai\squad_templates\sq_camp_cov_g3_j2",
            @"ai\squad_templates\sq_camp_cov_js1_j3",
            @"ai\squad_templates\sq_camp_elite_1",
            @"ai\squad_templates\sq_camp_elite_2",
            @"ai\squad_templates\sq_camp_elite_3",
            @"ai\squad_templates\sq_camp_elite_bob_1",
            @"ai\squad_templates\sq_camp_elite_general_1",
            @"ai\squad_templates\sq_camp_elite_jetpack_1",
            @"ai\squad_templates\sq_camp_elite_jetpack_2",
            @"ai\squad_templates\sq_camp_elite_jetpack_3",
            @"ai\squad_templates\sq_camp_elite_jetpack_4",
            @"ai\squad_templates\sq_camp_elite_minor_1",
            @"ai\squad_templates\sq_camp_elite_minor_2",
            @"ai\squad_templates\sq_camp_elite_minor_3",
            @"ai\squad_templates\sq_camp_elite_officer_1",
            @"ai\squad_templates\sq_camp_elite_ultra_1",
            @"ai\squad_templates\sq_camp_engineer_1",
            @"ai\squad_templates\sq_camp_ghost_1",
            @"ai\squad_templates\sq_camp_ghost_2",
            @"ai\squad_templates\sq_camp_ghost_elite_1",
            @"ai\squad_templates\sq_camp_ghost_elite_2",
            @"ai\squad_templates\sq_camp_ghost_grunt_1",
            @"ai\squad_templates\sq_camp_ghost_grunt_2",
            @"ai\squad_templates\sq_camp_grunt_1",
            @"ai\squad_templates\sq_camp_grunt_2",
            @"ai\squad_templates\sq_camp_grunt_3",
            @"ai\squad_templates\sq_camp_grunt_4",
            @"ai\squad_templates\sq_camp_grunt_fc_1",
            @"ai\squad_templates\sq_camp_grunt_fc_2",
            @"ai\squad_templates\sq_camp_hunters",
            @"ai\squad_templates\sq_camp_hunter_solo",
            @"ai\squad_templates\sq_camp_jackalsniper_nr_1",
            @"ai\squad_templates\sq_camp_jackalsniper_nr_2",
            @"ai\squad_templates\sq_camp_jackalsniper_nr_3",
            @"ai\squad_templates\sq_camp_jackalsniper_nr_4",
            @"ai\squad_templates\sq_camp_jackalsniper_sr_1",
            @"ai\squad_templates\sq_camp_jackalsniper_sr_2",
            @"ai\squad_templates\sq_camp_jackal_1",
            @"ai\squad_templates\sq_camp_jackal_2",
            @"ai\squad_templates\sq_camp_jackal_3",
            @"ai\squad_templates\sq_camp_jackal_4",
            @"ai\squad_templates\sq_camp_revenant",
            @"ai\squad_templates\sq_camp_shade",
            @"ai\squad_templates\sq_camp_shade_anti_air",
            @"ai\squad_templates\sq_camp_shade_flak",
            @"ai\squad_templates\sq_camp_skirmisher_closerange_4",
            @"ai\squad_templates\sq_camp_skirmisher_ne_1",
            @"ai\squad_templates\sq_camp_skirmisher_ne_2",
            @"ai\squad_templates\sq_camp_skirmisher_ne_3",
            @"ai\squad_templates\sq_camp_skirmisher_ne_4",
            @"ai\squad_templates\sq_camp_skirmisher_nr_1",
            @"ai\squad_templates\sq_camp_skirmisher_nr_2",
            @"ai\squad_templates\sq_camp_skirmisher_nr_3",
            @"ai\squad_templates\sq_camp_skirmisher_nr_4",
            @"ai\squad_templates\sq_camp_skirmisher_pp_1",
            @"ai\squad_templates\sq_camp_skirmisher_pp_2",
            @"ai\squad_templates\sq_camp_skirmisher_pp_3",
            @"ai\squad_templates\sq_camp_skirmisher_pp_4",
            @"ai\squad_templates\sq_camp_skirmisher_sr_1",
            @"ai\squad_templates\sq_camp_skirmisher_sr_2",
            @"ai\squad_templates\sq_camp_wraith",
            @"ai\squad_templates\sq_camp_grunt_1",
        };

        public static List<string> skipSpecialObjects = new List<string>()
        {
            "wp_valley_targetlaser",
            "jetpack_rack0",
            "jetpack_rack1",
            "v_wafer_sabre_player0",
            "v_wafer_sabre_player1",
            "v_wafer_sabre_player2",
            "v_wafer_sabre_player3",
            "v_warp_sabre_player0",
            "v_warp_sabre_player1",
            "v_warp_sabre_player2",
            "v_warp_sabre_player3",
            "v_corvette_sabre_player0",
            "v_corvette_sabre_player1",
            "v_corvette_sabre_player2",
            "v_corvette_sabre_player3",
            "v_landing_sabre_player0",
            "v_landing_sabre_player1",
            "v_landing_sabre_player2",
            "v_landing_sabre_player3",
        };

        public static List<List<string>> animation_mode_groups = new List<List<string>>()
        {
            new(){"wraith_d", "scorption_d", "revenant_d", "ghost_d", "falcon_d", "warthog_d", "truck_d", "sabre_d"},
            new(){"scorpion_g", "wraith_g",},
            new(){"warthog_p", "revenant_p"},
            new(){"shade_d", "turret_g", "warthog_g"},
            //new(){"drop_pod", "fork_p_l1" }
        };


        public class AnimationGraph
        {
            public string Path { get; set; } = "";
            public List<WeaponClassCopyGroup> weapon_class_copy_groups = new List<WeaponClassCopyGroup>();

            public class WeaponClassCopyGroup
            {
                required public List<string> WeaponClassesCopyTo { get; set; }
                required public List<string> WeaponClassesCopyFrom { get; set; }
            }
        }

        public static List<AnimationGraph> animation_graphs = new List<AnimationGraph>()
        {
            new(){
                Path = @"objects\\characters\\brute\\brute",
                weapon_class_copy_groups =
                {
                    new(){WeaponClassesCopyFrom = ["rifle"], WeaponClassesCopyTo = ["pistol"]},
                    new(){WeaponClassesCopyFrom = ["hammer"], WeaponClassesCopyTo = ["flag", "sword", "ball"] }
                }
            },
            new(){
                Path = @"objects\\characters\\elite_ai\\elite_ai",
                weapon_class_copy_groups =
                {
                    new(){WeaponClassesCopyFrom = ["sword"], WeaponClassesCopyTo = ["flag", "hammer", "ball"] }
                }
            },
            new(){
                Path = @"objects\characters\grunt\grunt",
                weapon_class_copy_groups =
                {
                    new(){WeaponClassesCopyFrom = ["pistol"], WeaponClassesCopyTo = ["flag", "hammer", "ball", "turret", "sword", "hammer"] }
                }
            },
            new(){
                Path = @"objects\characters\hunter\hunter",
            },
            new(){
                Path = @"objects\characters\jackal\jackal",
                weapon_class_copy_groups =
                {
                    new(){WeaponClassesCopyFrom = ["pistol"], WeaponClassesCopyTo = ["flag", "hammer", "ball", "turret", "sword", "hammer", "missile", "rifle"] }
                }
            },
            new(){
                Path = @"objects\characters\bugger\bugger",
                weapon_class_copy_groups =
                {
                    new(){WeaponClassesCopyFrom = ["pistol"], WeaponClassesCopyTo = ["flag", "hammer", "ball", "turret", "sword", "hammer", "missile", "rifle"] }
                }
            },
        };


        public static List<string> cutscenes = new List<string>()
        {
            @"cinematics\010lc_bodysearch\010lc_bodysearch_010",
            @"cinematics\010lc_bodysearch\010lc_bodysearch_020",
            @"cinematics\010lc_bodysearch\010lc_bodysearch_030",
            @"cinematics\010ld_wintercontingency\010ld_wintercontingency_010",
            @"cinematics\010ld_wintercontingency\010ld_wintercontingency_020",
            @"cinematics\010ld_wintercontingency\010ld_wintercontingency_030",
            @"cinematics\020la_sword\020la_sword_010",
            @"cinematics\020la_sword\020la_sword_011",
            @"cinematics\020la_sword\020la_sword_013",
            @"cinematics\020la_sword\020la_sword_014",
            @"cinematics\020la_sword\020la_sword_015",
            @"cinematics\020la_sword\020la_sword_016",
            @"cinematics\020la_sword\020la_sword_017",
            @"cinematics\020la_sword\020la_sword_018",
            @"cinematics\020la_sword\020la_sword_020",
            @"cinematics\020la_sword\020la_sword_030",
            @"cinematics\020la_sword\020la_sword_032",
            @"cinematics\020la_sword\020la_sword_034",
            @"cinematics\020la_sword\020la_sword_036",
            @"cinematics\020la_sword\020la_sword_037",
            @"cinematics\020lb_halsey\020lb_halsey_010",
            @"cinematics\020lb_halsey\020lb_halsey_020",
            @"cinematics\020lb_halsey\020lb_halsey_030",
            @"cinematics\020lb_halsey\020lb_halsey_040",
            @"cinematics\020lb_halsey\020lb_halsey_050",
            @"cinematics\020lb_halsey\020lb_halsey_060",
            @"cinematics\030la_recon\030la_recon_010",
            @"cinematics\030la_recon\030la_recon_020",
            @"cinematics\030lb_vista\030lb_vista_010",
            @"cinematics\035la_bigpush\035la_bigpush_010",
            @"cinematics\035la_bigpush\035la_bigpush_020",
            @"cinematics\035la_bigpush\035la_bigpush_030",
            @"cinematics\035la_bigpush\035la_bigpush_032",
            @"cinematics\035la_bigpush\035la_bigpush_038",
            @"cinematics\035la_bigpush\035la_bigpush_040",
            @"cinematics\035la_bigpush\035la_bigpush_044",
            @"cinematics\035la_bigpush\035la_bigpush_048",
            @"cinematics\035la_bigpush\035la_bigpush_050",
            @"cinematics\035la_bigpush\035la_bigpush_060",
            @"cinematics\035lb_falconcrash\035lb_falconcrash_010",
            @"cinematics\035lc_blackship_reveal\035lc_blackship_reveal_010",
            @"cinematics\035lc_blackship_reveal\035lc_blackship_reveal_020",
            @"cinematics\035lc_blackship_reveal\035lc_blackship_reveal_030",
            @"cinematics\045la_blastoff\045la_blastoff_005",
            @"cinematics\045la_blastoff\045la_blastoff_010",
            @"cinematics\045la_blastoff\045la_blastoff_020",
            @"cinematics\045la_blastoff\045la_blastoff_030",
            @"cinematics\045la_blastoff\045la_blastoff_040",
            @"cinematics\045la_katsplan\045la_katsplan_010",
            @"cinematics\045la_katsplan\045la_katsplan_020",
            @"cinematics\045la_katsplan\045la_katsplan_030",
            @"cinematics\045la_katsplan\045la_katsplan_040",
            @"cinematics\045la_katsplan_v2\045la_katsplan_050",
            @"cinematics\045la_katsplan_v2\045la_katsplan_060",
            @"cinematics\045lb_pitstop\045lb_pitstop_010",
            @"cinematics\045lb_pitstop\045lb_pitstop_020",
            @"cinematics\045lb_pitstop\045lb_pitstop_030",
            @"cinematics\045lb_pitstop\045lb_pitstop_040",
            @"cinematics\045lb_pitstop\045lb_pitstop_050",
            @"cinematics\045lc_aftship_landing\045lc_aftship_landing_010",
            @"cinematics\045le_bomb_delivery\045le_bomb_delivery_010",
            @"cinematics\045lf_jorge\045lf_jorge_010",
            @"cinematics\045lf_jorge\045lf_jorge_020",
            @"cinematics\045lf_jorge\045lf_jorge_030",
            @"cinematics\045lf_jorge\045lf_jorge_040",
            @"cinematics\050la_wake\050la_wake_010",
            @"cinematics\050la_wake\050la_wake_030",
            @"cinematics\050la_wake\050la_wake_040",
            @"cinematics\050la_wake\050la_wake_050",
            @"cinematics\050lb_reunited\050lb_reunited_010",
            @"cinematics\052la_airlift\052la_airlift_010",
            @"cinematics\052la_airlift\052la_airlift_020",
            @"cinematics\052lb_reflection\052lb_reflection_010",
            @"cinematics\052lb_reflection\052lb_reflection_013",
            @"cinematics\052lb_reflection\052lb_reflection_017",
            @"cinematics\052lb_reflection\052lb_reflection_020",
            @"cinematics\052lb_reflection\052lb_reflection_030",
            @"cinematics\052lb_reflection\052lb_reflection_040",
            @"cinematics\052lb_reflection\052lb_reflection_050",
            @"cinematics\052lb_reflection\052lb_reflection_060",
            @"cinematics\060la_return_to_sword\060la_return_to_sword_020",
            @"cinematics\060lb_tramride\060lb_tramride_010",
            @"cinematics\060lb_tramride\060lb_tramride_013",
            @"cinematics\060lb_tramride\060lb_tramride_017",
            @"cinematics\060lb_tramride\060lb_tramride_020",
            @"cinematics\060lb_tramride\060lb_tramride_022",
            @"cinematics\060lb_tramride\060lb_tramride_030",
            @"cinematics\060lb_tramride\060lb_tramride_034",
            @"cinematics\060lb_tramride\060lb_tramride_038",
            @"cinematics\060lb_tramride2\060lb_tramride_040",
            @"cinematics\060lb_tramride2\060lb_tramride_042",
            @"cinematics\060lc_the_package\060lc_the_package_010",
            @"cinematics\060lc_the_package\060lc_the_package_020",
            @"cinematics\060lc_the_package\060lc_the_package_030",
            @"cinematics\060lc_the_package\060lc_the_package_040",
            @"cinematics\060lc_the_package\060lc_the_package_050",
            @"cinematics\060lc_the_package\060lc_the_package_060",
            @"cinematics\060lc_the_package\060lc_the_package_065",
            @"cinematics\060lc_the_package\060lc_the_package_070",
            @"cinematics\060lc_the_package\060lc_the_package_080",
            @"cinematics\070la_carter\070la_carter_010",
            @"cinematics\070la_carter\070la_carter_020",
            @"cinematics\070la_carter\070la_carter_030",
            @"cinematics\070la2_carter_death\070la2_carter_death_010",
            @"cinematics\070lb_delivery\070lb_delivery_010",
            @"cinematics\070lb_delivery\070lb_delivery_020",
            @"cinematics\070lb_delivery\070lb_delivery_030",
            @"cinematics\070lb_delivery\070lb_delivery_040",
            @"cinematics\070lc_poa_launch\070lc_poa_launch_010",
            @"cinematics\070lc_poa_launch\070lc_poa_launch_015",
            @"cinematics\070lc_poa_launch\070lc_poa_launch_020",
            @"cinematics\070lc_poa_launch\070lc_poa_launch_030",
            @"cinematics\070lk_credits\070lk_credits_010",
            @"cinematics\080lb_re_intro\080lb_re_intro_000",
            @"cinematics\080lc_game_over\080lc_game_over_000",
            @"cinematics\080lc_game_over\080lc_game_over_010",
            @"cinematics\080ld_epilogue\080ld_epilogue_000",
            @"cinematics\000la_prologue\000la_prologue_000",
            @"cinematics\000la_prologue\010la_outpost_010",
            @"cinematics\000la_prologue\010la_outpost_020",
            @"cinematics\000la_prologue\010la_outpost_030",
            @"cinematics\000la_prologue\010la_outpost_035",
            @"cinematics\000la_prologue\010la_outpost_040",
            @"cinematics\010la_outpost\010la_outpost_050",
            @"cinematics\010la_outpost\010la_outpost_055",
        };

        public static RandomizedObjectList dialogue = new RandomizedObjectList()
        {
            Name = "dialogue",
            List = new List<RandomizedObjectDetails>()
            {
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_170_plf", Subtitle = "m10_10c_170_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_180_car", Subtitle = "m10_10c_180_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_190_car", Subtitle = "m10_10c_190_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_195_car", Subtitle = "m10_10c_195_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_200_car", Subtitle = "m10_10c_200_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_210_car", Subtitle = "m10_10c_210_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_220_car", Subtitle = "m10_10c_220_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_225_car", Subtitle = "m10_10c_225_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_230_car", Subtitle = "m10_10c_230_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_235_car", Subtitle = "m10_10c_235_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_237_car", Subtitle = "m10_10c_237_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_240_pla", Subtitle = "m10_10c_240_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_240_plf", Subtitle = "m10_10c_240_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_250_jun", Subtitle = "m10_10c_250_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_260_car", Subtitle = "m10_10c_260_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_270_kat", Subtitle = "m10_10c_270_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_280_jor", Subtitle = "m10_10c_280_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_290_car", Subtitle = "m10_10c_290_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_300_kat", Subtitle = "m10_10c_300_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_310_car", Subtitle = "m10_10c_310_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_320_kat", Subtitle = "m10_10c_320_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_330_car", Subtitle = "m10_10c_330_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_340_emi", Subtitle = "m10_10c_340_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_350_emi", Subtitle = "m10_10c_350_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_360_emi", Subtitle = "m10_10c_360_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_010_car", Subtitle = "m10_80c_010_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_020_tr1", Subtitle = "m10_80c_020_tr1"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_030_car", Subtitle = "m10_80c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_040_kat", Subtitle = "m10_80c_040_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_050_pla", Subtitle = "m10_80c_050_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_050_plf", Subtitle = "m10_80c_050_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_060_kat", Subtitle = "m10_80c_060_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_070_jor", Subtitle = "m10_80c_070_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_080_jor", Subtitle = "m10_80c_080_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_090_sar", Subtitle = "m10_80c_090_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_100_sar", Subtitle = "m10_80c_100_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_105_sar", Subtitle = "m10_80c_105_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_110_jor", Subtitle = "m10_80c_110_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_120_car", Subtitle = "m10_80c_120_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_130_jor", Subtitle = "m10_80c_130_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_135_jor", Subtitle = "m10_80c_135_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_140_sar", Subtitle = "m10_80c_140_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_150_sar", Subtitle = "m10_80c_150_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_155_sar", Subtitle = "m10_80c_155_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_170_emi", Subtitle = "m10_80c_170_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_180_car", Subtitle = "m10_80c_180_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_190_pla", Subtitle = "m10_80c_190_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_200_car", Subtitle = "m10_80c_200_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_210_pla", Subtitle = "m10_80c_210_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_220_pla", Subtitle = "m10_80c_220_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_230_sar", Subtitle = "m10_80c_230_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_250_car", Subtitle = "m10_80c_250_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_290_sar", Subtitle = "m10_80c_290_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_300_emi", Subtitle = "m10_80c_300_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_80c_310_car", Subtitle = "m10_80c_310_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_010_car", Subtitle = "m10_90c_010_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_020_kat", Subtitle = "m10_90c_020_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_030_kat", Subtitle = "m10_90c_030_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_040_car", Subtitle = "m10_90c_040_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_050_kat", Subtitle = "m10_90c_050_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_055_kat", Subtitle = "m10_90c_055_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_060_car", Subtitle = "m10_90c_060_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_063_sar", Subtitle = "m10_90c_063_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_065_emi", Subtitle = "m10_90c_065_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_070_jor", Subtitle = "m10_90c_070_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_075_jor", Subtitle = "m10_90c_075_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_080_jor", Subtitle = "m10_90c_080_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_090_sar", Subtitle = "m10_90c_090_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_100_jor", Subtitle = "m10_90c_100_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_110_jor", Subtitle = "m10_90c_110_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_120_sar", Subtitle = "m10_90c_120_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_130_jor", Subtitle = "m10_90c_130_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_140_sar", Subtitle = "m10_90c_140_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_150_jor", Subtitle = "m10_90c_150_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_155_jor", Subtitle = "m10_90c_155_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_160_sar", Subtitle = "m10_90c_160_sar"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_170_emi", Subtitle = "m10_90c_170_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_180_jor", Subtitle = "m10_90c_180_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_190_jor", Subtitle = "m10_90c_190_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_200_emi", Subtitle = "m10_90c_200_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_210_car", Subtitle = "m10_90c_210_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_220_car", Subtitle = "m10_90c_220_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_230_jor", Subtitle = "m10_90c_230_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_240_kat", Subtitle = "m10_90c_240_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_250_kat", Subtitle = "m10_90c_250_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_260_car", Subtitle = "m10_90c_260_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_270_kat", Subtitle = "m10_90c_270_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_280_hol", Subtitle = "m10_90c_280_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_290_car", Subtitle = "m10_90c_290_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_300_hol", Subtitle = "m10_90c_300_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_310_car", Subtitle = "m10_90c_310_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_90c_320_hol", Subtitle = "m10_90c_320_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_010_hol", Subtitle = "m10_10c_010_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_020_hol", Subtitle = "m10_10c_020_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_030_car", Subtitle = "m10_10c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_050_hol", Subtitle = "m10_10c_050_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_060_kat", Subtitle = "m10_10c_060_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_070_jor", Subtitle = "m10_10c_070_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_080_emf", Subtitle = "m10_10c_080_emf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_080_emi", Subtitle = "m10_10c_080_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_090_kat", Subtitle = "m10_10c_090_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_100_car", Subtitle = "m10_10c_100_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_110_hol", Subtitle = "m10_10c_110_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_120_car", Subtitle = "m10_10c_120_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_130_hol", Subtitle = "m10_10c_130_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_140_car", Subtitle = "m10_10c_140_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_150_hol", Subtitle = "m10_10c_150_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_160_car", Subtitle = "m10_10c_160_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m10\cinematic\m10_10c_170_pla", Subtitle = "m10_10c_170_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_085_car", Subtitle = "m20_10c_085_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_090_kat", Subtitle = "m20_10c_090_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_010_jor", Subtitle = "m20_20c_010_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_015_jor", Subtitle = "m20_20c_015_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_020_jor", Subtitle = "m20_20c_020_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_030_pla", Subtitle = "m20_20c_030_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_030_plf", Subtitle = "m20_20c_030_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_040_car", Subtitle = "m20_20c_040_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_050_jor", Subtitle = "m20_20c_050_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_060_car", Subtitle = "m20_20c_060_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_070_jor", Subtitle = "m20_20c_070_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_080_jor", Subtitle = "m20_20c_080_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_090_hal", Subtitle = "m20_20c_090_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_100_hal", Subtitle = "m20_20c_100_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_110_jor", Subtitle = "m20_20c_110_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_120_hal", Subtitle = "m20_20c_120_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_130_jor", Subtitle = "m20_20c_130_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_140_hal", Subtitle = "m20_20c_140_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_150_hal", Subtitle = "m20_20c_150_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_160_car", Subtitle = "m20_20c_160_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_170_hal", Subtitle = "m20_20c_170_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_180_jor", Subtitle = "m20_20c_180_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_190_hal", Subtitle = "m20_20c_190_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_200_jor", Subtitle = "m20_20c_200_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_210_hal", Subtitle = "m20_20c_210_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_220_jor", Subtitle = "m20_20c_220_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_230_pla", Subtitle = "m20_20c_230_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_230_plf", Subtitle = "m20_20c_230_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_240_car", Subtitle = "m20_20c_240_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_250_hal", Subtitle = "m20_20c_250_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_260_car", Subtitle = "m20_20c_260_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_270_hal", Subtitle = "m20_20c_270_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_280_car", Subtitle = "m20_20c_280_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_290_kat", Subtitle = "m20_20c_290_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_300_hal", Subtitle = "m20_20c_300_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_310_car", Subtitle = "m20_20c_310_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_320_hal", Subtitle = "m20_20c_320_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_330_hal", Subtitle = "m20_20c_330_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_335_hal", Subtitle = "m20_20c_335_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_340_car", Subtitle = "m20_20c_340_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_350_hal", Subtitle = "m20_20c_350_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_360_car", Subtitle = "m20_20c_360_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_370_hal", Subtitle = "m20_20c_370_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_380_car", Subtitle = "m20_20c_380_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_385_car", Subtitle = "m20_20c_385_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_390_jor", Subtitle = "m20_20c_390_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_20c_400_hal", Subtitle = "m20_20c_400_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_010_swo", Subtitle = "m20_10c_010_swo"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_020_car", Subtitle = "m20_10c_020_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_030_car", Subtitle = "m20_10c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_040_dot", Subtitle = "m20_10c_040_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_050_dot", Subtitle = "m20_10c_050_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_060_dot", Subtitle = "m20_10c_060_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_070_car", Subtitle = "m20_10c_070_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_075_car", Subtitle = "m20_10c_075_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m20\cinematic\m20_10c_080_car", Subtitle = "m20_10c_080_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_050_dot", Subtitle = "m20_10c_050_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_060_dot", Subtitle = "m20_10c_060_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_070_car", Subtitle = "m20_10c_070_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_080_car", Subtitle = "m20_10c_080_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_085_car", Subtitle = "m20_10c_085_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_090_kat", Subtitle = "m20_10c_090_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_10c_010_jun", Subtitle = "m30_10c_010_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_10c_020_kat", Subtitle = "m30_10c_020_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_10c_030_jun", Subtitle = "m30_10c_030_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_10c_040_jun", Subtitle = "m30_10c_040_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_10c_050_jun", Subtitle = "m30_10c_050_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_10c_060_jun", Subtitle = "m30_10c_060_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_010_jun", Subtitle = "m30_20c_010_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_020_pla", Subtitle = "m30_20c_020_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_020_plf", Subtitle = "m30_20c_020_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_030_jun", Subtitle = "m30_20c_030_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_040_kat", Subtitle = "m30_20c_040_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_045_kat", Subtitle = "m30_20c_045_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_050_jun", Subtitle = "m30_20c_050_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_060_kat", Subtitle = "m30_20c_060_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m30_20c_065_kat", Subtitle = "m30_20c_065_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_020_car", Subtitle = "m20_10c_020_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_030_car", Subtitle = "m20_10c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m30\cinematic\m20_10c_040_dot", Subtitle = "m20_10c_040_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_120_pla", Subtitle = "m35_10c_120_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_120_plf", Subtitle = "m35_10c_120_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_130_tr1", Subtitle = "m35_10c_130_tr1"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_140_tr2", Subtitle = "m35_10c_140_tr2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_150_kat", Subtitle = "m35_10c_150_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_160_kat", Subtitle = "m35_10c_160_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_170_pla", Subtitle = "m35_10c_170_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_170_plf", Subtitle = "m35_10c_170_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_010_pi2", Subtitle = "m35_20c_010_pi2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_015_pi2", Subtitle = "m35_20c_015_pi2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_020_pi2", Subtitle = "m35_20c_020_pi2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_030_jor", Subtitle = "m35_20c_030_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_035_jor", Subtitle = "m35_20c_035_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_040_jor", Subtitle = "m35_20c_040_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_20c_050_jor", Subtitle = "m35_20c_050_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_010_pla", Subtitle = "m35_30c_010_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_020_jor", Subtitle = "m35_30c_020_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_030_car", Subtitle = "m35_30c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_040_car", Subtitle = "m35_30c_040_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_045_car", Subtitle = "m35_30c_045_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_050_air", Subtitle = "m35_30c_050_air"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_060_jor", Subtitle = "m35_30c_060_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_070_car", Subtitle = "m35_30c_070_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_110_dot", Subtitle = "m35_30c_110_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_120_jor", Subtitle = "m35_30c_120_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_130_air", Subtitle = "m35_30c_130_air"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_30c_140_car", Subtitle = "m35_30c_140_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_010_dot", Subtitle = "m35_10c_010_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_020_dot", Subtitle = "m35_10c_020_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_030_dot", Subtitle = "m35_10c_030_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_040_dot", Subtitle = "m35_10c_040_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_050_car", Subtitle = "m35_10c_050_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_060_jun", Subtitle = "m35_10c_060_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_062_jun", Subtitle = "m35_10c_062_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_063_jun", Subtitle = "m35_10c_063_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_065_jun", Subtitle = "m35_10c_065_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_066_jun", Subtitle = "m35_10c_066_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_068_jun", Subtitle = "m35_10c_068_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_069_jun", Subtitle = "m35_10c_069_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_070_kat", Subtitle = "m35_10c_070_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_080_kat", Subtitle = "m35_10c_080_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_090_pla", Subtitle = "m35_10c_090_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_090_plf", Subtitle = "m35_10c_090_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m35\cinematic\m35_10c_100_kat", Subtitle = "m35_10c_100_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_120_car", Subtitle = "m45_10c_120_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_130_kat", Subtitle = "m45_10c_130_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_140_car", Subtitle = "m45_10c_140_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_150_kat", Subtitle = "m45_10c_150_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_160_car", Subtitle = "m45_10c_160_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_170_kat", Subtitle = "m45_10c_170_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_180_car", Subtitle = "m45_10c_180_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_190_kat", Subtitle = "m45_10c_190_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_200_car", Subtitle = "m45_10c_200_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_210_kat", Subtitle = "m45_10c_210_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_220_car", Subtitle = "m45_10c_220_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_230_kat", Subtitle = "m45_10c_230_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_240_car", Subtitle = "m45_10c_240_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_250_kat", Subtitle = "m45_10c_250_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_260_car", Subtitle = "m45_10c_260_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_270_jor", Subtitle = "m45_10c_270_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_280_car", Subtitle = "m45_10c_280_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_290_kat", Subtitle = "m45_10c_290_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_300_emi", Subtitle = "m45_10c_300_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_310_kat", Subtitle = "m45_10c_310_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_320_jor", Subtitle = "m45_10c_320_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_330_car", Subtitle = "m45_10c_330_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_340_jor", Subtitle = "m45_10c_340_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_350_kat", Subtitle = "m45_10c_350_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_360_kat", Subtitle = "m45_10c_360_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_370_kat", Subtitle = "m45_10c_370_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_380_car", Subtitle = "m45_10c_380_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_390_kat", Subtitle = "m45_10c_390_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_395_kat", Subtitle = "m45_10c_395_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_400_emi", Subtitle = "m45_10c_400_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_410_kat", Subtitle = "m45_10c_410_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_420_car", Subtitle = "m45_10c_420_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_430_kat", Subtitle = "m45_10c_430_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_440_car", Subtitle = "m45_10c_440_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_30c_010_sab", Subtitle = "m45_30c_010_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_30c_020_sab", Subtitle = "m45_30c_020_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_30c_030_sab", Subtitle = "m45_30c_030_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_30c_032_sab", Subtitle = "m45_30c_032_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_30c_034_sab", Subtitle = "m45_30c_034_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_002_jor", Subtitle = "m45_40c_002_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_004_sab", Subtitle = "m45_40c_004_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_005_jor", Subtitle = "m45_40c_005_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_006_sab", Subtitle = "m45_40c_006_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_008_sab", Subtitle = "m45_40c_008_sab"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_010_jor", Subtitle = "m45_40c_010_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_020_dot", Subtitle = "m45_40c_020_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_030_jor", Subtitle = "m45_40c_030_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_042_dot", Subtitle = "m45_40c_042_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_050_jor", Subtitle = "m45_40c_050_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_060_dot", Subtitle = "m45_40c_060_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_070_jor", Subtitle = "m45_40c_070_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_080_an9", Subtitle = "m45_40c_080_an9"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_090_hol", Subtitle = "m45_40c_090_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_100_pla", Subtitle = "m45_40c_100_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_100_plf", Subtitle = "m45_40c_100_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_110_hol", Subtitle = "m45_40c_110_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_116_hol", Subtitle = "m45_40c_116_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_120_pla", Subtitle = "m45_40c_120_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_120_plf", Subtitle = "m45_40c_120_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_130_sav", Subtitle = "m45_40c_130_sav"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_140_hol", Subtitle = "m45_40c_140_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_150_pla", Subtitle = "m45_40c_150_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_150_plf", Subtitle = "m45_40c_150_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_160_dot", Subtitle = "m45_40c_160_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_170_pla", Subtitle = "m45_40c_170_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_40c_170_plf", Subtitle = "m45_40c_170_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_50c_010_jor", Subtitle = "m45_50c_010_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_50c_020_hol", Subtitle = "m45_50c_020_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_50c_030_pla", Subtitle = "m45_50c_030_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_50c_030_plf", Subtitle = "m45_50c_030_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_50c_040_hol", Subtitle = "m45_50c_040_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_50c_050_jor", Subtitle = "m45_50c_050_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_012_dot", Subtitle = "m45_60c_012_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_020_jor", Subtitle = "m45_60c_020_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_030_jor", Subtitle = "m45_60c_030_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_040_jor", Subtitle = "m45_60c_040_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_050_pla", Subtitle = "m45_60c_050_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_050_plf", Subtitle = "m45_60c_050_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_060_jor", Subtitle = "m45_60c_060_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_072_dot", Subtitle = "m45_60c_072_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_080_jor", Subtitle = "m45_60c_080_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_090_jor", Subtitle = "m45_60c_090_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_100_pla", Subtitle = "m45_60c_100_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_100_plf", Subtitle = "m45_60c_100_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_110_jor", Subtitle = "m45_60c_110_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_120_jor", Subtitle = "m45_60c_120_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_125_jor", Subtitle = "m45_60c_125_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_130_jor", Subtitle = "m45_60c_130_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_140_dot", Subtitle = "m45_60c_140_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_150_at2", Subtitle = "m45_60c_150_at2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_160_jon", Subtitle = "m45_60c_160_jon"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_170_an9", Subtitle = "m45_60c_170_an9"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_180_air", Subtitle = "m45_60c_180_air"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_190_at1", Subtitle = "m45_60c_190_at1"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_200_hol", Subtitle = "m45_60c_200_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_210_dot", Subtitle = "m45_60c_210_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_212_dot", Subtitle = "m45_60c_212_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_213_dot", Subtitle = "m45_60c_213_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_214_dot", Subtitle = "m45_60c_214_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_60c_215_dot", Subtitle = "m45_60c_215_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_010_dot", Subtitle = "m45_10c_010_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_020_dot", Subtitle = "m45_10c_020_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_030_jun", Subtitle = "m45_10c_030_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_033_jun", Subtitle = "m45_10c_033_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_037_jun", Subtitle = "m45_10c_037_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_040_jor", Subtitle = "m45_10c_040_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_050_jun", Subtitle = "m45_10c_050_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_060_jor", Subtitle = "m45_10c_060_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_070_jun", Subtitle = "m45_10c_070_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_080_jor", Subtitle = "m45_10c_080_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_090_jor", Subtitle = "m45_10c_090_jor"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m45\cinematic\m45_10c_100_kat", Subtitle = "m45_10c_100_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_020_plf", Subtitle = "m50_20c_020_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_030_kat", Subtitle = "m50_20c_030_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_040_pla", Subtitle = "m50_20c_040_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_040_plf", Subtitle = "m50_20c_040_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_050_kat", Subtitle = "m50_20c_050_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_055_kat", Subtitle = "m50_20c_055_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_060_pla", Subtitle = "m50_20c_060_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_060_plf", Subtitle = "m50_20c_060_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_070_kat", Subtitle = "m50_20c_070_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_10c_010_pla", Subtitle = "m50_10c_010_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_10c_020_pla", Subtitle = "m50_10c_020_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_010_kat", Subtitle = "m50_20c_010_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m50\cinematic\m50_20c_020_pla", Subtitle = "m50_20c_020_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_010_jun", Subtitle = "m52_20c_010_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_020_jun", Subtitle = "m52_20c_020_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_030_emi", Subtitle = "m52_20c_030_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_040_emi", Subtitle = "m52_20c_040_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_050_jun", Subtitle = "m52_20c_050_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_060_emi", Subtitle = "m52_20c_060_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_070_car", Subtitle = "m52_20c_070_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_080_jun", Subtitle = "m52_20c_080_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_090_car", Subtitle = "m52_20c_090_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_100_kat", Subtitle = "m52_20c_100_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_110_car", Subtitle = "m52_20c_110_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_120_kat", Subtitle = "m52_20c_120_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_125_kat", Subtitle = "m52_20c_125_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_130_car", Subtitle = "m52_20c_130_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_140_kat", Subtitle = "m52_20c_140_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_150_kat", Subtitle = "m52_20c_150_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_160_car", Subtitle = "m52_20c_160_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_170_hol", Subtitle = "m52_20c_170_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_180_car", Subtitle = "m52_20c_180_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_190_kat", Subtitle = "m52_20c_190_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_200_car", Subtitle = "m52_20c_200_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_210_kat", Subtitle = "m52_20c_210_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_220_hol", Subtitle = "m52_20c_220_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_230_kat", Subtitle = "m52_20c_230_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_240_car", Subtitle = "m52_20c_240_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_250_jun", Subtitle = "m52_20c_250_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_260_jun", Subtitle = "m52_20c_260_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_270_emi", Subtitle = "m52_20c_270_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_280_kat", Subtitle = "m52_20c_280_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_290_car", Subtitle = "m52_20c_290_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_300_kat", Subtitle = "m52_20c_300_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_310_car", Subtitle = "m52_20c_310_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_320_kat", Subtitle = "m52_20c_320_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_330_car", Subtitle = "m52_20c_330_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_340_kat", Subtitle = "m52_20c_340_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_360_kat", Subtitle = "m52_20c_360_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_370_kat", Subtitle = "m52_20c_370_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_380_kat", Subtitle = "m52_20c_380_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_390_kat", Subtitle = "m52_20c_390_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_400_car", Subtitle = "m52_20c_400_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_410_jun", Subtitle = "m52_20c_410_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_420_car", Subtitle = "m52_20c_420_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_430_kat", Subtitle = "m52_20c_430_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_440_car", Subtitle = "m52_20c_440_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_450_kat", Subtitle = "m52_20c_450_kat"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_460_car", Subtitle = "m52_20c_460_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_470_jun", Subtitle = "m52_20c_470_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_480_pla", Subtitle = "m52_20c_480_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_490_cv1", Subtitle = "m52_20c_490_cv1"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_500_cv2", Subtitle = "m52_20c_500_cv2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_530_cvf", Subtitle = "m52_20c_530_cvf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_550_cf3", Subtitle = "m52_20c_550_cf3"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_560_cf4", Subtitle = "m52_20c_560_cf4"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_570_cv1", Subtitle = "m52_20c_570_cv1"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_580_cv2", Subtitle = "m52_20c_580_cv2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_610_cvf", Subtitle = "m52_20c_610_cvf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_20c_620_cf3", Subtitle = "m52_20c_620_cf3"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_10c_015_car", Subtitle = "m52_10c_015_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_10c_025_car", Subtitle = "m52_10c_025_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_10c_030_car", Subtitle = "m52_10c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_10c_040_pla", Subtitle = "m52_10c_040_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_10c_040_plf", Subtitle = "m52_10c_040_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m52\cinematic\m52_10c_050_car", Subtitle = "m52_10c_050_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_080_pla", Subtitle = "m60_10c_080_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_080_plf", Subtitle = "m60_10c_080_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_090_car", Subtitle = "m60_10c_090_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_010_jun", Subtitle = "m60_20c_010_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_020_car", Subtitle = "m60_20c_020_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_030_emi", Subtitle = "m60_20c_030_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_040_jun", Subtitle = "m60_20c_040_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_050_car", Subtitle = "m60_20c_050_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_060_dot", Subtitle = "m60_20c_060_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_070_dot", Subtitle = "m60_20c_070_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_080_car", Subtitle = "m60_20c_080_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_090_dot", Subtitle = "m60_20c_090_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_100_jun", Subtitle = "m60_20c_100_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_110_emi", Subtitle = "m60_20c_110_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_120_jun", Subtitle = "m60_20c_120_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_130_car", Subtitle = "m60_20c_130_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_140_dot", Subtitle = "m60_20c_140_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_150_car", Subtitle = "m60_20c_150_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_160_car", Subtitle = "m60_20c_160_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_170_car", Subtitle = "m60_20c_170_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_180_dot", Subtitle = "m60_20c_180_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_190_emi", Subtitle = "m60_20c_190_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_200_hal", Subtitle = "m60_20c_200_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_210_car", Subtitle = "m60_20c_210_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_220_hal", Subtitle = "m60_20c_220_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_230_hal", Subtitle = "m60_20c_230_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_240_car", Subtitle = "m60_20c_240_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_250_hal", Subtitle = "m60_20c_250_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_260_car", Subtitle = "m60_20c_260_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_270_hal", Subtitle = "m60_20c_270_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_280_car", Subtitle = "m60_20c_280_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_290_hal", Subtitle = "m60_20c_290_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_300_car", Subtitle = "m60_20c_300_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_310_hal", Subtitle = "m60_20c_310_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_320_hal", Subtitle = "m60_20c_320_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_330_emi", Subtitle = "m60_20c_330_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_340_car", Subtitle = "m60_20c_340_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_350_hal", Subtitle = "m60_20c_350_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_360_car", Subtitle = "m60_20c_360_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_370_hal", Subtitle = "m60_20c_370_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_20c_380_hal", Subtitle = "m60_20c_380_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_010_emi", Subtitle = "m60_30c_010_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_020_hal", Subtitle = "m60_30c_020_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_030_hal", Subtitle = "m60_30c_030_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_040_emi", Subtitle = "m60_30c_040_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_050_hal", Subtitle = "m60_30c_050_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_060_car", Subtitle = "m60_30c_060_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_070_hal", Subtitle = "m60_30c_070_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_080_hal", Subtitle = "m60_30c_080_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_090_hal", Subtitle = "m60_30c_090_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_100_car", Subtitle = "m60_30c_100_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_110_hal", Subtitle = "m60_30c_110_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_120_hal", Subtitle = "m60_30c_120_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_130_hal", Subtitle = "m60_30c_130_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_140_pla", Subtitle = "m60_30c_140_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_140_plf", Subtitle = "m60_30c_140_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_150_hal", Subtitle = "m60_30c_150_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_160_pla", Subtitle = "m60_30c_160_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_160_plf", Subtitle = "m60_30c_160_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_170_car", Subtitle = "m60_30c_170_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_180_hal", Subtitle = "m60_30c_180_hal"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_190_car", Subtitle = "m60_30c_190_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_200_jun", Subtitle = "m60_30c_200_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_205_jun", Subtitle = "m60_30c_205_jun"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_210_car", Subtitle = "m60_30c_210_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_220_car", Subtitle = "m60_30c_220_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_230_dot", Subtitle = "m60_30c_230_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_240_car", Subtitle = "m60_30c_240_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_250_dot", Subtitle = "m60_30c_250_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_30c_260_car", Subtitle = "m60_30c_260_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_010_hol", Subtitle = "m60_10c_010_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_020_hol", Subtitle = "m60_10c_020_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_030_car", Subtitle = "m60_10c_030_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_040_hol", Subtitle = "m60_10c_040_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_050_car", Subtitle = "m60_10c_050_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_060_hol", Subtitle = "m60_10c_060_hol"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m60\cinematic\m60_10c_070_car", Subtitle = "m60_10c_070_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_065_car", Subtitle = "m70_10c_065_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_070_pla", Subtitle = "m70_10c_070_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_070_plf", Subtitle = "m70_10c_070_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_080_car", Subtitle = "m70_10c_080_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_090_caf", Subtitle = "m70_10c_090_caf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_090_car", Subtitle = "m70_10c_090_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_110_emi", Subtitle = "m70_10c_110_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_120_car", Subtitle = "m70_10c_120_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_125_car", Subtitle = "m70_10c_125_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_130_car", Subtitle = "m70_10c_130_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_140_car", Subtitle = "m70_10c_140_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_145_car", Subtitle = "m70_10c_145_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_010_key", Subtitle = "m70_40c_010_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_020_key", Subtitle = "m70_40c_020_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_030_pla", Subtitle = "m70_40c_030_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_030_plf", Subtitle = "m70_40c_030_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_040_key", Subtitle = "m70_40c_040_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_051_key", Subtitle = "m70_40c_051_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_061_emi", Subtitle = "m70_40c_061_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_071_key", Subtitle = "m70_40c_071_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_081_sg2", Subtitle = "m70_40c_081_sg2"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_101_emi", Subtitle = "m70_40c_101_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_111_emi", Subtitle = "m70_40c_111_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_121_emi", Subtitle = "m70_40c_121_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_131_emi", Subtitle = "m70_40c_131_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_141_emi", Subtitle = "m70_40c_141_emi"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_151_tr1", Subtitle = "m70_40c_151_tr1"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_161_pla", Subtitle = "m70_40c_161_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_161_plf", Subtitle = "m70_40c_161_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_171_pla", Subtitle = "m70_40c_171_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_171_plf", Subtitle = "m70_40c_171_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_181_pla", Subtitle = "m70_40c_181_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_181_plf", Subtitle = "m70_40c_181_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_191_key", Subtitle = "m70_40c_191_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_40c_201_tr4", Subtitle = "m70_40c_201_tr4"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_50c_010_key", Subtitle = "m70_50c_010_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_50c_020_key", Subtitle = "m70_50c_020_key"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_50c_030_pla", Subtitle = "m70_50c_030_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_55c_010_pla", Subtitle = "m70_55c_010_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_010_dot", Subtitle = "m70_10c_010_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_020_dot", Subtitle = "m70_10c_020_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_030_dot", Subtitle = "m70_10c_030_dot"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_040_car", Subtitle = "m70_10c_040_car"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_050_pla", Subtitle = "m70_10c_050_pla"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_050_plf", Subtitle = "m70_10c_050_plf"},
                new RandomizedObjectDetails{Path = @"sound\dialog\levels\m70\cinematic\m70_10c_060_car", Subtitle = "m70_10c_060_car"},
            }
        };
    }
    
}