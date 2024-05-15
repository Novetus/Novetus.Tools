#region Usings
using Novetus.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
#endregion

namespace Novetus.ReleasePreparer
{
    #region ReleasePreparer
    class ReleasePreparer
    {
        static void Main(string[] args)
        {
            string novpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\\Novetus\\data";

            if (args.Length > 0)
            {
                if (args.Contains("-snapshot"))
                {
                    string infopath = novpath + @"\\config\\info.json";
                    string currver = GetBranch(infopath);
                    TurnOnInitialSequence(infopath);

                    string pathbeta = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\\betaversion.txt";
                    Console.WriteLine("Creating " + pathbeta);
                    if (!File.Exists(pathbeta))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(pathbeta))
                        {
                            sw.Write(currver);
                        }
                    }
                    Console.WriteLine("Created " + pathbeta);
                }
                else if (args.Contains("-release"))
                {
                    DoRelease(novpath);
                }
            }
            else
            {
                DoRelease(novpath);
            }
        }

        public static void DoRelease(string novpath)
        {
            string infopath = novpath + @"\\config\\info.json";
            string currbranch = GetBranch(infopath);
            TurnOnInitialSequence(infopath);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\\releaseversion.txt";
            Console.WriteLine("Creating " + path);
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(currbranch);
                }
            }
            Console.WriteLine("Created " + path);
        }

        public static string GetBranch(string infopath)
        {
            //READ
            string section = "ProgramInfo";
            JSONFile json = new JSONFile(infopath, section, false);
            string versionbranch, extendedVersionNumber, extendedVersionTemplate, extendedVersionRevision, isLite;
            versionbranch = json.JsonReadValue(section, "Branch", "0.0");
            extendedVersionNumber = json.JsonReadValue(section, "ExtendedVersionNumber", "False");
            extendedVersionTemplate = json.JsonReadValue(section, "ExtendedVersionTemplate", "%version%");
            extendedVersionRevision = json.JsonReadValue(section, "ExtendedVersionRevision", "-1");
            isLite = json.JsonReadValue(section, "IsLite", "False");

            string novpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\\Novetus\\data\\bin\\Novetus.exe";

            if (!extendedVersionNumber.Equals("False"))
            {
                var versionInfo = FileVersionInfo.GetVersionInfo(novpath);
                return extendedVersionTemplate.Replace("%version%", versionbranch)
                            .Replace("%build%", versionInfo.ProductBuildPart.ToString())
                            .Replace("%revision%", versionInfo.FilePrivatePart.ToString())
                            .Replace("%extended-revision%", (!extendedVersionRevision.Equals("-1") ? extendedVersionRevision : ""))
                            .Replace("%lite%", (!isLite.Equals("False") ? " (Lite)" : ""));
            }
            else
            {
                return versionbranch;
            }
        }

        public static void TurnOnInitialSequence(string infopath)
        {
            //READ
            string section = "ProgramInfo";
            JSONFile json = new JSONFile(infopath, section, false);

            string initialBootup = json.JsonReadValue(section, "InitialBootup", "True");
            if (Convert.ToBoolean(initialBootup) == false)
            {
                json.JsonWriteValue(section, "InitialBootup", "True");
            }
        }
    }
    #endregion
}
