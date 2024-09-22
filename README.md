# Curso Online Rascunho

Para executar migrations:
```bash
dotnet ef migrations add CriacaoDoCurso --verbose --project CursoOnline.Dados/ --startup-project CursoOnline.Web/
```

Para remover uma migration:
```bash
dotnet ef migrations remove --project src/CursoOnline.Dados/ --startup-project src/CursoOnline.Web/
```
