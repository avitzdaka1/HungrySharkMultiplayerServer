using System.CodeDom.Compiler;

namespace ServerApp
{
    public class Fruits
    {
        private int x;
        private int y;

        public Fruits(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public override string ToString()
        {
            return "X: " + x + " Y: " + y;
        }
    }
}