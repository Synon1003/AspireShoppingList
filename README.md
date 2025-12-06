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
Unit (WebApi Controllers)
```bash
cd .\ShoppingList.Service.UnitTests
dotnet test
```
Integration (WebApi + MongoDB)
```bash
cd .\ShoppingList.Service.Tests
dotnet test
```
Vue components (Client components)
```bash
cd .\ShoppingList.Client
pnpm run test
```
E2e (Client + WebApi + MongoDB)
```bash
cd .\ShoppingList.Client.Tests
dotnet test --settings ShoppingList.Client.runsettings
```
