-- --------------------------------------------------------
-- Servidor:                     localhost
-- Versão do servidor:           11.8.2-MariaDB-ubu2404 - mariadb.org binary distribution
-- OS do Servidor:               debian-linux-gnu
-- HeidiSQL Versão:              12.10.0.7000
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Copiando estrutura do banco de dados para impedimento
CREATE DATABASE IF NOT EXISTS `impedimento` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `impedimento`;

-- Copiando estrutura para tabela impedimento.Advogados
CREATE TABLE IF NOT EXISTS `Advogados` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` varchar(100) DEFAULT NULL,
  `Telefone` varchar(100) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `Sigla` varchar(3) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.Advogados: ~2 rows (aproximadamente)
DELETE FROM `Advogados`;
INSERT INTO `Advogados` (`Id`, `Nome`, `Telefone`, `Email`, `Sigla`) VALUES
	(1, 'RBC', NULL, 'RBC', 'RBC'),
	(2, 'AMF', NULL, NULL, 'AMF'),
	(3, 'MPC', NULL, NULL, 'MPC');

-- Copiando estrutura para tabela impedimento.Empresas
CREATE TABLE IF NOT EXISTS `Empresas` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `CpfCnpj` longtext DEFAULT NULL,
  `Nome` longtext DEFAULT NULL,
  `Ativo` tinyint(1) NOT NULL,
  `AdministradoraGlobal` tinyint(1) NOT NULL,
  `DataContrato` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.Empresas: ~0 rows (aproximadamente)
DELETE FROM `Empresas`;
INSERT INTO `Empresas` (`Id`, `CpfCnpj`, `Nome`, `Ativo`, `AdministradoraGlobal`, `DataContrato`) VALUES
	(1, '01033831000122', 'M&A Suporte', 1, 1, NULL);

-- Copiando estrutura para tabela impedimento.Impedimento
CREATE TABLE IF NOT EXISTS `Impedimento` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `DataImpedimento` date NOT NULL,
  `AdvogadoId` int(11) NOT NULL,
  `Objeto` longtext DEFAULT NULL,
  `ParteA` text DEFAULT NULL,
  `ParteB` text DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Impedimento_AdvogadoId` (`AdvogadoId`),
  CONSTRAINT `FK_Impedimento_Advogados_AdvogadoId` FOREIGN KEY (`AdvogadoId`) REFERENCES `Advogados` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.Impedimento: ~3 rows (aproximadamente)
DELETE FROM `Impedimento`;
INSERT INTO `Impedimento` (`Id`, `DataImpedimento`, `AdvogadoId`, `Objeto`, `ParteA`, `ParteB`) VALUES
	(1, '2025-07-01', 2, 'opinião legal sobre responsabilidade da Consulente por superveniências ativas no âmbito de restruturação societária da área de logística da Vale.\r\n\r\nValor: R$ 100 milhões.\r\n', 'Ultrafértil S.A. (já é cliente)', 'Vale S.A. (já advogamos contra)'),
	(2, '2025-07-01', 3, 'Os principais credores são trabalhistas, além da Fundação Atlântico Seguridade Social.', 'SEREDE SERVIÇOS DE REDE S.A.', NULL),
	(3, '2025-07-01', 1, 'Serviço: assessorar o Consulente na compra (i) dos créditos detidos pelo Itáu e pela Planner contra a massa falida da Crefisul Leasing e (ii) dos direitos da massa falida do Banco Crefisul na Crefisul Leasing (o banco era o controlador da Leasing).', ' Galápagos Capital (sócios: André Di Sarno, Arnaldo Curvello, Bruno Carvalho e Carlos Fonseca)', NULL);

