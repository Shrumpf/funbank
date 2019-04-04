exports.getBalance = function(req, res) {
  const accountId = req.params.id;

  var sql = `SELECT balance FROM accounts WHERE id = ${accountId}`;
  db.query(sql, (err, result) => {
    if (err) {
      console.log(err);
    } else {
      res.send(result);
    }
  });
};

exports.setBalance = function(req, res) {
  const accountId = parseInt(req.body.accountId);
  const balance = parseFloat(req.body.balance);

    if (!accountId || accountId === null) {
        res.status(500).send();
    }

    if (!balance || balance === null) {
        res.status(500).send();
    }

    var sql = `UPDATE accounts SET balance = ${balance} WHERE id = ${accountId}`;

    db.query(sql, (err, result) => {
      if (err) {
        console.error(err);
      } else {
        res.status(200).send();
      }
    });
};
