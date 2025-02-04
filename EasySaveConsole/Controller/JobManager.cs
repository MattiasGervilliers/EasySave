using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Model;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    internal class JobManager
    {
        private Vue vue;
        private Modele model;

        public JobManager()
        {
            Vue vue = new Vue();
            Modele model = new Modele();
            vue.AfficheMenu();

        }
        public void ChoixLangue()
        {
            vue.AfficheChoixLangue();
        }
        public void CreerConfiguration()
        {
            vue.afficheCreer();
            //creer une config avec nom, src file etc ...
        }

    }
}
