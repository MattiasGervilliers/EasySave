using BackupEngine.Settings;
using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class LanguageController
    {
        public void SaveLanguage(Language language)
        {
            BackupModel.UpdateLanguage(language);
        }
    }
}
