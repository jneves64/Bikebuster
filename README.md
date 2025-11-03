# Guia rápido para rodar o projeto BikeBuster

## 1. Restaurar dependências
No diretório do projeto:
```bash
cd Bikebuster-master/Bikebuster-master
dotnet restore
```

## 2. Subir infraestrutura
Inicie os containers do Postgres e RabbitMQ:
```bash
docker compose -f compose.infra.yml up --build
```

## 3. Atualizar banco de dados
Execute as migrações do Entity Framework:
```bash
dotnet ef database update
```

## 4. Rodar o projeto
Inicie a aplicação:
```bash
dotnet run
```

## 5. Acessar o Swagger
Abra no navegador:
```
http://localhost:5236/swagger/index.html
```
