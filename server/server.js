const express = require("express");
const jwt = require("jsonwebtoken");
const app = express();
const mysql = require('mysql2');

const port = process.env.PORT;

const jsonMiddleware = express.json();
app.use(jsonMiddleware);

const secret = process.env.JWT_SECRET;

const jwtMiddleware = require("express-jwt")({
	secret: secret
}).unless({ path: ["/users/login"] });
app.use(jwtMiddleware);


const connection = mysql.createConnection({
	host: process.env.HOST,
	user: process.env.USER,
	password: process.env.PASSWORD,
	database: process.env.DATABASE
});

/*
const pool = mysql.createPool({
	host: process.env.HOST,
	user: process.env.USER,
	password: process.env.PASSWORD,
	database: process.env.DATABASE,
	port: port,
	waitForConnections: true,
	connectionLimit: 10,
	queueLimit: 0
});
*/


app.get("/users", function(req, res){
	const users = [];

	connection.query('SELECT * FROM users LEFT JOIN level_saves USING (user_id) ORDER BY user_id', function(err, results) {
		if(err) return res.send(err);

		var length = results.length;
		var lastId = results[0].user_id;
		var index = 0;
		var result;

		for(var i = 0; i < length; i++){
			result = results[i];

			if(lastId != result.user_id) index++;
			lastId = result.user_id;

			if(users[index] == null){
				users[index] = {
					"username": result.username,
					"levelSaves": []
				}
			}

			users[index].levelSaves[result.level_index] = {
				"timeSpan": result.time_span,
				"score": result.score,
				"lifeCount": result.life_count
			};
		}

		res.send({ "list": users });
	});
});

app.post("/users/login", function (req, res, next) {
	const username = req.body.username;

	connection.query('SELECT user_id FROM users WHERE username = "' + username + '";', function(err, results) {
		if(err) return res.send(err);

		if(results[0] != null)
		{
			jwt.sign({ id: results[0].user_id }, secret, function (err, token) {
				if (err) return next(err);

				res.send(token);
			});
		}
		else
		{

			connection.query('INSERT INTO users (username) VALUES ( "' + username + '");', function(err, results) {
				if(err) return res.send(err);

				const _id = results.insertId;

				connection.query('INSERT INTO level_saves (user_id, time_span, score, life_count, level_index) VALUE (' + _id + ', 0 , 0, 0, 0);', function(err, results) {
					if(err) return res.send(err);

					connection.query('INSERT INTO level_saves (user_id, time_span, score, life_count, level_index) VALUE (' + _id + ', 0 , 0, 0, 1);', function(err, results) {
						if(err) return res.send(err);

						jwt.sign({ id: _id }, secret, function (err, token) {
							if (err) return next(err);
							res.send(token);
						});
					});
				});
			});
		}
	});
});

app.get("/users/me", function (req, res){
	const token = req.headers.authorization.slice(7);

	jwt.verify(token, secret, function (err, decoded) {
		if(err) return res.send(err);
		
		connection.query('SELECT * FROM users LEFT JOIN level_saves USING (user_id) WHERE user_id = ' + decoded.id + ';', function(err, results) {
			if(err) return res.send(err);

			const levelSaves = [];
			var result;

			for(var i = results.length - 1; i >= 0; i--){
				result = results[i];

				if(result.level_index == null)  break;
				
				levelSaves[i] = {
					"levelIndex": result.level_index,
					"timeSpan": result.time_span,
					"score": result.score,
					"lifeCount": result.life_count
				};
			}

			res.send({
				"username": results[0].username,
				"levelSaves": levelSaves
			});
		});
	});
});

app.post("/users/updateLevelSave/:level_index", function (req, res) {
	const levelSave = req.body;
	const levelIndex = req.params.level_index;
	const token = req.headers.authorization.slice(7);

	jwt.verify(token, secret, function (err, decoded) {
		if(err) return res.send(err);
		
		connection.query('SELECT * FROM level_saves WHERE user_id = ' + decoded.id + ' AND level_index = ' + levelIndex + ';', function(err, results){
			if(err) return res.send(err);

			var rocket;

			if(results[0] != null){
				rocket = 'UPDATE level_saves SET time_span = ' + levelSave.timeSpan + ', score = ' + levelSave.score + ', life_count = ' + levelSave.lifeCount + ' WHERE user_id = ' + decoded.id + ' AND level_index = ' + levelIndex + ';';
			}
			else{
				rocket = 'INSERT INTO level_saves (user_id, time_span, score, life_count, level_index) VALUE (' + decoded.id + ', ' + levelSave.timeSpan + ' ,' + levelSave.score + ', ' + levelSave.lifeCount + ', ' + levelIndex + ');';
			}
			
			connection.query(rocket, function(err, results) {
				if(err) return res.send(err);

				res.sendStatus(200);
			});
		});
	});
});

app.get("/users/:level_index", function(req, res){
	const users = [];
	const levelIndex = req.params.level_index;

	connection.query('SELECT * FROM users LEFT JOIN level_saves USING (user_id) WHERE level_index = ' + levelIndex + ' ORDER BY time_span', function(err, results) {
		if(err) return res.send(err);

		var length = results.length;
		var result;

		for(var i = 0; i < length; i++){
			result = results[i];

			users[i] = {
				"username": result.username,
				"levelSaves": [{
					"timeSpan": result.time_span,
					"score": result.score,
					"lifeCount": result.life_count
				}]
			}
		}

		res.send({ "list": users });
	});
});


app.listen(port, function (err) {
	if (err) console.error(err);
	else console.log("Listening to http://localhost:" + port);
});