using BackupEngine.SettingsRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.View
{
    internal class ViewLister
    {
        private Language Langue;

        public ViewLister(Language langue)
        {
            this.Langue = langue;
        }

        public void AfficheConfiguration()
        {
            if (this.Langue == Language.French)
            {
                Console.WriteLine("Liste des configurations de sauvegarde : ");
            }
            else
            {
                Console.WriteLine("List of backup configurations: ");
            }
        }
    }
}
