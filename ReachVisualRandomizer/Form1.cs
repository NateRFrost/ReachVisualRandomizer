using System.Diagnostics;
using System.Security.Cryptography;
using Microsoft.Win32;
using static ReachVisualRandomizer.Randomizer;

namespace ReachVisualRandomizer
{
    public partial class Form1 : Form
    {
        Randomizer Randomizer = new Randomizer();
        RandomizerSettings Settings = new RandomizerSettings();

        public Form1()
        {
            InitializeComponent();
        }

        public static string? GetSteamPath()
        {
            const string steamRegistryKey = @"Software\Valve\Steam";
            const string steamRegistryValue = "SteamPath";

            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(steamRegistryKey))
            {
                if (key != null)
                {
                    object? value = key.GetValue(steamRegistryValue);
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }

            return null;
        }

        private static string SelectFolder()
        {
            using (FolderBrowserDialog folder_browser_dialogue = new FolderBrowserDialog())
            {
                folder_browser_dialogue.InitialDirectory = @"c:\";
                var steam_apps_path = GetSteamPath();
                if (steam_apps_path != null)
                {
                    Debug.WriteLine(steam_apps_path);
                    folder_browser_dialogue.InitialDirectory = steam_apps_path + @"\steamapps\common";
                }
                else
                {
                    Debug.WriteLine("steam path not found from registery");
                }
                if (folder_browser_dialogue.ShowDialog() == DialogResult.OK)
                {
                    return folder_browser_dialogue.SelectedPath;
                }
            }
            return "";
        }

        private void MCCPathButton_Click(object sender, EventArgs e)
        {
            MCCPathBox.Text = SelectFolder();
        }

        private void HREKPathButton_Click(object sender, EventArgs e)
        {
            HREKPathBox.Text = SelectFolder();
        }

        private async void begin_randomization_button_Click(object sender, EventArgs e)
        {
            begin_randomization_button.Enabled = false;
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            var progress = new Progress<int>(v => { progressBar1.Value = v; });
            var text_progress = new Progress<string>(v => { progress_label.Text += v + "\n"; });
            await Task.Run(() => begin_randomization(progress, text_progress));
            begin_randomization_button.Enabled = true;
        }

        private void begin_randomization(IProgress<int> progress, IProgress<string> text_progress)
        {
            int seed = 0;
            foreach (char c in seed_box.Text)
            {
                seed += c % 200;
            }
            Settings.Seed = seed;
            Settings.MCCPath = MCCPathBox.Text;
            Settings.EkPath = HREKPathBox.Text;
            Settings.RandomizeSquads = randomize_squads_checkbox.Checked;
            Settings.GiveVehicleChance = (float)give_vehicle_updown.Value;
            Settings.MakeMuleChance = (float)mule_updown.Value;
            Settings.MakeHunterChance = (float)hunter_chance_updown.Value;
            Settings.MakeEngineerChance = (float)engineer_chance_updown.Value;
            Settings.RandomizeVehicles = randomize_vehicles_checkbox.Checked;
            Settings.RandomizeWeapons = randomize_weapons_checkbox.Checked;
            Settings.RandomizeEquipments = randomize_equipment_checkbox.Checked;
            Settings.RandomizeEnvironmentalObjects = randomize_objects_checkbox.Checked;
            Settings.RandomizeWeaponStashTypes = randomize_weapon_stash_type_checkbox.Checked;
            Settings.RandomizeCutscenes = randomize_cutscenes_checkbox.Checked;
            Settings.RandomizeStartingProfiles = randomize_starting_profiles_checkbox.Checked;
            Randomizer.Randomize(Settings, progress, text_progress);
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
