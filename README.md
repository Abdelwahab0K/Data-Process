This is a dashboard project developed for Maghreb Steel using ASP.NET MVC.

## Features

- Filtering (Jour, Mois, PceID, etc.)
- KPI Charts with CanvasJS
- Excel Export using ClosedXML
- Dynamic Sidebar and Search
- Modular Partial Views

## Technologies

- ASP.NET Core MVC
- Bootstrap 5
- jQuery
- SQL Server
- ClosedXML
- CanvasJS

## Run Locally

1. Clone the repo  
2. Open `LandingPage.sln` in Visual Studio  
3. Configure your DB in `appsettings.json`  
4. Press `F5` or run the project

   
🔹 1. Page de Connexion (/Home/Login)
Utilité : permet à l’utilisateur de se connecter avec ses identifiants sécurisés.

Fonctionnalités :

Vérification des identifiants.

Redirection vers le dashboard après connexion réussie.

Capture d’écran :

![image alt](https://github.com/Abdelwahab0K/Data-Process/blob/main/image_2025-05-28_112711144.png)


🔹 2. Tableau de Bord (Dashboard) (/Dashboard/Index)
Utilité : page principale pour visualiser les données de suivi de production.

Fonctionnalités :

Tableau central avec recherche, tri, pagination.

Filtres dynamiques par Jour, Mois, Année, PCE ID, etc.

Export Excel des résultats filtrés.

Mise à jour automatique des données.

Capture d’écran :

![image alt](https://github.com/Abdelwahab0K/Data-Process/blob/c2757e01caaf0c3535643d61e2c21983c35644a4/image_2025-05-28_144240015.png)   





## Author

- [AbdelwahabOK](https://github.com/AbdelwahabOK)
