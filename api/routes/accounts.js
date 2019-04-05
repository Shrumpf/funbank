function getRandomInt() {
  min = Math.ceil();
  max = Math.floor(999999999999999);
  return (
    Math.floor(Math.random() * (999999999999999 - 100000000000000 + 1)) +
    100000000000000
  );
}

exports.login = function(req, res) {
  const accountId = parseInt(req.body.id);
  const password = req.body.password;

  const token = "fb" + getRandomInt();

  let sql = `UPDATE accounts SET token = ? WHERE id = ? AND pass = ?`;

  db.query(sql, [token, accountId, password], (err, result) => {
    if (err) {
      console.error(err);
    } else {
      if (result && result.affectedRows > 0) {
        res.status(200).send({ token });
      }
      console.log(result);
    }
  });
};

exports.getToken = function(req, res) {
  const accountId = parseInt(req.body.id);
  const password = req.body.password;

  let sql = `SELECT token FROM accounts WHERE id = ? AND pass = ?`;

  db.query(sql, [accountId, password], (err, result) => {
    if (err) {
      console.error(err);
    } else {
      if (result && result.length > 0) {
        res.status(200).send(result[0]);
      }
    }
  });
};

exports.getBalance = async function(req, res) {
  const accountId = parseInt(req.params.id);
  const auth = req.body.auth;

  let sql = `SELECT token FROM accounts WHERE id = ?`;

  db.query(sql, [accountId], (err, result) => {
    if (err) {
      console.error(err);
    } else {
      let token = result[0] ? result[0].token : undefined;
      if ((!token && !auth) || auth !== token) {
        res.status(403).send();
        return;
      } else {
        let sql = `SELECT balance FROM accounts WHERE id = ${accountId}`;
        db.query(sql, (err, result) => {
          if (err) {
            console.error(err);
          } else {
            res.send(result);
          }
        });
      }
    }
  });
};

exports.setBalance = function(req, res) {
  const accountId = parseInt(req.body.accountId);
  const balance = parseFloat(req.body.balance);
  const auth = req.body.auth;

  if (!accountId || accountId === null) {
    res.status(500).send();
  }

  if (!balance || balance === null) {
    res.status(500).send();
  }
  let sql = `SELECT token FROM accounts WHERE id = ?`;

  db.query(sql, [accountId], (err, result) => {
    if (err) {
      console.error(err);
    } else {
      let token = result[0] ? result[0].token : undefined;
      if ((!token && !auth) || auth !== token) {
        res.status(403).send();
        return;
      } else {
        var sql = `UPDATE accounts SET balance = ? WHERE id = ?`;
        db.query(sql, [balance, accountId], (err, result) => {
          if (err) {
            console.error(err);
          } else {
            res.status(200).send();
          }
        });
      }
    }
  });
};