-- Copiando estrutura para tabela impedimento.ImpedimentoVerificacao
CREATE TABLE IF NOT EXISTS `ImpedimentoVerificacao` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ImpedimentoId` int(11) NOT NULL,
  `AdvogadoId` int(11) NOT NULL,
  `FlagImpedimento` longtext DEFAULT NULL,
  `DataVerificacao` datetime(6) NOT NULL,
  `Observacao` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ImpedimentoVerificacao_AdvogadoId` (`AdvogadoId`),
  KEY `IX_ImpedimentoVerificacao_ImpedimentoId` (`ImpedimentoId`),
  CONSTRAINT `FK_ImpedimentoVerificacao_Advogados_AdvogadoId` FOREIGN KEY (`AdvogadoId`) REFERENCES `Advogados` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ImpedimentoVerificacao_Impedimento_ImpedimentoId` FOREIGN KEY (`ImpedimentoId`) REFERENCES `Impedimento` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.ImpedimentoVerificacao: ~0 rows (aproximadamente)
DELETE FROM `ImpedimentoVerificacao`;

-- Copiando estrutura para tabela impedimento.SistemaAuditorias
CREATE TABLE IF NOT EXISTS `SistemaAuditorias` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioId` int(11) NOT NULL,
  `Classe` longtext NOT NULL,
  `ClasseId` int(11) NOT NULL,
  `ValorAnterior` longtext NOT NULL,
  `ValorNovo` longtext NOT NULL,
  `Operacao` longtext NOT NULL,
  `DataAlteracao` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaAuditorias_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_SistemaAuditorias_SistemaUsuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `SistemaUsuarios` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaAuditorias: ~0 rows (aproximadamente)
DELETE FROM `SistemaAuditorias`;

-- Copiando estrutura para tabela impedimento.SistemaGrupoMenus
CREATE TABLE IF NOT EXISTS `SistemaGrupoMenus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SistemaMenuId` int(11) NOT NULL,
  `SistemaGrupoMenuId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaGrupoMenus_SistemaGrupoMenuId` (`SistemaGrupoMenuId`),
  KEY `IX_SistemaGrupoMenus_SistemaMenuId` (`SistemaMenuId`),
  CONSTRAINT `FK_SistemaGrupoMenus_SistemaGrupos_SistemaGrupoMenuId` FOREIGN KEY (`SistemaGrupoMenuId`) REFERENCES `SistemaGrupos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_SistemaGrupoMenus_SistemaMenus_SistemaMenuId` FOREIGN KEY (`SistemaMenuId`) REFERENCES `SistemaMenus` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaGrupoMenus: ~0 rows (aproximadamente)
DELETE FROM `SistemaGrupoMenus`;

-- Copiando estrutura para tabela impedimento.SistemaGrupos
CREATE TABLE IF NOT EXISTS `SistemaGrupos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` longtext DEFAULT NULL,
  `Ativo` tinyint(1) NOT NULL,
  `GrupoDeMenu` tinyint(1) NOT NULL,
  `UsoInterno` tinyint(1) NOT NULL,
  `AdminSistema` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaGrupos: ~0 rows (aproximadamente)
DELETE FROM `SistemaGrupos`;
INSERT INTO `SistemaGrupos` (`Id`, `Nome`, `Ativo`, `GrupoDeMenu`, `UsoInterno`, `AdminSistema`) VALUES
	(1, 'Grupo Admin', 1, 0, 1, 1);

-- Copiando estrutura para tabela impedimento.SistemaGrupoUsuarios
CREATE TABLE IF NOT EXISTS `SistemaGrupoUsuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SistemaUsuarioId` int(11) NOT NULL,
  `SistemaGrupoUsuarioId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaGrupoUsuarios_SistemaGrupoUsuarioId` (`SistemaGrupoUsuarioId`),
  KEY `IX_SistemaGrupoUsuarios_SistemaUsuarioId` (`SistemaUsuarioId`),
  CONSTRAINT `FK_SistemaGrupoUsuarios_SistemaGrupos_SistemaGrupoUsuarioId` FOREIGN KEY (`SistemaGrupoUsuarioId`) REFERENCES `SistemaGrupos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_SistemaGrupoUsuarios_SistemaUsuarios_SistemaUsuarioId` FOREIGN KEY (`SistemaUsuarioId`) REFERENCES `SistemaUsuarios` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaGrupoUsuarios: ~1 rows (aproximadamente)
DELETE FROM `SistemaGrupoUsuarios`;
INSERT INTO `SistemaGrupoUsuarios` (`Id`, `SistemaUsuarioId`, `SistemaGrupoUsuarioId`) VALUES
	(1, 1, 1);

