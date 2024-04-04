// Load http module that is standard with all Node installations
const ROSLIB = require('roslib');
const express = require("express");
const bodyparser = require('body-parser');
const app = express();
app.use(bodyparser.urlencoded({extended:true}));
// name and port
const host = '0.0.0.0';
const port = '8000';
// COLORS
// I found it hard and cluttered to read logs, so i wanted to have colors for messages
let color_norm = `\x1b[0m`;
let color_yell = `\x1b[33m`; let color_grn = `\x1b[32m`;
// Parsed variables
// TODO: Ommiting the fingers here since we're working piece by piece
// TODO: Going to need to figure out how to map the JSON object to the 
//       specific node points
let node1; let node2; let node3; 
let node4; let node5; let node6;
let pos_x; let pos_y; let pos_z;
// Handle ros connection to rosbridge websocket
var ros = new ROSLIB.Ros({
    url : 'ws://localhost:9090'
});
ros.on('connection', function() {
    console.log('Connected to websocket server.');
})
ros.on('error', function(error) {
    console.log('Error connecting to websocket server: ', error);
})
ros.on('close', function() {
    console.log('Connection to websocket server closed')
})
// Create ROS topics
var sendData = new ROSLIB.Topic({
    ros: ros,
    name: '/unity_incoming',
    messageType: 'std_msgs/String'
})
// Handle requests
app.get('/', (req, res) => {
    // here root should load the html page
    res.send('root');
})
app.get('/get', (req, res) => {
    console.log("Get page requested")
    res.send('GET data');
})
app.post('/get', (req, res) => {
    if (req.body.Connection != undefined) {
        // This means that we have sent the initial data from the 
        // unity succesfully. I'm sure there's another way to do this 
        // but for testing purposes this should work fine 
        console.log(`${color_grn}${req.body.Connection}: Connection established with UNITY${color_norm}`);
        console.log('Now sending data to rosbridge...');
        var pubString = new ROSLIB.Message({
            data: `Connection to Unity : ${req.body.Connection}`,
        });
        sendData.publish(pubString);
	console.log("Connection data sent.");
    } else if (req.body.SUBMIT != undefined) {
	// Data recieved is a submit move
	console.log(`${color_grn}Submit movement received from UNITY${color_norm}`);
	
    // Send as string in different format
        var pubMove = new ROSLIB.Message({
            data: `SUBMIT:${req.body.joint1}:${req.body.joint2}:${req.body.joint3}:${req.body.joint4}:${req.body.joint5}:${req.body.joint6}`
        });
	sendData.publish(pubMove);
	console.log(`Data sent was: `+`${req.body.joint1}:${req.body.joint2}:${req.body.joint3}:${req.body.joint4}:${req.body.joint5}:${req.body.joint6}`);
    } else if (req.body.HOME != undefined) {
    	// Data recieved is a homing move
	    console.log(`${color_grn}Homing movement received from UNITY${color_norm}`);
        // Send as a string and add colons to make it easily parsed
        var pubMove = new ROSLIB.Message({ 
            data: `HOME:${req.body.joint1}:${req.body.joint2}:${req.body.joint3}:${req.body.joint4}:${req.body.joint5}:${req.body.joint6}`
        });
	sendData.publish(pubMove);
	console.log(`Data sent was: `+`${req.body.joint1}:${req.body.joint2}:${req.body.joint3}:${req.body.joint4}:${req.body.joint5}:${req.body.joint6}`);
    }	
    res.status(201).send("POST data");
})
app.put('/get', (req, res) => {
    res.send('PUT data');
})
app.delete('/get', (req, res) => {
    res.send("DELETE data");
})
// Setup the server on the port
app.listen(port, host, () => {
    console.log(`Server is running on port ${port}`);
})
