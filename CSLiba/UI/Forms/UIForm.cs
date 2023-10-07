using CSLiba.UI.ConfigSystem.ConfigStructs;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MaterialSkin.MaterialSkinManager;
using CSLiba.Game.Enumerations;
using CSLiba.UI.Forms;
using CSLiba.UI.ConfigSystem;
using Microsoft.Xna.Framework;
using CSLiba.Core;
using CSLiba.Game;
using System.Threading;
using System.Net;
using System.Reflection;
using CSLiba.Game.Features;
using CSLiba.Game.Objects;

namespace CSLiba.UI
{
    public partial class UIForm : MaterialForm
    {

        public static MaterialSkinManager materialSkinManager;
        private Panel panel;
        private MaterialLabel label;

        public UIForm()
        {
            InitializeComponent();
            Initialize();
            InitializeOffsets();
            LoadConfig();

            //panel = new Panel();
            //label = new MaterialLabel();
            //panel.Dock = DockStyle.Fill;
            //label.Text = "Войдите в аккаунт";
            //label.Dock = DockStyle.Fill;
            //label.TextAlign = ContentAlignment.MiddleCenter;
            //tabPage1.Controls.Add(panel);
            //panel.Controls.Add(label);
            //panel.BringToFront();
            //label.BringToFront();

            //materialButton9.Click += (s, e) =>
            //{
            //    panel.Controls.Remove(label);
            //    tabPage1.Controls.Remove(panel);
            //};
        }

        private void InitializeOffsets()
        {
            //string[] offsetsStr = new WebClient().DownloadString("https://raw.githubusercontent.com/frk1/hazedumper/master/csgo.cs").Split('\n');
            //materialLabel30.Text = offsetsStr[2].Replace(".", ":");
            //materialLabel30.Text += "\n// " + DateTime.UtcNow.ToString("yyyy:MM:dd").Replace(":", "-") + " " + DateTime.UtcNow.ToString("HH:mm:ss:FFFFFF") + " UTC";
            //materialLabel30.Text += Environment.NewLine + Program.hoursLeft + " hours left";

            materialLabel42.Text = "";
            foreach(GameMap map in SmokeHelper.maps)
            {
                materialLabel42.Text += $"{map.name} - {map.helperPoints.Count} smokes {Environment.NewLine}";
            }
        }

        private void Initialize()
        {
            materialSkinManager = Instance;
            materialSkinManager.AddFormToManage(this);

            Height -= 60;
            MaximumSize = Size;
            MinimumSize = Size;
            DrawerWidth = 250;
            MaximizeBox = false;

            sliderRadarX.RangeMax = Screen.PrimaryScreen.Bounds.Width;
            sliderRadarY.RangeMax = Screen.PrimaryScreen.Bounds.Height;

            sliderSpecX.RangeMax = Screen.PrimaryScreen.Bounds.Width;
            sliderSpecY.RangeMax = Screen.PrimaryScreen.Bounds.Height;

            materialTabControl1.SelectedIndexChanged += (s, e) => Text = materialTabControl1.TabPages[materialTabControl1.SelectedIndex].Text;

            sliderSmoothX.onValueChanged += (s, e) => weapon.rcsSmoothX = sliderSmoothX.Value;

            sliderRadarX.onValueChanged += (s, e) => Configuration.current.radarX = sliderRadarX.Value;
            sliderRadarY.onValueChanged += (s, e) => Configuration.current.radarY = sliderRadarY.Value;
            sliderRadarWidth.onValueChanged += (s, e) => Configuration.current.radarW = sliderRadarWidth.Value;
            sliderRadarHeight.onValueChanged += (s, e) => Configuration.current.radarH = sliderRadarHeight.Value;
            materialSlider5.onValueChanged += (s, e) => Configuration.current.radarEntityRadius = materialSlider5.Value;
            materialSlider4.onValueChanged += (s, e) => Configuration.current.radarScale = materialSlider4.Value;

            sliderSpecX.onValueChanged += (s, e) => Configuration.current.specX = sliderSpecX.Value;
            sliderSpecY.onValueChanged += (s, e) => Configuration.current.specY = sliderSpecY.Value;
            sliderSpecWidth.onValueChanged += (s, e) => Configuration.current.specW= sliderSpecWidth.Value;
        }
        
