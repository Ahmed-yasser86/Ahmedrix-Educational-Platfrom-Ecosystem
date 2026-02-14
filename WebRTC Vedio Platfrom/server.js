
const express = require('express');
const http = require('http');
const socketIo = require('socket.io');

const app = express();
const server = http.createServer(app);
const io = socketIo(server, {
  cors: {
    origin: '*',
    methods: ['GET', 'POST']
  }
});

// Serve static files from the root directory
app.use(express.static('.'));

// Socket.io dynamic namespace handling (rooms like /1234567)
io.of(/^\/\d{7}$/).on('connection', (socket) => {
  const namespace = socket.nsp;
  console.log(`User connected to namespace: ${
    namespace.name}`);

  // Broadcast within the same namespace only
  socket.broadcast.emit('message', 'A new user has joined the call.');
  // Notify existing peers a new client connected (used for politeness handling)
  socket.broadcast.emit('connected peer');

  socket.on('disconnect', () => {
    console.log(`User disconnected from namespace: ${namespace.name}`);
    socket.broadcast.emit('message', 'A user has left the call.');
    socket.broadcast.emit('disconnected peer');
  });

  socket.on('signal', (data) => {
    socket.broadcast.emit('signal', data);
  });
});

// Start server
server.listen(3000, () => {
  console.log('Server running at http://localhost:3000');
});