-- Copiando estrutura para tabela impedimento.SistemaMensagens
CREATE TABLE IF NOT EXISTS `SistemaMensagens` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UsuarioId` int(11) NOT NULL,
  `Titulo` longtext DEFAULT NULL,
  `Texto` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaMensagens_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_SistemaMensagens_SistemaUsuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `SistemaUsuarios` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaMensagens: ~0 rows (aproximadamente)
DELETE FROM `SistemaMensagens`;

-- Copiando estrutura para tabela impedimento.SistemaMensagensCaixa
CREATE TABLE IF NOT EXISTS `SistemaMensagensCaixa` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MensagemId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  `Lida` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaMensagensCaixa_MensagemId` (`MensagemId`),
  KEY `IX_SistemaMensagensCaixa_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_SistemaMensagensCaixa_SistemaMensagens_MensagemId` FOREIGN KEY (`MensagemId`) REFERENCES `SistemaMensagens` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_SistemaMensagensCaixa_SistemaUsuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `SistemaUsuarios` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaMensagensCaixa: ~0 rows (aproximadamente)
DELETE FROM `SistemaMensagensCaixa`;

-- Copiando estrutura para tabela impedimento.SistemaMensagensPara
CREATE TABLE IF NOT EXISTS `SistemaMensagensPara` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `MensagemId` int(11) NOT NULL,
  `UsuarioId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaMensagensPara_MensagemId` (`MensagemId`),
  KEY `IX_SistemaMensagensPara_UsuarioId` (`UsuarioId`),
  CONSTRAINT `FK_SistemaMensagensPara_SistemaMensagens_MensagemId` FOREIGN KEY (`MensagemId`) REFERENCES `SistemaMensagens` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_SistemaMensagensPara_SistemaUsuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `SistemaUsuarios` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaMensagensPara: ~0 rows (aproximadamente)
DELETE FROM `SistemaMensagensPara`;

-- Copiando estrutura para tabela impedimento.SistemaMenus
CREATE TABLE IF NOT EXISTS `SistemaMenus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nome` longtext NOT NULL,
  `Rota` longtext NOT NULL,
  `Icone` longtext NOT NULL,
  `Divisor` tinyint(1) NOT NULL,
  `MenuPaiId` int(11) DEFAULT NULL,
  `Ordem` int(11) NOT NULL,
  `Ativo` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaMenus_MenuPaiId` (`MenuPaiId`),
  CONSTRAINT `FK_SistemaMenus_SistemaMenus_MenuPaiId` FOREIGN KEY (`MenuPaiId`) REFERENCES `SistemaMenus` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaMenus: ~11 rows (aproximadamente)
DELETE FROM `SistemaMenus`;
INSERT INTO `SistemaMenus` (`Id`, `Nome`, `Rota`, `Icone`, `Divisor`, `MenuPaiId`, `Ordem`, `Ativo`) VALUES
	(1, 'Menu Sistema', '.', '.', 0, NULL, 0, 1),
	(2, 'Home', '/', 'mdi-home-outline', 0, 1, 1, 1),
	(3, 'Cadastro', '/#', 'mdi-archive-outline', 0, 1, 3, 1),
	(4, 'Sistema', '/#', 'mdi-cog-outline', 0, 1, 4, 1),
	(5, 'Empresas', '/Cadastro/Empresa', 'mdi-chevron-right', 0, 3, 1, 1),
	(6, 'Usuários', '/Sistema/Usuario', 'mdi-chevron-right', 0, 4, 1, 1),
	(7, 'Grupos', '/Sistema/Grupo', 'mdi-chevron-right', 0, 4, 2, 1),
	(8, 'Permissões', '/Sistema/Permissao', 'mdi-chevron-right', 0, 4, 3, 1),
	(9, 'Menu', '/Sistema/Menu', 'mdi-chevron-right', 0, 4, 4, 1),
	(10, 'Parâmetros', '/Sistema/Parametro', 'mdi-chevron-right', 0, 4, 5, 1),
	(11, 'Impedimento', '/Impedimento', '', 0, 1, 2, 1);

-- Copiando estrutura para tabela impedimento.SistemaParametros
CREATE TABLE IF NOT EXISTS `SistemaParametros` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmailFrom` longtext DEFAULT NULL,
  `EmailTo` longtext DEFAULT NULL,
  `EmailCc` longtext DEFAULT NULL,
  `EmailCco` longtext DEFAULT NULL,
  `EmailServidor` longtext DEFAULT NULL,
  `EmailPorta` int(11) NOT NULL,
  `EmailLogin` longtext DEFAULT NULL,
  `EmailSenha` longtext DEFAULT NULL,
  `EmailSsl` tinyint(1) NOT NULL,
  `PastaTemporarios` longtext NOT NULL,
  `PastaArquivos` longtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaParametros: ~0 rows (aproximadamente)
