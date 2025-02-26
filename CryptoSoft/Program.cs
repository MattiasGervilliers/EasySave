using System;
using System.Diagnostics;
using System.Text;

namespace CryptoSoft
{
    /// <summary>
    /// La classe Program est responsable de l'exécution principale du programme. Elle utilise la classe Encoder pour
    /// effectuer des opérations de chiffrement en fonction des arguments passés en ligne de commande.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Méthode principale du programme. Elle reçoit des arguments de ligne de commande pour décider s'il faut
        /// chiffrer ou déchiffrer un fichier et affiche le temps écoulé pour l'exécution de l'opération.
        /// </summary>
        /// <param name="args">Tableau des arguments passés en ligne de commande.</param>
        /// <returns>Retourne 0 en cas de succès, -1 en cas d'erreur.</returns>
        static int Main(string[] args)
        {
            int returnValue = 0;  // Code de retour par défaut (succès)
            Stopwatch stopWatch = new Stopwatch();  // Stopwatch pour mesurer le temps d'exécution
            stopWatch.Start();  // Démarre le chronomètre

            try
            {
                // Vérifie si le troisième argument (args[2]) est "True" pour décider d'encrypter ou de décrypter
                if (args[2] == "True")
                {
                    // Appelle la méthode Encrypt de la classe Encoder pour chiffrer le fichier
                    Encoder.Encrypt(args[0], args[1]);
                }
                else
                {
                    // Si ce n'était pas "True", la méthode Decrypt pourrait être appelée ici, mais elle est commentée
                    // Encoder.Decrypt(args[0]);
                }
            }
            catch (Exception e)
            {
                // En cas d'exception (par exemple si les arguments sont mal fournis ou le fichier est introuvable),
                // on retourne -1 pour signaler une erreur
                return -1;
            }

            // Arrête le chronomètre après l'exécution de l'opération
            stopWatch.Stop();
            int elapsedTime = (int)stopWatch.ElapsedMilliseconds;  // Temps écoulé en millisecondes
            Console.WriteLine(elapsedTime);  // Affiche le temps d'exécution dans la console
            return returnValue;  // Retourne 0 pour indiquer que l'exécution s
