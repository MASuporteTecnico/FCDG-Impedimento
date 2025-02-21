# MaSistemas

## Modelo padrão para os sistemas da M&A

![GitHub branch status](https://img.shields.io/github/checks-status/masuporte/MaSistemas/main)


## Recurso

- Modelo padrão para sistemas utilizando netcore 8 + vue 3 (+vuetify)

## Detalhes Técnicos
**MaSistemas** usa os seguintes projetos para funcionar:
- [Dotnet Core 8]
- [VueJS 3]
- [Vuetify 3]

# Compilação e Publicação do Projeto
**MaSistemas** requer para compilação e publicação:
- [Microsoft .NET SDK 8]
- [Node JS v18 ou Superior]
- [Vue Cli 4 ou Superior]
- [VS Code] Para edição e compilação/publicação utilizando as tasks do VS Code

### Antes de tudo
1.  Abra a pasta principal do projeto "MaSistemas" (local do arquivo MaSistemas.sln) em um console/shell e execute:
```
dotnet restore
```

2.  abra a pasta "MaSistemas/Web/VueApp" em um console/shell e execute:
```
npm install
```


#### Tasks do **VS Code** para compilação e publicação
1. build - Compila o projeto Backend utilizando o DotNet SDK 8.
2. buildFrontEnd - Compila o Frontend em Vue3 (vite).
3. serveFrontEnd - Excuta o npm para servir o FrontEnd na porta especificada (usado durante o desenvolvimento)
4. publishWindows-x64 - Publica o projeto "Publish\Windows-x64".
5. publishLinux-x64   - Publica o projeto "Publish\Linux-x64".
6. publishLinux-Arm64 - Publica o projeto "Publish\Linux-Arm64".


[Microsoft .NET SDK 8]: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
[Node JS v18 ou Superior]: https://nodejs.org/
[Vue Cli 4 ou Superior]: https://cli.vuejs.org/
[VS Code]: https://code.visualstudio.com/download