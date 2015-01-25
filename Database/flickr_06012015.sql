-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.5-10.0.14-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win32
-- HeidiSQL Version:             8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for flickr_tables
DROP DATABASE IF EXISTS `flickr_tables`;
CREATE DATABASE IF NOT EXISTS `flickr_tables` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `flickr_tables`;


-- Dumping structure for table flickr_tables.categories
DROP TABLE IF EXISTS `categories`;
CREATE TABLE IF NOT EXISTS `categories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Category` varchar(200) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.commentsuser
DROP TABLE IF EXISTS `commentsuser`;
CREATE TABLE IF NOT EXISTS `commentsuser` (
  `Id` smallint(11) NOT NULL AUTO_INCREMENT,
  `PhotoId` varchar(200) NOT NULL,
  `UserId` varchar(200) NOT NULL,
  `CommentatorId` varchar(200) NOT NULL,
  `Comment` varchar(900) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.friends
DROP TABLE IF EXISTS `friends`;
CREATE TABLE IF NOT EXISTS `friends` (
  `IdFriend` varchar(100) NOT NULL,
  `PhotoUrl` varchar(300) DEFAULT NULL,
  `UserName` varchar(100) DEFAULT NULL,
  `FullName` varchar(200) DEFAULT NULL,
  `Description` varchar(100) DEFAULT NULL,
  `Tags` smallint(11) DEFAULT NULL,
  PRIMARY KEY (`IdFriend`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.friendtags
DROP TABLE IF EXISTS `friendtags`;
CREATE TABLE IF NOT EXISTS `friendtags` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FriendId` varchar(100) DEFAULT NULL,
  `Tag` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_friendtags_friends` (`FriendId`),
  CONSTRAINT `FK_friendtags_friends` FOREIGN KEY (`FriendId`) REFERENCES `friends` (`IdFriend`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.linkfriendcategory
DROP TABLE IF EXISTS `linkfriendcategory`;
CREATE TABLE IF NOT EXISTS `linkfriendcategory` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdFriend` varchar(100) NOT NULL,
  `IdCategory` int(11) NOT NULL,
  `Tag` varchar(300) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IdFriend_IdCategory_Tag` (`IdFriend`,`IdCategory`,`Tag`),
  KEY `FK_linkfriendcategory_categories` (`IdCategory`),
  KEY `FK_linkfriendcategory_words` (`Tag`),
  CONSTRAINT `FK_linkfriendcategory_categories` FOREIGN KEY (`IdCategory`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_linkfriendcategory_friends` FOREIGN KEY (`IdFriend`) REFERENCES `friends` (`IdFriend`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.linkfriends
DROP TABLE IF EXISTS `linkfriends`;
CREATE TABLE IF NOT EXISTS `linkfriends` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IdUser` varchar(50) DEFAULT NULL,
  `IdFriend` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IdFriend1_IdFriend2` (`IdUser`,`IdFriend`),
  KEY `FK_linkfriends_friends` (`IdFriend`),
  CONSTRAINT `FK_linkfriends_friends` FOREIGN KEY (`IdFriend`) REFERENCES `friends` (`IdFriend`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_linkfriends_user` FOREIGN KEY (`IdUser`) REFERENCES `user` (`UserId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.linkusercategory
DROP TABLE IF EXISTS `linkusercategory`;
CREATE TABLE IF NOT EXISTS `linkusercategory` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(100) NOT NULL,
  `Category` int(11) NOT NULL,
  `Tag` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserId_Category_Tag` (`UserId`,`Category`,`Tag`),
  KEY `FK_linkusercategory_categories` (`Category`),
  CONSTRAINT `FK_linkusercategory_categories` FOREIGN KEY (`Category`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `F_U` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.user
DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `UserId` varchar(100) NOT NULL,
  `UserName` varchar(200) DEFAULT NULL,
  `PhotoUrl` varchar(300) DEFAULT NULL,
  `Full Name` varchar(200) DEFAULT NULL,
  `Token` varchar(300) DEFAULT NULL,
  PRIMARY KEY (`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.usertags
DROP TABLE IF EXISTS `usertags`;
CREATE TABLE IF NOT EXISTS `usertags` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(100) NOT NULL,
  `Tag` varchar(300) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_usertags_user` (`UserId`),
  CONSTRAINT `FK_usertags_user` FOREIGN KEY (`UserId`) REFERENCES `user` (`UserId`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.


-- Dumping structure for table flickr_tables.words
DROP TABLE IF EXISTS `words`;
CREATE TABLE IF NOT EXISTS `words` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Word` varchar(50) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Words_categories` (`CategoryId`),
  CONSTRAINT `FK_Words_categories` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
