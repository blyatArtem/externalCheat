using GameOverlay.Drawing;
using GameOverlay.Windows;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLiba.Game
{
    public static class Overlay
    {
        public static void Initialize()
        {
            var gfx = new Graphics()
            {
                MeasureFPS = true,
                PerPrimitiveAntiAliasing = false
                //TextAntiAliasing = true,
            };

            window = new GraphicsWindow(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, gfx)
            {
                IsTopmost = true,
                IsVisible = true,
                FPS = 244
            };


            window.SetupGraphics += (s, e) =>
            {
                Graphics g = e.Graphics;
                Factory factory = g.GetFontFactory();

                mouseLeft = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\mouse\left.png");
                mouseRight = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\mouse\right.png");

                ur = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\0.png");

                s1 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\1.png");
                s2 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\2.png");
                s3 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\3.png");
                s4 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\4.png");
                s5 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\5.png");
                s6 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\6.png");

                n1 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\7.png");
                n2 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\8.png");
                n3 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\9.png");
                n4 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\10.png");

                a1 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\11.png");
                a2 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\12.png");
                a3 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\13.png");

                bs = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\14.png");
                ber = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\15.png");
                lem = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\16.png");
                sup = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\17.png");
                gl = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\ranks\18.png");

                l1 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\1.png");
                l2 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\2.png");
                l3 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\3.png");
                l4 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\4.png");
                l5 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\5.png");
                l6 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\6.png");
                l7 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\7.png");
                l8 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\8.png");
                l9 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\9.png");
                l10 = g.CreateImage(Directory.GetCurrentDirectory() + @"\content\levels\10.png");

                fontText = new GameOverlay.Drawing.Font(factory, "Arial", 13, true);
                fontRadarText = new GameOverlay.Drawing.Font(factory, "Arial", 10, true);
                fontIcons = new GameOverlay.Drawing.Font(factory, "csgo_icons", 16, true);
                fontRadarIcons = new GameOverlay.Drawing.Font(factory, "csgo_icons", 12, true);

                brushText = g.CreateSolidBrush(255, 255, 255, 255);
                brushOutline = g.CreateSolidBrush(new Color(0, 0, 0, 200));
                brushTBG = g.CreateSolidBrush(new Color(236, 72, 100, 50));
                brushCTBG = g.CreateSolidBrush(new Color(176, 195, 217, 50));
                brushT = g.CreateSolidBrush(new Color(236, 72, 100));
                brushCT = g.CreateSolidBrush(new Color(176, 195, 217));
                brushRed = g.CreateSolidBrush(Color.Red);
                brushLime = g.CreateSolidBrush(Color.Green);

                GameData.Initialize(g);
            };


            window.DrawGraphics += (s, e) =>
            {
                UpdateRainbow();
                GameData.FrameAction(e.Graphics, e.FrameCount);
            };
        }

        private static void UpdateRainbow()
        {
            if (r == 255 && g < 255 && b == 0)
                g += speed;
            else if (r > 0 && g == 255 && b == 0)
                r -= speed;
            else if (r == 0 && g == 255 && b < 255)
                b += speed;
            else if (r == 0 && g > 0 && b == 255)
                g -= speed;
            else if (r < 255 && g == 0 && b == 255)
                r += speed;
            else if (r == 255 && g == 0 && b > 0)
                b -= speed;
        }

        public static IBrush GetBrushByVector(Graphics g, Microsoft.Xna.Framework.Vector4 vector) => vector.X >= 0 ? g.CreateSolidBrush(vector.X, vector.Y, vector.Z, vector.W) : GetRainbowBrush(g, vector.W);

        public static IBrush GetRainbowBrush(Graphics g, float alpha) => g.CreateSolidBrush(r, Overlay.g, b, alpha);

        public static void Run()
        {
            window.Create();
            window.Join();
        }

        public static Microsoft.Xna.Framework.Vector3 Size
        {
            get => new Microsoft.Xna.Framework.Vector3(window.Width, window.Height, 0);
        }

        public static GameOverlay.Drawing.Font fontText, fontIcons, fontRadarText, fontRadarIcons;
        public static IBrush brushText, brushOutline, brushT, brushCT, brushTBG, brushCTBG, brushRed, brushLime, brushRainbow;

        public static Image ur;
        public static Image s1, s2, s3, s4, s5, s6;
        public static Image n1, n2, n3, n4;
        public static Image a1, a2, a3, bs;
        public static Image ber, lem, sup, gl;

        public static Image l1, l2, l3, l4, l5, l6, l7, l8, l9, l10;

        public static Image mouseLeft, mouseRight;

        public static GraphicsWindow window;

        private static byte r = 255, g, b, speed = 5;

    }
}
