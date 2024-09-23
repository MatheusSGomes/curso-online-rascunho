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
