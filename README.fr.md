# EasySave

[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/MattiasGervilliers/EasySave/blob/main/README.md)
[![fr](https://img.shields.io/badge/lang-fr-red.svg)](https://github.com/MattiasGervilliers/EasySave/blob/main/README.fr.md)

## Description

EasySave est une application console permettant d'effectuer des sauvegardes de fichiers et dossiers de manière simple et efficace.

### Fonctionnalités principales :

- Création et exécution de travaux de sauvegarde (jusqu'à 5).
- Deux types de sauvegarde disponibles : complète et différentielle.
- Possibilité d’exécuter une ou plusieurs sauvegardes via des arguments.
- Sauvegarde de fichiers provenant de disques locaux, externes ou lecteurs réseaux.
- Génération automatique de logs journaliers et d'un fichier d'état en temps réel.

## Utilisation

Exécuter une sauvegarde via la ligne de commande :

```sh
console.exe 1      # Exécute la sauvegarde n°1.
console.exe 1-3    # Exécute les sauvegardes 1, 2 et 3.
console.exe 1;3    # Exécute les sauvegardes 1 et 3.
```

## Configuration

Le fichier `settings.json` permet de modifier les paramètres de l’application, notamment le chemin des logs, le chemin de l'état en temps réel et le format des logs (XML ou JSON).

### Exemple de fichier `settings.json` :

```json
{
  "Language": 1,
  "LogPath": "C:\\Chemin\\Vers\\Les\\Logs",
  "StatePath": "C:\\Chemin\\Vers\\Etat\\Temps\\Reel",
  "LogFormat": 1,
  "Configurations": [
    {
      "Name": "Sauvegarde1",
      "SourcePath": "C:\\Source",
      "DestinationPath": "C:\\Destination",
      "BackupType": 0
    }
  ]
}
```

- **Language** : Définit la langue de l’application (0 = français, 1 = anglais). Par défaut : `1`.
- **LogPath** : Définit l’emplacement des fichiers de logs. Par défaut : `C:\\Chemin\\De\\EasySave\\Logs`.
- **StatePath** : Définit l’emplacement du fichier d’état en temps réel. Par défaut : `C:\\Chemin\\De\\EasySave\\Logs\\state.json`.
- **LogFormat** : Définit le format des fichiers de logs (0 = JSON, 1 = XML). Par défaut : `0`.
- **Configurations** : Liste des sauvegardes configurées.

## Logs et suivi en temps réel

### Logs journaliers

Chaque action de sauvegarde est enregistrée dans un fichier journalier au format XML ou JSON, incluant :

- Date et heure de l’action
- Nom de la sauvegarde
- Fichier source et destination
- Taille du fichier
- Temps de transfert (négatif en cas d’échec)

### Fichier d’état

Un fichier `state.json` est mis à jour en temps réel avec :

- Le nom de la sauvegarde en cours
- La date de la dernière action exécutée
- L'état de la sauvegarde (Completed, Active)
- Le nombre de fichiers transférables
- La taille totale des fichiers à sauvegarder (en octets)
- Le nombre de fichiers restant à sauvegarder
- Le chemin d'origine du fichier en train d'être sauvegardé
- Le chemin de destination du fichier en train d'être sauvegardé