        #region config

        private void LoadConfig()
        {
            SetTheme(Configuration.current.theme);
            LoadWeapon(ConfigSystem.Configuration.current.pistol);

            buttonSelectorPistol.UseAccentColor = true;
            buttonSelectorSMG.UseAccentColor = false;
            buttonSelectorHeavy.UseAccentColor = false;
            buttonSelectorSniper.UseAccentColor = false;
            buttonSelectorRifle.UseAccentColor = false;

            checkboxBoxes.Checked = Configuration.current.boxesEnemy;
            checkboxSkeletons.Checked = Configuration.current.skeletonsEnemy;
            checkboxAimCrosshair.Checked = Configuration.current.aimCrosshair;
            materialCheckbox1.Checked = Configuration.current.health;
            checkboxSpectators.Checked = Configuration.current.spectators;
            checkboxRankViewer.Checked = Configuration.current.rankViewer;
            checkboxLines.Checked = Configuration.current.lines;
            checkboxFov.Checked = Configuration.current.fov;
            checkboxBhop.Checked = Configuration.current.bhop;
            checkboxHitDrawer.Checked = Configuration.current.hitDrawer;
            checkboxBombtimer.Checked = Configuration.current.bombTimer;
            checkboxRadar.Checked = Configuration.current.radar;
            checkBoxWeaponsEsp.Checked = Configuration.current.groundedWeapons;
            checkboxHealthString.Checked = Configuration.current.healthString;
            materialCheckbox2.Checked = Configuration.current.grenades;
            checkboxSmokesHelper.Checked = Configuration.current.smokeHelper;
            checkboxTeam.Checked = Configuration.current.onTeam;
            materialCheckbox4.Checked = Configuration.current.radarRounded;
            materialCheckbox7.Checked = Configuration.current.hitboxesEnemy;

            materialSwitch1.Checked = Configuration.current.nicknames;
            materialSwitch2.Checked = Configuration.current.scopeWarning;
            materialSwitch3.Checked = Configuration.current.defuseWarning;
            materialSwitch4.Checked = Configuration.current.distance;
            materialSwitch5.Checked = Configuration.current.weapons;


            if (Configuration.current.boxesEnemyType == 0)
                materialRadioButton1.Checked = true;
            else if (Configuration.current.boxesEnemyType == 1)
                materialRadioButton2.Checked = true;
            else if (Configuration.current.boxesEnemyType == 2)
                materialRadioButton3.Checked = true;

            if (Configuration.current.healthSide == 0)
                materialRadioButton12.Checked = true;
            else if (Configuration.current.healthSide == 1)
                materialRadioButton13.Checked = true;
            else if (Configuration.current.healthSide == 2)
                materialRadioButton14.Checked = true;

            if (Configuration.current.aimCrosshairStyle == GameOverlay.Drawing.CrosshairStyle.Dot)
                materialRadioButton7.Checked = true;
            else if (Configuration.current.aimCrosshairStyle == GameOverlay.Drawing.CrosshairStyle.Plus)
                materialRadioButton8.Checked = true;
            else if (Configuration.current.aimCrosshairStyle == GameOverlay.Drawing.CrosshairStyle.Cross)
                materialRadioButton10.Checked = true;
            else if (Configuration.current.aimCrosshairStyle == GameOverlay.Drawing.CrosshairStyle.Gap)
                materialRadioButton9.Checked = true;
            else if (Configuration.current.aimCrosshairStyle == GameOverlay.Drawing.CrosshairStyle.Diagonal)
                materialRadioButton11.Checked = true;

            materialSlider1.Value = Configuration.current.aimCrosshairSize;
            materialSlider2.Value = Configuration.current.aimCrosshairStroke;
            sliderRadarX.Value = Configuration.current.radarX;
            sliderRadarY.Value = Configuration.current.radarY;
            sliderRadarWidth.Value = Configuration.current.radarW;
            sliderRadarHeight.Value = Configuration.current.radarH;
            materialSlider4.Value = Configuration.current.radarScale;
            materialSlider5.Value = Configuration.current.radarEntityRadius;
            sliderSpecX.Value = Configuration.current.specX;
            sliderSpecY.Value = Configuration.current.specY;
            sliderSpecWidth.Value = Configuration.current.specW;

            if (DrawerShowIconsWhenHidden)
                Width += 55;

            if (Configuration.current.bhopKey.Count > 0)
                materialLabel7.Text = "Bhop key: " + string.Join(" ", Configuration.current.bhopKey);

            if (Configuration.current.aimToggleKey.Count > 0)
                materialLabel31.Text = "Aim toggle: " + string.Join(" ", Configuration.current.aimToggleKey);

            if (Configuration.current.whToggleKey.Count > 0)
                materialLabel38.Text = "WH toggle: " + string.Join(" ", Configuration.current.whToggleKey);

            RefreshPirtureBoxes();
        }

