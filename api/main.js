require('dotenv-safe').config({allowEmptyValues: true});
const express = require('express');
const app = express();

const routes = require('./routes');
const humans = require('./routes/human');
const accounts = require('./routes/accounts');

var mysql = require("mysql");
var bodyParser = require("body-parser");
var connection = mysql.createConnection({
  host: process.env.MYSQL_HOST,
  user: process.env.MYSQL_USER,
  password: process.env.MYSQL_PASSWORD,
  database: process.env.MYSQL_DB,
  multipleStatements: true
});

connection.connect();

global.db = connection;

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.get('/v1/health', routes.health);

app.get('/v1/accounts/:id/getBalance', accounts.getBalance);
app.post('/v1/accounts/setBalance', accounts.setBalance);


app.listen(process.env.PORT || 3000, () => {
    console.log(`API running on port ${process.env.PORT || 3000}`);
});