DELETE FROM `SistemaParametros`;

-- Copiando estrutura para tabela impedimento.SistemaPermissoes
CREATE TABLE IF NOT EXISTS `SistemaPermissoes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SistemaGrupoUsuarioId` int(11) DEFAULT NULL,
  `SistemaGrupoMenuId` int(11) DEFAULT NULL,
  `SistemaUsuarioId` int(11) DEFAULT NULL,
  `SistemaMenuId` int(11) DEFAULT NULL,
  `PermissaoDeGrupoUsuario` tinyint(1) NOT NULL,
  `PermissaoDeGrupoMenu` tinyint(1) NOT NULL,
  `UsoInterno` tinyint(1) NOT NULL,
  `Ativo` tinyint(1) NOT NULL,
  `Index` tinyint(1) NOT NULL,
  `Edit` tinyint(1) NOT NULL,
  `Save` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaPermissoes_SistemaGrupoMenuId` (`SistemaGrupoMenuId`),
  KEY `IX_SistemaPermissoes_SistemaGrupoUsuarioId` (`SistemaGrupoUsuarioId`),
  KEY `IX_SistemaPermissoes_SistemaMenuId` (`SistemaMenuId`),
  KEY `IX_SistemaPermissoes_SistemaUsuarioId` (`SistemaUsuarioId`),
  CONSTRAINT `FK_SistemaPermissoes_SistemaGrupos_SistemaGrupoMenuId` FOREIGN KEY (`SistemaGrupoMenuId`) REFERENCES `SistemaGrupos` (`Id`),
  CONSTRAINT `FK_SistemaPermissoes_SistemaGrupos_SistemaGrupoUsuarioId` FOREIGN KEY (`SistemaGrupoUsuarioId`) REFERENCES `SistemaGrupos` (`Id`),
  CONSTRAINT `FK_SistemaPermissoes_SistemaMenus_SistemaMenuId` FOREIGN KEY (`SistemaMenuId`) REFERENCES `SistemaMenus` (`Id`),
  CONSTRAINT `FK_SistemaPermissoes_SistemaUsuarios_SistemaUsuarioId` FOREIGN KEY (`SistemaUsuarioId`) REFERENCES `SistemaUsuarios` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaPermissoes: ~0 rows (aproximadamente)
DELETE FROM `SistemaPermissoes`;

-- Copiando estrutura para tabela impedimento.SistemaUsuarios
CREATE TABLE IF NOT EXISTS `SistemaUsuarios` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EmpresaId` int(11) DEFAULT NULL,
  `Ativo` tinyint(1) NOT NULL,
  `Admin` tinyint(1) NOT NULL,
  `Senha` longtext NOT NULL,
  `Salt` longtext NOT NULL,
  `Telefone` longtext NOT NULL,
  `EMail` longtext NOT NULL,
  `MenuLateral` tinyint(1) NOT NULL,
  `Nome` longtext NOT NULL,
  `ChaveResetSenha` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SistemaUsuarios_EmpresaId` (`EmpresaId`),
  CONSTRAINT `FK_SistemaUsuarios_Empresas_EmpresaId` FOREIGN KEY (`EmpresaId`) REFERENCES `Empresas` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Copiando dados para a tabela impedimento.SistemaUsuarios: ~1 rows (aproximadamente)
DELETE FROM `SistemaUsuarios`;
INSERT INTO `SistemaUsuarios` (`Id`, `EmpresaId`, `Ativo`, `Admin`, `Senha`, `Salt`, `Telefone`, `EMail`, `MenuLateral`, `Nome`, `ChaveResetSenha`) VALUES
	(1, 1, 1, 1, '2K4a9C0ow5Ld5ObzJI60gomyKOd0IlLsRPMlBh3s7UIZ7TQVER0d8YK3wc9YXRWzMIpMX3sVX1YC3ant7cm/7w==', 'AYmL/ZbezPlCZaWWhSJBpA==', '', 'gerencia@masuporte.com.br', 1, 'Administrador', '');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
