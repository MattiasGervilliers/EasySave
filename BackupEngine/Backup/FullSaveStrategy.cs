using System;
using System.IO;
using BackupEngine.State;
using System.Linq;
using BackupEngine.Log;
using System.Text;
using System.Diagnostics;

namespace BackupEngine.Backup
{
    // La classe FullSaveStrategy implémente une stratégie de sauvegarde complète. Elle hérite de la classe SaveStrategy.
    // Cette stratégie copie tous les fichiers du dossier source vers un dossier de destination.
    public class FullSaveStrategy : SaveStrategy
    {
        // Constructeur qui initialise la configuration de la sauvegarde
        public FullSaveStrategy(BackupConfiguration configuration) : base(configuration) { }

        // Méthode principale qui exécute la sauvegarde complète
        public override void Save(string uniqueDestinationPath)
        {
            // Si le cryptage est activé dans la configuration, utiliser la stratégie de cryptage
            if (Configuration.Encrypt)
            {
                TransferStrategy = new CryptStrategy();
            }
            else
            {
                // Sinon, utiliser la stratégie de copie
                TransferStrategy = new CopyStrategy();
            }

            // Obtenir le chemin absolu du dossier source à partir de la configuration
            string sourcePath = Configuration.SourcePath.GetAbsolutePath();

            // Vérifier que le dossier source existe
            if (!Directory.Exists(sourcePath))
            {
                // Si le dossier source n'existe pas, lever une exception
                throw new DirectoryNotFoundException($"Le dossier source '{sourcePath}' n'existe pas.");
            }

            // Récupérer tous les fichiers à sauvegarder dans le dossier source
            string[] files = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
            int totalFiles = files.Length; // Nombre total de fichiers à sauvegarder
            long totalSize = files.Sum(file => new FileInfo(file).Length); // Taille totale des fichiers à sauvegarder
            int remainingFiles = totalFiles; // Nombre de fichiers restants
            long remainingSize = totalSize; // Taille restante des fichiers à sauvegarder

            // Mettre à jour l'état de la sauvegarde avant de commencer
            OnStateUpdated(new StateEvent(
                "Full Backup",   // Nom de la sauvegarde
                "Active",        // Statut de la sauvegarde (actif)
                totalFiles,      // Nombre total de fichiers
                totalSize,       // Taille totale des fichiers
                remainingFiles,  // Fichiers restants à transférer
                remainingSize,   // Taille restante à transférer
                "",              // Aucun fichier spécifique au départ
                ""               // Aucune destination spécifique au départ
            ));

            // Parcourir tous les fichiers pour les copier dans le dossier de destination
            foreach (string file in files)
            {
                // Obtenir le chemin relatif du fichier par rapport au dossier source
                string relativePath = file.Substring(sourcePath.Length + 1);
                // Créer le chemin de destination pour ce fichier
                string destFile = Path.Combine(uniqueDestinationPath, relativePath);
                // Créer les répertoires nécessaires dans le dossier de destination
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));

                try
                {
                    // Mesurer le temps nécessaire pour transférer le fichier
                    DateTime start = DateTime.Now;
                    // Transférer le fichier en utilisant la stratégie de transfert
                    TransferStrategy.TransferFile(file, destFile);
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - start; // Calculer la durée du transfert

                    // Mise à jour de l'état avec les informations du fichier transféré
                    remainingFiles--;  // Décrémenter le nombre de fichiers restants
                    remainingSize -= new FileInfo(file).Length;  // Réduire la taille restante

                    // Envoi d'un événement d'état avec les informations mises à jour
                    OnStateUpdated(new StateEvent(
                        "Full Backup",  // Nom de la sauvegarde
                        "Active",       // Statut de la sauvegarde
                        totalFiles,     // Nombre total de fichiers
                        totalSize,      // Taille totale des fichiers
                        remainingFiles, // Fichiers restants
                        remainingSize,  // Taille restante
                        file,           // Fichier source
                        destFile        // Fichier de destination
                    ));

                    // Créer un événement de transfert avec la durée et les informations du fichier
                    TransferEvent transferEvent = new TransferEvent(Configuration, duration, new FileInfo(file), new FileInfo(destFile));
                    // Envoyer l'événement de transfert
                    OnTransfer(transferEvent);
                }
                catch (Exception e)
                {
                    // En cas d'erreur lors de la copie du fichier, afficher un message d'erreur
                    Console.WriteLine($"Erreur lors de la copie du fichier {file} : {e.Message}");
                    // Créer un événement de transfert avec une durée de -1 pour indiquer une erreur
                    OnTransfer(new TransferEvent(Configuration, new TimeSpan(-1), new FileInfo(file), new FileInfo(destFile)));
                }
            }

            // Mise à jour de l'état à la fin de la sauvegarde
            OnStateUpdated(new StateEvent(
                "Full Backup", // Nom de la sauvegarde
                "Completed",   // Statut de la sauvegarde (terminée)
                totalFiles,    // Nombre total de fichiers
                totalSize,     // Taille totale des fichiers
                0,             // Aucun fichier restant
                0,             // Aucune taille restante
                "",            // Aucun fichier spécifique à la fin
                ""             // Aucune destination spécifique à la fin
            ));

            // Affichage d'un message indiquant que la sauvegarde est terminée
            Console.WriteLine($"Sauvegarde complète effectuée dans : {uniqueDestinationPath}");
        }
    }
}
