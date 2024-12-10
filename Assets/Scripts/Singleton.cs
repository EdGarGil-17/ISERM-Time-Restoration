using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    public string userID;
    public bool gameStarted = false;

    private void Awake()
    {
        MakeSingleton();
    }


    void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static class CSVManager
    {
        private static string ReportDirectoryName = "Report";
        private static string ReportFileName = "Report.csv";
        private static string ReportSeparator = ",";
        private static string[] ReportHeaders = new string[7]
        {
            "Patient ID",
            "Stimulus Speed",
            "Stimulus Thickness",
            "Stimulus Separation",
            "Stimulus Possition",
            "Stimulus Transparency(%)",
            "Stimulus Frequency"
        };
        private static string TimeStampHeader = "Time Stamp (UTC)";


        #region Interactions

        public static void AppendToReport(string[] strings)
        {
            VerifyDirectory();
            VerifyFile();
            using (StreamWriter sw = File.AppendText(GetFilePath()))
            {
                string FinalString = "";

                for (int i = 0; i < strings.Length; i++)
                {
                    if (FinalString != "")
                    {
                        FinalString += ReportSeparator;
                    }

                    FinalString += strings[i];
                }
                FinalString += ReportSeparator + GetTimeStamp();
                sw.WriteLine(FinalString);
            }
        }

        public static void CreateReport()
        {
            Debug.Log(GetFilePath());
            VerifyDirectory();
            using (StreamWriter sw = File.CreateText(GetFilePath()))
            {
                string FinalString = "";

                for (int i = 0; i < ReportHeaders.Length; i++)
                {
                    if (FinalString != "")
                    {
                        FinalString += ReportSeparator;
                    }
                    FinalString += ReportHeaders[i];
                }

                FinalString += ReportSeparator + TimeStampHeader;
                sw.WriteLine(FinalString);
            }
        }

        #endregion


        #region Operations

        public static void VerifyDirectory()
        {
            string dir = GetDirectoryPath();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public static void VerifyFile()
        {
            string file = GetFilePath();
            if (!File.Exists(file))
            {
                CreateReport();
            }
        }

        #endregion



        #region Queries
        public static string GetDirectoryPath()
        {
            return Application.persistentDataPath + "/" + ReportDirectoryName;

        }

        public static string GetFilePath()
        {
            return GetDirectoryPath() + "/" + ReportFileName;
        }

        public static string GetTimeStamp()
        {
            return System.DateTime.UtcNow.ToString();
        }

        #endregion
    }

}
