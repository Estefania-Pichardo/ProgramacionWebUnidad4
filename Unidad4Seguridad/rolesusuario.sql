-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: rolesusuario
-- ------------------------------------------------------
-- Server version	5.7.18-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `alumno`
--

DROP TABLE IF EXISTS `alumno`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `alumno` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `NumControl` varchar(9) NOT NULL,
  `Nombre` varchar(45) NOT NULL,
  `IdMaestro` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `NumControl_UNIQUE` (`NumControl`),
  KEY `fk_IdMaestro_idx` (`IdMaestro`),
  CONSTRAINT `fk_IdMaestro` FOREIGN KEY (`IdMaestro`) REFERENCES `docente` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alumno`
--

LOCK TABLES `alumno` WRITE;
/*!40000 ALTER TABLE `alumno` DISABLE KEYS */;
INSERT INTO `alumno` VALUES (1,'171G0242','Estefania Pichardo Montes',1),(2,'171G0235','Saul Maldonado Gomez',1),(4,'171G0227','Nayla Garcia',4),(5,'171G0228','Sabastian Garcia ',2),(11,'171G0215','Enrique de la Peña',2),(13,'171G0244','David Andres Rios',3),(24,'171G0236','Esmeralda Lopez',1),(25,'171G0222','Carlos Alejandro Ramos',9),(26,'555555','Alumno de prueba',6),(29,'171G0256','Nayla Garcia',1),(37,'12347','Alumno prueba',5),(39,'14141','hola',5),(50,'171G0225','Alumno de ejemplo',1);
/*!40000 ALTER TABLE `alumno` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `director`
--

DROP TABLE IF EXISTS `director`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `director` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  `Contraseña` tinytext NOT NULL,
  `Clave` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Clave_UNIQUE` (`Clave`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `director`
--

LOCK TABLES `director` WRITE;
/*!40000 ALTER TABLE `director` DISABLE KEYS */;
INSERT INTO `director` VALUES (1,'Hector Padilla Lara','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92',2020);
/*!40000 ALTER TABLE `director` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `docente`
--

DROP TABLE IF EXISTS `docente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `docente` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  `Contraseña` tinytext NOT NULL,
  `Activo` bit(1) DEFAULT b'1',
  `Clave` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Clave_UNIQUE` (`Clave`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `docente`
--

LOCK TABLES `docente` WRITE;
/*!40000 ALTER TABLE `docente` DISABLE KEYS */;
INSERT INTO `docente` VALUES (1,'Ernestina Leija Ramirez','93FA3E4624676F2E9AA143911118B4547087E9B6E0B6076F2E1027D7A2DA2B0A','',1010),(2,'Adriana Ramirez','93FA3E4624676F2E9AA143911118B4547087E9B6E0B6076F2E1027D7A2DA2B0A','',3030),(3,'Arturo Borrego','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92','',4040),(4,'Juan Jose Reyes ','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92','',5050),(5,'Carlos Garza','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92','',6060),(6,'Pruebas editado','CA978112CA1BBDCAFAC231B39A23DC4DA786EFF8147C4E72B9807785AFEE48BB','',1212),(7,'Jose luis Lara Mendez','8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92','',1313),(8,'Docente de prueba','4C8B422307AC7BDF38C2C17BAB533EAD4FC28D6DAEC176B195EF8A25A20A53E2','',1515),(9,'Dora Lilia Guadiana','499791C1224940E9DC3DA36594B19E3D91BFF794DFF580E4FFA43269EEE41D64','',3656),(10,'Docente de ejemplo ','B221D9DBB083A7F33428D7C2A3C3198AE925614D70210E28716CCAA7CD4DDB79','',3535);
/*!40000 ALTER TABLE `docente` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-12-13 12:41:00
