using LogLib; // Importation du namespace LogLib pour utiliser les classes de gestion des logs
using System;

class Program
{
    static void Main()
    {
        // Création d'une instance de LogWriter qui va écrire les logs dans le répertoire "C:\Logs\EasySave"
        LogWriter logWriter = new LogWriter("C:\\Logs\\EasySave");

        // Création d'un log spécifique au processus de sauvegarde (ConsoleLog)
        ConsoleLog consoleLog = new ConsoleLog(
            LogLevel.INFO, // Niveau de log : INFO (indique une information générale)
            "Test 1", // Message associé au log
            "Backup213", // Nom de la sauvegarde
            @"\\Serveur\Source\Fichier.txt", // Chemin source du fichier sauvegardé
            @"\\Serveur\Destination\Fichier.txt", // Chemin de destination du fichier sauvegardé
            4096, // Taille du fichier sauvegardé en octets (4 Ko)
            450 // Temps de transfert en millisecondes
        );

        // Écriture du log dans un fichier JSON via LogWriter
        logWriter.WriteLog(consoleLog);

        // Affichage d'un message de confirmation dans la console
        Console.WriteLine("Log écrit avec succès.");
    }
}
