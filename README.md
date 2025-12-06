## Shopping List in Aspire

### Setup
```bash
cd .\ShoppingList.AppHost
dotnet run
```

### Components
- MongoDB
- Asp.Net Core WebApi
- Nuxt (Vue.js) app
- Aspire orchestration

### Tests
Integration (WebApi + MongoDB)
```bash
cd .\ShoppingList.Service.Tests
dotnet test
```
E2e (Client + WebApi + MongoDB)
```bash
cd .\ShoppingList.Client.Tests
dotnet test --settings ShoppingList.Client.runsettings
```
