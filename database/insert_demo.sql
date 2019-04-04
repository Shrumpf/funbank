INSERT INTO humans (pass, name, firstname, rights)
VALUES ('12345', 'Doe', 'Customer', 0);

INSERT INTO accounts (humanId, balance)
SELECT LAST_INSERT_ID(), 1337
FROM humans
WHERE humans.id = LAST_INSERT_ID();

INSERT INTO humans (pass, name, firstname, rights)
VALUES ('12345', 'Doe', 'Employee', 1);

INSERT INTO atm (fivehundred, twohundred, onehundred, fifty, twenty, ten, five, zip)
VALUES (10, 20, 40, 80, 100, 120, 150, '07747');