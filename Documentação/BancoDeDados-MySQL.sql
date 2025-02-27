-- MySQL dump 10.13  Distrib 8.0.36, for Linux (x86_64)
--
-- Host: localhost    Database: masistemas
-- ------------------------------------------------------
-- Server version	5.7.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Empresas`
--

DROP TABLE IF EXISTS `Empresas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Empresas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(50) NOT NULL,
  `CpfCnpj` varchar(20) DEFAULT NULL,
  `Ativo` tinyint(1) DEFAULT '1',
  `AdministradoraGlobal` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Empresas`
--

LOCK TABLES `Empresas` WRITE;
/*!40000 ALTER TABLE `Empresas` DISABLE KEYS */;
INSERT INTO `Empresas` VALUES (1,'M&A Suporte','12.123.123/0001-12',1,1);
/*!40000 ALTER TABLE `Empresas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaGrupoMenus`
--

DROP TABLE IF EXISTS `SistemaGrupoMenus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaGrupoMenus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SistemaMenuId` int(11) NOT NULL,
  `SistemaGrupoMenuId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_SistemaGrupoMenus_SistemaMenus` (`SistemaMenuId`),
  KEY `FK_SistemaGrupoMenus_SistemaGrupos` (`SistemaGrupoMenuId`),
  CONSTRAINT `FK_SistemaGrupoMenus_SistemaGrupos` FOREIGN KEY (`SistemaGrupoMenuId`) REFERENCES `SistemaGrupos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_SistemaGrupoMenus_SistemaMenus` FOREIGN KEY (`SistemaMenuId`) REFERENCES `SistemaMenus` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COMMENT='SISTEMA';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaGrupoMenus`
--

