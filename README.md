# Films
# Projeto criado no Visual Studio 2019

Instuções para rodar o projeto:
# banco de dados
Utilizar o migrations executando os seguintes comandos dentro da pasta do projeto "/Films":
  "Add-Migration InitialMigration" para criação das classes do banco.
  "Update-Database" para criação do banco e aplicação das alterações.
# projeto
  Dentro do projeto executar "dotnet run" para iniciar a API. A primeira vez que o projeto iniciar, 
  sera feita a carga do banco a partir do arquivo csv fornecido, e o mesmo devera estar na pasta "/Data".
  O método da controller da API que irá retornar os dados é o "GetMovieList". Exemplo de chamada:
  http://localhost:5000/api/movielists
# testes dos dados
  A classe de testes dos dados está dentro do projeto "Films.tests".
  Para execução manual: comando "VSTest.Console.exe Films.Tests.dll" dentro da pasta on estao as dlls ("\Films.tests\bin\Debug\netcoreapp3.1")
  
