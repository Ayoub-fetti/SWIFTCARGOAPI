# SwiftCargo API - MVP

**SwiftCargo API** est le socle d'une solution logistique moderne, conçu pour offrir une gestion de fret international simple, sécurisée et efficace. Ce projet, développé avec .NET 8, sert de Minimum Viable Product (MVP) robuste, démontrant les fonctionnalités essentielles tout en étant prêt à évoluer vers une plateforme complète et riche en fonctionnalités.

## 🌟 À propos du projet

Ce projet a été construit pour fournir une API RESTful performante et sécurisée pour la gestion des expéditions. L'architecture est pensée pour être évolutive, avec une séparation claire des responsabilités (contrôleurs, services, accès aux données), garantissant ainsi la maintenabilité et la facilité d'ajout de nouvelles fonctionnalités.

## ✨ Fonctionnalités clés implémentées

Ce MVP inclut déjà un ensemble de fonctionnalités puissantes qui forment une base solide :

*   **Système d'Authentification JWT** : Sécurisation des endpoints avec des JSON Web Tokens, incluant l'inscription et la connexion par nom d'utilisateur ou e-mail.
*   **Gestion des Rôles** : Un système de rôles (`Admin`, `User`) avec un endpoint sécurisé permettant aux administrateurs de modifier les rôles des autres utilisateurs.
*   **Seeder de Données Automatisé** : Création automatique d'un utilisateur `Admin` au premier lancement de l'application pour une initialisation facile.
*   **CRUD Complet pour les Expéditions** : Gestion complète des cargaisons (Créer, Lire, Mettre à jour, Supprimer).
*   **Mises à Jour Partielles (HTTP PATCH)** : Flexibilité pour mettre à jour des informations spécifiques d'une expédition (ex: uniquement le poids) sans avoir à renvoyer l'objet entier.
*   **Génération de Rapports PDF** : Un endpoint dédié pour générer des rapports PDF professionnels listant les expéditions, avec une option de filtrage par date.
*   **Documentation d'API Interactive (Swagger)** : Une documentation claire, générée automatiquement à partir du code, avec une interface utilisateur permettant de tester les endpoints directement, y compris l'authentification.

## 🛠️ Stack Technologique

Ce projet s'appuie sur des technologies modernes et éprouvées pour garantir performance et fiabilité.

*   **Framework** : .NET 8 / ASP.NET Core Web API
*   **Base de données** : MySQL
*   **ORM** : Entity Framework Core 8
*   **Authentification** : JWT (JSON Web Tokens)
*   **Packages Notables** :
    *   `Microsoft.AspNetCore.Authentication.JwtBearer` : Pour la gestion de l'authentification JWT.
    *   `BCrypt.Net-Next` : Pour le hachage sécurisé des mots de passe.
    *   `Microsoft.AspNetCore.JsonPatch` : Pour la gestion des requêtes `HTTP PATCH`.
    *   `QuestPDF` : Pour une génération de documents PDF élégante et performante.
    *   `Swashbuckle.AspNetCore` : Pour la génération de la documentation OpenAPI (Swagger).

## 🚀 Démarrage rapide

### Prérequis
*   SDK .NET 8
*   Un serveur MySQL fonctionnel

### Installation & Configuration
1.  Clonez le dépôt.
2.  Configurez votre chaîne de connexion dans `appsettings.json` :
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "server=localhost;port=3306;database=swiftcargoapi;user=votre_user;password=votre_mdp"
    }
    ```
3.  Configurez les identifiants de l'administrateur par défaut dans `appsettings.json` :
    ```json
    "AdminUser": {
      "Username": "admin",
      "Email": "admin@swiftcargo.com",
      "Password": "VotreMotDePasseAdminSecret!"
    }
    ```
4.  Ouvrez un terminal à la racine du projet et appliquez les migrations pour créer la base de données :
    ```bash
    dotnet ef database update
    ```
5.  Lancez l'application :
    ```bash
    dotnet run
    ```
6.  Accédez à la documentation interactive via `https://localhost:PORT/swagger`.

## 🔮 Potentiel et Évolutions Futures

Ce MVP n'est qu'un début. L'architecture en place permet d'envisager facilement l'ajout de fonctionnalités à haute valeur ajoutée :

*   **Historique des Expéditions** : Suivi détaillé de chaque étape du transport (création d'un modèle `TrackingEvent`).
*   **Notifications par E-mail** : Envoi automatique d'e-mails (avec MailKit) lors des changements de statut importants ("En transit", "Livré").
*   **Validation Avancée** : Utilisation de `FluentValidation` pour des règles de validation métier plus complexes et découplées des modèles.
*   **Soft Deletes** : Mise en place d'une suppression logique (`IsDeleted`) pour ne jamais perdre de données et permettre la restauration.
*   **Tests Unitaires et d'Intégration** : Pour garantir la fiabilité et la non-régression du code.
*   **Déploiement et Conteneurisation** : Utilisation de Docker pour faciliter le déploiement dans n'importe quel environnement.