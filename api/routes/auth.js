exports.isEmployee = async function (token) {
    let [rows, fields] = await db.query(`SELECT token FROM humans WHERE token = ? AND rights > 0`, [token]);
    return rows[0] && rows[0].token ? true : false;
};

exports.isAccount = async function (token) {
    let [rows, fields] = await db.query(`SELECT token FROM accounts WHERE token = ?`, [token]);
    return rows[0] && rows[0].token ? true : false;
};

exports.isAtm = async function(atmId, token) {
    let [rows, fields] = await db.query(`SELECT token FROM atm WHERE id = ? AND token = ?`, [atmId, token]);
    return rows[0] && rows[0].token ? true : false;
}