using HtmlMinifier;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace CopyFiles
{
    class Program
    {
        /// <summary>
        /// Author: Vivek Gupta
        /// Date : 24 Feb 2016
        /// Description: To make Release Folder to deploy on production
        /// </summary>

        //files extensions which are to be ignored
        static string[] ignoreFiles = ConfigurationManager.AppSettings["IgnoreFiles"].ToString().Split(',');

        //folders which are to be ignored
        static string[] ignoreFolders = ConfigurationManager.AppSettings["IgnoreFolders"].ToString().Split(',');

        static bool buildSolution = Convert.ToBoolean(ConfigurationManager.AppSettings["buildSolution"].ToString());

        static void Main(string[] args)
        {
            bool isMinify = false;
            string path = string.Empty, copyPath = string.Empty, sourcePath = string.Empty;

            DateTime fromDateTime = DateTime.Now.AddDays(-7); //default value for release note is one week

            if (args != null && args.Length > 0)
            {
                fromDateTime = DateTime.Now.AddDays(-Convert.ToSByte(args[0]));

                if (args.Length > 1)
                {
                    path = args[1];
                    path = path.Trim();

                    if (!String.IsNullOrEmpty(path))
                    {
                        isMinify = !path.ToUpper().Contains("OPR");
                        copyPath = String.Format(@"{0}..\..\Bikewale{1}-Releases\Content\website\", path, isMinify ? string.Empty : "OPR");
                    }
                }
            }

            //calling function to copy files
            CopyAllFiles(path, copyPath, fromDateTime, isMinify);

            sourcePath = String.Format(@"{0}build\", copyPath);
            MoveBuildFolderContents(copyPath, sourcePath);

            sourcePath = String.Format(@"{0}build\", copyPath.Replace(@"\website\", @"\cdn\"));
            MoveBuildFolderContents(string.Format(@"{0}..\cdn\", copyPath), sourcePath);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 17th July 2017
        /// Description : MOve build contents to actual folder structure
        /// </summary>
        /// <param name="targetPath"></param>
        private static void MoveBuildFolderContents(string targetPath, string sourcePath)
        {

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            if (System.IO.Directory.Exists(targetPath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                foreach (string s in files)
                {
                    System.IO.File.Copy(s, System.IO.Path.Combine(targetPath, System.IO.Path.GetFileName(s)), true);
                }

                //it will read all folders(directories) inside the folder and then call itself to create sub folders
                foreach (string directories in Directory.GetDirectories(sourcePath))
                {
                    string folderName = directories.Replace(sourcePath, "");
                    string newCopyPath = targetPath + folderName;

                    if (Convert.ToInt32(Array.IndexOf(ignoreFolders, folderName)) < 0)
                    {
                        newCopyPath = newCopyPath + @"\";
                        MoveBuildFolderContents(newCopyPath, directories);//calling itself
                    }
                }

                Directory.Delete(sourcePath, true);

            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
        }

        /// <summary>
        /// Modified By : Sushil Kumar on 17th July 2017
        /// Description : Minifies the contents of the given view.
        /// </summary>
        /// <param name="filePath"> The file path. </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string MinifyHtml(string filePath, Features features)
        {
            using (var reader = new StreamReader(filePath))
            {
                return reader.MinifyHtmlCode(features);
            }
        }

        /// <summary>
        /// Craeted By : vivek gupta
        /// Date : 24 feb 2016
        /// Desc: this function works recursively to create folders and sub folders and copy files to respective folders
        /// Modified By : Sushil Kumar 
        /// Description : Added minification of cshtml,html,aspx,ascx and other files 
        /// Modified By : Sushil Kumar on 17th July 2017
        /// Description : Change cdn folder name
        /// </summary>
        /// <param name="path"></param>
        /// <param name="copyPath"></param>
        public static void CopyAllFiles(string path, string targetPath, DateTime fromDateTime, bool isMinify)
        {
            var features = new Features(null);

            path.Replace(@"\\", @"\");
            targetPath.Replace(@"\\", @"\");
            //it will read all files inside the folder
            foreach (string fileName in Directory.GetFiles(path))
            {

                //files extension to check ignored file list
                string fileExtension = Path.GetExtension(fileName);

                //date when file was last written to check how many days older file need to copy
                DateTime lastModifiedDate = File.GetLastWriteTime(fileName);

                //required checks
                if (!string.IsNullOrEmpty(fileExtension) && Convert.ToInt32(Array.IndexOf(ignoreFiles, fileExtension)) < 0 /*&& lastModifiedDate >= fromDateTime*/)
                {
                    //if folder does not exist, it wil create new one
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    string filePathName = Path.GetFileName(fileName);

                    //copy files to the respective folders, it will even overwrite files if already exists
                    if ((!fileExtension.Equals(".config") && !fileExtension.Equals(".xml")) || filePathName.Equals("Web.config") || filePathName.Equals("rewriterules.config") || filePathName.Equals("web_browsers_patch.xml") || filePathName.Equals("wurfl.xml") || filePathName.Equals("BingSiteAuth.xml"))
                    {
                        File.Copy(fileName, targetPath + filePathName, true);
                    }


                    if (fileExtension.Equals(".js") || fileExtension.Equals(".css") || filePathName.Contains("appshell") || filePathName.Contains("woff") || filePathName.Contains("woff2"))
                    {

                        string newTargetPath = targetPath.Replace(@"\website\", @"\cdn\");
                        if (!Directory.Exists(newTargetPath))
                        {
                            Directory.CreateDirectory(newTargetPath);
                        }

                        //copy files to the respective folders, it will even overwrite files if already exists
                        File.Copy(fileName, newTargetPath + filePathName, true);
                    }

                    if (isMinify && fileName.IsHtmlFile())
                    {

                        string ntargetPath = targetPath + filePathName;
                        // Minify contents
                        string minifiedContents = MinifyHtml(ntargetPath, features);
                        // Write to the same file
                        File.WriteAllText(ntargetPath, minifiedContents, Encoding.UTF8);
                    }

                    Console.WriteLine(filePathName);

                }
            }

            //it will read all folders(directories) inside the folder and then call itself to create sub folders
            foreach (string directories in Directory.GetDirectories(path))
            {
                string folderName = directories.Replace(path, "");
                string newCopyPath = targetPath + folderName;

                if (Convert.ToInt32(Array.IndexOf(ignoreFolders, folderName)) < 0)
                {
                    newCopyPath = newCopyPath + @"\";
                    CopyAllFiles(directories, newCopyPath, fromDateTime, isMinify);//calling itself
                }
            }
        }
    }
}