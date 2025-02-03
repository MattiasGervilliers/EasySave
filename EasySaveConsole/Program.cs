using LogLib;
using System;

class Program
{
    static void Main()
    {
        // Utiliser la méthode statique de LogFactory pour créer un LogWriter
        LogWriter logWriter = LogFactory.CreateLogWriter("C:\\Logs\\EasySave");

        // Créer un log
        Log log = new Log(
            level: LogLevel.TRACE, // Ajout du niveau de log
            message: "Test log 2"
        );

        // Écrire le log
        logWriter.WriteLog(log);

        Console.WriteLine("Log écrit avec succès.");
    }
}