LOCK TABLES `SistemaGrupoMenus` WRITE;
/*!40000 ALTER TABLE `SistemaGrupoMenus` DISABLE KEYS */;
INSERT INTO `SistemaGrupoMenus` VALUES (1,6,3),(3,15,4),(4,9,4),(5,5,4),(6,20,4),(7,4,4),(8,25,3);
/*!40000 ALTER TABLE `SistemaGrupoMenus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaGrupoUsuarios`
--

DROP TABLE IF EXISTS `SistemaGrupoUsuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaGrupoUsuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SistemaUsuarioId` int(11) NOT NULL,
  `SistemaGrupoUsuarioId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_SistemaGrupoUsuarios_SistemaUsuarios` (`SistemaUsuarioId`),
  KEY `FK_SistemaGrupoUsuarios_SistemaGrupos` (`SistemaGrupoUsuarioId`),
  CONSTRAINT `FK_SistemaGrupoUsuarios_SistemaGrupos` FOREIGN KEY (`SistemaGrupoUsuarioId`) REFERENCES `SistemaGrupos` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_SistemaGrupoUsuarios_SistemaUsuarios` FOREIGN KEY (`SistemaUsuarioId`) REFERENCES `SistemaUsuarios` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COMMENT='SISTEMA';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaGrupoUsuarios`
--

LOCK TABLES `SistemaGrupoUsuarios` WRITE;
/*!40000 ALTER TABLE `SistemaGrupoUsuarios` DISABLE KEYS */;
INSERT INTO `SistemaGrupoUsuarios` VALUES (1,1,1),(4,3,2),(5,4,5),(7,5,5);
/*!40000 ALTER TABLE `SistemaGrupoUsuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaGrupos`
--

DROP TABLE IF EXISTS `SistemaGrupos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaGrupos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(60) NOT NULL,
  `Ativo` tinyint(1) DEFAULT '1',
  `GrupoDeMenu` tinyint(1) DEFAULT '0',
  `UsoInterno` tinyint(1) DEFAULT '0',
  `AdminSistema` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COMMENT='SISTEMA';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaGrupos`
--

LOCK TABLES `SistemaGrupos` WRITE;
/*!40000 ALTER TABLE `SistemaGrupos` DISABLE KEYS */;
INSERT INTO `SistemaGrupos` VALUES (1,'GrupoAdmin',1,0,1,1),(2,'GrupoUsusariosAdmin',1,0,0,0),(3,'GrupoMenuCadastro',1,1,0,0),(4,'GrupoMenuSistema',1,1,0,0),(5,'UsuariosSistemaLeitura',1,0,0,0);
/*!40000 ALTER TABLE `SistemaGrupos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaMenus`
--

DROP TABLE IF EXISTS `SistemaMenus`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaMenus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MenuPaiId` int(11) DEFAULT NULL,
  `Rota` varchar(50) DEFAULT NULL,
  `Icone` varchar(50) DEFAULT NULL,
  `Nome` varchar(50) DEFAULT NULL,
  `Divisor` tinyint(1) DEFAULT '0',
  `Ordem` int(11) DEFAULT '0',
  `Ativo` tinyint(1) DEFAULT '1',
  `Publico` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaMenus`
--

LOCK TABLES `SistemaMenus` WRITE;
/*!40000 ALTER TABLE `SistemaMenus` DISABLE KEYS */;
INSERT INTO `SistemaMenus` VALUES (1,0,'.','.','Menu Sistema',0,0,1,0),(2,1,'/#','mdi-archive-outline','Cadastro',0,2,1,0),(3,1,'/#','mdi-cog-outline','Sistema',0,4,1,0),(4,3,'/Sistema/Usuario','mdi-chevron-right','Usuários',0,1,1,0),(5,3,'/Sistema/Parametro','mdi-chevron-right','Parâmetros',0,5,1,0),(6,2,'/Cadastro/Empresa','mdi-chevron-right','Empresas',0,1,1,0),(7,1,'/','mdi-home-outline','Home',0,1,1,1),(9,3,'/Sistema/Menu','mdi-chevron-right','Menu',0,3,1,0),(15,3,'/Sistema/Grupo','mdi-chevron-right','Grupos',0,2,1,0),(20,3,'/Sistema/Permissao','mdi-chevron-right','Permissões',0,4,1,1),(21,1,'/Logout','mdi-exit-to-app','Sair',0,5,1,1),(24,1,'','mdi-ray-start-end','<divisor>',1,3,1,1),(25,2,'/Cadastro/Teste','mdi-chevron-right','Teste',0,2,1,1),(27,3,'/Sistema/Arquivo','mdi-chevron-right','Arquivos',0,6,1,1);
/*!40000 ALTER TABLE `SistemaMenus` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaParametros`
--

DROP TABLE IF EXISTS `SistemaParametros`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaParametros` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmailFrom` varchar(100) DEFAULT NULL,
  `EmailTo` varchar(100) DEFAULT NULL,
  `EmailCc` varchar(100) DEFAULT NULL,
  `EmailCco` varchar(100) DEFAULT NULL,
  `EmailServidor` varchar(100) DEFAULT NULL,
  `EmailPorta` int(11) DEFAULT '0',
  `EmailLogin` varchar(100) DEFAULT NULL,
  `EmailSenha` varchar(100) DEFAULT NULL,
  `EmailSsl` tinyint(1) DEFAULT '0',
  `PastaTemporarios` varchar(250) DEFAULT NULL,
  `PastaArquivos` varchar(250) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaParametros`
--

LOCK TABLES `SistemaParametros` WRITE;
/*!40000 ALTER TABLE `SistemaParametros` DISABLE KEYS */;
INSERT INTO `SistemaParametros` VALUES (1,'barbosa@rafaelbarbosa.com.br','teste1@teste.com.br','teste2@teste.com.br','teste3@teste.com.br','smtp.rafaelbarbosa.com.br',587,'barbosa@rafaelbarbosa.com.br','teste1',0,'/var/tmp/MaSistemas/Temp','/var/tmp/MaSistemas/Arq');
/*!40000 ALTER TABLE `SistemaParametros` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaPermissoes`
--

DROP TABLE IF EXISTS `SistemaPermissoes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaPermissoes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SistemaGrupoUsuarioId` int(11) DEFAULT NULL,
  `SistemaGrupoMenuId` int(11) DEFAULT NULL,
  `SistemaUsuarioId` int(11) DEFAULT NULL,
  `SistemaMenuId` int(11) DEFAULT NULL,
  `PermissaoDeGrupoUsuario` tinyint(1) DEFAULT '0',
  `PermissaoDeGrupoMenu` tinyint(1) DEFAULT '0',
  `UsoInterno` tinyint(1) DEFAULT '0',
  `Index` tinyint(1) DEFAULT '0',
  `Edit` tinyint(1) DEFAULT '0',
  `Save` tinyint(1) DEFAULT '0',
  `Ativo` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COMMENT='SISTEMA';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaPermissoes`
--

LOCK TABLES `SistemaPermissoes` WRITE;
/*!40000 ALTER TABLE `SistemaPermissoes` DISABLE KEYS */;
INSERT INTO `SistemaPermissoes` VALUES (11,2,4,NULL,NULL,1,1,0,1,1,1,1),(12,5,3,NULL,NULL,1,1,0,1,1,0,1);
/*!40000 ALTER TABLE `SistemaPermissoes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `SistemaUsuarios`
--

DROP TABLE IF EXISTS `SistemaUsuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `SistemaUsuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(50) NOT NULL,
  `EMail` varchar(50) DEFAULT NULL,
  `Senha` varchar(100) NOT NULL,
  `TentativasErradas` int(11) DEFAULT '0',
  `Ativo` tinyint(1) DEFAULT '1',
  `Salt` varchar(50) DEFAULT NULL,
  `ChaveResetSenha` varchar(100) DEFAULT NULL,
  `Admin` tinyint(1) DEFAULT '1',
  `Interno` tinyint(1) DEFAULT '1',
  `ResetarSenha` tinyint(1) DEFAULT '1',
  `EmpresaId` int(11) DEFAULT NULL,
  `Telefone` varchar(45) DEFAULT NULL,
  `MenuLateral` tinyint(1) DEFAULT '1',
  PRIMARY KEY (`Id`),
  KEY `fk_SistemaUsuarios_1_idx` (`EmpresaId`),
  CONSTRAINT `fk_SistemaUsuarios_1` FOREIGN KEY (`EmpresaId`) REFERENCES `Empresas` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COMMENT='SISTEMA';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `SistemaUsuarios`
--

LOCK TABLES `SistemaUsuarios` WRITE;
/*!40000 ALTER TABLE `SistemaUsuarios` DISABLE KEYS */;
INSERT INTO `SistemaUsuarios` VALUES (1,'Administrador','gerencia@masuporte.com.br','2K4a9C0ow5Ld5ObzJI60gomyKOd0IlLsRPMlBh3s7UIZ7TQVER0d8YK3wc9YXRWzMIpMX3sVX1YC3ant7cm/7w==',0,1,'AYmL/ZbezPlCZaWWhSJBpA==','',1,0,0,1,'(21)98878-9000',1),(3,'Rafael Barbosa','barbosa@masuporte.com.br','Nxc8INB4Vi9A/h/TJ4y9V6942N0UzaasjXKN1UAOg+5Wi05UmwYF56BzhMnWyzlG4yazEMl25d7rB+IT+9VjGw==',0,1,'FOLBzmMQba8Zh2PIBSGunA==','',0,0,0,1,'',1),(4,'Renato Mapelli','renato.mapelli@masuporte.com.br','zcgEPwwsZnkvVefKA/qG9BM++AkSSWj2ye3KvMH+F7Uq+kMr47vF8yLNPWJnso/L40x3BsornamV2McVN5TpWA==',0,1,'HN/jd6mh/387zutPeJOdAg==','',0,1,1,1,'',1),(5,'Diego Albuquerque','diego.albuquerque@masuporte.com.br','VDQLxO2kxOgWvJ/AJOl4n7F2CCXBuMtR8Rk/RrICM13OHqAKoq1xJLqfBifnF56WeIwmapbAHo+QC6LaCEG7+A==',0,1,'09zBccWDF7JTU+z8rSNt2Q==','',0,1,1,1,'',1);
/*!40000 ALTER TABLE `SistemaUsuarios` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-02-13 19:09:34
