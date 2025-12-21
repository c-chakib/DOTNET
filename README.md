# GestionBudgetWinForms

Petit projet WinForms (.NET Framework4.7.2) pour gérer des transactions (CRUD) avec MySQL.

## Prérequis
- Windows
- Visual Studio2019/2022 (avec workload `.NET desktop development`)
- MySQL Server (local ou distant) et MySQL Workbench
- Internet pour restaurer les packages NuGet
- (Optionnel) Docker Desktop (Windows containers mode) pour conteneuriser l'application

## Contenu du dépôt
- `GestionBudgetWinForms/` : projet WinForms
- `App.config` : chaîne de connexion `DefaultConnection` (attention aux identifiants)

## Préparation locale (collaborateur)
1. Cloner le dépôt:

```bash
git clone https://github.com/c-chakib/DOTNET.git
cd DOTNET
```

2. Ouvrir la solution dans Visual Studio: `GestionBudgetWinForms.sln` (ou le fichier `.csproj` sous le dossier du projet)

3. Installer/Restaurer les packages NuGet:
- Visual Studio: Clic droit sur la solution -> `Restore NuGet Packages`
- ou en ligne de commande (depuis le dossier solution):

```bash
dotnet restore
```

4. Configurer la connexion MySQL
- Ouvrir `GestionBudgetWinForms/App.config`
- Modifier la clé `DefaultConnection` avec les informations locales (host, port, database, user, password). Exemple:

```xml
<add name="DefaultConnection" connectionString="Server=127.0.0.1;Port=3306;Database=BudgetDB;Uid=appuser;Pwd=yourPassword;" providerName="MySql.Data.MySqlClient" />
```

> Important: ne commitez pas vos mots de passe. Le fichier `App.config` contient un commentaire pour rappeler de modifier la chaîne.

5. Créer la base (optionnel)
- Le projet essaie de créer la base et la table automatiquement si la connexion est valide.
- Si vous préférez, créez la DB et l'utilisateur depuis MySQL Workbench:

```sql
CREATE DATABASE IF NOT EXISTS BudgetDB;
CREATE USER 'appuser'@'localhost' IDENTIFIED BY 'yourPassword';
GRANT ALL PRIVILEGES ON BudgetDB.* TO 'appuser'@'localhost';
FLUSH PRIVILEGES;
```

6. Build & Run
- Dans Visual Studio: `Build` -> `Rebuild Solution` puis `Start (F5)` pour lancer l'application.
- En ligne de commande (build):

```bash
dotnet build GestionBudgetWinForms/GestionBudgetWinForms.csproj
```

Note: pour exécuter l'application graphique, utilisez Visual Studio.

## Docker (Windows containers) — optionnel
Cette application cible .NET Framework4.7.2 (WinForms) et nécessite Windows. Pour exécuter dans un conteneur, utilisez Docker Desktop en mode Windows containers.

### Pré-requis Docker
- Docker Desktop sur Windows
- Docker Desktop configuré en `Switch to Windows containers...` (menu contextuel)

### Construction et exécution
1. Assurez-vous que MySQL est accessible depuis le conteneur. Si MySQL tourne sur la machine hôte, le conteneur Windows peut y accéder via `host.docker.internal`.
2. Construire l'image et lancer le conteneur :

```powershell
# depuis le dossier racine du repo
docker-compose -f docker-compose.windows.yml build
docker-compose -f docker-compose.windows.yml up
```

Le `docker-compose.windows.yml` fourni définit la variable d'environnement `DEFAULT_CONNECTION` utilisée par l'application pour remplacer la chaîne dans `App.config`. Exemple :

```
DEFAULT_CONNECTION=Server=host.docker.internal;Port=3306;Database=BudgetDB;Uid=root;Pwd=yourPassword;
```

### Notes
- Les conteneurs Windows ne peuvent pas exécuter des conteneurs Linux sur le même démon. Si vous préférez des conteneurs Linux, vous devez porter l'application sur .NET Core/.NET6+ et utiliser une autre technologie UI (par exemple, web) — pas couvert ici.
- Si MySQL s'exécute sur une autre machine, adaptez le `DEFAULT_CONNECTION` en conséquence.
- Pour la production, ne stockez pas les identifiants dans `App.config` ; utilisez un gestionnaire de secrets ou des variables d'environnement et des fichiers `.env` exclus de VCS.

## Vérification
- Ouvrez MySQL Workbench et testez la connexion avec les mêmes identifiants (Test Connection).
- Lancez l'application: si la connexion est correcte, la table `Transactions` sera créée automatiquement.

## Remarques de sécurité & livraison
- Remplacez les mots de passe par des placeholders ou utilisez des variables d'environnement pour la livraison.
- Ajoutez un `.gitignore` (déjà présent) et ne poussez pas `bin/`, `obj/` ni les dossiers contenant des secrets.

## Problèmes connus
- Le projet utilise `MySql.Data` ADO.NET provider; si vous préférez `MySqlConnector`, changez la chaîne de connexion et le provider.

---
Si tu veux, je peux aussi :
- remplacer le mot de passe dans `App.config` par un placeholder et pousser la modification,
- ajouter des instructions d'exécution automatisée ou des scripts.
