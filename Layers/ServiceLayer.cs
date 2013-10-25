using System.Windows.Media;

namespace WpfApplication2.Layers
{
    public enum Tools {Hand, Point, Line, Ellipse, Rectangle, Triangle, Square, Erase, Fill};
    public enum Size { Small, Medium, Large };
    public sealed class ServiceLayer
    {
        private static readonly ServiceLayer Instance = new ServiceLayer();
        ServiceLayer()
        {
            CurrentTool = Tools.Hand;
            CurrentColor = Colors.Black;
            CurrentSize = Size.Medium;
        }
        public Tools CurrentTool;
        public Color CurrentColor;
        public Size CurrentSize;
        public static ServiceLayer GetInstance
        {
            get
            {
                return Instance;
            }
        }
        
    }
}
