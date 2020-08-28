﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAC_24Frame
{
    public class SystemEnv
    {
        /*
        *      Product Info
        */
        public static string GetProductKey()
        {
            return Properties.Settings.Default.ProductKey;
        }

        public static void SetProductKey(string key)
        {
            Properties.Settings.Default.ProductKey = key;
            Properties.Settings.Default.Save();
        }

        public const string PRODUCT_TYPE = "24Frame";

        public const string UPDATE_SETTING_INFO = "SETUP";
        public const string UPDATE_IMAGE_RESOURCE = "IMG";


        /*
         *      Downlaod Path & Folder
         */
        public static string DownloadFolderPath = @"C:\PAC_Resource\Downlaod";
        public static string DefaultFolderPath = @"C:\PAC_Resource\Default";

        public static void InitResourceFolder()
        {
            try
            {
                Directory.CreateDirectory(DownloadFolderPath);
                Directory.CreateDirectory(DefaultFolderPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("(System) Init Error = {0}", e.Message);
            }
        }

        public static string[] IMG_TYPEs = new string[] { ".jpg", ".png", };


        private static string applicationPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        public static string CCTV_IP_FILE = applicationPath + @"\LocalCamIP.txt";
        public static string DEVICE_ID_FILE = applicationPath + @"\ConfigSerailNumber.txt";

    }
}
