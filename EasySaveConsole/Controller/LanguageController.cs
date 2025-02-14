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
        /// <summary>
        /// Update the controller language
        /// </summary>
        /// <param name="newlanguage"></param>
        public void UpdateLanguage(Language newlanguage)
        {
            _newLanguage = newlanguage;

        }
        /// <summary>
        /// Update the model language
        /// </summary>
        public void Execute()
        {
            BackupModel.UpdateLanguage(_newLanguage);
        }
        /// <summary>
        /// Return the saved language
        /// </summary>
        /// <returns></returns>
        public Language? GetLanguage()
        {
            return BackupModel.GetLanguage();
        }
    }
}
