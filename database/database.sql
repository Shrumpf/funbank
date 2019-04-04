CREATE DATABASE IF NOT EXISTS funbank;

USE funbank;

CREATE TABLE IF NOT EXISTS humans (
    id smallint(5) NOT NULL AUTO_INCREMENT,
    pass varchar(64) NOT NULL,
    name varchar(64) NOT NULL,
    firstname varchar(64) NOT NULL,
    rights tinyint DEFAULT 0 NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS accounts (
    id smallint(5) NOT NULL AUTO_INCREMENT,
    humanId smallint(5) NOT NULL,
    balance DOUBLE(10, 2),
    PRIMARY KEY (id),
    FOREIGN KEY (humanId) REFERENCES humans(id)
);

CREATE TABLE IF NOT EXISTS atm (
    id smallint(5) NOT NULL AUTO_INCREMENT,
    fivehundred tinyint(3),
    twohundred tinyint(3),
    onehundred tinyint(3),
    fifty tinyint(3),
    twenty tinyint(3),
    ten tinyint(3),
    five tinyint(3),
    zip varchar(5) NOT NULL,
    PRIMARY KEY(id)
);

CREATE TABLE IF NOT EXISTS errorCodes (
    errorcode tinyint(2) NOT NULL,
    description varchar(255),
    PRIMARY KEY (errorcode)
);

CREATE TABLE IF NOT EXISTS errorcode_atm(
    id smallint(5) NOT NULL AUTO_INCREMENT,
    atm_id smallint(5) NOT NULL,
    errorcode tinyint(2) NOT NULL,
    errorDateTime datetime DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (atm_id) REFERENCES atm(id),
    FOREIGN KEY (errorcode) REFERENCES errorCodes(errorcode)
);

CREATE TABLE IF NOT EXISTS transfers(
    id smallint(5) NOT NULL AUTO_INCREMENT,
    sender smallint(5) NOT NULL,
    reciever smallint(5) NOT NULL,
    amount DOUBLE(10, 2) NOT NULL,
    transferDate datetime DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    FOREIGN KEY (sender) REFERENCES humans(id),
    FOREIGN KEY (reciever) REFERENCES humans(id)
);