
DROP TABLE IF EXISTS Empresas;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
-- SQLINES FOR EVALUATION USE ONLY (14 DAYS)
CREATE TABLE Empresas (
  Id int NOT NULL IDENTITY,
  Nome varchar(50) NOT NULL,
  CpfCnpj varchar(20) DEFAULT NULL,
  Ativo bit DEFAULT '1',
  AdministradoraGlobal bit DEFAULT '0',
  PRIMARY KEY (Id)
)  ;

DROP TABLE IF EXISTS SistemaGrupos;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaGrupos (
  Id int NOT NULL IDENTITY,
  Nome varchar(60) NOT NULL,
  Ativo bit DEFAULT '1',
  GrupoDeMenu bit DEFAULT '0',
  UsoInterno bit DEFAULT '0',
  AdminSistema bit DEFAULT '0',
  PRIMARY KEY (Id)
)   ;


DROP TABLE IF EXISTS SistemaMenus;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaMenus (
  Id int NOT NULL IDENTITY,
  MenuPaiId int DEFAULT NULL,
  Rota varchar(50) DEFAULT NULL,
  Icone varchar(50) DEFAULT NULL,
  Nome varchar(50) DEFAULT NULL,
  Divisor bit DEFAULT '0',
  Ordem int DEFAULT '0',
  Ativo bit DEFAULT '1',
  Publico bit DEFAULT '1',
  PRIMARY KEY (Id)
)  ;

