using System.IO;
using UnityEngine;

namespace BGS.IO
{
    /// <summary>
    /// This class provides methods for file input/output operations.
    /// </summary>
    public static class IOSystem
    {
        [Tooltip("The path to the directory where files will be saved.")]
        private static readonly string DirPath = $"{Application.persistentDataPath}/";

        /// <summary>
        /// Checks if a file exists at the specified path.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="format">The format of the file (default is "json").</param>
        /// <param name="path">The path to the file (default is "").</param>
        /// <returns>True if the file exists, false otherwise.</returns>
        public static bool Exists(string name, string format = "json", string path = "")
        {
            return Directory.Exists($"{DirPath}{path}") && File.Exists($"{DirPath}{path}{name}.{format}");
        }

        /// <summary>
        /// Saves a file with the specified text at the specified path.
        /// </summary>
        /// <param name="text">The text to save in the file.</param>
        /// <param name="name">The name of the file.</param>
        /// <param name="format">The format of the file (default is "json").</param>
        /// <param name="path">The path to save the file at (default is "").</param>
        public static void SaveFile(string text, string name, string format = "json", string path = "")
        {
            if (!Directory.Exists($"{DirPath}{path}"))
                Directory.CreateDirectory($"{DirPath}{path}");

            File.WriteAllText($"{DirPath}{path}{name}.{format}", text);
        }

        /// <summary>
        /// Loads a file from the specified path.
        /// </summary>
        /// <param name="name">The name of the file.</param>
        /// <param name="format">The format of the file (default is "json").</param>
        /// <param name="path">The path to load the file from (default is "").</param>
        /// <returns>The text from the file, or null if the file does not exist.</returns>
        public static string LoadFile(string name, string format = "json", string path = "")
        {
            if (!Exists(name, format, path))
            {
                Debug.LogError($"File {name}.{format} not found in {DirPath}{path}");
                return null;
            }

            return  File.ReadAllText($"{DirPath}{path}{name}.{format}");
        }
    }
}