using UnityEngine;
using System;
using System.IO;

public class DataExporter : MonoBehaviour
{
    
    private const string fileName = "User Data.csv";

    public static void WriteData( Record record)
    {

        string filePath = Path.Combine(Application.dataPath, fileName);
        
        // Calculate duration from Time_Stamp and EndTime
        TimeSpan duration = DateTime.Parse(record.EndTime) - DateTime.Parse(record.Time_Stamp);
        string durationString = duration.ToString(@"mm\:ss");

        // Check if the file exists
        bool fileExists = File.Exists(filePath);

        // Open or create the file for writing
        // StreamWriter is automatically closed and disposed of when exiting the using block
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            // If the file is newly created, write the headers
            if (!fileExists)
            {
                sw.WriteLine("Time_Stamp,Game_Duration,Tickets_Given,Average_Score");
            }

            // Write the data
            sw.WriteLine($"{record.Time_Stamp},{durationString},{record.Score},{record.Average_Score}");   
        }
    }
    public static void DeleteData()
    {
        string filePath = Path.Combine(Application.dataPath, fileName);
        // Check if the file exists before attempting to delete
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log("File deleted: " + filePath);
        }
        else
        {
            Debug.Log("File not found: " + filePath);
        }
    }

}

// Define the structure of your data
public struct Record
{
    public string Time_Stamp;
    public string EndTime;
    public int Score;
    public float Average_Score;
}

