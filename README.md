# Curso Online Rascunho

Para criar migrations:
```bash
dotnet ef migrations add CriacaoDoCurso --verbose --project CursoOnline.Dados/ --startup-project CursoOnline.Web/
```

Para remover uma migration:
```bash
dotnet ef migrations remove --project src/CursoOnline.Dados/ --startup-project src/CursoOnline.Web/
```

Para executar migrations criadas no banco de dados: 
```bash
dotnet ef database update --verbose --project CursoOnline.Dados/ --startup-project CursoOnline.Web/
```

Para criar migrations:
```bash
dotnet ef migrations add --project src/CursoOnline.Dados/CursoOnline.Dados.csproj --startup-project src/CursoOnline.Web/CursoOnline.Web.csproj --context CursoOnline.Dados.Contextos.ApplicationDbContext --configuration Debug CriacaoDoAluno --output-dir Migrations
```

Dica: https://macoratti.net/21/03/efc_idesig1.htm

Para executar migrations no banco de dados:
```bash
cd ../CursoOnline.Web/
dotnet ef database update
```
