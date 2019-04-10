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
  const humanId = parseInt(req.body.id);
  const password = req.body.password;

  const token = "fb" + getRandomInt();

  let sql = `UPDATE humans SET token = ? WHERE id = ? AND pass = ?`;
  try {
    let [result] = await db.query(sql, [token, humanId, password]);
    if (result && result.affectedRows > 0) {
      res.status(200).send({ token });
    }
  }
  catch (error) {
    res.status(500).send();
  }
};

exports.getToken = async function (req, res) {
  const humanId = parseInt(req.body.id);
  const password = req.body.password;

  let sql = `SELECT token FROM humans WHERE id = ? AND pass = ?`;
  try {
    let [result] = await db.query(sql, [humanId, password]);
    if (result && result.length > 0) {
      res.status(200).send(result[0]);
    }
    else {
      res.status(403).send();
    }
  }
  catch (error) {
    res.status(500).send();
  }
};

exports.getCustomers = async function (req, res) {
  const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
  const isAuthenticated = await auth.isEmployee(token);

  if (isAuthenticated) {
    let sql = `SELECT h.id, h.name, h.firstname,
    (SELECT GROUP_CONCAT(id SEPARATOR ',')
        FROM accounts
        WHERE humanId = h.id) as accountIds
    FROM humans as h
    WHERE h.rights = 0`;

    let result = await db.query(sql);
    res.send(result[0].map((itm, idx) => {
      return {
        id: itm.id,
        name: itm.name,
        firstname: itm.firstname,
        accountIds: itm.accountIds.split(',')
      }
    }));
  }
  else {
    res.status(403).send();
  }
};

function getToken(bearerToken) {
  if (bearerToken.startsWith('Bearer ')) {
    // Remove Bearer from string
    bearerToken = bearerToken.slice(7, bearerToken.length);
  }

  return bearerToken;
}

exports.getCustomerById = async function (req, res) {
  const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
  const isAuthenticated = await auth.isEmployee(token);
  const accountId = req.params.id;


  if (isAuthenticated) {
    let sql = `SELECT h.id, h.name, h.firstname,
    (SELECT GROUP_CONCAT(id SEPARATOR ',')
        FROM accounts
        WHERE humanId = h.id) as accountIds
    FROM humans as h
    WHERE h.rights = 0
    AND h.id = ? `;

    let result = await db.query(sql, [accountId]);

    if (result.length > 0) {
      res.send(result[0].map((itm, idx) => {
        return {
          id: itm.id,
          name: itm.name,
          firstname: itm.firstname,
          accountIds: itm.accountIds.split(',')
        }
      }));
    } else {
      res.status(404).send();
    }
  }
  else {
    res.status(403).send();
  }
}