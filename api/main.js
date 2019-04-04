const express = require('express');
const app = express();

const routes = require('./routes');
const humans = require('./routes/human');
const accounts = require('./routes/accounts');

var mysql = require("mysql");
var bodyParser = require("body-parser");
var connection = mysql.createConnection({
  host: "localhost",
  user: "root",
  password: "",
  database: "funbank"
});

connection.connect();

global.db = connection;

app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());

app.get('/api/v1/health', routes.health);

app.get('/api/v1/accounts/:id/getBalance', accounts.getBalance);
app.post('/api/v1/accounts/setBalance', accounts.setBalance);

app.listen(3000, () => {
    console.log('API running on port 3000');
});