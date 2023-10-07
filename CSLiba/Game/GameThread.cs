using GameOverlay.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSLiba.Game
{
    public class GameThread
    {
        public GameThread(Action method, string name, Graphics g, bool sta = false)
        {
            brush = g.CreateSolidBrush(threadColors[thrIndex]);
            thrIndex++;

            this.name = name;
            thread = new Thread(delegate ()
            {
                Stopwatch stopwatch = new Stopwatch();
                while (true)
                {
                    //stopwatch.Start();
                    method.Invoke();
                    //stopwatch.Stop();

                    //elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

                    //stopwatch.Restart();
                    Thread.Sleep(1);
                }
            });
            if (sta)
                thread.SetApartmentState(ApartmentState.STA);
            thread.Name = name;
            thread.Start();
        }

        public void RememberValue()
        {
            msList.Add(elapsedMilliseconds);
            if (msList.Count > 40)
            {
                msList.RemoveAt(0);
            }
        }

        public List<float> msList = new List<float>();

        public static List<Color> threadColors = new List<Color>
        {
            new Color(178, 0, 211),
            new Color(236, 72, 100),
            new Color(0, 137, 123),
            new Color(176, 195, 217),
            new Color(235, 174, 39),
            new Color(212, 85, 66),
        };
        public static int thrIndex = 0;

        public IBrush brush;
        public Thread thread;
        public string name;
        public float elapsedMilliseconds;

    }
}
