using System;
using System.Diagnostics;
using static ReachVisualRandomizer.RandomizedItems;
using Bungie;
using Bungie.Tags;
using System.Security.Cryptography.X509Certificates;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Drawing.Text;
using System.Runtime.CompilerServices;

namespace ReachVisualRandomizer
{ 
	public class Randomizer
	{
        public int Seed = 0;
        public string ek_path = "";
		public Randomizer(string ek_path = @"C:\Program Files (x86)\Steam\steamapps\common\HREK")
		{
			Random rand = new Random(Seed);
            this.ek_path = ek_path;

        }

		public void Randomize(RandomizerSettings settings, IProgress<int> progress, IProgress<string> text_progress)
		{
			Debug.WriteLine("Beginning randomization");
            progress.Report(3);
            text_progress.Report("Unzipping game data. This can take 5+ minutes");
            UnzipTags(settings);
            progress.Report(10);
            var param = new ManagedBlamStartupParameters();
            ManagedBlamCrashCallback del = info => { };
            if (!ManagedBlamSystem.IsInitialized)
            {
                ManagedBlamSystem.Start(settings.EkPath, del, param);
            }
            
            text_progress.Report("Giving vehicle and weapon animations for enemies");
            progress.Report(14);
            FixAnimationGraphs();
            text_progress.Report("Fixing jetpack enemies");
            progress.Report(17);
            FixCharacterProperties();
            if (settings.RandomizeCutscenes)
            {
                text_progress.Report("Randomizing cutscenes");
                RandomizeCutscenes(settings);
            }
            int level_no = 0;
            foreach (Level level in Levels)
			{
                level_no++;
                if (level.Randomize)
                {
                    Debug.WriteLine("Beginning randomization for " + level.FancyName);
                    LevelRandomizer level_randomizer = new LevelRandomizer(level, settings);
                    level_randomizer.Randomize();
                    text_progress.Report("Building " + level.FancyName + ". This can take 5+ minutes");
                    level_randomizer.BuildCacheFile();
                    System.Threading.Thread.Sleep(500);
                    level_randomizer.MoveFileToMCC();
                    Debug.WriteLine("Finished randomization for " + level.FancyName);
                    text_progress.Report("The level " + level.FancyName + " is ready to play");
                }
                progress.Report(20 + ((80 / Levels.Count) * level_no));
            }
            ManagedBlamSystem.Stop();
            progress.Report(100);
            Debug.WriteLine("Finished randomization");
        }

        public void UnzipTags(RandomizerSettings settings)
        {
            Debug.WriteLine("Unzipping tags");
            string seven_z_path = "7za.exe";
            try
            {
                ProcessStartInfo process = new ProcessStartInfo();
                process.WindowStyle = ProcessWindowStyle.Hidden;
                process.FileName = seven_z_path;
                process.Arguments = string.Format("x \"{0}\" -o\"{1}\" -y", settings.EkPath + @"\HREK.7z", settings.EkPath);
                Process? x = Process.Start(process);
                x?.WaitForExit();
            }
            catch (System.Exception e)
            {
                throw new Exception("7za.exe failed\n" + e);
            }
        }

