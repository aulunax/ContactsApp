# ContactsApp


## Uruchamianie
### Visual Studio 2022
Upewnić się żę ‘ASP.NET and Web Development’ oraz ‘Node’ są zainstalowane jako część Visual Studio 2022 Tools.

Następnie otworzyć projekt w Visual Studio 2022, i wystartować przyciskiem Run, lub wcisnąć F5 na klawiaturze.

### Konsola
Frontend:
- Wejść do folderu ContactsApp.Client
- Uruchomić w konsoli ```npm install```
- Uruchomić w konsoli ```npm run dev``` co powinno wystartować frontend na porcie podanym na outpucie

Backend:
- Wejść do głównego folderu aplikacji ContactsApp (NIE ContactsApp.Server)
- Uruchomić w konsol ```dotnet dev-certs https --trust```
- Uruchomić w konsol ```dotnet restore```
- Uruchomić w konsol ```dotnet run --launch-profile https --project ContactsApp.Server``` co powinno wystartować REST API serwer
