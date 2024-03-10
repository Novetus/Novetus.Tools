using System.IO;

namespace Novetus.Core
{
    public static class IOSafe
    {
        public static class File
        {

            public static void Delete(string src)
            {
                if (System.IO.File.Exists(src))
                {
                    System.IO.File.SetAttributes(src, FileAttributes.Normal);
                    System.IO.File.Delete(src);
                }
            }
        }
    }
}
