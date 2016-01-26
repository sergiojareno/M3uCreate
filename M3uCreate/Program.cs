using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace M3uCreate
{
    class Program
    {
        static readonly Encoding encoding = Encoding.GetEncoding(1252);

        static void Main()
        {
            string path = Directory.GetCurrentDirectory();
            ProcessDirectory(path);
        }

        private static void ProcessDirectory(string path)
        {
            Console.WriteLine("Processing " + path);

            // Delete existing M3U
            foreach (var file in Directory.GetFiles(path, "*.m3u")) File.Delete(file);

            // Build the list of audio files
            var regexAudioFiles = new Regex(@"\.(mp3|wma|wav|ogg|mid|midi|flac|m4a|voc|aiff)$", RegexOptions.IgnoreCase);
            var m3UFileName = Path.Combine(path, Path.GetFileName(path) + ".m3u");
            StreamWriter writer = null;
            try
            {
                foreach (var file in Directory.GetFiles(path, "*.*").Where(f => regexAudioFiles.IsMatch(f)).OrderBy(f => f))
                {
                    //Lazy creation of writer so we don't create empty m3u files in directories with no audio files
                    if (writer == null) writer = new StreamWriter(m3UFileName, false, encoding);
                    writer.WriteLine(Path.GetFileName(file));
                }
            }
            finally
            {
                if (writer != null) writer.Close();
            }

            // Process subdirectories
            foreach (var file in Directory.GetDirectories(path)) ProcessDirectory(file);

        }


    }
}