DROP TABLE IF EXISTS SistemaUsuarios;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaUsuarios (
  Id int NOT NULL IDENTITY,
  Nome varchar(50) NOT NULL,
  EMail varchar(50) DEFAULT NULL,
  Senha varchar(100) NOT NULL,
  TentativasErradas int DEFAULT '0',
  Ativo bit DEFAULT '1',
  Salt varchar(50) DEFAULT NULL,
  ChaveResetSenha varchar(100) DEFAULT NULL,
  Admin bit DEFAULT '1',
  Interno bit DEFAULT '1',
  ResetarSenha bit DEFAULT '1',
  EmpresaId int DEFAULT NULL,
  Telefone varchar(45) DEFAULT NULL,
  MenuLateral bit DEFAULT '1',
  PRIMARY KEY (Id)
,
  CONSTRAINT fk_SistemaUsuarios_1 FOREIGN KEY (EmpresaId) REFERENCES Empresas (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
)   ;

CREATE INDEX fk_SistemaUsuarios_1_idx ON SistemaUsuarios (EmpresaId);


DROP TABLE IF EXISTS SistemaGrupoMenus;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaGrupoMenus (
  Id int NOT NULL IDENTITY,
  SistemaMenuId int NOT NULL,
  SistemaGrupoMenuId int NOT NULL,
  PRIMARY KEY (Id)
,
  CONSTRAINT FK_SistemaGrupoMenus_SistemaGrupos FOREIGN KEY (SistemaGrupoMenuId) REFERENCES SistemaGrupos (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT FK_SistemaGrupoMenus_SistemaMenus FOREIGN KEY (SistemaMenuId) REFERENCES SistemaMenus (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
)   ;

CREATE INDEX FK_SistemaGrupoMenus_SistemaMenus ON SistemaGrupoMenus (SistemaMenuId);
CREATE INDEX FK_SistemaGrupoMenus_SistemaGrupos ON SistemaGrupoMenus (SistemaGrupoMenuId);


DROP TABLE IF EXISTS SistemaGrupoUsuarios;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaGrupoUsuarios (
  Id int NOT NULL IDENTITY,
  SistemaUsuarioId int NOT NULL,
  SistemaGrupoUsuarioId int NOT NULL,
  PRIMARY KEY (Id)
,
  CONSTRAINT FK_SistemaGrupoUsuarios_SistemaGrupos FOREIGN KEY (SistemaGrupoUsuarioId) REFERENCES SistemaGrupos (Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT FK_SistemaGrupoUsuarios_SistemaUsuarios FOREIGN KEY (SistemaUsuarioId) REFERENCES SistemaUsuarios (Id) ON DELETE NO ACTION ON UPDATE NO ACTION
)   ;

CREATE INDEX FK_SistemaGrupoUsuarios_SistemaUsuarios ON SistemaGrupoUsuarios (SistemaUsuarioId);
CREATE INDEX FK_SistemaGrupoUsuarios_SistemaGrupos ON SistemaGrupoUsuarios (SistemaGrupoUsuarioId);



DROP TABLE IF EXISTS SistemaParametros;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaParametros (
  Id int NOT NULL IDENTITY,
  EmailFrom varchar(100) DEFAULT NULL,
  EmailTo varchar(100) DEFAULT NULL,
  EmailCc varchar(100) DEFAULT NULL,
  EmailCco varchar(100) DEFAULT NULL,
  EmailServidor varchar(100) DEFAULT NULL,
  EmailPorta int DEFAULT '0',
  EmailLogin varchar(100) DEFAULT NULL,
  EmailSenha varchar(100) DEFAULT NULL,
  EmailSsl bit DEFAULT '0',
  PastaTemporarios varchar(250) DEFAULT NULL,
  PastaArquivos varchar(250) DEFAULT NULL,
  PRIMARY KEY (Id)
)  ;

--

DROP TABLE IF EXISTS SistemaPermissoes;
/* SQLINES DEMO *** d_cs_client     = @@character_set_client */;
/* SQLINES DEMO *** cter_set_client = utf8mb4 */;
CREATE TABLE SistemaPermissoes (
  Id int NOT NULL IDENTITY,
  SistemaGrupoUsuarioId int DEFAULT NULL,
  SistemaGrupoMenuId int DEFAULT NULL,
  SistemaUsuarioId int DEFAULT NULL,
  SistemaMenuId int DEFAULT NULL,
  PermissaoDeGrupoUsuario bit DEFAULT '0',
  PermissaoDeGrupoMenu bit DEFAULT '0',
  UsoInterno bit DEFAULT '0',
  [Index] bit DEFAULT '0',
  Edit bit DEFAULT '0',
  [Save] bit DEFAULT '0',
  Ativo bit DEFAULT '1',
  PRIMARY KEY (Id)
)   ;



/* */ 
SET IDENTITY_INSERT masistemas.dbo.Empresas ON;
INSERT INTO Empresas(Id,Nome,CpfCnpj,Ativo,AdministradoraGlobal)  VALUES (1,'M&A Suporte','12.123.123/0001-12',1,1);

SET IDENTITY_INSERT masistemas.dbo.SistemaGrupos ON;
INSERT INTO SistemaGrupos(Id,Nome,Ativo,GrupoDeMenu,UsoInterno,AdminSistema) VALUES (1,'GrupoAdmin',1,0,1,1),(2,'GrupoUsusariosAdmin',1,0,0,0),(3,'GrupoMenuCadastro',1,1,0,0),(4,'GrupoMenuSistema',1,1,0,0),(5,'UsuariosSistemaLeitura',1,0,0,0);

SET IDENTITY_INSERT masistemas.dbo.SistemaMenus ON;
INSERT INTO SistemaMenus (Id,MenuPaiId,Rota,Icone,Nome,Divisor,Ordem,Ativo,Publico) VALUES (1,0,'.','.','Menu Sistema',0,0,1,0),(2,1,'/#','mdi-archive-outline','Cadastro',0,2,1,0),(3,1,'/#','mdi-cog-outline','Sistema',0,4,1,0),(4,3,'/Sistema/Usuario','mdi-chevron-right','Usuários',0,1,1,0),(5,3,'/Sistema/Parametro','mdi-chevron-right','Parâmetros',0,5,1,0),(6,2,'/Cadastro/Empresa','mdi-chevron-right','Empresas',0,1,1,0),(7,1,'/','mdi-home-outline','Home',0,1,1,1),(9,3,'/Sistema/Menu','mdi-chevron-right','Menu',0,3,1,0),(15,3,'/Sistema/Grupo','mdi-chevron-right','Grupos',0,2,1,0),(20,3,'/Sistema/Permissao','mdi-chevron-right','Permissões',0,4,1,1),(21,1,'/Logout','mdi-exit-to-app','Sair',0,5,1,1),(24,1,'','mdi-ray-start-end','<divisor>',1,3,1,1),(25,2,'/Cadastro/Teste','mdi-chevron-right','Teste',0,2,1,1),(27,3,'/Sistema/Arquivo','mdi-chevron-right','Arquivos',0,6,1,1);

SET IDENTITY_INSERT masistemas.dbo.SistemaUsuarios ON;
INSERT INTO SistemaUsuarios (Id,Nome,EMail,Senha,TentativasErradas,Ativo,Salt,ChaveResetSenha,Admin,Interno,ResetarSenha,EmpresaId,Telefone,MenuLateral) VALUES (1,'Administrador','gerencia@masuporte.com.br','2K4a9C0ow5Ld5ObzJI60gomyKOd0IlLsRPMlBh3s7UIZ7TQVER0d8YK3wc9YXRWzMIpMX3sVX1YC3ant7cm/7w==',0,1,'AYmL/ZbezPlCZaWWhSJBpA==','',1,0,0,1,'(21)98878-9000',1),(3,'Rafael Barbosa','barbosa@masuporte.com.br','Nxc8INB4Vi9A/h/TJ4y9V6942N0UzaasjXKN1UAOg+5Wi05UmwYF56BzhMnWyzlG4yazEMl25d7rB+IT+9VjGw==',0,1,'FOLBzmMQba8Zh2PIBSGunA==','',0,0,0,1,'',1),(4,'Renato Mapelli','renato.mapelli@masuporte.com.br','zcgEPwwsZnkvVefKA/qG9BM++AkSSWj2ye3KvMH+F7Uq+kMr47vF8yLNPWJnso/L40x3BsornamV2McVN5TpWA==',0,1,'HN/jd6mh/387zutPeJOdAg==','',0,1,1,1,'',1),(5,'Diego Albuquerque','diego.albuquerque@masuporte.com.br','VDQLxO2kxOgWvJ/AJOl4n7F2CCXBuMtR8Rk/RrICM13OHqAKoq1xJLqfBifnF56WeIwmapbAHo+QC6LaCEG7+A==',0,1,'09zBccWDF7JTU+z8rSNt2Q==','',0,1,1,1,'',1);

SET IDENTITY_INSERT masistemas.dbo.SistemaParametros ON;
INSERT INTO SistemaParametros(Id,EmailFrom,EmailTo,EmailCc,EmailCco,EmailServidor,EmailPorta,EmailLogin,EmailSenha,EmailSsl,PastaTemporarios,PastaArquivos) VALUES (1,'barbosa@rafaelbarbosa.com.br','teste1@teste.com.br','teste2@teste.com.br','teste3@teste.com.br','smtp.rafaelbarbosa.com.br',587,'barbosa@rafaelbarbosa.com.br','teste1',0,'/var/tmp/MaSistemas/Temp','/var/tmp/MaSistemas/Arq');

SET IDENTITY_INSERT masistemas.dbo.SistemaGrupoMenus ON;
INSERT INTO SistemaGrupoMenus(Id,SistemaMenuId,SistemaGrupoMenuId) VALUES (1,6,3),(3,15,4),(4,9,4),(5,5,4),(6,20,4),(7,4,4),(8,25,3);


SET IDENTITY_INSERT masistemas.dbo.SistemaGrupoUsuarios ON;
INSERT INTO SistemaGrupoUsuarios(Id,SistemaUsuarioId,SistemaGrupoUsuarioId) VALUES (1,1,1),(4,3,2),(5,4,5),(7,5,5);

SET IDENTITY_INSERT masistemas.dbo.SistemaPermissoes ON;
INSERT INTO SistemaPermissoes(Id,SistemaGrupoUsuarioId,SistemaGrupoMenuId,SistemaUsuarioId,SistemaMenuId,PermissaoDeGrupoUsuario,PermissaoDeGrupoMenu,UsoInterno,"Index",Edit,"Save",Ativo) VALUES (11,2,4,NULL,NULL,1,1,0,1,1,1,1),(12,5,3,NULL,NULL,1,1,0,1,1,0,1);
/* */


SET IDENTITY_INSERT masistemas.dbo.Empresas OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaGrupoMenus OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaGrupoUsuarios OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaGrupos OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaMenus OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaParametros OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaPermissoes OFF;
SET IDENTITY_INSERT masistemas.dbo.SistemaUsuarios OFF;