        public void RefreshPirtureBoxes()
        {
            pictureBox1.BackColor = Configuration.current.boxEnemyColor.ToColor();
            pictureBox2.BackColor = Configuration.current.skeletonEnemyColor.ToColor();
            pictureBox8.BackColor = Configuration.current.spottedBoxEnemyColor.ToColor();
            pictureBox7.BackColor = Configuration.current.spottedSkeletonEnemyColor.ToColor();
            pictureBox9.BackColor = Configuration.current.aimCrosshairColor.ToColor();
            pictureBox4.BackColor = Configuration.current.hitsEspColor.ToColor();
            pictureBox3.BackColor = Configuration.current.radarBackgroundColor.ToColor();
            pictureBox5.BackColor = Configuration.current.radarBorderColor.ToColor();
            pictureBox10.BackColor = Configuration.current.radarLocalPlayerColor.ToColor();
            pictureBox6.BackColor = Configuration.current.radarEnemyColor.ToColor();
            pictureBox11.BackColor = Configuration.current.weaponsEspColor.ToColor();
            pictureBox12.BackColor = Configuration.current.radarGrenadesColor.ToColor();
            pictureBox13.BackColor = Configuration.current.specBackgroundColor.ToColor();
            pictureBox14.BackColor = Configuration.current.fovColor.ToColor();
            pictureBox15.BackColor = Configuration.current.hitboxEnemyColor.ToColor();
            pictureBox16.BackColor = Configuration.current.spottedHitboxEnemyColor.ToColor();
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (!Configuration.Load(out string filename))
            {
                MaterialSnackBar SnackBarMessage2 = new MaterialSnackBar("Error.", ":(", true);
                SnackBarMessage2.Show(this);
                return;
            }
            else
                LoadConfig();
            MaterialSnackBar SnackBarMessage = new MaterialSnackBar("Successfully.", ":)", true);
            SnackBarMessage.Show(this);
        }

        private void materialButton2_Click(object sender, EventArgs e)
        {
            Configuration.Save();
        }

        #endregion

        #region aim 

        private void materialLabel4_Click(object sender, EventArgs e)
        {
            KeyReaderForm krForm = new KeyReaderForm();
            krForm.ShowDialog();
            if (!krForm.Cancel)
            {
                weapon.aimAssistKey = krForm.outKeys;
                materialLabel2.Text = "Aim key: " + string.Join(" ", weapon.aimAssistKey);
            }
            RefreshPirtureBoxes();
        }

        private void materialLabel5_Click(object sender, EventArgs e)
        {
            KeyReaderForm krForm = new KeyReaderForm();
            krForm.ShowDialog();
            if (!krForm.Cancel)
            {
                weapon.triggerbotKey = krForm.outKeys;
                materialLabel3.Text = "Trigger key: " + string.Join(" ", weapon.triggerbotKey);
            }
            RefreshPirtureBoxes();
        }

