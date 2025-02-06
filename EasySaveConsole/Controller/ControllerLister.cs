using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.SettingsRepository;

namespace EasySaveConsole.Controller
{
    internal class ControllerLister
    {
        private ViewLister vue;
        private Language langue;

        public ControllerLister(Language Langue)
        {
            this.langue = Langue;
            vue = new ViewLister(Langue);
        }
        public void AfficheConfiguration()
        {
            vue.AfficheConfiguration();
            Console.ReadLine();
            Console.Clear();
        }
    }
}
