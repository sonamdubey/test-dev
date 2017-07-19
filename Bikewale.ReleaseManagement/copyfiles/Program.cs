using HtmlMinifier;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using System;
using System.Collections.Generic;
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
            //source folder path from where files to take
            string copyPath = String.Format(@"D:\Bikewale-Releases\{0}\website", DateTime.Now.ToString("ddMMMyyyy"));
            bool isMinify = false;
            //target folder path where files need to copy
            string path = string.Empty;
            DateTime fromDateTime = DateTime.Now.AddDays(-7); //default value for release note is one week

            if(args != null && args.Length > 0)
            {
                fromDateTime = DateTime.Now.AddDays(-Convert.ToSByte(args[0]));

                if (args.Length > 1)
                {
                    path = args[1];
                    if(!path.ToUpper().Contains("OPR"))
                    {
                        path = @"D:\work\bikewaleweb\BikeWale.UI\";
                        isMinify = true;
                    }
                    else
                    {
                        path = @"D:\work\bikewaleweb\BikeWaleOPR.UI\";
                    }
                }
            }          
                    

            if (buildSolution)
                BuildSolution();
            //calling function to copy files
            CopyAllFiles(path, copyPath,fromDateTime,isMinify);

            MoveBuildFolderContents(path, copyPath);

            //create bat file for html minification
           // CreateHTMLMinifyBat(copyPath);

            //System.Diagnostics.Process.Start(@"D:\WebDevelopment\BikeWale_Releases\copyfiles\minifyHTML.bat");
        }

        private static void MoveBuildFolderContents(string path, string copyPath)
        {
            //// Use Path class to manipulate file and directory paths.
            //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            //string destFile = System.IO.Path.Combine(targetPath, fileName);

            //// To copy a folder's contents to a new location:
            //// Create a new target folder, if necessary.
            //if (!System.IO.Directory.Exists(targetPath))
            //{
            //    System.IO.Directory.CreateDirectory(targetPath);
            //}

            //// To copy a file to another location and 
            //// overwrite the destination file if it already exists.
            //System.IO.File.Copy(sourceFile, destFile, true);

            //// To copy all the files in one directory to another directory.
            //// Get the files in the source folder. (To recursively iterate through
            //// all subfolders under the current directory, see
            //// "How to: Iterate Through a Directory Tree.")
            //// Note: Check for target path was performed previously
            ////       in this code example.
            //if (System.IO.Directory.Exists(sourcePath))
            //{
            //    string[] files = System.IO.Directory.GetFiles(sourcePath);

            //    // Copy the files and overwrite destination files if they already exist.
            //    foreach (string s in files)
            //    {
            //        // Use static Path methods to extract only the file name from the path.
            //        fileName = System.IO.Path.GetFileName(s);
            //        destFile = System.IO.Path.Combine(targetPath, fileName);
            //        System.IO.File.Copy(s, destFile, true);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Source path does not exist!");
            //}

        }

        private static void BuildSolution()
        {
            try
            {
                //string projectFileName = ConfigurationManager.AppSettings["solutionPath"].ToString();
                //ProjectCollection proj = new ProjectCollection();
                //Dictionary<string, string> GlobalProperty = new Dictionary<string, string>();
                //GlobalProperty.Add("Configuration", "Release");
                //GlobalProperty.Add("Platform", "Any CPU");

                //BuildRequestData buildRequest = new BuildRequestData(projectFileName, GlobalProperty, null, new string[] { "Build" }, null);

                //BuildResult buildResult = BuildManager.DefaultBuildManager.Build(new BuildParameters(proj), buildRequest);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.Message);
            }
        }
        
        /// <summary>
        /// Minifies the contents of the given view.
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
        /// </summary>
        /// <param name="path"></param>
        /// <param name="copyPath"></param>
        public static void CopyAllFiles(string path, string targetPath,DateTime fromDateTime,bool isMinify)
        {
            Console.WriteLine(path);
            Console.WriteLine(targetPath);
            var features = new Features(null);

            path.Replace(@"\\", @"\");
            targetPath.Replace(@"\\", @"\");
            //it will read all files inside the folder
            foreach (string fileName in Directory.GetFiles(path))
            {
                //Console.WriteLine(fileName);

                //files extension to check ignored file list
                string fileExtension = Path.GetExtension(fileName);

                //date when file was last written to check how many days older file need to copy
                DateTime lastModifiedDate = File.GetLastWriteTime(fileName);

                //required checks
                if (!string.IsNullOrEmpty(fileExtension) && Convert.ToInt32(Array.IndexOf(ignoreFiles, fileExtension)) < 0 && lastModifiedDate >= fromDateTime)
                {
                    //if folder does not exist, it wil create new one
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    //copy files to the respective folders, it will even overwrite files if already exists
                    File.Copy(fileName, targetPath + Path.GetFileName(fileName), true);

                    if (fileExtension.Equals(".js") || fileExtension.Equals(".css"))
                    {
                        string newTargetPath = targetPath.Replace(@"\website\", @"\css n js\");
                        if (!Directory.Exists(newTargetPath))
                        {
                            Directory.CreateDirectory(newTargetPath);
                        }

                        //copy files to the respective folders, it will even overwrite files if already exists
                        File.Copy(fileName, newTargetPath + Path.GetFileName(fileName), true);
                    }

                    if (isMinify && fileName.IsHtmlFile())
                    {
                        Console.WriteLine("Beginning Minification");
                        string ntargetPath = targetPath + Path.GetFileName(fileName);
                        // Minify contents
                        string minifiedContents = MinifyHtml(ntargetPath, features);

                        // Write to the same file
                        File.WriteAllText(ntargetPath, minifiedContents, Encoding.UTF8);
                        Console.WriteLine("Minified file : " + ntargetPath);
                    }

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
