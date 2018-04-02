using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal
{
    class HSB
    {//djm added, it makes it simpler to have this code in here than in the C#

        public float rChan, gChan, bChan;

        public HSB()
        {
            rChan = gChan = bChan = 0;
        }

        public void fromHSB(float h, float s, float b, int num = 0)
        {
            float red = b;
            float green = b;
            float blue = b;
            if (s != 0)
            {
                float max = b;
                float dif = b * s / 255f;
                float min = b - dif;

                float h2 = h * 360f / 255f;

                if (h2 < 60f)
                {
                    red = max;
                    green = h2 * dif / 60f + min;
                    blue = min;
                }
                else if (h2 < 120f)
                {
                    red = -(h2 - 120f) * dif / 60f + min;
                    green = max;
                    blue = min;
                }
                else if (h2 < 180f)
                {
                    red = min;
                    green = max;
                    blue = (h2 - 120f) * dif / 60f + min;
                }
                else if (h2 < 240f)
                {
                    red = min;
                    green = -(h2 - 240f) * dif / 60f + min;
                    blue = max;
                }
                else if (h2 < 300f)
                {
                    red = (h2 - 240f) * dif / 60f + min;
                    green = min;
                    blue = max;
                }
                else if (h2 <= 360f)
                {
                    red = max;
                    green = min;
                    blue = -(h2 - 360f) * dif / 60 + min;
                }
                else
                {
                    red = 0;
                    green = 0;
                    blue = 0;
                }
            }

            rChan = (float) Math.Round(Math.Min(Math.Max(red, 0f), 255));
            gChan = (float) Math.Round(Math.Min(Math.Max(green, 0), 255));
            bChan = (float) Math.Round(Math.Min(Math.Max(blue, 0), 255));
            

            switch(num)
            {
                case 1:
                    rChan = 50;
                    bChan = 200;
                    break;
                case 2:
                    rChan = 10;
                    break;
                case 3:
                    bChan = 200;
                    break;
                case 4:
                    bChan = 100;
                    break;
                case 5:
                    rChan = 150;
                    break;
                case 6:
                    rChan = 10;
                    bChan = 150;
                    break;
                case 7:
                    rChan = 255;
                    break;
                case 8:
                    bChan = 200;
                    break;
            }
        }
    }
}