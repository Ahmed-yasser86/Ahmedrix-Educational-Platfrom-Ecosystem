const ws = new WebSocket('ws://localhost:54321');
ws.onopen = () => {
  console.log('Connected to chat server');
  
  ws.send(JSON.stringify({
    type: 'join',
    roomId: 'general',
    userId: 'user_123',
    username: 'Alice'
  }));
};

ws.onmessage = (event) => {
  const data = JSON.parse(event.data);
  
  switch(data.type) {
    case 'message':
      console.log(`${data.username}: ${data.text}`);
      break;
    case 'user_joined':
      console.log(`${data.username} joined the room`);
      break;
    case 'user_left':
      console.log(`${data.username} left the room`);
      break;
  }
};

function sendMessage(text) {
  ws.send(JSON.stringify({
    type: 'message',
    text: text
  }));
}

function leaveRoom() {
  ws.send(JSON.stringify({
    type: 'leave'
  }));
}