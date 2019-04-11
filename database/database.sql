CREATE DATABASE IF NOT EXISTS funbank;

USE funbank;

CREATE TABLE IF NOT EXISTS humans (
    id smallint(5) NOT NULL AUTO_INCREMENT,
    pass varchar(64) NOT NULL,
    name varchar(64) NOT NULL,
    firstname varchar(64) NOT NULL,
    city varchar(64) NOT NULL,
    rights tinyint DEFAULT 0 NOT NULL,
    token varchar(255),
    PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS accounts (
    id smallint(5) NOT NULL AUTO_INCREMENT,
    humanId smallint(5) NOT NULL,
    pass varchar(64) NOT NULL,
    balance DOUBLE(10, 2),
    token varchar(255),
    PRIMARY KEY (id),
    FOREIGN KEY (humanId) REFERENCES humans(id)
);

CREATE TABLE IF NOT EXISTS atm (
    id smallint(5) NOT NULL AUTO_INCREMENT,
    fivehundred smallint(3),
    twohundred smallint(3),
    onehundred smallint(3),
    fifty smallint(3),
    twenty smallint(3),
    ten smallint(3),
    five smallint(3),
    zip varchar(5) NOT NULL,
    token varchar(255) NOT NULL,
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
    FOREIGN KEY (sender) REFERENCES accounts(id),
    FOREIGN KEY (reciever) REFERENCES accounts(id)
);