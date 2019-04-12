-- phpMyAdmin SQL Dump
-- version 4.8.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 12. Apr 2019 um 13:15
-- Server-Version: 10.1.37-MariaDB
-- PHP-Version: 7.3.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `funbank`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `accounts`
--

CREATE TABLE `accounts` (
  `id` smallint(5) NOT NULL,
  `humanId` smallint(5) NOT NULL,
  `pass` varchar(64) NOT NULL,
  `balance` double(10,2) DEFAULT NULL,
  `token` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `accounts`
--

INSERT INTO `accounts` (`id`, `humanId`, `pass`, `balance`, `token`) VALUES
(32765, 32760, '12345', NULL, NULL),
(32767, 32760, '12345', NULL, NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `atm`
--

CREATE TABLE `atm` (
  `id` smallint(5) NOT NULL,
  `fivehundred` smallint(3) DEFAULT NULL,
  `twohundred` smallint(3) DEFAULT NULL,
  `onehundred` smallint(3) DEFAULT NULL,
  `fifty` smallint(3) DEFAULT NULL,
  `twenty` smallint(3) DEFAULT NULL,
  `ten` smallint(3) DEFAULT NULL,
  `five` smallint(3) DEFAULT NULL,
  `zip` varchar(5) NOT NULL,
  `token` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `atm`
--

INSERT INTO `atm` (`id`, `fivehundred`, `twohundred`, `onehundred`, `fifty`, `twenty`, `ten`, `five`, `zip`, `token`) VALUES
(1, 10, 20, 40, 80, 100, 120, 150, '07747', 'atm01'),
(2, 10, 20, 40, 80, 100, 120, 150, '07747', 'atm02'),
(3, 10, 20, 40, 80, 100, 120, 150, '07747', 'atm03'),
(4, 10, 20, 40, 80, 100, 120, 150, '07747', 'atm04');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `errorcodes`
--

CREATE TABLE `errorcodes` (
  `errorcode` tinyint(2) NOT NULL,
  `description` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `errorcode_atm`
--

CREATE TABLE `errorcode_atm` (
  `id` smallint(5) NOT NULL,
  `atm_id` smallint(5) NOT NULL,
  `errorcode` tinyint(2) NOT NULL,
  `errorDateTime` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `humans`
--

CREATE TABLE `humans` (
  `id` smallint(5) NOT NULL,
  `pass` varchar(64) NOT NULL,
  `name` varchar(64) NOT NULL,
  `firstname` varchar(64) NOT NULL,
  `city` char(30) DEFAULT NULL,
  `rights` tinyint(4) NOT NULL DEFAULT '0',
  `token` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `humans`
--

INSERT INTO `humans` (`id`, `pass`, `name`, `firstname`, `city`, `rights`, `token`) VALUES
(32760, '12345', 'Merkel', 'Angela', 'Berlin', 0, NULL),
(32765, '12345', 'Mueller', 'Max', 'Erfurt', 0, NULL),
(32767, '12345', 'Doering', 'Christopher', NULL, 0, NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `transfers`
--

CREATE TABLE `transfers` (
  `id` smallint(5) NOT NULL,
  `sender` smallint(5) NOT NULL,
  `reciever` smallint(5) NOT NULL,
  `amount` double(10,2) NOT NULL,
  `transferDate` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `accounts`
--
ALTER TABLE `accounts`
  ADD PRIMARY KEY (`id`),
  ADD KEY `humanId` (`humanId`);

--
-- Indizes für die Tabelle `atm`
--
ALTER TABLE `atm`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `errorcodes`
--
ALTER TABLE `errorcodes`
  ADD PRIMARY KEY (`errorcode`);

--
-- Indizes für die Tabelle `errorcode_atm`
--
ALTER TABLE `errorcode_atm`
  ADD PRIMARY KEY (`id`),
  ADD KEY `atm_id` (`atm_id`),
  ADD KEY `errorcode` (`errorcode`);

--
-- Indizes für die Tabelle `humans`
--
ALTER TABLE `humans`
  ADD PRIMARY KEY (`id`);

--
-- Indizes für die Tabelle `transfers`
--
ALTER TABLE `transfers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `sender` (`sender`),
  ADD KEY `reciever` (`reciever`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `accounts`
--
ALTER TABLE `accounts`
  MODIFY `id` smallint(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32768;

--
-- AUTO_INCREMENT für Tabelle `atm`
--
ALTER TABLE `atm`
  MODIFY `id` smallint(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `errorcode_atm`
--
ALTER TABLE `errorcode_atm`
  MODIFY `id` smallint(5) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT für Tabelle `humans`
--
ALTER TABLE `humans`
  MODIFY `id` smallint(5) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=32768;

--
-- AUTO_INCREMENT für Tabelle `transfers`
--
ALTER TABLE `transfers`
  MODIFY `id` smallint(5) NOT NULL AUTO_INCREMENT;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `accounts`
--
ALTER TABLE `accounts`
  ADD CONSTRAINT `accounts_ibfk_1` FOREIGN KEY (`humanId`) REFERENCES `humans` (`id`);

--
-- Constraints der Tabelle `errorcode_atm`
--
ALTER TABLE `errorcode_atm`
  ADD CONSTRAINT `errorcode_atm_ibfk_1` FOREIGN KEY (`atm_id`) REFERENCES `atm` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `errorcode_atm_ibfk_2` FOREIGN KEY (`errorcode`) REFERENCES `errorcodes` (`errorcode`);

--
-- Constraints der Tabelle `transfers`
--
ALTER TABLE `transfers`
  ADD CONSTRAINT `transfers_ibfk_1` FOREIGN KEY (`sender`) REFERENCES `accounts` (`id`),
  ADD CONSTRAINT `transfers_ibfk_2` FOREIGN KEY (`reciever`) REFERENCES `accounts` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