        public void LoadWeapon(SerializebleWeapon weapon)
        {
            this.weapon = weapon;

            checkboxAimAssist.Checked = weapon.aimAssist;
            checkboxRCS.Checked = weapon.rcs;
            checkboxTargetLock.Checked = weapon.targetLock;
            checkboxShake.Checked = weapon.shake;
            checkboxTriggerbot.Checked = weapon.triggerbot;
            checkboxCanFireCheck.Checked = weapon.canFireCheck;
            materialCheckbox5.Checked = weapon.jumpDelay;
            materialCheckbox3.Checked = weapon.spottedAim;
            materialCheckbox6.Checked = weapon.flashDelay;

            sliderFOV.Value = (int)(weapon.fov * 10);
            sliderXSpeed.Value = weapon.xSpeed;
            sliderYSpeed.Value = weapon.ySpeed;
            sliderXShake.Value = weapon.xShake;
            sliderYShake.Value = weapon.yShake;
            sliderSmoothX.Value = weapon.rcsSmoothX;

            if (weapon.aimBone == 8)
                radioButtonBone8.Checked = true;
            else if (weapon.aimBone == 6)
                radioButtonBone6.Checked = true;

            if (weapon.aimAssistKey.Count > 0)
                materialLabel2.Text = "Aim key: " + string.Join(" ", weapon.aimAssistKey);
            else
                materialLabel2.Text = "AimKey: ";
            if (weapon.triggerbotKey.Count > 0)
                materialLabel3.Text = "Trigger key: " + string.Join(" ", weapon.triggerbotKey);
            else
                materialLabel3.Text = "Trigger key: ";
        }

        public SerializebleWeapon weapon;

        private void buttonSelectorPistol_Click(object sender, EventArgs e)
        {
            buttonSelectorPistol.UseAccentColor = true;
            buttonSelectorSMG.UseAccentColor = false;
            buttonSelectorHeavy.UseAccentColor = false;
            buttonSelectorSniper.UseAccentColor = false;
            buttonSelectorRifle.UseAccentColor = false;
            LoadWeapon(ConfigSystem.Configuration.current.pistol);
        }

        private void buttonSelectorSMG_Click(object sender, EventArgs e)
        {
            buttonSelectorPistol.UseAccentColor = false;
            buttonSelectorSMG.UseAccentColor = true;
            buttonSelectorHeavy.UseAccentColor = false;
            buttonSelectorSniper.UseAccentColor = false;
            buttonSelectorRifle.UseAccentColor = false;
            LoadWeapon(ConfigSystem.Configuration.current.smg);
        }

        private void buttonSelectorHeavy_Click(object sender, EventArgs e)
        {
            buttonSelectorPistol.UseAccentColor = false;
            buttonSelectorSMG.UseAccentColor = false;
            buttonSelectorHeavy.UseAccentColor = true;
            buttonSelectorSniper.UseAccentColor = false;
            buttonSelectorRifle.UseAccentColor = false;
            LoadWeapon(ConfigSystem.Configuration.current.heavy);
        }

        private void buttonSelectorRifle_Click(object sender, EventArgs e)
        {
            buttonSelectorPistol.UseAccentColor = false;
            buttonSelectorSMG.UseAccentColor = false;
            buttonSelectorHeavy.UseAccentColor = false;
            buttonSelectorSniper.UseAccentColor = false;
            buttonSelectorRifle.UseAccentColor = true;
            LoadWeapon(ConfigSystem.Configuration.current.rifle);
        }

        private void buttonSelectorSniper_Click(object sender, EventArgs e)
        {
            buttonSelectorPistol.UseAccentColor = false;
            buttonSelectorSMG.UseAccentColor = false;
            buttonSelectorHeavy.UseAccentColor = false;
            buttonSelectorSniper.UseAccentColor = true;
            buttonSelectorRifle.UseAccentColor = false;
            LoadWeapon(ConfigSystem.Configuration.current.sniper);
        }

        private void sliderFOV_onValueChanged(object sender, int newValue)
        {
            weapon.fov = sliderFOV.Value / 10f;
        }

        private void sliderXSpeed_onValueChanged(object sender, int newValue)
        {
            weapon.xSpeed = sliderXSpeed.Value;
        }

        private void sliderYSpeed_onValueChanged(object sender, int newValue)
        {
            weapon.ySpeed = sliderYSpeed.Value;
        }

        private void sliderXShake_onValueChanged(object sender, int newValue)
        {
            weapon.xShake = sliderXShake.Value;
        }

