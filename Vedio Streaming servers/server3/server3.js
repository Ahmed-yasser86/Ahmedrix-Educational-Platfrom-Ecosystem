// server.js
const NodeMediaServer = require('node-media-server');
const config = require('./config.json');

const nms = new NodeMediaServer(config);
nms.run();

console.log('Node-Media-Server started with config:', config);