        public void FixAnimationGraphs()
        {
            foreach (var animation_graph in animation_graphs)
            {
                Debug.WriteLine("Fixing animation graph: " + animation_graph.Path);
                var tag_path = TagPath.FromPathAndType(animation_graph.Path, "jmad");
                using (TagFile tag_file = new TagFile(tag_path))
                {
                    var content = GetField(tag_file, "content");
                    var modes = ((TagFieldStruct)content).Elements[0].Fields.Where(x => x.DisplayName == "modes").FirstOrDefault();
                    if (modes != null)
                    {
                        //Create missing animation modes from analagous animation modes (ie warthog passenger animation is copied from revenant passenger animation)
                        foreach (List<string> animation_mode_group in animation_mode_groups)
                        {
                            int index = -1;
                            foreach (TagFieldBlockElement mode in ((TagFieldBlock)modes).Elements)
                            {
                                var label = mode.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                if (label != null)
                                {
                                    if (animation_mode_group.Contains(((TagFieldElementStringID)label).Data))
                                    {
                                        index = mode.ElementIndex;
                                        break;
                                    }
                                }
                            }
                            if (index != -1)
                            {
                                foreach (string animation_mode in animation_mode_group)
                                {
                                    bool found = false;
                                    foreach (TagFieldBlockElement mode in ((TagFieldBlock)modes).Elements)
                                    {
                                        var label = mode.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                        if (label != null)
                                        {
                                            if (((TagFieldElementStringID)label).Data == animation_mode)
                                            {
                                                found = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!found)
                                    {
                                        TagFieldBlock modes_block = (TagFieldBlock)modes;
                                        modes_block.CopyElement(index);
                                        modes_block.PasteAppendElement();
                                        var new_mode = modes_block.Elements[modes_block.Elements.Count - 1];
                                        var label = new_mode.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                        if (label != null)
                                        {
                                            ((TagFieldElementStringID)label).Data = animation_mode;
                                        }
                                    }
                                }
                            }
                        }

                        //Add missing weapon classes for each animation mode
                        /*
                        List<string> weapon_classes_found = new List<string>();
                        foreach (TagFieldBlockElement mode in ((TagFieldBlock)modes).Elements)
                        {
                            var weapon_class_block = mode.Fields.Where(x => x.DisplayName == "weapon class").FirstOrDefault();
                            if (weapon_class_block != null)
                            {
                                
                                foreach (var weapon_class_element in ((TagFieldBlock)weapon_class_block).Elements)
                                {
                                    var label = weapon_class_element.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                    if (label != null)
                                    {
                                        string weapon_class_string = ((TagFieldElementStringID)label).Data;
                                        if (weapon_classes.Contains(weapon_class_string))
                                        {
                                            weapon_classes_found.Add(weapon_class_string);
                                        }
                                    }
                                }

                            }
                        }
                        */
                        foreach (TagFieldBlockElement mode in ((TagFieldBlock)modes).Elements)
                        {
                            
                            var weapon_class_block = mode.Fields.Where(x => x.DisplayName == "weapon class").FirstOrDefault();
                            if (weapon_class_block != null)
                            {
                                /*
                                List<string> mode_weapon_classes_found = new List<string>();
                                foreach (var weapon_class_element in ((TagFieldBlock)weapon_class_block).Elements)
                                {
                                    var label = weapon_class_element.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                    if (label != null)
                                    {
                                        string weapon_class_string = ((TagFieldElementStringID)label).Data;
                                        if (weapon_classes.Contains(weapon_class_string))
                                        {
                                            mode_weapon_classes_found.Add(weapon_class_string);
                                        }
                                    }
                                }
                                */
                                foreach (var weapon_class_copy_group in animation_graph.weapon_class_copy_groups)
                                {
                                    foreach (var weapon_class in weapon_class_copy_group.WeaponClassesCopyTo)
                                    {
                                        //if (!mode_weapon_classes_found.Contains(weapon_class))
                                        int copy_weapon_class_index = -1;
                                        foreach (var copy_weapon_class in weapon_class_copy_group.WeaponClassesCopyFrom)
                                        {
                                            foreach (var weapon_class_element in (TagFieldBlock)weapon_class_block)
                                            {
                                                var label = weapon_class_element.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                                if (label != null)
                                                {
                                                    string weapon_class_string = ((TagFieldElementStringID)label).Data;
                                                    if (copy_weapon_class_index == -1 && copy_weapon_class.Contains(weapon_class_string))
                                                    {
                                                        //Debug.WriteLine("copying " + copy_weapon_class + " to " + weapon_class);
                                                        copy_weapon_class_index = weapon_class_element.ElementIndex;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (copy_weapon_class_index != -1)
                                            {
                                                ((TagFieldBlock)weapon_class_block).CopyElement(copy_weapon_class_index);
                                                int tries = 0;
                                                while (!((TagFieldBlock)weapon_class_block).ClipboardContainsBlockElement() && tries < 20)
                                                {
                                                    ((TagFieldBlock)weapon_class_block).CopyElement(copy_weapon_class_index);
                                                    tries++;
                                                    System.Threading.Thread.Sleep(150);

                                                }
                                                if (((TagFieldBlock)weapon_class_block).ClipboardContainsBlockElement())
                                                {
                                                    ((TagFieldBlock)weapon_class_block).PasteAppendElement();
                                                    var new_weapon_class = ((TagFieldBlock)weapon_class_block).Elements[((TagFieldBlock)weapon_class_block).Elements.Count - 1];
                                                    var label = new_weapon_class.Fields.Where(x => x.DisplayName == "label").FirstOrDefault();
                                                    if (label != null)
                                                    {
                                                        ((TagFieldElementStringID)label).Data = weapon_class;
                                                    }
                                                }
                                                else
                                                {
                                                    Debug.WriteLine("weapon class element failed to copy");
                                                    throw new Exception("An animation's weapon class element failed to copy. index: " + copy_weapon_class_index + " block size: " + ((TagFieldBlock)weapon_class_block).Elements.Count);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("modes not found for " + animation_graph);
                    }
                    tag_file.Save();

                }
            }
        }

        public void FixCharacterProperties()
        {

            foreach (var character in characters.List)
            {
                if (character.Name.Contains("jetpack"))
                {
                    var tag_path = TagPath.FromPathAndType(character.Path, "char");
                    using (TagFile tag_file = new TagFile(tag_path))
                    {
                        var movement_block = GetField(tag_file, "movement properties");
                        foreach (var element in ((TagFieldBlock)movement_block).Elements)
                        {
                            var movement_flags = GetField(element, "movement flags");
                            
                            foreach (TagValueFlagItem flag in ((TagFieldFlags)movement_flags).Items)
                            {
                                //Debug.WriteLine("Flag: " + flag.FlagName);
                                if (flag.FlagName == "hop (to cover path segements)" || flag.FlagName == "hop (to end of path)")
                                {
                                    ((TagFieldFlags)movement_flags).RawValue |= (uint)(1 << flag.FlagIndex);
                                }
                            }

                        }
                        tag_file.Save();
                    }
                }
            }
        }

        public void RandomizeCutscenes(RandomizerSettings settings)
        {
            Random rand = new Random(settings.Seed);
            foreach (var cutscene in cutscenes)
            {
                Debug.WriteLine("Randomizing cutscene: " + cutscene);
                var tag_path = TagPath.FromPathAndType(cutscene, "cisc*");
                using (TagFile tag_file = new TagFile(tag_path))
                {
                    var shots_field = GetField(tag_file, "shots");
                    foreach (var shot_element in ((TagFieldBlock)shots_field).Elements)
                    {
                        var dialogue_field = GetField(shot_element, "dialogue");
                        foreach (var dialogue_element in ((TagFieldBlock)dialogue_field).Elements)
                        {
                            RandomizedObjectDetails? random_dialogue = dialogue.GetRandomObjectWeighted(rand);
                            if (random_dialogue != null)
                            {
                                foreach (var field in dialogue_element.Fields)
                                {
                                    if (field.DisplayName == "dialogue" || field.DisplayName == "female dialogue")
                                    {
                                        TagPath random_dialgoue_path = TagPath.FromPathAndType(random_dialogue.Path, "snd!");
                                        ((TagFieldReference)field).Path = random_dialgoue_path;
                                    }
                                    if (field.DisplayName == "subtitle" || field.DisplayName == "female subtitle")
                                    {
                                        ((TagFieldElementStringID)field).Data = random_dialogue.Subtitle;
                                    }
                                }
                            }
                            else
                            {
                                Debug.Print("No random dialogue found");
                            }
                        
                        }
                    }
                    var scene_objects = GetField(tag_file, "objects");
                    foreach (var object_element in ((TagFieldBlock)scene_objects).Elements)
                    {
                        string? object_type_path = null;
                        foreach (var field in object_element.Fields)
                        {
                            if (field.DisplayName == "object type")
                            {
                                if (((TagFieldReference)field).Path != null)
                                {
                                    object_type_path = ((TagFieldReference)field).Path.RelativePath;
                                }
                            }
                        }
                        if (object_type_path != null && bipeds.List.Any(x=> object_type_path == x.Path))
                        {
                            foreach (var field in object_element.Fields)
                            {
                                if (field.DisplayName == "variant name")
                                {
                                    var biped_type = bipeds.List.Where(List => object_type_path == List.Path).FirstOrDefault();
                                    if (biped_type != null)
                                    {
                                        var variant = biped_type.Variants.GetRandomObjectWeighted(rand);
                                        if (variant != null)
                                        {
                                            ((TagFieldElementStringID)field).Data = variant.Name;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    tag_file.Save();
                }
            }
        }


        private class LevelRandomizer
		{
            Level Level;
            string LevelPath;
            string LevelResourcesPath;
            TagFile TagFile;
            Random Rand;

            RandomizerSettings Settings;

            public LevelRandomizer(Level level, RandomizerSettings settings)
			{
                Level = level;
                Settings = settings;
                int seed = settings.Seed;
                foreach (char c in level.Name)
                    seed += c % 100;
                Rand = new Random(settings.Seed);
                LevelPath = @"levels\solo\" + level.Name + @"\" + level.Name;
                LevelResourcesPath = @"levels\solo\" + level.Name + @"\resources\" + level.Name;
                var test_path = Bungie.Tags.TagPath.FromPathAndType(LevelPath, "scnr*");
                TagFile = new Bungie.Tags.TagFile(test_path);
            }

			public void Randomize()
			{
                if (!Level.CutsceneOnly)
                {
                    AddToPalettes();
                    ClearDesignerZones();
                    if (Settings.RandomizeSquads)
                    {
                        RandomizeSquads();
                    }
                    if (Settings.RandomizeStartingProfiles)
                    {
                        RandomizeProfiles();
                    }
                    RandomizeVariousWorldObjects();
                }
                
                Debug.WriteLine("saving scenario");
                TagFile.Save();
                // bad code. ideally disposing the tag file should happen elsewhere
                TagFile.Dispose();
            }

            public void BuildCacheFile()
            {
                Debug.WriteLine("Building cache file");
                
                string tool_path = Settings.EkPath + @"\tool.exe";
                try
                {
                    ProcessStartInfo process = new ProcessStartInfo();
                    process.WindowStyle = ProcessWindowStyle.Hidden;
                    process.FileName = tool_path;
                    process.Arguments = string.Format("build-cache-file " + LevelPath);
                    Process? x = Process.Start(process);
                    x?.WaitForExit();
                }
                catch (System.Exception e)
                {
                    throw new Exception("7za.exe failed\n" + e);
                }
            
            }

            public void CopyFileToMCC()
            {
                string fileToCopy = Settings.EkPath + @"\maps\" + Level.Name + @".map";
                string destinationDirectory = Settings.MCCPath + @"\haloreach\maps\";
                File.Copy(fileToCopy, destinationDirectory + Path.GetFileName(fileToCopy), true);
            }

            public void MoveFileToMCC()
            {
                string fileToMove = Settings.EkPath + @"\maps\" + Level.Name + @".map";
                string destinationDirectory = Settings.MCCPath + @"\haloreach\maps\";
                File.Move(fileToMove, destinationDirectory + Path.GetFileName(fileToMove), true);
            }


            private static void AddObjectsToPalette(TagField palette, RandomizedObjectList randomized_object_list)
            {
                TagFieldBlock palette_block = (TagFieldBlock)palette;
                foreach (var object_type in randomized_object_list.List)
                {
                    {
                        bool found = false;
                        foreach (var element in palette_block.Elements)
                        {
                            if (element.ElementHeaderText.ToLower().Contains(object_type.Name.ToLower()))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            int count = palette_block.Elements.Count;
                            palette_block.AddElement();

                            var tag = ((TagFieldBlockElement)palette_block.Elements[count]);
                            var tgb = (TagFieldReference)tag.Fields[0];

                            tgb.Path = TagPath.FromPathAndType(object_type.Path, randomized_object_list.Type);
                            //tgb.Path = TagPath.FromPathAndExtension(object_type.Path, randomized_object_list.Type);


                        }
                    }
                }
                foreach (var element in palette_block.Elements)
                {
                    foreach (var object_type in randomized_object_list.List)
                    {
                        if (element.ElementHeaderText.ToLower() == object_type.Name.ToLower())
                        {
                            object_type.PaletteIndex = element.ElementIndex;
                            //break;
                            continue;
                        }
                    }
                }
                foreach (var object_type in randomized_object_list.List)
                {
                    if (object_type.PaletteIndex == -1)
                    {
                        Debug.WriteLine(object_type.Name + " was not successfully added to the palette");
                    }
                }
            }

            private static void AddObjectsToTag(TagFile tag_file, RandomizedObjectList randomized_object_list)
            {
                var new_palette = tag_file.Fields.Where(x => x.DisplayName.Contains(randomized_object_list.PaletteName)).FirstOrDefault();
                if (new_palette != null)
                {
                    {
                        AddObjectsToPalette(new_palette, randomized_object_list);
                    }
                }
            }

            private void AddToResourcePalette(RandomizedObjectList randomized_object_list)
            {
                Debug.WriteLine("adding to resource palette: " + randomized_object_list.Name);
                //TagPath tag_path = TagPath.FromPathAndType(LevelResourcesPath, randomized_object_list.ResourceFileType);
                TagPath tag_path = TagPath.FromPathAndExtension(LevelResourcesPath, randomized_object_list.ResourceFileExtension);
                //Debug.WriteLine(tag_path.Type);
                using (TagFile resource_file = new TagFile(tag_path))
                {
                    //Debug.WriteLine(resource_file.Path);
                    AddObjectsToTag(resource_file, randomized_object_list);
                    resource_file.Save();
                }
            }

            private void AddToScenarioPalette(RandomizedObjectList randomized_object_list)
            {
                Debug.WriteLine("adding to scenario palette: " + randomized_object_list.Name);
                AddObjectsToTag(TagFile, randomized_object_list);
            }

            private void AddToPalettes()
            {
                Debug.WriteLine("Adding to Palettes");
                vehicles.ResetIndexes();
                weapons.ResetIndexes();
                equipments.ResetIndexes();
                scenerys.ResetIndexes();
                crates.ResetIndexes();
                machines.ResetIndexes();
                AddToResourcePalette(vehicles);
                AddToResourcePalette(weapons);
                AddToResourcePalette(equipments);
                AddToResourcePalette(scenerys);
                AddToResourcePalette(crates);
                AddToResourcePalette(machines);
                AddToScenarioPalette(vehicles);
                AddToScenarioPalette(characters);
                AddToScenarioPalette(weapons);
                //AddToScenarioPalette(equipments);
            }

            private void RandomizeWorldObjects(RandomizedObjectList randomized_object_list)
            {
                Debug.WriteLine("randomizing world objects: " + randomized_object_list.Name);
                var tag_path = TagPath.FromPathAndExtension(LevelResourcesPath, randomized_object_list.ResourceFileExtension);
                using (var resource_file = new TagFile(tag_path))
                {
                    var tag_field_block = GetField(resource_file, randomized_object_list.Name);
                    foreach (var element in ((TagFieldBlock)tag_field_block).Elements)
                    {
                        var name_field = ((TagElement)element).Fields.Where(x => x.DisplayName == "name").FirstOrDefault();
                        if (name_field != null)
                        {
                            TagFieldBlock names_block = ((TagFieldBlockIndex)name_field).GetReferencedBlock();
                            
                            var index = ((TagFieldBlockIndex)name_field).Value;
                            if (index >= 0)
                            {
                                var name_element = ((TagFieldBlock)names_block).Elements[((TagFieldBlockIndex)name_field).Value];
                                var name_string_field = name_element.Fields.Where(x => x.DisplayName == "name").FirstOrDefault();
                                if (name_string_field != null)
                                {
                                    var name = ((TagFieldElementStringID)name_string_field).Data;
                                    if (skipSpecialObjects.Contains(name.ToLower()))
                                    {
                                        Debug.WriteLine("Skipping randomizing object: " + name);
                                        continue;
                                    }
                                }
                            }
                            
                        }
                        var type_field = ((TagElement)element).Fields.Where(x => x.DisplayName == "type").FirstOrDefault();
                        if (type_field != null)
                        {
                            int index = ((TagFieldBlockIndex)type_field).Value;
                            var object_type = randomized_object_list.List.Where(x => x.PaletteIndex == index).FirstOrDefault();
                            //if (randomized_object_list.List.Any(x => x.PaletteIndex == index))
                            if (object_type != null && (object_type.SubCategory != SubCategory.WeaponStash || Settings.RandomizeWeaponStashTypes))
                            {
                                var randomized_object = randomized_object_list.GetRandomObjectWeighted(Rand, require_palette_index: true);
                                if (randomized_object != null && randomized_object.PaletteIndex != -1)
                                {
                                    ((TagFieldBlockIndex)type_field).Value = randomized_object.PaletteIndex;
                                    TagFieldStruct? weapon_data = (TagFieldStruct?)((TagElement)element).Fields.Where(x => x.DisplayName == "weapon data").FirstOrDefault();
                                    if (weapon_data != null)
                                    {
                                        var roundsLeft = ((TagElement)weapon_data.Elements.First()).Fields.Where(x => x.DisplayName == "rounds left").FirstOrDefault();
                                        if (roundsLeft != null)
                                        {
                                            ((TagFieldElementInteger)roundsLeft).Data = 0;
                                        }
                                        var roundsLoaded = ((TagElement)weapon_data.Elements.First()).Fields.Where(x => x.DisplayName == "rounds loaded").FirstOrDefault();
                                        if (roundsLoaded != null)
                                        {
                                            ((TagFieldElementInteger)roundsLoaded).Data = 0;
                                        }
                                    }
                                    TagFieldStruct? object_data = (TagFieldStruct?)((TagElement)element).Fields.Where(x => x.DisplayName == "object data").FirstOrDefault();
                                    if (object_data != null && randomized_object.SubCategory == SubCategory.ArmorAbility)
                                    {
                                        var rotation = ((TagElement)object_data.Elements.First()).Fields.Where(x => x.DisplayName == "rotation").FirstOrDefault();
                                        if (rotation != null)
                                        {
                                            ((TagFieldElementArraySingle)rotation).Data = [0.0f, 0.0f, 0.0f];
                                            
                                            
                                        }
                                    }
                                }
                            }
                            index = ((TagFieldBlockIndex)type_field).Value;
                            object_type = randomized_object_list.List.Where(x => x.PaletteIndex == index).FirstOrDefault();
                            if (object_type != null)
                            {
                                var variant = object_type.Variants.GetRandomObjectWeighted(Rand);
                                if (variant != null)
                                {
                                    TagFieldStruct? permutationData;
                                    permutationData = (TagFieldStruct?)((TagElement)element).Fields.Where(x => x.DisplayName == "permutation data").FirstOrDefault();
                                    if (permutationData != null)
                                    {
                                        var variant_name_field = ((TagElement)permutationData.Elements.First()).Fields.Where(x => x.DisplayName == "variant name").FirstOrDefault();
                                        if (variant_name_field != null)
                                        {
                                            ((TagFieldElementStringID)variant_name_field).Data = variant.Name;
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                    resource_file.Save();
                }
            }

            private void RandomizeVariousWorldObjects()
            {
                if (Settings.RandomizeVehicles)
                {
                    RandomizeWorldObjects(vehicles);
                }
                if (Settings.RandomizeWeapons)
                {
                    RandomizeWorldObjects(weapons);
                }
                if (Settings.RandomizeEquipments)
                {
                    RandomizeWorldObjects(equipments);
                }
                if (Settings.RandomizeEnvironmentalObjects)
                {
                    RandomizeWorldObjects(scenerys);
                    RandomizeWorldObjects(crates);
                    RandomizeWorldObjects(machines);
                }
                
            }

            private void ClearDesignerZones()
            {
                TagField? designer_zones = GetField(TagFile, "designer zones");
                if (designer_zones != null)
                {
                    foreach (TagElement designer_zone in ((TagFieldBlock)designer_zones).Elements)
                    {
                        foreach (string block_type in new List<string> { "character", "vehicle", "equipment", "weapon", "scenery", "crate" })
                        {
                            var tag_field_block = designer_zone.Fields.Where(x => x.FieldPathWithoutIndexes.Contains("Block:" + block_type)).FirstOrDefault();
                            //var tag_field_block = GetField(designer_zone, block_type);
                            if (tag_field_block != null)
                            {
                                ((TagFieldBlock)tag_field_block).RemoveAllElements();
                            }
                            else
                            {
                                throw new Exception("tag field block not found" + block_type);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("designer zones not found");
                }
            }

            private void RandomizeProfiles()
            {
                var profiles_field = GetField(TagFile, "player starting profile");
                foreach (var profile in ((TagFieldBlock)profiles_field).Elements)
                {
                    var primary_type = weapons.GetRandomObjectWeighted(Rand, require_palette_index: true);
                    var secondary_type = weapons.GetRandomObjectWeighted(Rand, require_palette_index: true);
                    var equipment_type = equipments.GetRandomObjectWeighted(Rand, require_palette_index: true, subcategories: [SubCategory.ArmorAbility]);
                    foreach (var field in profile.Fields)
                    {
                        if (field.FieldName == "primary weapon" && primary_type != null)
                        {
                            ((TagFieldReference)field).Path = TagPath.FromPathAndType(primary_type.Path, "weap");
                        }
                        else if (field.FieldName == "secondary weapon" && secondary_type != null)
                        {
                            ((TagFieldReference)field).Path = TagPath.FromPathAndType(secondary_type.Path, "weap");
                        }
                        else if (field.FieldName == "starting equipment" && equipment_type != null)
                        {
                            ((TagFieldReference)field).Path = TagPath.FromPathAndType(equipment_type.Path, "eqip");
                        }
                        else if (field.FieldName == "primaryrounds loaded" && primary_type != null)
                        {
                            ((TagFieldElementInteger)field).Data = primary_type.AmmoMag;
                        }
                        else if (field.FieldName == "secondaryrounds loaded" && secondary_type != null)
                        {
                            ((TagFieldElementInteger)field).Data = secondary_type.AmmoMag;
                        }
                        else if (field.FieldName == "primaryrounds total" && primary_type != null)
                        {
                            ((TagFieldElementInteger)field).Data = primary_type.AmmoTotal;
                        }
                        else if (field.FieldName == "secondaryrounds total" && secondary_type != null)
                        {
                            ((TagFieldElementInteger)field).Data = secondary_type.AmmoTotal;
                        }
                        var grenadeValues = new List<string> { "starting fragmentation grenade count", "starting plasma grenade count" };
                        if (grenadeValues.Contains(field.FieldName, StringComparer.OrdinalIgnoreCase))
                        {
                            ((TagFieldElementInteger)field).Data = Rand.Next(0, 3);
                        }
                    }
                }
            }

            private void RandomizeSquads()
            {
                TagField? squads = GetField(TagFile, "squads");
                if (squads != null)
                {
                    foreach (TagElement squad in ((TagFieldBlock)squads).Elements)
                    {
                        SquadRandomizer squad_randomizer = new SquadRandomizer(squad, Rand, Settings);
                        squad_randomizer.Randomize();
                    }
                }
            }  

            


            private class SquadRandomizer
            {
                TagElement Squad;
                Random Rand;
                RandomizerSettings Settings;

                string TemplateName;
                int TemplateSquadSize;
                RandomizedObjectDetails? old_vehicle;
                //Faction Faction;
                bool SkipSpecialEnemyTypes = false;

                public SquadRandomizer(TagElement squad, Random rand, RandomizerSettings settings)
                {
                    Squad = squad;
                    Rand = rand;
                    Settings = settings;

                    TemplateName = GetTemplateName();
                    if (!string.IsNullOrEmpty(TemplateName))
                    {
                        TemplateSquadSize = GetTemplateTemplateSquadSize();
                        old_vehicle = GetTemplateVehicleType();
                    }
                    else
                    {
                        TemplateSquadSize = 0;
                    }

                    if (skipSpecialEnemySquads.Any(x => Squad.ElementHeaderText.ToLower().Contains(x.ToLower())))
                    {
                        SkipSpecialEnemyTypes = true;
                    }
                }
                public void Randomize()
                {
                    Debug.WriteLine("randomizing " + Squad.ElementHeaderText);
                    if (skipKeyWords.Any(x => Squad.ElementHeaderText.ToLower().Contains(x.ToLower())) && !forceNoSkipKeyWords.Any(x => Squad.ElementHeaderText.ToLower().Contains(x.ToLower())))
                    {
                        Debug.WriteLine("Skipping squad: " + Squad.ElementHeaderText);
                        return;
                    }
                    if (skipKeyWords.Any(x => TemplateName.ToLower().Contains(x.ToLower())) && !forceNoSkipKeyWords.Any(x => TemplateName.ToLower().Contains(x.ToLower())))
                    {
                        Debug.WriteLine("Skipping squad: " + TemplateName);
                        return;
                    }
                    ClearSpawnPointsOverrides();
                    RemoveTemplate();
                    var designer = Squad.Fields.Where(x => x.DisplayName == "designer").FirstOrDefault();
                    if (designer != null)
                    {
                        var cells = ((TagFieldStruct)designer).Elements[0].Fields[0];
                        if (((TagFieldBlock)cells).Elements.Count == 0)
                        {
                            ((TagFieldBlock)cells).AddElement();
                        }

                        foreach (var cell in ((TagFieldBlock)cells).Elements)
                        {
                            var normal_diff_count = GetNormalDiffCountOfCell(cell);
                            if (normal_diff_count == 0)
                            {
                                if (!string.IsNullOrEmpty(TemplateName))
                                {
                                    normal_diff_count = TemplateSquadSize;
                                }
                                else
                                {
                                    normal_diff_count = Rand.Next(1, 4);
                                }
                            }

                            var characters = cell.Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
                            if (characters != null)
                            {
                                if (!SkipSpecialEnemyTypes)
                                {
                                    RandomizeCellVehicles(cell);
                                }

                                RandomizeCellWeaponsAndCharacters(cell);
                            }

                        }
                    }
                }

                private void RandomizeCellVehicles(TagElement cell)
                {
                    bool give_vehicle = Rand.NextDouble() <= Settings.GiveVehicleChance;
                    var vehicle_field = cell.Fields.Where(x => x.DisplayName == "vehicle type").FirstOrDefault();
                    if (vehicle_field != null)
                    {
                        if (old_vehicle == null)
                        {
                            int vehicle_index = ((TagFieldBlockIndex)vehicle_field).Value;
                            if (vehicle_index != -1)
                            {
                                old_vehicle = vehicles.List.Where(x => x.PaletteIndex == vehicle_index).FirstOrDefault();
                            }
                            
                        }
                        else
                        {
                            Debug.WriteLine("old vehicle: " + old_vehicle.Name + " index: " + old_vehicle.PaletteIndex);
                        }
                        if ((give_vehicle) || (old_vehicle != null))
                        {
                            var new_vehicle = vehicles.GetRandomObjectWeighted(Rand, subcategories: [SubCategory.Land, SubCategory.Air, SubCategory.Civilian], require_palette_index: true);
                            if (old_vehicle != null)
                            {
                                if (old_vehicle.SubCategory == SubCategory.Air)
                                {
                                    new_vehicle = vehicles.GetRandomObjectWeighted(Rand, subcategories: [SubCategory.Air, SubCategory.Space], require_palette_index: true);
                                }
                                if (old_vehicle.SubCategory == SubCategory.Space)
                                {
                                    new_vehicle = vehicles.GetRandomObjectWeighted(Rand, subcategories: [SubCategory.Space, SubCategory.Air], require_palette_index: true);
                                }
                            }
                            if (new_vehicle != null)
                            {
                                Debug.WriteLine("new vehicle: " +  new_vehicle.Name);
                                ((TagFieldBlockIndex)vehicle_field).Value = new_vehicle.PaletteIndex;
                                SetNormalDiffCountOfCell(cell, new_vehicle.Seats);
                                var vehicle_variant_field = cell.Fields.Where(x => x.DisplayName == "vehicle variant").FirstOrDefault();
                                if (vehicle_variant_field != null)
                                {
                                    var variant = new_vehicle.Variants.GetRandomObjectWeighted(Rand);
                                    if (variant != null)
                                    {
                                        Debug.WriteLine("new vehicle variant: " +  variant.Name);
                                        ((TagFieldElementStringID)vehicle_variant_field).Data = variant.Name;
                                    }
                                }
                            }

                        }

                    }
                }

                private void RandomizeCellWeaponsAndCharacters(TagElement cell)
                {
                    var characters_field = cell.Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
                    if (characters_field != null)
                    {
                        RemoveAllWeaponsFromCell(cell);
                        var make_mule = Rand.NextDouble() <= Settings.MakeMuleChance;
                        var make_engineer = Rand.NextDouble() <= Settings.MakeEngineerChance;
                        var make_hunter = Rand.NextDouble() <= Settings.MakeHunterChance;
                        var character_elements = ((TagFieldBlock)characters_field);
                        if (make_mule && !SkipSpecialEnemyTypes)
                        {
                            if (character_elements != null)
                            {
                                var mule = characters.GetRandomObjectWeighted(Rand, names: ["mule"]);
                                if (mule != null)
                                {
                                    AddCharacterToCell(cell, mule.PaletteIndex);
                                }
                            }
                        }
                        else if (make_engineer && !SkipSpecialEnemyTypes)
                        {
                            
                            if (character_elements != null)
                            {
                                var engineer = characters.GetRandomObjectWeighted(Rand, names: ["engineer"]);
                                if (engineer != null)
                                {
                                    AddCharacterToCell(cell, engineer.PaletteIndex);
                                }
                            }
                        }
                        else if (make_hunter && !SkipSpecialEnemyTypes)
                        {
                            if (character_elements != null)
                            {
                                var hunter = characters.GetRandomObjectWeighted(Rand, names: ["hunter"]);
                                if (hunter != null)
                                {
                                    AddCharacterToCell(cell, hunter.PaletteIndex);
                                }
                            }
                        }
                        else
                        {

                            var random_weapon = weapons.GetRandomObjectWeighted(Rand, require_palette_index: true);
                            if (random_weapon != null)
                            {
                                SetWeaponOfCell(cell, random_weapon.PaletteIndex);
                                if (!string.IsNullOrEmpty(TemplateName) && GetNormalDiffCountOfCell(cell) == 0)
                                {
                                    SetNormalDiffCountOfCell(cell, TemplateSquadSize);
                                }
                                List<string> characters_added = new List<string>();
                                RemoveAllCharactersFromCell(cell);
                                while (character_elements.Elements.Count < Settings.CharactersPerCell)
                                {
                                    var count = character_elements.Elements.Count;
                                    character_elements.AddElement();
                                    var tag = ((TagFieldBlockElement)character_elements.Elements[count]);
                                    var tgb = (TagFieldBlockIndex)tag.Fields[1];
                                    var random_character = random_weapon.CompatibleEnemies.GetRandomObjectWeighted(Rand, require_palette_index: true);
                                    if (SkipSpecialEnemyTypes)
                                    {
                                        random_character = random_weapon.CompatibleEnemies.GetRandomObjectWeighted(Rand, names: ["elite", "grunt", "brute", "jackal", "skirmisher", "bugger", "hunter"], require_palette_index: true);
                                    }
                                    if (random_character != null)
                                    {
                                        tgb.Value = random_character.PaletteIndex;
                                        characters_added.Add(random_character.Name);
                                    }
                                }
                                var weapons_field = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
                                if (weapons_field != null)
                                {
                                    var weapon_block = (TagFieldBlock)weapons_field;
                                    while (weapon_block.Elements.Count < Settings.WeaponsPerCell)
                                    {
                                        random_weapon = weapons.GetRandomObjectWeighted(Rand, compatible_character_names: characters_added, require_palette_index: true);
                                        if (random_weapon != null)
                                        {
                                            AddWeaponToCell(cell, random_weapon.PaletteIndex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
                
                private string GetTemplateName()
                {
                    var find = ((TagFieldBlockElement)Squad).Fields.Where(x => x.DisplayName.ToString().ToLower().Contains("squad template index")).FirstOrDefault();
                    if (find != null)
                    {
                        TagFieldElementInteger tfr = (TagFieldElementInteger)find;

                    }
                    var findb = ((Bungie.Tags.TagFieldBlockElement)Squad).Fields.Where(x => x.DisplayName.ToString() == "template").FirstOrDefault();
                    if (findb != null)
                    {
                        TagFieldElementStringIDWithMenu tfr = (TagFieldElementStringIDWithMenu)findb;
                        if (tfr.Data == "")
                        {
                        }
                        else
                        {
                            return tfr.Data;
                        }
                    }
                    return "";
                }


                private int GetTemplateTemplateSquadSize()
                {
                    int enemy_count = 0;
                    if (!string.IsNullOrEmpty(TemplateName))
                    {
                        var template_with_prefix = @"ai\squad_templates\" + TemplateName;
                        var found = SquadTemplates.Where(x => x.ToLower() == template_with_prefix.ToLower()).FirstOrDefault();
                        if (found != null)
                        {
                            var tag_path = TagPath.FromPathAndType(found, "sqtm*");
                            using (var template_tag_file = new TagFile(tag_path))
                            {
                                foreach (var element in template_tag_file.Elements)
                                {
                                    foreach (var field in ((TagFieldBlock)element.Fields[1]).Elements)
                                    {
                                        foreach (var ele_field in field.Fields)
                                        {
                                            if (ele_field.DisplayName.ToString().Contains("normal diff count"))
                                            {
                                                var tfr = (TagFieldElementInteger)ele_field;
                                                enemy_count += (int)tfr.Data;
                                            }
                                        }
                                    }
                                }
                                return enemy_count;
                            }
                        }
                        else
                        {
                           // Debug.WriteLine("Template not found: " + TemplateName);
                        }
                    }
                    return enemy_count;
                }


                private bool TemplateHasVehicle()
                {
                    if (!string.IsNullOrEmpty(TemplateName))
                    {
                        var template_with_prefix = @"ai\squad_templates\" + TemplateName;
                        var found = SquadTemplates.Where(x => x.ToLower() == template_with_prefix.ToLower()).FirstOrDefault();
                        if (found != null)
                        {
                            var tag_path = TagPath.FromPathAndType(found, "sqtm*");
                            using (var template_tag_file = new TagFile(tag_path))
                            {
                                foreach (var element in template_tag_file.Elements)
                                {
                                    foreach (var field in ((TagFieldBlock)element.Fields[1]).Elements)
                                    {
                                        foreach (var ele_field in field.Fields)
                                        {
                                            if (ele_field.DisplayName.ToString().Contains("vehicle type"))
                                            {
                                                var tfr = (TagFieldElementInteger)ele_field;
                                                if (tfr.FieldPath != null)
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Debug.WriteLine("Template not found: " + TemplateName);
                        }
                    }
                    return false;
                }

                private RandomizedObjectDetails? GetTemplateVehicleType()
                {
                    if (!string.IsNullOrEmpty(TemplateName))
                    {
                        var template_with_prefix = @"ai\squad_templates\" + TemplateName;
                        var found = SquadTemplates.Where(x => x.ToLower() == template_with_prefix.ToLower()).FirstOrDefault();
                        if (found != null)
                        {
                            var tag_path = TagPath.FromPathAndType(found, "sqtm*");
                            using (var template_tag_file = new TagFile(tag_path))
                            {
                                foreach (var element in template_tag_file.Elements)
                                {
                                    foreach (var field in ((TagFieldBlock)element.Fields[1]).Elements)
                                    {
                                        foreach (var ele_field in field.Fields)
                                        {
                                            if (ele_field.DisplayName.ToString().Contains("vehicle type"))
                                            {
                                                var tfr = (TagFieldReference)ele_field;
                                                if (tfr.FieldPath != null)
                                                {
                                                    return vehicles.List.Where(x => x.Path == tfr.FieldPath).FirstOrDefault();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Debug.WriteLine("Template not found: " + TemplateName);
                        }
                    }
                    return null;
                }

                private void RemoveTemplate()
                {
                    var find = ((TagFieldBlockElement)Squad).Fields.Where(x => x.DisplayName.ToString().ToLower().Contains("squad template index")).FirstOrDefault();
                    if (find != null)
                    {
                        TagFieldElementInteger tfr = (TagFieldElementInteger)find;
                        tfr.Data = -1;
                    }
                    var findb = ((Bungie.Tags.TagFieldBlockElement)Squad).Fields.Where(x => x.DisplayName.ToString() == "template").FirstOrDefault();
                    if (findb != null)
                    {
                        TagFieldElementStringIDWithMenu tfr = (TagFieldElementStringIDWithMenu)findb;
                        tfr.Data = "";
                    }
                }

                private static int GetNormalDiffCountOfCell(TagElement cell)
                {
                    var normal_diff_count = cell.Fields.Where(x => x.DisplayName == "normal diff count").FirstOrDefault();
                    if (normal_diff_count != null)
                    {
                        var tfr = (TagFieldElementInteger)normal_diff_count;
                        return (int)tfr.Data;
                    }
                    return 0;
                }

                private static void SetNormalDiffCountOfCell(TagElement cell, int count)
                {
                    var normal_diff_count = cell.Fields.Where(x => x.DisplayName == "normal diff count").FirstOrDefault();
                    if (normal_diff_count != null)
                    {
                        var tfr = (TagFieldElementInteger)normal_diff_count;
                        tfr.Data = count;
                    }
                }

                private static void RemoveAllWeaponsFromCell(TagElement cell)
                {
                    var weapons = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
                    if (weapons != null)
                    {
                        ((TagFieldBlock)weapons).RemoveAllElements();
                    }
                }

                private static void SetWeaponOfCell(TagElement cell, int palette_index)
                {
                    var weapon_block = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
                    if (weapon_block != null)
                    {
                        if (((TagFieldBlock)weapon_block).Elements.Count < 1)
                        {
                            ((TagFieldBlock)weapon_block).AddElement();
                        }
                        var weapon_type = ((TagFieldBlock)weapon_block).Elements[0].Fields.Where(x => x.DisplayName == "weapon type").FirstOrDefault();
                        if (weapon_type != null)
                        {
                            ((TagFieldBlockIndex)weapon_type).Value = palette_index;
                        }
                    }
                }

                private static void RemoveAllCharactersFromCell(TagElement cell)
                {
                    var characters = cell.Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
                    if (characters != null)
                    {
                        ((TagFieldBlock)characters).RemoveAllElements();
                    }
                }

                private static void AddWeaponToCell(TagElement cell, int palette_index)
                {
                    var weapon_block = cell.Fields.Where(x => x.DisplayName == "initial weapon").FirstOrDefault();
                    if (weapon_block != null)
                    {
                        if (((TagFieldBlock)weapon_block).Elements.Count >= 8)
                        {
                            return;
                        }
                        var count = ((TagFieldBlock)weapon_block).Elements.Count;
                        ((TagFieldBlock)weapon_block).AddElement();
                        var weapon_type = ((TagFieldBlock)weapon_block).Elements[count].Fields.Where(x => x.DisplayName == "weapon type").FirstOrDefault();
                        if (weapon_type != null)
                        {
                            ((TagFieldBlockIndex)weapon_type).Value = palette_index;
                        }
                    }
                }

                private static void AddCharacterToCell(TagElement cell, int palette_index)
                {
                    var characters = cell.Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
                    if (characters != null)
                    {
                        if (((TagFieldBlock)characters).Elements.Count >= 8)
                        {
                            return;
                        }
                        var count = ((TagFieldBlock)characters).Elements.Count;
                        ((TagFieldBlock)characters).AddElement();
                        var character_type = ((TagFieldBlock)characters).Elements[count].Fields.Where(x => x.DisplayName == "character type").FirstOrDefault();
                        if (character_type != null)
                        {
                            ((TagFieldBlockIndex)character_type).Value = palette_index;
                        }
                    }
                }

                private void ClearSpawnPointsOverrides()
                {
                    var spawn_point_block = GetField(Squad, "spawn points");
                    foreach (TagFieldBlockElement spawn_point in ((TagFieldBlock)spawn_point_block).Elements)
                    {
                        var character_field = GetField(spawn_point, "character type");
                        ((TagFieldBlockIndex)character_field).Value = -1;
                        var weapon_field = GetField(spawn_point, "initial weapon");
                        ((TagFieldBlockIndex)weapon_field).Value = -1;
                        if (!SkipSpecialEnemyTypes)
                        {
                            var vehicle_field = GetField(spawn_point, "vehicle type");
                            if (vehicles.List.Any(x => x.PaletteIndex == ((TagFieldBlockIndex)vehicle_field).Value && ((TagFieldBlockIndex)vehicle_field).Value != -1))
                            {
                                old_vehicle = vehicles.List.Where(x => x.PaletteIndex == ((TagFieldBlockIndex)vehicle_field).Value).FirstOrDefault();
                                ((TagFieldBlockIndex)vehicle_field).Value = -1;
                            }
                            var seat_field = GetField(spawn_point, "seat type");
                            ((TagFieldEnum)seat_field).Value = 0;
                        }
                    }
                }
            }
        }

        private static TagField GetField(TagFile tag_file, string field_name)
        {
            var fields_found = new List<string>();
            foreach (var field in tag_file.Fields)
            {
                fields_found.Add(field.DisplayName);
                if (field.DisplayName == field_name)
                {
                    return field;
                }
            }
            throw new Exception("Field not found\nTag file: " + tag_file.Path  + "\nField: " + field_name + "\nvalid fields:\n" + string.Join("\n", fields_found));
        }

        private static TagField GetField(TagElement tag_element, string field_name)
        {
            foreach (var field in tag_element.Fields)
            {
                if (field.DisplayName == field_name)
                {
                    return field;
                }
            }
            throw new Exception("Field not found: " + field_name);
        }
    }


}
