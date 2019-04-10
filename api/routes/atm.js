const auth = require('./auth');

function getRandomInt() {
    min = Math.ceil();
    max = Math.floor(999999999999999);
    return (
        Math.floor(Math.random() * (999999999999999 - 100000000000000 + 1)) +
        100000000000000
    );
}

function getToken(bearerToken) {
    if (bearerToken.startsWith('Bearer ')) {
        // Remove Bearer from string
        bearerToken = bearerToken.slice(7, bearerToken.length);
    }

    return bearerToken;
}

exports.getAtm = async function (req, res) {
    const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
    let isAuthenticated;
    if (token.startsWith('fb')) {
        isAuthenticated = await auth.isAccount(token);
    }
    else {
        isAuthenticated = await auth.isAtm(atmId, token);
    }

    if (isAuthenticated) {
        let sql = `SELECT id, fivehundred, twohundred,
        onehundred, fifty, twenty, ten, five, zip FROM atm`;
        try {
            const [result] = await db.query(sql);
            res.send(result[0]);
            console.log(`getAtm}`);
        }
        catch (error) {
            res.status(500).send();
        }
    }
    else {
        res.status(403).send();
    }
};

exports.getAtmById = async function (req, res) {
    const atmId = parseInt(req.params.id);
    const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
    let isAuthenticated;
    if (token.startsWith('fb')) {
        isAuthenticated = await auth.isAccount(token);
    }
    else {
        isAuthenticated = await auth.isAtm(atmId, token);
    }

    if (isAuthenticated) {
        let sql = `SELECT id, fivehundred, twohundred,
        onehundred, fifty, twenty, ten, five, zip FROM atm WHERE id = ?`;
        try {
            const [result] = await db.query(sql, [atmId]);
            res.send(result[0]);
            console.log(`getAtmById: ${atmId}`)
        }
        catch (error) {
            res.status(500).send();
        }
    }
    else {
        res.status(403).send();
    }
};

exports.setBills = async function (req, res) {
    const atmId = parseInt(req.params.id);
    const token = getToken(req.headers['x-access-token'] || req.headers['authorization']);
    let isAuthenticated;
    if (token.startsWith('fb')) {
        isAuthenticated = await auth.isAccount(token);
    }
    else {
        isAuthenticated = await auth.isAtm(atmId, token);
    }

    const b = req.body;

    if (isAuthenticated) {
        let sql = `UPDATE atm SET fivehundred = ?, twohundred = ?, onehundred = ?, fifty = ?, twenty = ?, ten = ?, five = ? WHERE id = ?`;
        try {
            const [result] = await db.query(sql, [b.fivehundred, b.twohundred, b.onehundred, b.fifty, b.twenty, b.ten, b.five, atmId, token]);
            if (result.affectedRows > 0) {
                res.status(200).send();
                console.log(`set bills from atm: ${atmId} set to ${b}`);
            }
            else {
                res.status(500).send();
                console.log(`could set bills from atm ${atmId}`);
            }
        }
        catch (error) {
            res.status(500).send();
            console.error(error);
        }
    }
    else {
        res.status(403).send();
        console.warn(`Not authentificated`)
    }
};