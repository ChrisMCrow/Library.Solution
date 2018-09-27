-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: Sep 28, 2018 at 12:17 AM
-- Server version: 5.6.35
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `city_library`
--
CREATE DATABASE IF NOT EXISTS `city_library` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `city_library`;

-- --------------------------------------------------------

--
-- Table structure for table `authors`
--

CREATE TABLE `authors` (
  `id` int(32) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  `given_name` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `authors`
--

INSERT INTO `authors` (`id`, `last_name`, `given_name`) VALUES
(1, 'Merrick', 'Kramer'),
(3, 'Yancy', 'Peter'),
(4, 'Smith', 'Donny');

-- --------------------------------------------------------

--
-- Table structure for table `authors_books`
--

CREATE TABLE `authors_books` (
  `id` int(32) NOT NULL,
  `author_id` int(32) NOT NULL,
  `book_id` int(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `authors_books`
--

INSERT INTO `authors_books` (`id`, `author_id`, `book_id`) VALUES
(1, 1, 1),
(2, 3, 1),
(3, 1, 2),
(5, 3, 4),
(6, 2, 5),
(13, 3, 2),
(19, 1, 3),
(20, 3, 5);

-- --------------------------------------------------------

--
-- Table structure for table `books`
--

CREATE TABLE `books` (
  `id` int(32) NOT NULL,
  `title` varchar(255) NOT NULL,
  `cost` float NOT NULL,
  `current_count` int(32) NOT NULL,
  `total_count` int(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `books`
--

INSERT INTO `books` (`id`, `title`, `cost`, `current_count`, `total_count`) VALUES
(1, 'As the World Ends', 14.99, 14, 10),
(2, 'At War With Life', 19.99, 2, 5),
(3, 'My First Baby Step', 9.99, 17, 20),
(4, 'Sword of Thunder', 16, 8, 8),
(5, 'The Wind - an Epic Poem', 15.99, 3, 3);

-- --------------------------------------------------------

--
-- Table structure for table `checkouts`
--

CREATE TABLE `checkouts` (
  `id` int(32) NOT NULL,
  `book_id` int(32) NOT NULL,
  `patrons_id` int(32) NOT NULL,
  `checkout_date` datetime NOT NULL,
  `due_date` datetime NOT NULL,
  `returned` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `checkouts`
--

INSERT INTO `checkouts` (`id`, `book_id`, `patrons_id`, `checkout_date`, `due_date`, `returned`) VALUES
(1, 1, 1, '2018-09-04 00:00:00', '2018-09-01 00:00:00', 1),
(2, 2, 3, '2018-09-20 00:00:00', '2018-09-30 00:00:00', 0),
(3, 4, 3, '2018-09-20 00:00:00', '2018-09-30 00:00:00', 1),
(4, 1, 3, '2018-09-12 00:00:00', '2018-09-19 00:00:00', 1),
(6, 4, 6, '2018-09-27 09:34:37', '2018-10-11 09:34:37', 0),
(7, 1, 1, '2018-09-27 09:35:11', '2018-10-11 09:35:11', 1),
(8, 1, 1, '2018-09-27 09:37:03', '2018-10-11 09:37:03', 1),
(9, 3, 3, '2018-09-27 09:37:14', '2018-10-11 09:37:14', 0),
(10, 1, 1, '2018-09-27 09:40:01', '2018-10-11 09:40:01', 1),
(11, 2, 2, '2018-09-27 09:40:45', '2018-10-11 09:40:45', 1),
(12, 1, 5, '2018-09-27 09:41:22', '2018-10-11 09:41:22', 0),
(13, 1, 1, '2018-09-27 09:41:48', '2018-10-11 09:41:48', 1),
(14, 1, 1, '2018-09-27 09:42:39', '2018-10-11 09:42:39', 1),
(15, 1, 1, '2018-09-27 09:46:49', '2018-10-11 09:46:49', 1),
(16, 1, 1, '2018-09-27 09:47:27', '2018-10-11 09:47:27', 1),
(17, 1, 1, '2018-09-27 09:47:29', '2018-10-11 09:47:29', 1),
(18, 1, 1, '2018-09-27 09:47:30', '2018-10-11 09:47:30', 1),
(19, 1, 1, '2018-09-27 09:53:14', '2018-10-11 09:53:14', 1),
(20, 1, 1, '2018-09-27 09:55:36', '2018-10-11 09:55:36', 1),
(21, 3, 2, '2018-09-27 09:55:46', '2018-10-11 09:55:46', 0),
(22, 2, 1, '2018-09-27 09:58:07', '2018-10-11 09:58:07', 1),
(23, 2, 1, '2018-09-27 09:58:13', '2018-10-11 09:58:13', 0),
(24, 2, 1, '2018-09-27 09:58:15', '2018-10-11 09:58:15', 0),
(25, 2, 1, '2018-09-27 09:58:18', '2018-10-11 09:58:18', 0),
(26, 1, 1, '2018-09-27 10:44:14', '2018-10-11 10:44:14', 0),
(27, 3, 3, '2018-09-27 11:19:18', '2018-10-11 11:19:18', 0),
(28, 1, 3, '2018-09-27 11:22:27', '2018-10-11 11:22:27', 0),
(29, 3, 4, '2018-09-27 11:27:52', '2018-10-11 11:27:52', 1),
(30, 3, 4, '2018-09-27 15:12:58', '2018-10-11 15:12:58', 0),
(31, 1, 1, '2018-09-27 15:13:08', '2018-10-11 15:13:08', 0),
(32, 1, 4, '2018-09-27 15:13:15', '2018-10-11 15:13:15', 0);

-- --------------------------------------------------------

--
-- Table structure for table `patrons`
--

CREATE TABLE `patrons` (
  `id` int(32) NOT NULL,
  `last_name` varchar(255) NOT NULL,
  `given_name` varchar(255) NOT NULL,
  `overdue` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `patrons`
--

INSERT INTO `patrons` (`id`, `last_name`, `given_name`, `overdue`) VALUES
(1, 'Nelson', 'Brian', 1),
(2, 'Lee', 'Ryan', 0),
(3, 'Nguyen', 'Skye', 1),
(4, 'Crow', 'Chris', 0),
(5, 'Richard', 'William St Paul', 0),
(6, 'Lind', 'Lilly', 0),
(7, 'Lee', 'Chan', 0),
(8, 'Bradley', 'Cathy', 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `authors`
--
ALTER TABLE `authors`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `authors_books`
--
ALTER TABLE `authors_books`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `books`
--
ALTER TABLE `books`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `checkouts`
--
ALTER TABLE `checkouts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `patrons`
--
ALTER TABLE `patrons`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `authors`
--
ALTER TABLE `authors`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;
--
-- AUTO_INCREMENT for table `authors_books`
--
ALTER TABLE `authors_books`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;
--
-- AUTO_INCREMENT for table `books`
--
ALTER TABLE `books`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
--
-- AUTO_INCREMENT for table `checkouts`
--
ALTER TABLE `checkouts`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;
--
-- AUTO_INCREMENT for table `patrons`
--
ALTER TABLE `patrons`
  MODIFY `id` int(32) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
