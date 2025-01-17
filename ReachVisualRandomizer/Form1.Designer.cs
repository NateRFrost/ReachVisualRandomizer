namespace ReachVisualRandomizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            begin_randomization_button = new Button();
            HREKPathBox = new TextBox();
            MCCPathBox = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            randomize_starting_profiles_checkbox = new CheckBox();
            label10 = new Label();
            randomize_cutscenes_checkbox = new CheckBox();
            label9 = new Label();
            engineer_chance_updown = new NumericUpDown();
            label2 = new Label();
            hunter_chance_updown = new NumericUpDown();
            hunter_chance_label = new Label();
            mule_updown = new NumericUpDown();
            mule_chance_label = new Label();
            label8 = new Label();
            seed_box = new TextBox();
            randomize_squads_label = new Label();
            randomize_squads_checkbox = new CheckBox();
            label7 = new Label();
            randomize_weapon_stash_type_checkbox = new CheckBox();
            label6 = new Label();
            randomize_objects_checkbox = new CheckBox();
            label5 = new Label();
            randomize_equipment_checkbox = new CheckBox();
            label4 = new Label();
            randomize_weapons_checkbox = new CheckBox();
            give_vehicle_label = new Label();
            label3 = new Label();
            give_vehicle_updown = new NumericUpDown();
            randomize_vehicles_checkbox = new CheckBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            MCCPathButton = new Button();
            HREKPathButton = new Button();
            progressBar1 = new ProgressBar();
            panel1 = new Panel();
            progress_label = new Label();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)engineer_chance_updown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)hunter_chance_updown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)mule_updown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)give_vehicle_updown).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // begin_randomization_button
            // 
            begin_randomization_button.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            begin_randomization_button.Location = new Point(1380, 455);
            begin_randomization_button.Name = "begin_randomization_button";
            begin_randomization_button.Size = new Size(234, 34);
            begin_randomization_button.TabIndex = 1;
            begin_randomization_button.Text = "Start Randomization";
            begin_randomization_button.UseVisualStyleBackColor = true;
            begin_randomization_button.Click += begin_randomization_button_Click;
            // 
            // HREKPathBox
            // 
            HREKPathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            HREKPathBox.Location = new Point(3, 45);
            HREKPathBox.Name = "HREKPathBox";
            HREKPathBox.RightToLeft = RightToLeft.No;
            HREKPathBox.ScrollBars = ScrollBars.Horizontal;
            HREKPathBox.Size = new Size(575, 31);
            HREKPathBox.TabIndex = 3;
            HREKPathBox.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\HREK";
            // 
            // MCCPathBox
            // 
            MCCPathBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MCCPathBox.Location = new Point(3, 3);
            MCCPathBox.Name = "MCCPathBox";
            MCCPathBox.Size = new Size(575, 31);
            MCCPathBox.TabIndex = 4;
            MCCPathBox.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Halo The Master Chief Collection";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel1.Controls.Add(randomize_starting_profiles_checkbox, 1, 12);
            tableLayoutPanel1.Controls.Add(label10, 0, 12);
            tableLayoutPanel1.Controls.Add(randomize_cutscenes_checkbox, 1, 11);
            tableLayoutPanel1.Controls.Add(label9, 0, 11);
            tableLayoutPanel1.Controls.Add(engineer_chance_updown, 1, 4);
            tableLayoutPanel1.Controls.Add(label2, 0, 4);
            tableLayoutPanel1.Controls.Add(hunter_chance_updown, 1, 3);
            tableLayoutPanel1.Controls.Add(hunter_chance_label, 0, 3);
            tableLayoutPanel1.Controls.Add(mule_updown, 1, 2);
            tableLayoutPanel1.Controls.Add(mule_chance_label, 0, 2);
            tableLayoutPanel1.Controls.Add(label8, 0, 0);
            tableLayoutPanel1.Controls.Add(seed_box, 1, 0);
            tableLayoutPanel1.Controls.Add(randomize_squads_label, 0, 1);
            tableLayoutPanel1.Controls.Add(randomize_squads_checkbox, 1, 1);
            tableLayoutPanel1.Controls.Add(label7, 0, 10);
            tableLayoutPanel1.Controls.Add(randomize_weapon_stash_type_checkbox, 1, 10);
            tableLayoutPanel1.Controls.Add(label6, 0, 9);
            tableLayoutPanel1.Controls.Add(randomize_objects_checkbox, 1, 9);
            tableLayoutPanel1.Controls.Add(label5, 0, 8);
            tableLayoutPanel1.Controls.Add(randomize_equipment_checkbox, 1, 8);
            tableLayoutPanel1.Controls.Add(label4, 0, 7);
            tableLayoutPanel1.Controls.Add(randomize_weapons_checkbox, 1, 7);
            tableLayoutPanel1.Controls.Add(give_vehicle_label, 0, 5);
            tableLayoutPanel1.Controls.Add(label3, 0, 6);
            tableLayoutPanel1.Controls.Add(give_vehicle_updown, 1, 5);
            tableLayoutPanel1.Controls.Add(randomize_vehicles_checkbox, 1, 6);
            tableLayoutPanel1.Location = new Point(12, 12);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 13;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(647, 477);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // randomize_starting_profiles_checkbox
            // 
            randomize_starting_profiles_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_starting_profiles_checkbox.AutoSize = true;
            randomize_starting_profiles_checkbox.Checked = true;
            randomize_starting_profiles_checkbox.CheckState = CheckState.Checked;
            randomize_starting_profiles_checkbox.Location = new Point(407, 436);
            randomize_starting_profiles_checkbox.Name = "randomize_starting_profiles_checkbox";
            randomize_starting_profiles_checkbox.Size = new Size(22, 37);
            randomize_starting_profiles_checkbox.TabIndex = 25;
            randomize_starting_profiles_checkbox.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label10.AutoSize = true;
            label10.Location = new Point(4, 433);
            label10.Name = "label10";
            label10.Size = new Size(241, 43);
            label10.TabIndex = 24;
            label10.Text = "Randomize starting loadouts";
            // 
            // randomize_cutscenes_checkbox
            // 
            randomize_cutscenes_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_cutscenes_checkbox.AutoSize = true;
            randomize_cutscenes_checkbox.Checked = true;
            randomize_cutscenes_checkbox.CheckState = CheckState.Checked;
            randomize_cutscenes_checkbox.Location = new Point(407, 385);
            randomize_cutscenes_checkbox.Name = "randomize_cutscenes_checkbox";
            randomize_cutscenes_checkbox.Size = new Size(22, 44);
            randomize_cutscenes_checkbox.TabIndex = 23;
            randomize_cutscenes_checkbox.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label9.AutoSize = true;
            label9.Location = new Point(4, 382);
            label9.Name = "label9";
            label9.Size = new Size(339, 50);
            label9.TabIndex = 22;
            label9.Text = "Randomize cutscene dialogue and biped variants";
            // 
            // engineer_chance_updown
            // 
            engineer_chance_updown.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            engineer_chance_updown.DecimalPlaces = 3;
            engineer_chance_updown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            engineer_chance_updown.Location = new Point(407, 146);
            engineer_chance_updown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            engineer_chance_updown.Name = "engineer_chance_updown";
            engineer_chance_updown.Size = new Size(180, 31);
            engineer_chance_updown.TabIndex = 9;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(4, 143);
            label2.Name = "label2";
            label2.Size = new Size(312, 37);
            label2.TabIndex = 8;
            label2.Text = "Overwrite squad with engineer chance";
            // 
            // hunter_chance_updown
            // 
            hunter_chance_updown.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            hunter_chance_updown.DecimalPlaces = 3;
            hunter_chance_updown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            hunter_chance_updown.Location = new Point(407, 108);
            hunter_chance_updown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            hunter_chance_updown.Name = "hunter_chance_updown";
            hunter_chance_updown.Size = new Size(180, 31);
            hunter_chance_updown.TabIndex = 7;
            hunter_chance_updown.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            // 
            // hunter_chance_label
            // 
            hunter_chance_label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            hunter_chance_label.AutoSize = true;
            hunter_chance_label.Location = new Point(4, 105);
            hunter_chance_label.Name = "hunter_chance_label";
            hunter_chance_label.Size = new Size(295, 37);
            hunter_chance_label.TabIndex = 6;
            hunter_chance_label.Text = "Overwrite squad with hunter chance";
            // 
            // mule_updown
            // 
            mule_updown.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            mule_updown.DecimalPlaces = 3;
            mule_updown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            mule_updown.Location = new Point(407, 70);
            mule_updown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            mule_updown.Name = "mule_updown";
            mule_updown.Size = new Size(180, 31);
            mule_updown.TabIndex = 5;
            mule_updown.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            // 
            // mule_chance_label
            // 
            mule_chance_label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            mule_chance_label.AutoSize = true;
            mule_chance_label.Location = new Point(4, 67);
            mule_chance_label.Name = "mule_chance_label";
            mule_chance_label.Size = new Size(280, 37);
            mule_chance_label.TabIndex = 4;
            mule_chance_label.Text = "Overwrite squad with guta chance";
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label8.AutoSize = true;
            label8.Location = new Point(4, 1);
            label8.Name = "label8";
            label8.Size = new Size(51, 37);
            label8.TabIndex = 20;
            label8.Text = "Seed";
            // 
            // seed_box
            // 
            seed_box.Location = new Point(407, 4);
            seed_box.Name = "seed_box";
            seed_box.Size = new Size(150, 31);
            seed_box.TabIndex = 21;
            // 
            // randomize_squads_label
            // 
            randomize_squads_label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_squads_label.AutoSize = true;
            randomize_squads_label.Location = new Point(4, 39);
            randomize_squads_label.Name = "randomize_squads_label";
            randomize_squads_label.Size = new Size(221, 27);
            randomize_squads_label.TabIndex = 0;
            randomize_squads_label.Text = "Randomize enemy squads";
            // 
            // randomize_squads_checkbox
            // 
            randomize_squads_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_squads_checkbox.AutoSize = true;
            randomize_squads_checkbox.Checked = true;
            randomize_squads_checkbox.CheckState = CheckState.Checked;
            randomize_squads_checkbox.Location = new Point(407, 42);
            randomize_squads_checkbox.Name = "randomize_squads_checkbox";
            randomize_squads_checkbox.Size = new Size(22, 21);
            randomize_squads_checkbox.TabIndex = 1;
            randomize_squads_checkbox.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label7.AutoSize = true;
            label7.Location = new Point(4, 331);
            label7.Name = "label7";
            label7.Size = new Size(396, 50);
            label7.TabIndex = 18;
            label7.Text = "Weapon stashes won't only randomize to similar variants";
            // 
            // randomize_weapon_stash_type_checkbox
            // 
            randomize_weapon_stash_type_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_weapon_stash_type_checkbox.AutoSize = true;
            randomize_weapon_stash_type_checkbox.Location = new Point(407, 334);
            randomize_weapon_stash_type_checkbox.Name = "randomize_weapon_stash_type_checkbox";
            randomize_weapon_stash_type_checkbox.Size = new Size(22, 44);
            randomize_weapon_stash_type_checkbox.TabIndex = 19;
            randomize_weapon_stash_type_checkbox.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label6.AutoSize = true;
            label6.Location = new Point(4, 303);
            label6.Name = "label6";
            label6.Size = new Size(258, 27);
            label6.TabIndex = 16;
            label6.Text = "Randomize misc. world objects";
            // 
            // randomize_objects_checkbox
            // 
            randomize_objects_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_objects_checkbox.AutoSize = true;
            randomize_objects_checkbox.Checked = true;
            randomize_objects_checkbox.CheckState = CheckState.Checked;
            randomize_objects_checkbox.Location = new Point(407, 306);
            randomize_objects_checkbox.Name = "randomize_objects_checkbox";
            randomize_objects_checkbox.Size = new Size(22, 21);
            randomize_objects_checkbox.TabIndex = 17;
            randomize_objects_checkbox.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new Point(4, 275);
            label5.Name = "label5";
            label5.Size = new Size(242, 27);
            label5.TabIndex = 14;
            label5.Text = "Randomize world equipment";
            // 
            // randomize_equipment_checkbox
            // 
            randomize_equipment_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_equipment_checkbox.AutoSize = true;
            randomize_equipment_checkbox.Checked = true;
            randomize_equipment_checkbox.CheckState = CheckState.Checked;
            randomize_equipment_checkbox.Location = new Point(407, 278);
            randomize_equipment_checkbox.Name = "randomize_equipment_checkbox";
            randomize_equipment_checkbox.Size = new Size(22, 21);
            randomize_equipment_checkbox.TabIndex = 15;
            randomize_equipment_checkbox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(4, 247);
            label4.Name = "label4";
            label4.Size = new Size(227, 27);
            label4.TabIndex = 12;
            label4.Text = "Randomize world weapons";
            // 
            // randomize_weapons_checkbox
            // 
            randomize_weapons_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_weapons_checkbox.AutoSize = true;
            randomize_weapons_checkbox.Checked = true;
            randomize_weapons_checkbox.CheckState = CheckState.Checked;
            randomize_weapons_checkbox.Location = new Point(407, 250);
            randomize_weapons_checkbox.Name = "randomize_weapons_checkbox";
            randomize_weapons_checkbox.Size = new Size(22, 21);
            randomize_weapons_checkbox.TabIndex = 13;
            randomize_weapons_checkbox.UseVisualStyleBackColor = true;
            // 
            // give_vehicle_label
            // 
            give_vehicle_label.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            give_vehicle_label.AutoSize = true;
            give_vehicle_label.Location = new Point(4, 181);
            give_vehicle_label.Name = "give_vehicle_label";
            give_vehicle_label.Size = new Size(217, 37);
            give_vehicle_label.TabIndex = 2;
            give_vehicle_label.Text = "Give squad vehicle chance";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(4, 219);
            label3.Name = "label3";
            label3.Size = new Size(217, 27);
            label3.TabIndex = 10;
            label3.Text = "Randomize world vehicles";
            // 
            // give_vehicle_updown
            // 
            give_vehicle_updown.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            give_vehicle_updown.DecimalPlaces = 3;
            give_vehicle_updown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            give_vehicle_updown.Location = new Point(407, 184);
            give_vehicle_updown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            give_vehicle_updown.Name = "give_vehicle_updown";
            give_vehicle_updown.Size = new Size(180, 31);
            give_vehicle_updown.TabIndex = 3;
            give_vehicle_updown.Value = new decimal(new int[] { 2, 0, 0, 131072 });
            // 
            // randomize_vehicles_checkbox
            // 
            randomize_vehicles_checkbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            randomize_vehicles_checkbox.AutoSize = true;
            randomize_vehicles_checkbox.Checked = true;
            randomize_vehicles_checkbox.CheckState = CheckState.Checked;
            randomize_vehicles_checkbox.Location = new Point(407, 222);
            randomize_vehicles_checkbox.Name = "randomize_vehicles_checkbox";
            randomize_vehicles_checkbox.Size = new Size(22, 21);
            randomize_vehicles_checkbox.TabIndex = 11;
            randomize_vehicles_checkbox.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62.5F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel2.Controls.Add(MCCPathBox, 0, 0);
            tableLayoutPanel2.Controls.Add(HREKPathBox, 0, 1);
            tableLayoutPanel2.Controls.Add(MCCPathButton, 1, 0);
            tableLayoutPanel2.Controls.Add(HREKPathButton, 1, 1);
            tableLayoutPanel2.Location = new Point(684, 343);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(930, 85);
            tableLayoutPanel2.TabIndex = 9;
            // 
            // MCCPathButton
            // 
            MCCPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            MCCPathButton.Location = new Point(584, 3);
            MCCPathButton.Name = "MCCPathButton";
            MCCPathButton.Size = new Size(343, 34);
            MCCPathButton.TabIndex = 5;
            MCCPathButton.Text = "Select MCC Folder";
            MCCPathButton.UseVisualStyleBackColor = true;
            MCCPathButton.Click += MCCPathButton_Click;
            // 
            // HREKPathButton
            // 
            HREKPathButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            HREKPathButton.Location = new Point(584, 45);
            HREKPathButton.Name = "HREKPathButton";
            HREKPathButton.Size = new Size(343, 34);
            HREKPathButton.TabIndex = 6;
            HREKPathButton.Text = "Select HREK Folder";
            HREKPathButton.UseVisualStyleBackColor = true;
            HREKPathButton.Click += HREKPathButton_Click;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            progressBar1.Location = new Point(687, 455);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(654, 34);
            progressBar1.TabIndex = 10;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel1.AutoScroll = true;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(progress_label);
            panel1.Location = new Point(687, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(924, 302);
            panel1.TabIndex = 11;
            // 
            // progress_label
            // 
            progress_label.AutoSize = true;
            progress_label.Location = new Point(13, 13);
            progress_label.Name = "progress_label";
            progress_label.Size = new Size(0, 25);
            progress_label.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(1643, 507);
            Controls.Add(panel1);
            Controls.Add(progressBar1);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(begin_randomization_button);
            ForeColor = SystemColors.ActiveCaptionText;
            Name = "Form1";
            Text = " Reach Visual Randomizer";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)engineer_chance_updown).EndInit();
            ((System.ComponentModel.ISupportInitialize)hunter_chance_updown).EndInit();
            ((System.ComponentModel.ISupportInitialize)mule_updown).EndInit();
            ((System.ComponentModel.ISupportInitialize)give_vehicle_updown).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Button begin_randomization_button;
        private TextBox HREKPathBox;
        private TextBox MCCPathBox;
        private NumericUpDown mule_updown;
        private TableLayoutPanel tableLayoutPanel1;
        private Label randomize_squads_label;
        private CheckBox randomize_squads_checkbox;
        private Label give_vehicle_label;
        private NumericUpDown give_vehicle_updown;
        private TableLayoutPanel tableLayoutPanel2;
        private Button MCCPathButton;
        private Button HREKPathButton;
        private Label mule_chance_label;
        private NumericUpDown engineer_chance_updown;
        private Label label2;
        private NumericUpDown hunter_chance_updown;
        private Label hunter_chance_label;
        private CheckBox randomize_vehicles_checkbox;
        private Label label3;
        private CheckBox randomize_weapons_checkbox;
        private Label label4;
        private CheckBox randomize_equipment_checkbox;
        private Label label5;
        private CheckBox randomize_objects_checkbox;
        private Label label6;
        private CheckBox randomize_weapon_stash_type_checkbox;
        private Label label7;
        private Label label8;
        private TextBox seed_box;
        private CheckBox randomize_cutscenes_checkbox;
        private Label label9;
        private CheckBox randomize_starting_profiles_checkbox;
        private Label label10;
        private ProgressBar progressBar1;
        private Panel panel1;
        private Label progress_label;
    }
}
