require("dotenv-safe").config({ allowEmptyValues: true });
const fs = require("fs");
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

const db = connection;

const clean = async () => {
  let sql = `DROP DATABASE ${process.env.MYSQL_DB}`;

  db.query(sql, (err, result) => {
    if (!err) {
      let sql = fs.readFile("../database/database.sql", "utf8", (err, data) => {
        db.query(data, (err, result) => {
          if (!err) {
            console.log("clean success");
            db.end();
            process.exit(0);
          } else {
            console.error(err);
          }
        });
      });
    } else {
      console.error(err);
    }
  });
};

try {
  clean();
} catch (err) {
  console.error(err);
}
