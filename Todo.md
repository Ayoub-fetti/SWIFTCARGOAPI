# 📋 TODO List - Développement de SwiftCargoApi (.NET 8 & MySQL)


## 🗄️ Étape 1 : Configuration de la Base de Données (MySQL)
- [ ] Configurer la chaîne de connexion (Connection String) dans `appsettings.json` (Serveur, BDD, User, Mot de passe)
- [ ] Créer le dossier `Data/` à la racine
- [ ] Créer la classe `AppDbContext.cs` dans le dossier `Data`
- [ ] Lier `AppDbContext` à MySQL dans le fichier `Program.cs`
- [ ] Tester la connexion en lançant l'application : `dotnet run` (Vérifier qu'il n'y a pas d'erreur de build)

---

## 🔐 Étape 2 : Sécurité & Authentification JWT
- [ ] Créer le dossier `Models/` à la racine
- [ ] Créer le modèle `User.cs` (`Id`, `Username`, `PasswordHash`, `Role`) dans `Models/`
- [ ] Ajouter le `DbSet<User>` dans `AppDbContext.cs`
- [ ] Configurer les clés et paramètres JWT dans `appsettings.json`
- [ ] Injecter et configurer le Middleware d'Authentification (`AddAuthentication` et `AddJwtBearer`) dans `Program.cs`
- [ ] Ajouter `app.UseAuthentication();` et `app.UseAuthorization();` au bon endroit dans `Program.cs`
- [ ] Générer la première migration EF Core pour MySQL :  
  `dotnet ef migrations add InitialCreate`
- [ ] Appliquer la migration pour créer les tables dans MySQL :  
  `dotnet ef database update`

---

## 🕹️ Étape 3 : Logique d'Authentification (Contrôleur)
- [ ] Créer une classe utilitaire de Hashage (ou installer le package `BCrypt.Net-Next`) pour sécuriser les mots de passe
- [ ] Créer le dossier `Controllers/` s'il n'existe pas
- [ ] Créer le fichier `AuthController.cs` dans `Controllers/`
- [ ] Coder le endpoint `POST /api/auth/register` (Création d'utilisateur avec mot de passe hashé)
- [ ] Coder le endpoint `POST /api/auth/login` (Vérification et génération du Token JWT signé)
- [ ] Lancer l'API et vérifier que la génération du token fonctionne

---

## 📦 Étape 4 : Métier - Gestion du Fret International
- [ ] Créer le modèle `Shipment.cs` (`Id`, `TrackingNumber`, `Origin`, `Destination`, `Weight`, `Status`, `EstimatedDelivery`) dans `Models/`
- [ ] Ajouter le `DbSet<Shipment>` dans `AppDbContext.cs`
- [ ] Créer et appliquer la deuxième migration MySQL :  
  `dotnet ef migrations add AddShipmentTable`  
  `dotnet ef database update`
- [ ] Créer le fichier `ShipmentController.cs` dans `Controllers/`
- [ ] Ajouter l'attribut `[Authorize]` au-dessus du contrôleur pour bloquer les accès non authentifiés
- [ ] Implémenter les routes CRUD :
  - [ ] `GET /api/shipment` (Récupérer toutes les cargaisons)
  - [ ] `GET /api/shipment/{id}` (Récupérer une cargaison par son ID)
  - [ ] `POST /api/shipment` (Créer une nouvelle expédition)
  - [ ] `PUT /api/shipment/{id}` (Mettre à jour les détails d'une expédition)
  - [ ] `DELETE /api/shipment/{id}` (Supprimer une expédition)
- [ ] Ajouter une route spécifique `PUT /api/shipment/{id}/status` restreinte aux rôles Admin/Driver pour changer le statut du transport

---

## 🚀 Étape 5 : Tests des Endpoints sur Postman
- [ ] **Test Inscription :** Envoyer une requête `POST` sur `/api/auth/register` avec un JSON (username, password, role) -> Vérifier le statut 200/201
- [ ] **Test Connexion :** Envoyer une requête `POST` sur `/api/auth/login` -> Copier le Token JWT reçu dans la réponse
- [ ] **Test Accès Bloqué :** Tenter un `GET` sur `/api/shipment` *sans token* -> Vérifier que l'API renvoie une erreur `401 Unauthorized`
- [ ] **Test Accès Autorisé :** Dans Postman, aller dans l'onglet **Authorization**, choisir **Bearer Token**, coller le token, et relancer le `GET` -> Vérifier que la liste vide (ou les données) s'affiche (Statut 200)
- [ ] **Test CRUD complet :** Tester les requêtes `POST` (création de cargaison), `PUT` (modification) et `DELETE` avec le token actif