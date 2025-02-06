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
    internal class ControllerSuppr
    {
        private string NomSuppr;
        private Language Langue;
        public ControllerSuppr(Language Langue)
        {
            ViewSuppr vue = new ViewSuppr();
            vue.AfficheDemandeNom(Langue);
            NomSuppr = Console.ReadLine();
            // Model.suppr(NomSuppr)

        }
    }
}
