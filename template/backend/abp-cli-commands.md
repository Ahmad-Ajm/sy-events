# ABP CLI Commands (Template)

## 1) Create Solution
```
abp new {{SOLUTION_NAME}} -t app -u angular -d ef -dbms PostgreSQL --mobile none --pwa
```

## 2) Update NPM/Angular (inside angular/)
```
npm install
```

## 3) Configure Conventional Controllers (HttpApi.Host)
- Ensure `EventManagementHttpApiHostModule` configures ConventionalControllers for `EventManagementApplicationModule`.

## 4) Generate Angular Proxies (after backend runs once)
```
cd angular
abp generate-proxy -t ng
```

## 5) Migrations
```
cd aspnet-core/src/{{EF_PROJECT_NAME}}
dotnet ef migrations add InitialCreate
cd ../../src/{{DB_MIGRATOR_PROJECT}}
dotnet run
```

Replace placeholders {{SOLUTION_NAME}}, {{EF_PROJECT_NAME}}, {{DB_MIGRATOR_PROJECT}} as needed.
