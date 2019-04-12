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

  app.get('/', (req, res) => res.redirect('/dashboard'));
  app.get('/login/', (req, res) => res.render('login', { data: {} }));
  app.post('/login', (req, res) => {
    humans.employeeLogin(req, res)
      .then((result) => {
        if (result) {
          res.cookie('token', result[0].token);
          res.cookie('rights', result[0].rights);
          res.redirect('/dashboard');
        }
        else {
          res.render('login', { data: { err: 'Not authorized' } })
        }
      })
      .catch((err) => console.error(err));
  });
  app.get('/dashboard', (req, res) => {
    if (req.cookies.token && req.cookies.rights > 0) {
      res.render('dashboard');
    }
    else {
      res.redirect('/login', { data: { err: 'Not authorized' } });
    }
  });

  app.get('/atm', (req, res) => {
    if (req.cookies.token && req.cookies.rights > 0) {
      req.headers['authorization'] = 'Bearer ' + req.cookies.token;
      atm.getAtm(req, res, true).then((result) => {
        res.render('atm', { atm: result, token: req.cookies.token });
      });
    }
    else {
      res.redirect('/login', { data: { err: 'Not authorized' } });
    }
  });

  app.get('/customers', (req, res) => {
    if (req.cookies.token && req.cookies.rights > 0) {
      req.headers['authorization'] = 'Bearer ' + req.cookies.token;
      humans.getCustomers(req, res, true).then((result) => {
        res.render('customers', { customers: result });
      }).catch((err) => console.error(err));

    }
    else {
      res.redirect('/login', { data: { err: 'Not authorized' } });
    }
  });

  app.get('/customers/:id/edit', (req, res) => {
    if (req.cookies.token && req.cookies.rights > 0) {
      req.headers['authorization'] = 'Bearer ' + req.cookies.token;

      humans.getCustomerById(req, res, true).then((result) => {
        res.render('editcustomer', { customer: result[0], token: req.cookies.token });
      }).catch((err) => console.error(err));

    }
    else {
      res.redirect('/login', { data: { err: 'Not authorized' } });
    }
  });

  app.post('/customers/:id/edit', (req, res) => {
    if (req.cookies.token && req.cookies.rights > 0) {
      req.headers['authorization'] = 'Bearer ' + req.cookies.token;

      humans.editCustomer(req, res).then((result) => {
        if (result) {
          humans.getCustomerById(req, res, true).then((result) => {
            res.render('editcustomer', { customer: result[0], token: req.cookies.token });
          }).catch((err) => console.error(err));
        }
        else {
          res.status(500).send();
        }
      }).catch((err) => console.error(err));
    }
    else {
      res.redirect('/login', { data: { err: 'Not authorized' } });
    }
  });

  
  // -----------------API-ROUTES--------------------
  
  app.get('/v1/health', routes.health);
  
  app.post('/v1/accounts/login', accounts.login);
  app.post('/v1/accounts/token', accounts.getToken);
  app.get('/v1/accounts/:id/getBalance', accounts.getBalance);
  app.put('/v1/accounts/:id/setBalance', accounts.setBalance);
  app.post('/v1/accounts/transfer', accounts.transfer);
  app.delete('/v1/accounts/:id', accounts.deleteAccount);
  
  app.put('/v1/humans/:id/account', accounts.createAccount);

  app.post('/v1/humans/login', humans.login);
  app.post('/v1/humans/token', humans.getToken);
  app.get('/v1/humans', (req, res) => humans.getCustomers(req, res, false));
  // app.post('/v1/humans', humans.createHuman);
  app.get('/v1/humans/:id', humans.getCustomerById);
  // app.put('/v1/humans/:id', humans.updateCustomer);

  // app.post('/v1/atm/login', atm.login);
  // app.post('/v1/atm/token', atm.token);
  app.get('/v1/atm', (req, res) => atm.getAtm(req, res, false));
  // app.post('/v1/atm', atm.createAtm);
  app.get('/v1/atm/:id', atm.getAtmById);
  app.put('/v1/atm/:id', atm.setBills);

  app.listen(process.env.PORT || 3000, () => {
    console.log(`API running on port ${process.env.PORT || 3000}`);
  });
}

main();