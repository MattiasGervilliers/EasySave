using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.View;
using BackupEngine;
using BackupEngine.Settings;

namespace EasySaveConsole.Controller
{
    internal class ControllerLister
    {
        private ViewLister vue;

        public ControllerLister(Language Langue)
        {
            ViewLister viewLister = new ViewLister(Langue);
            viewLister.AfficheConfiguration();
            Console.ReadLine();
            Console.Clear();
        }
    }
}
