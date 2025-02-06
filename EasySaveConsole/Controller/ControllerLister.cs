using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.View;
using BackupEngine;

namespace EasySaveConsole.Controller
{
    internal class ControllerLister
    {
        private ViewLister vue;

        public ControllerLister()
        {
            ViewLister viewLister = new ViewLister();
            viewLister.AfficheConfiguration();
            Console.ReadLine();
            Console.Clear();
        }
    }
}
