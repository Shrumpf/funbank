const auth = require('./auth');

function getRandomInt() {
  min = Math.ceil();
  max = Math.floor(999999999999999);
  return (
    Math.floor(Math.random() * (999999999999999 - 100000000000000 + 1)) +
    100000000000000
  );
}

exports.login = async function (req, res) {
  const accountId = parseInt(req.body.id);
  const password = req.body.password;

  const token = "fb" + getRandomInt();

  let sql = `UPDATE accounts SET token = ? WHERE id = ? AND pass = ?`;
  try {
    let [result] = await db.query(sql, [token, accountId, password]);
    if (result && result.affectedRows > 0) {
      res.status(200).send({ token });
      console.log(`${accountId} logged in`)
    }
  }
  catch (error) {
    res.status(500).send();
  }
};

exports.getToken = async function (req, res) {
  const accountId = parseInt(req.body.id);
  const password = req.body.password;

  let sql = `SELECT token FROM accounts WHERE id = ? AND pass = ?`;
  try {
    let [result] = await db.query(sql, [accountId, password]);
    if (result && result.length > 0) {
      res.status(200).send(result[0]);
      console.log(`${accountId} requested his token.`)
    }
    else {
      res.status(403).send();
    }
  }
  catch (error) {
    res.status(500).send();
  }
};

function getToken(bearerToken) {
  if (bearerToken.startsWith('Bearer ')) {
    // Remove Bearer from string
    bearerToken = bearerToken.slice(7, bearerToken.length);
  }

  return bearerToken;
}

exports.getBalance = async function (req, res) {
  const accountId = parseInt(req.params.id);
  const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
  const isAuthenticated = await auth.isAccount(token);

  if (isAuthenticated) {
    let sql = `SELECT balance FROM accounts WHERE id = ? AND token = ?`;
    try {
      const [result] = await db.query(sql, [accountId, token]);
      res.send(result[0]);
      console.log(`${accountId} requested his balance.`)
    }
    catch (error) {
      res.status(500).send();
    }
  }
  else {
    res.status(403).send();
  }
};

exports.setBalance = async function (req, res) {
  const accountId = parseInt(req.params.id);
  const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
  const isAuthenticated = await auth.isAccount(token);
  const balance = parseInt(req.body.balance);

  if (isAuthenticated) {
    let sql = `UPDATE accounts SET balance = ? WHERE id = ? AND token = ?`;
    try {
      const [result] = await db.query(sql, [balance, accountId, token]);
      res.status(200).send();
      console.log(`balance from Account: ${accountId} set to ${balance}`);
    }
    catch (error) {
      res.status(500).send();
    }
  }
  else {
    res.status(403).send();
  }
};

// /POST accounts/transfer
  // {sender, reciever, amout}
exports.transfer = async function (req, res) {
  const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
  const isAuthenticated = await auth.isAccount(token);
  const {reciever, sender, amount} = req.body;

  if (isAuthenticated) {
    let updateSender = `UPDATE accounts SET balance = balance - ? WHERE id = ? and token = ?;`;
    let updateReviever = `UPDATE accounts SET balance = balance + ? WHERE id = ?;`
    let transfer = `INSERT INTO transfers (sender, reciever, amount) VALUES (?, ?, ?);`

    try {
      await db.query(updateSender, [amount, sender, token]);
      await db.query(updateReviever, [amount, reciever]);
      await db.query(transfer, [sender, reciever, amount]);

      res.status(200).send();
      console.log(`Transfered ${amount}€ from Account ${sender} to Account ${reciever}`);
    }
    catch (error) {
      res.status(500).send();
    }
  }
  else {
    res.status(403).send();
  }
}