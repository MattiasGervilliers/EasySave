using BackupEngine;
using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Controller;
namespace EasySaveConsole.Controller
{
    internal class SaveController : IController
    {
        private SaveAction _action;
        private string? _configName;
        private BackupConfiguration? _backupConfig;
        private bool _result;
        /// <summary>
        /// Initializes a new instance of the SaveController with an action and optional configuration parameters.
        /// </summary>
        public SaveController(SaveAction action, string? configName = null, BackupConfiguration? backupConfig = null)
        {
            _action = action;
            _configName = configName?.Trim();
            _backupConfig = backupConfig;
        }
        /// <summary>
        /// Executes the specified save action (Add, Delete, Launch).
        /// </summary>
        public void Execute()
        {
            switch (_action)
            {
                case SaveAction.Add:
                    _result = AddBackupConfiguration();
                    break;
                case SaveAction.Delete:
                    _result = DeleteBackupConfiguration();
                    break;
                case SaveAction.Launch:
                    _result = LaunchBackup();
                    break;
            }
        }
        /// <summary>
        /// Returns the result of the executed action.
        /// </summary>
        public bool GetResult() => _result;
        /// <summary>
        /// Adds a new backup configuration if provided.
        /// </summary>
        internal bool AddBackupConfiguration()
        {
            if (_backupConfig != null)
            {
                BackupModel.AddConfig(_backupConfig);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deletes a backup configuration if it exists.
        /// </summary>
        internal bool DeleteBackupConfiguration()
        {
            if (!string.IsNullOrEmpty(_configName))
            {
                BackupConfiguration? configToDelete = BackupModel.FindConfig(_configName);
                if (configToDelete != null)
                {
                    BackupModel.DeleteConfig(configToDelete);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Launches a backup if the configuration exists.
        /// </summary>
        internal bool LaunchBackup()
        {
            if (!string.IsNullOrEmpty(_configName))
            {
                BackupConfiguration? configToLaunch = BackupModel.FindConfig(_configName);
                if (configToLaunch != null)
                {
                    BackupModel.LaunchConfig(configToLaunch);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Updates the configuration name.
        /// </summary>
        public void UpdateConfigName(string configName)
        {
            this._configName = configName;
        }
        /// <summary>
        /// Updates the backup configuration.
        /// </summary>
        public void UpdateConfiguration(BackupConfiguration backupConfiguration)
        {
            this._backupConfig = backupConfiguration;
        }
        /// <summary>
        /// Updates the save action.
        /// </summary>
        public void UpdateAction(SaveAction saveAction)
        {
            this._action = saveAction;
        }
        /// <summary>
        /// Retrieves the list of all backup configurations.
        /// </summary>
        public List<BackupConfiguration> GetConfigurations()
        {
            return BackupModel.GetConfigs();
        }
        /// <summary>
        /// Checks if the backup configuration exists and returns it if found.
        /// </summary>

        public BackupConfiguration? BackupExist()
        {

            if (this._configName != null)
            {
                return BackupModel.FindConfig(this._configName);
            }
            else
            {
                return null;
            }
        }
    }
}


























/*
namespace EasySaveConsole.Controller
{
    internal class SaveController
    {
        public void AddBackupConfiguration(BackupConfiguration backupConfiguration)
        {
            BackupModel.AddConfig(backupConfiguration);

            //Console.Clear();
        }
        public void DeleteConfiguration(BackupConfiguration backupConfiguration)
        {
            BackupModel.DeleteConfig(backupConfiguration);
        }
        public BackupConfiguration? BackupExist(String Name)
        {
            if (BackupModel.FindConfig(Name ?? "") != null)
            {
                return BackupModel.FindConfig(Name ?? "");
            }
            return null;
        }
        public List<BackupConfiguration> GetConfigurations()
        {
            return BackupModel.GetConfigs();
        }
        public void LaunchBackup(string Name)
        {
            BackupConfiguration? config = BackupModel.FindConfig(Name ?? "");
            BackupModel.LaunchConfig(config);
        }

        public BackupConfiguration? GetBackupConfiguration(string name)
        {
            return BackupModel.FindConfig(name);
        }

        public void LaunchBackup(BackupConfiguration backupConfiguration)
        {
            BackupModel.LaunchConfig(backupConfiguration);
        }
    }
}

 */