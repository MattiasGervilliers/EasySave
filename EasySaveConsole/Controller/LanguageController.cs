using BackupEngine.Settings;
using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class LanguageController : IController
    {
        private Language _newLanguage;
        public void UpdateLanguage(Language newlanguage)
        {
            _newLanguage = newlanguage;

        }
        public void Execute()
        {
            BackupModel.UpdateLanguage(_newLanguage);
        }
    }
}
