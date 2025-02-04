namespace LogLib
{
    /// <summary>
    /// Classe statique qui sert de fabrique (factory) pour créer des objets LogWriter
    /// </summary>
    public static class LogFactory
    {
        /// <summary>
        /// Méthode statique pour instancier un LogWriter en spécifiant le chemin du répertoire des logs
        /// </summary>
        /// <param name="logDirectoryPath"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static LogWriter CreateLogWriter(string logDirectoryPath)
        {
            /// Vérifie si le chemin fourni est null, vide ou composé uniquement d'espaces
            if (string.IsNullOrWhiteSpace(logDirectoryPath))
            {
                throw new ArgumentException("Le chemin du répertoire de logs ne peut pas être vide.");
            }

            /// Retourne une nouvelle instance de LogWriter avec le chemin spécifié
            return new LogWriter(logDirectoryPath);
        }
    }
}
