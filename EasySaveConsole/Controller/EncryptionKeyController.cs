using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class EncryptionKeyController : IController
    {
        private string _encryptionKey;
        public void UpdateEncryptionKey(string key)
        {
            _encryptionKey = key;

        }
        public void Execute()
        {
            BackupModel.UpdateEncrytpionKey(_encryptionKey);
        }
    }
}
