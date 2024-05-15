/*
Author: Sam C
Date: May 15, 2024
Version: 2.0.0
*/

DROP DATABASE IF EXISTS `chatTCP`;
CREATE DATABASE IF NOT EXISTS `chatTCP`;
USE `chatTCP`;

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users`(
	`userID` INT NOT NULL,
    `username` VARCHAR(45) NOT NULL,
    `password` VARCHAR(45) NOT NULL,
    `displayname` VARCHAR(45) NOT NULL,
    `role` SMALLINT NOT NULL DEFAULT 1,
    PRIMARY KEY(`userID`));

DROP TABLE IF EXISTS `scores`;
CREATE TABLE IF NOT EXISTS `scores`(
	`user` INT NOT NULL,
    `wins` INT NOT NULL,
    `losses` INT NOT NULL,
    `draws` INT NOT NULL,
    FOREIGN KEY (`user`) REFERENCES users(`userID`));