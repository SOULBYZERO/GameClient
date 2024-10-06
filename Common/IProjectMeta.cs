using System.Windows.Media.Imaging;

namespace Common
{
    public interface IProjectMeta
    {
        public string Name { get; }
        public BitmapImage Image { get; }
        public string AppInfo { get; }
        public void Run();
    }
}