        private void sliderYShake_onValueChanged(object sender, int newValue)
        {
            weapon.yShake = sliderYShake.Value;
        }

        private void checkboxAimAssist_CheckedChanged(object sender, EventArgs e)
        {
            weapon.aimAssist = checkboxAimAssist.Checked;
        }

        private void checkboxRCS_CheckedChanged(object sender, EventArgs e)
        {
            weapon.rcs = checkboxRCS.Checked;
        }

        private void materialCheckbox3_CheckedChanged_2(object sender, EventArgs e)
        {
            weapon.spottedAim = materialCheckbox3.Checked;
        }

        private void radioButtonBone8_CheckedChanged(object sender, EventArgs e)
        {
            weapon.aimBone = 8;
        }

        private void radioButtonBone6_CheckedChanged(object sender, EventArgs e)
        {
            weapon.aimBone = 6;
        }


        private void materialCheckbox6_CheckedChanged(object sender, EventArgs e)
        {
            weapon.flashDelay = materialCheckbox6.Checked;
        }

        private void checkboxShake_CheckedChanged(object sender, EventArgs e)
        {
            weapon.shake = checkboxShake.Checked;
        }

        private void checkboxTriggerbot_CheckedChanged(object sender, EventArgs e)
        {
            weapon.triggerbot = checkboxTriggerbot.Checked;
        }

        private void checkboxCanFireCheck_CheckedChanged(object sender, EventArgs e)
        {
            weapon.canFireCheck = checkboxCanFireCheck.Checked;
        }

        private void checkboxTargetLock_CheckedChanged(object sender, EventArgs e)
        {
            weapon.targetLock = checkboxTargetLock.Checked;
        }

        #endregion

        #region esp
        private void materialCheckbox3_CheckedChanged_1(object sender, EventArgs e)
        {
            Configuration.current.onTeam = checkboxTeam.Checked;
        }

        private void checkboxBoxes_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.boxesEnemy = checkboxBoxes.Checked;
        }

