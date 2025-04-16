using static System.Environment;

namespace Northwind.EntityModels;

/// <summary>
/// Logger for Northwind database context operations.
/// Provides functionality to record database-related messages to timestamped log files.
/// </summary>
public class NorthwindContextLogger
{
    /// <summary>
    /// Writes a message to a log file on the desktop with a timestamp in the filename.
    /// Each log entry is appended to a file named with the current date and time.
    /// </summary>
    /// <param name="message">The message to write to the log file.</param>
    public static void WriteLine(string message)
    {
        // Define log directory on the user's desktop
        string folder = Path.Combine(GetFolderPath(SpecialFolder.DesktopDirectory), "book-logs");

        // Create the log directory if it doesn't exist
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        // Generate a timestamp for the log filename
        string dateTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        // Construct the full log file path with timestamp
        string path = Path.Combine(folder, $"northwindlog-{dateTimeStamp}.txt");

        // Append the message to the log file
        StreamWriter textFile = File.AppendText(path);
        textFile.WriteLine(message);
        textFile.Close();
    }
}

