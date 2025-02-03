namespace LogLib
{
    public static class LogFactory // Classe statique
    {
        // Méthode statique pour créer un LogWriter
        public static LogWriter CreateLogWriter(string logDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(logDirectoryPath))
            {
                throw new ArgumentException("Le chemin du répertoire de logs ne peut pas être vide.");
            }

            return new LogWriter(logDirectoryPath);
        }
    }
}
