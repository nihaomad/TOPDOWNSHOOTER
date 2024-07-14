/* using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TOPDOWNSHOOTER
{
    public class Camera
    {

    }
}
*/

using System.Drawing;

namespace TOPDOWNSHOOTER
{
    public class Camera
    {
        public Point Offset { get; private set; }

        private int screenWidth;
        private int screenHeight;

        public Camera(int screenWidth, int screenHeight)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            Offset = new Point(0, 0);
        }

        public void Update(Point playerPosition)
        {
            Offset = new Point(
                playerPosition.X - screenWidth / 2,
                playerPosition.Y - screenHeight / 2);
        }

        public Point Transform(Point original)
        {
            return new Point(original.X - Offset.X, original.Y - Offset.Y);
        }

        public Rectangle Transform(Rectangle original)
        {
            return new Rectangle(
                original.X - Offset.X,
                original.Y - Offset.Y,
                original.Width,
                original.Height);
        }
    }
}