        private void checkboxSkeletons_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.skeletonsEnemy = checkboxSkeletons.Checked;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox1);
            if (color != Vector4.Zero)
                Configuration.current.boxEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox2);
            if (color != Vector4.Zero)
                Configuration.current.skeletonEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox8);
            if (color != Vector4.Zero)
                Configuration.current.spottedBoxEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox7);
            if (color != Vector4.Zero)
                Configuration.current.spottedSkeletonEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void materialRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.boxesEnemyType = 0;
        }

        private void materialRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.boxesEnemyType = 1;
        }

        private void materialRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.boxesEnemyType = 2;
        }

        private void materialCheckbox1_CheckedChanged_1(object sender, EventArgs e)
        {
            Configuration.current.health = materialCheckbox1.Checked;
        }

        private void materialRadioButton12_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.healthSide = 0;
        }

        private void materialRadioButton13_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.healthSide = 1;
        }

        private void materialRadioButton14_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.healthSide = 2;
        }
        private void checkboxLines_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.lines = checkboxLines.Checked;
        }

        #endregion

        #region crosshair

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox9);
            if (color != Vector4.Zero)
                Configuration.current.aimCrosshairColor = color;
            RefreshPirtureBoxes();
        }

        private void checkboxAimCrosshair_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.aimCrosshair = checkboxAimCrosshair.Checked;
        }

        private void materialSlider1_onValueChanged(object sender, int newValue)
        {
            Configuration.current.aimCrosshairSize = materialSlider1.Value;
        }

        private void materialSlider2_onValueChanged(object sender, int newValue)
        {
            Configuration.current.aimCrosshairStroke = materialSlider2.Value;
        }

        private void materialRadioButton7_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.aimCrosshairStyle = GameOverlay.Drawing.CrosshairStyle.Dot;
        }

        private void materialRadioButton8_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.aimCrosshairStyle = GameOverlay.Drawing.CrosshairStyle.Plus;
        }

        private void materialRadioButton10_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.aimCrosshairStyle = GameOverlay.Drawing.CrosshairStyle.Cross;
        }

        private void materialRadioButton9_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.aimCrosshairStyle = GameOverlay.Drawing.CrosshairStyle.Gap;
        }

        private void materialRadioButton11_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.aimCrosshairStyle = GameOverlay.Drawing.CrosshairStyle.Diagonal;
        }

        #endregion

        #region themes

        private void materialButton3_Click(object sender, EventArgs e)
        {
            Configuration.current.theme = FormTheme.Default;
            SetTheme(Configuration.current.theme);
        }

        private void materialButton4_Click(object sender, EventArgs e)
        {
            Configuration.current.theme = FormTheme.DarkGreen;
            SetTheme(Configuration.current.theme);
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            Configuration.current.theme = FormTheme.DarkRed;
            SetTheme(Configuration.current.theme);
        }

        private void materialButton6_Click(object sender, EventArgs e)
        {
            Configuration.current.theme = FormTheme.DarkYellow;
            SetTheme(Configuration.current.theme);
        }

        private void SetTheme(FormTheme theme = FormTheme.Default)
        {
            if (theme == FormTheme.Default)
            {
                materialSkinManager.Theme = Themes.LIGHT;
                materialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Indigo500,
                    Primary.Indigo700,
                    Primary.Indigo100,
                    Accent.Indigo400,
                    TextShade.WHITE);
            }
            else if (theme == FormTheme.DarkGreen)
            {
                materialSkinManager.Theme = Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Teal500,
                    Primary.Teal700,
                    Primary.Teal200,
                    Accent.Teal400,
                    TextShade.WHITE);
            }
            else if (theme == FormTheme.DarkRed)
            {
                materialSkinManager.Theme = Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Red300,
                    Primary.Red500,
                    Primary.Red100,
                    Accent.Red200,
                    TextShade.WHITE);
            }
            else if (theme == FormTheme.DarkYellow)
            {
                materialSkinManager.Theme = Themes.DARK;
                materialSkinManager.ColorScheme = new ColorScheme(
                    Primary.Yellow400,
                    Primary.Yellow300,
                    Primary.Yellow500,
                    Accent.Yellow200,
                    TextShade.BLACK);
            }

            Invalidate();
            RefreshPirtureBoxes();
        }

        #endregion

        #region esp string

        private void checkboxHealthString_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.healthString = checkboxHealthString.Checked;
        }

        private void materialSwitch1_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.nicknames = materialSwitch1.Checked;
        }

        private void materialSwitch2_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.scopeWarning = materialSwitch2.Checked;
        }

        private void materialSwitch4_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.distance = materialSwitch4.Checked;
        }

        private void materialSwitch3_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.defuseWarning = materialSwitch3.Checked;
        }

        private void materialSwitch5_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.weapons = materialSwitch5.Checked;
        }

        #endregion

        #region misc

        private void materialSwitch6_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.displayThreads = materialSwitch6.Checked;
        }

        private void materialSlider3_onValueChanged(object sender, int newValue)
        {
            Overlay.window.FPS = materialSlider3.Value + 30;
        }

        private void materialCheckbox2_CheckedChanged_1(object sender, EventArgs e)
        {
            Configuration.current.spectators = checkboxSpectators.Checked;
        }

        private void checkboxRankViewer_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.rankViewer = checkboxRankViewer.Checked;
        }
        private void materialCheckbox2_CheckedChanged_2(object sender, EventArgs e)
        {
            Configuration.current.fov = checkboxFov.Checked;
        }

        private void materialCheckbox2_CheckedChanged_3(object sender, EventArgs e)
        {
            Configuration.current.bhop = checkboxBhop.Checked;
        }

        private void materialLabel6_Click(object sender, EventArgs e)
        {
            KeyReaderForm krForm = new KeyReaderForm();
            krForm.ShowDialog();
            if (!krForm.Cancel)
            {
                Configuration.current.bhopKey = krForm.outKeys;
                materialLabel7.Text = "Bhop key: " + string.Join(" ", Configuration.current.bhopKey);
            }
            RefreshPirtureBoxes();
        }

        private void checkboxBombtimer_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.bombTimer = checkboxBombtimer.Checked;
        }

        #endregion

        #region WorldEsp

        private void materialCheckbox3_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.smokeHelper = checkboxSmokesHelper.Checked;
        }

        private void materialCheckbox2_CheckedChanged_4(object sender, EventArgs e)
        {
            Configuration.current.hitDrawer = checkboxHitDrawer.Checked;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox4);
            if (color != Vector4.Zero)
                Configuration.current.hitsEspColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox11);
            if (color != Vector4.Zero)
                Configuration.current.weaponsEspColor = color;
            RefreshPirtureBoxes();
        }

        private void materialCheckbox2_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.groundedWeapons = checkBoxWeaponsEsp.Checked;
            lock(EspWorld.block)
            {
                EspWorld.items.Clear();
            }
        }

        #endregion

        #region radar

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox12);
            if (color != Vector4.Zero)
                Configuration.current.radarGrenadesColor = color;
            RefreshPirtureBoxes();
        }

        private void checkboxRadar_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.radar = checkboxRadar.Checked;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox3);
            if (color != Vector4.Zero)
                Configuration.current.radarBackgroundColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox5);
            if (color != Vector4.Zero)
                Configuration.current.radarBorderColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox10);
            if (color != Vector4.Zero)
                Configuration.current.radarLocalPlayerColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox6);
            if (color != Vector4.Zero)
                Configuration.current.radarEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void materialCheckbox2_CheckedChanged_5(object sender, EventArgs e)
        {
            Configuration.current.grenades = materialCheckbox2.Checked;
        }

        #endregion

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox13);
            if (color != Vector4.Zero)
                Configuration.current.specBackgroundColor = color;
            RefreshPirtureBoxes();
        }

        private void materialButton7_Click(object sender, EventArgs e)
        {
            Thread STAThread = new Thread(
               delegate ()
               {
                   Clipboard.SetText($"{GameData.data.localPlayer.origin.X}f? {GameData.data.localPlayer.origin.Y}f? {GameData.data.localPlayer.origin.Z}f".Replace(",", ".").Replace("?", ","));
               });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            Thread STAThread = new Thread(
                  delegate ()
                  {
                      Clipboard.SetText($"{GameData.data.localPlayer.EyePosition.X}f? {GameData.data.localPlayer.EyePosition.Y}f? {GameData.data.localPlayer.EyePosition.Z}f".Replace(",", ".").Replace("?", ","));
                  });
            STAThread.SetApartmentState(ApartmentState.STA);
            STAThread.Start();
            STAThread.Join();
        }

        private void materialCheckbox4_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.radarRounded = materialCheckbox4.Checked;
        }

        private void materialCheckbox5_CheckedChanged(object sender, EventArgs e)
        {
            weapon.jumpDelay = materialCheckbox5.Checked;
        }

        private void materialLabel36_Click(object sender, EventArgs e)
        {
            KeyReaderForm krForm = new KeyReaderForm();
            krForm.ShowDialog();
            if (!krForm.Cancel)
            {
                Configuration.current.aimToggleKey = krForm.outKeys;
                materialLabel31.Text = "Aim toggle: " + string.Join(" ", Configuration.current.aimToggleKey);
            }
            RefreshPirtureBoxes();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox14);
            if (color != Vector4.Zero)
                Configuration.current.fovColor = color;
            RefreshPirtureBoxes();
        }

        private void materialLabel39_Click(object sender, EventArgs e)
        {
            KeyReaderForm krForm = new KeyReaderForm();
            krForm.ShowDialog();
            if (!krForm.Cancel)
            {
                Configuration.current.whToggleKey = krForm.outKeys;
                materialLabel38.Text = "WH toggle: " + string.Join(" ", Configuration.current.whToggleKey);
            }
            RefreshPirtureBoxes();
        }

        private void materialCheckbox7_CheckedChanged(object sender, EventArgs e)
        {
            Configuration.current.hitboxesEnemy = materialCheckbox7.Checked;
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox15);
            if (color != Vector4.Zero)
                Configuration.current.hitboxEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {
            Vector4 color = ColorReaderForm.GetColor(pictureBox16);
            if (color != Vector4.Zero)
                Configuration.current.spottedHitboxEnemyColor = color;
            RefreshPirtureBoxes();
        }

        private void materialButton9_Click(object sender, EventArgs e)
        {

        }
    }
}
