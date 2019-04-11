require('dotenv-safe').config({ allowEmptyValues: true });
const express = require('express');
const app = express();

const path = require('path');

const cookieParser = require('cookie-parser')
const bluebird = require('bluebird');

const routes = require('./routes');
const humans = require('./routes/human');
const accounts = require('./routes/accounts');
const atm = require('./routes/atm');

async function main() {
  var mysql = require("mysql2/promise");
  var bodyParser = require("body-parser");
  var connection = await mysql.createConnection({
    host: process.env.MYSQL_HOST,
    user: process.env.MYSQL_USER,
    password: process.env.MYSQL_PASSWORD,
    database: process.env.MYSQL_DB,
    multipleStatements: true,
    Promise: bluebird
  });

  connection.connect();

  global.db = connection;

  app.set("views", __dirname + "/views");
  app.set("view engine", "ejs");
  app.use(cookieParser())
  app.use(bodyParser.urlencoded({ extended: false }));
  app.use(bodyParser.json());
  app.use(express.static(path.join(__dirname, "public")));

  // ------------------VIEWS------------------- 

  app.get('/', (req, res) => res.render('index'));
  app.get('/login/', (req, res) => res.render('login', {data: {}}));
  app.post('/login', (req, res) => {
    humans.employeeLogin(req, res)
      .then((result) => {
        if (result) {
          res.cookie('token', result[0].token);
          res.cookie('rights', result[0].rights);
          res.redirect('/dashboard');
        }
        else {
          res.render('login', {data: {err: 'Not authorized'}})
        }
      })
      .catch((err) => console.error(err));
  });
  app.get('/dashboard', (req, res) => {
    if (req.cookies.token) {
      res.render('dashboard');
    }
    else {
      res.redirect('/login');
    }
  })

  // -----------------API-ROUTES--------------------

  app.get('/v1/health', routes.health);

  app.post('/v1/accounts/login', accounts.login);
  app.post('/v1/accounts/token', accounts.getToken);
  // app.post('/v1/accounts', accounts.createAccount);
  app.get('/v1/accounts/:id/getBalance', accounts.getBalance);
  app.put('/v1/accounts/:id/setBalance', accounts.setBalance);
  app.post('/v1/accounts/transfer', accounts.transfer);

  app.post('/v1/humans/login', humans.login);
  app.post('/v1/humans/token', humans.getToken);
  app.get('/v1/humans', humans.getCustomers);
  // app.post('/v1/humans', humans.createHuman);
  app.get('/v1/humans/:id', humans.getCustomerById);
  // app.put('/v1/humans/:id', humans.updateCustomer);

  // app.post('/v1/atm/login', atm.login);
  // app.post('/v1/atm/token', atm.token);
  app.get('/v1/atm', atm.getAtm);
  // app.post('/v1/atm', atm.createAtm);
  app.get('/v1/atm/:id', atm.getAtmById);
  app.put('/v1/atm/:id', atm.setBills);

  app.listen(process.env.PORT || 3000, () => {
    console.log(`API running on port ${process.env.PORT || 3000}`);
  });
}

main();