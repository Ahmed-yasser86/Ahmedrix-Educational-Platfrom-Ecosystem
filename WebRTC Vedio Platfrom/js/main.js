'use strict'



const VideoFX = class { 
  constructor() {
    this.filters = ['grayscale', 'sepia', 'noir', 'psychedelic'];
  }
  
  cycleFilter() {
    const filter = this.filters.shift();
    this.filters.push(filter);
    return filter;
  }
};

const $self = {
  rtcConfig : null,
  mediaConistraints: { video: true, audio: true },
  isPolite: false,
  isMakingOffer: false,
  isIgnoringOffer: false,
  isSettingRemoteAnswerPending: false,
  mediaStream : new MediaStream(),
  mediaTracks : {},
  features : {
    audio : false
  },
}


const FeatureFunctions = {
  audio : function(){
    const status = document.querySelector('#videos #mic-status');
   
    const isPeerMuted = !$peer.features.audio ;
    status.setAttribute('aria-hidden', !isPeerMuted); 
  },

  video : function(){


    if($peer.mediaTracks.video){
      if($peer.features.video){
     $peer.mediaStream.addTrack($peer.mediaTracks.video);

      }else{
      $peer.mediaStream.removeTrack($peer.mediaTracks.video);
       DisplayStream(peer.mediaStream,'#peer');
      }
    }

  },

}


$self.filters = new VideoFX();

$self.messageQueue = [];




let $peer = {
  connection : new RTCPeerConnection($self.rtcConfig),
  mediaStream : new MediaStream(),
  mediaTracks : {},
  features : {
    audio : false
  },

}


// request user media 
RequestUserMedia($self.mediaConistraints);







function handelSelfVedio(event) {
if ($peer.connection.connectionState !== 'connected') {return; }

const filter = `filter-${$self.filters.cycleFilter()}`;

const fdc = $peer.connection.createDataChannel(filter);
event.target.className = filter;
fdc.onclose = function() {
  console.log(`Filter data channel closed ${filter}`);
}
}




const namespace = prepareNamespace(window.location.hash, true);


const socket = io('/' + namespace, { autoConnect: false });

RegisterSocketCallbacks();

socket.on('connect', () => {
  console.log('Connected to signaling server in namespace:', namespace);
});

socket.on('message', (msg) => {
  console.log('Message from server:', msg);
});

document.querySelector('#self').addEventListener('click',handelSelfVedio);


socket.on('signal', (data) => {
  console.log('Signal received:', data);
});

function prepareNamespace(hash, set_location) {
  let ns = hash.replace(/^#/, ''); 
  if (/^[0-9]{7}$/.test(ns)) {
    console.log('Checked existing namespace', ns);
    return ns;
  }
  ns = Math.random().toString().substring(2, 9);
  console.log('Created new namespace', ns);
  if (set_location) window.location.hash = ns;
  return ns;
}




document.querySelector('#header h1').innerText = "Room ID is"+ namespace;

document.querySelector('#call-button').addEventListener('click', handelcallbutton)

function handelcallbutton(event) {
  const callbutton = event.target;

  if (callbutton.className === 'join') {
    console.log('Join a call');
    callbutton.className = 'leave';
    callbutton.innerText = 'Leave Call';
    JoinCall();
  } else {
    console.log('Leave a call');
    callbutton.className = 'join';
    callbutton.innerText = 'Join Call';
    LeaveCall();
  }
}


function JoinCall() {

socket.open();
sendRoomToDotNet();
}

function LeaveCall() {

socket.close();
ResetPeer($peer);
}


function handelSocketConnect() {
    console.log('Socket connected');
  EstablishCallFeature() ;
  }

function handelSocketDisconnect() {
    console.log('Socket disconnected');
    ResetPeer($peer);
    EstablishCallFeature($peer);
  }

function handelSocketConnectedToPeer(){
    console.log('Socket connected to peer');
    $self.isPolite = true;
    }

async function handelSocketSingl({ description, candidate }) {
  if (description) {
    const ready_for_offer = !$self.isMakingOffer && ($peer.connection.signalingState === 'stable' || $self.isSettingRemoteAnswerPending);
    const isOfferCollision = description.type === 'offer' && !ready_for_offer;
    $self.isIgnoringOffer = !$self.isPolite && isOfferCollision;

    if ($self.isIgnoringOffer) {
      console.log('Ignoring offer');
      return;
    }

    $self.isSettingRemoteAnswerPending = description.type === 'answer';
    console.log('Setting remote description:', description);
    await $peer.connection.setRemoteDescription(description);
    $self.isSettingRemoteAnswerPending = false;

    if (description.type === 'offer') {
      const answer = await $peer.connection.createAnswer();
      await $peer.connection.setLocalDescription(answer);
      socket.emit('signal', { description: $peer.connection.localDescription });
    }
  } else if (candidate) {
    try {
      await $peer.connection.addIceCandidate(candidate);
      console.log('Added remote ICE candidate');
    } catch (e) {
      if (!$self.isIgnoringOffer) {
        console.error("Couldn't add remote ICE candidate:", e);
      }
    }
  }
}


function RegisterSocketCallbacks() {
    socket.on('connect', handelSocketConnect);
    socket.on('disconnected peer', handelSocketDisconnect);
    socket.on('connected peer', handelSocketConnectedToPeer);
    socket.on('signal', handelSocketSingl);
}

function RegisterRTCcallbacks(peer) {

  $peer.connection.onnegotiationneeded = handelRtcConNegotiation ;
  $peer.connection.onicecandidate = handelRTCIceCandidate ;
  $peer.connection.ontrack = handelRTCPeerTrack ;
  $peer.connection.onconnectionstatechange = handelRtcConnectionChange ;
  $peer.connection.ondatachannel = handelRtcDataChannel ;
}



function handelRtcDataChannel({channel}) {

  const label = channel.label;
  console.log('Data channel received with label:', label);

  if(label.startsWith('filter-')){
    document.querySelector('#peer').className = label;
      channel.onopen = function() {channel.close();}

  }

  if(label.startsWith('image-')){
    ReciveFiles(channel);
  }

}

async function handelRtcConNegotiation(){
  
$self.isMakingOffer = true;
console.log('Starting negotiation as making offer');
const offer = await $peer.connection.createOffer();
await $peer.connection.setLocalDescription(offer);
socket.emit('signal', { description: $peer.connection.localDescription });
$self.isMakingOffer = false;


}



function handelRtcConnectionChange(){

const connection_state = $peer.connection.connectionState;
console.log('Connection state changed to:', connection_state); 
document.querySelector('body').className =  connection_state;
}

function handelRTCIceCandidate({ candidate }) {
console.log('New ICE candidate:', candidate);

socket.emit('signal', { candidate });

}


function EstablishCallFeature(){
   RegisterRTCcallbacks($peer);
    addChatChannel($peer);
  AddStreamingMedia($peer);
  AddFeatureChannel($peer);
}


// User Media Functions

function AddStreamingMedia($peer) {
  let track_list = Object.keys($self.mediaTracks);
  for (let track of track_list) {
    const mediaTrack = $self.mediaTracks[track];
    if (mediaTrack) { 
      try {
        $peer.connection.addTrack(mediaTrack, $self.mediaStream);
      } catch (e) {
        console.warn('addTrack failed:', e);
      }
    }
  }
}


function HandleUserMediaTrack(){
  // to do
}

function handelRTCPeerTrack({track}){
console.log('New peer media track received');
$peer.mediaTracks[track.kind] = track;
$peer.mediaStream.addTrack(track);

DisplayStream($peer.mediaStream , '#peer');
}

async function RequestUserMedia(mediaConstraints) {
  $self.media = await navigator.mediaDevices.getUserMedia(mediaConstraints);

  $self.mediaTracks.audio = $self.media.getAudioTracks()[0];
  $self.mediaTracks.video = $self.media.getVideoTracks()[0];

  if ($self.mediaTracks.audio) {
    $self.mediaTracks.audio.enabled = !!$self.features.audio;
  }

  if ($self.mediaTracks.audio) $self.mediaStream.addTrack($self.mediaTracks.audio);
  if ($self.mediaTracks.video) $self.mediaStream.addTrack($self.mediaTracks.video);

  DisplayStream($self.mediaStream, '#self');
}



document.querySelector("#toggle-mic").setAttribute('aria-checked',$self.features.audio);


document.querySelector('footer').addEventListener('click', handleMediaButtons);

function DisplayStream(stream , selector)
{
document.querySelector(selector).srcObject = stream;

}


function handleMediaButtons(event) {
  const target = event.target;

  if (target.tagName !== 'BUTTON') return;

  switch (target.id) {
    case 'toggle-mic':
      toggleMic(target);
      break;
    case 'toggle-cam':
      toggleCam(target);
      break;
  }
}

function toggleMic(button) {
  const audio = $self.mediaTracks.audio;

  if (!audio) {
    console.warn('No audio track available to toggle.');
    return;
  }

  const enabled_state = audio.enabled = !audio.enabled;

  $self.features.audio = enabled_state;
  button.setAttribute('aria-checked', enabled_state);
  ShareFeature('audio');

}


function toggleCam(button) {
  const video = $self.mediaTracks.video;
  const enabled_state = video.enabled = !video.enabled;

  $self.features.video = enabled_state;
  button.setAttribute('aria-checked', enabled_state);

    ShareFeature('video');

  if (enabled_state) {
    $self.mediaStream.addTrack($self.mediaTracks.video);
  } else {
    $self.mediaStream.removeTrack($self.mediaTracks.video);
    DisplayStream($self.mediaStream,'#self');
  }
}



function ResetPeer(peer){
  DisplayStream(null , '#peer');
  try {
    if ($peer.connection && typeof $peer.connection.close === 'function')
       $peer.connection.close();
  } catch (e) {
    console.warn('Error closing existing peer connection:', e);
  }


   $peer.connection = new RTCPeerConnection($self.rtcConfig);
   $peer.mediaStream = new MediaStream();
   $peer.mediaTracks = {};
   $peer.features ={};

   ResetScreenToDefault();
    
    console.log('Connection closed, hiding mute status');
}



// Chat Functions 


function AppenedChatMessage(message, sender , log_element) {
  const log = document.querySelector(log_element);
  const li = document.createElement('li');
  li.className = sender;
  li.innerText = message.text;
  li.dataset.timestamp = message.timeStamp ;
  log.appendChild(li);
  log.scrollTop = log.scrollHeight;


}


function handleMessageForm(event) {

  event.preventDefault();
  const input = document.querySelector('#chat-msg');
  const message = {}
  message.text= input.value;      
  message.timeStamp = Date.now();  
    if (message.text.trim() === '') return;

  AppenedChatMessage(message, 'self', '#chat-log');
  AppenedOrQueueMessage(message, $peer);
  input.value = '';


}

document.querySelector('#chat-form').addEventListener('submit', handleMessageForm);

function addChatChannel($peer) {

  $peer.connection.chatChannel = $peer.connection.createDataChannel('chat', {negotiated: true, id: 100});
  $peer.connection.chatChannel.onmessage = (event) => {

    const message = JSON.parse(event.data);
    if(!message.id) {

      const respons = {
        id: message.timeStamp,
        timeStamp: Date.now()
      }
     AppenedOrQueueMessage(respons, $peer);
     AppenedChatMessage(message, 'peer', '#chat-log');
    }else{
      handleRespons(message);
    }
     
  }
  $peer.connection.chatChannel.onopen = function() {
    console.log('Chat data channel opened');
    while($self.messageQueue.length > 0&& $peer.connection.chatChannel.readyState ==='open'){
       let message = $self.messageQueue.shift();
          AppenedOrQueueMessage(message, $peer, false);
      }}

  $peer.connection.chatChannel.onclose = function() {
    console.log('Chat data channel closed');
  };


}



function QueueMessage(message, push=true) {
 if(push){
  $self.messageQueue.push(message);
}else{
  return $self.messageQueue.shift(message);
}
}

//send message or queue it
function AppenedOrQueueMessage(message, $peer,push=true) {

  const chatChannel = $peer.connection.chatChannel;

  if(!chatChannel || chatChannel.readyState !=='open'){
    QueueMessage(message, push);
    return;
  }

  if (message.image || message.file){
    sendFile($peer,message);
  }
  else{
    try{
    $peer.connection.chatChannel.send(JSON.stringify(message));
    }
    catch(e){
      console.error('Error sending message:', e);
      QueueMessage(message , true);
      return;
    }
  }
}

function handleRespons(Respons) {
  const sent_item = document.querySelector('#chat-log *[data-timestamp="' + Respons.id + '"]');
  
  if (!sent_item) {
    console.warn('Could not find message with timestamp:', Respons.id);
    return;
  }
  
  const classes = ['received'];

  if(Respons.timeStamp - Respons.id > 1000){
    classes.push('delayed');
  }

  sent_item.classList.add(...classes);
}




// handling feature code

function AddFeatureChannel($peer) {
  $peer.featuresChannel = $peer.connection.createDataChannel('features', { negotiated: true, id: 110 });

  $peer.featuresChannel.onmessage = function(event) {
    const features = JSON.parse(event.data);
    const feature_list = Object.keys(features);
    
    for (let f of feature_list) {
      $peer.features[f] = features[f];
      
      if (typeof FeatureFunctions[f] === 'function') {
        FeatureFunctions[f]();
      }
    }
  };

   $peer.featuresChannel.onopen = function() {
    console.log('A new feature channel has opened');
    $peer.features.BinaryDataType = $peer.featuresChannel.binaryType;

    $peer.featuresChannel.send(JSON.stringify($self.features));
  };
}



// call this when peer disconnected 
function ResetScreenToDefault(){

    const status = document.querySelector('#videos #mic-status');
    status.setAttribute('aria-hidden', true); 
}

function ShareFeature(...features){
  const featuresToshare = {};

  if($peer.featuresChannel && $peer.featuresChannel.readyState === 'open'){
    for (let f of features){
      featuresToshare[f] = $self.features[f];
    }
    
    try {
      $peer.featuresChannel.send(JSON.stringify(featuresToshare));
    } catch(e) {
      console.log('Error sending feature:', e);
    }
  }
}


//  handling image



document.querySelector('#chat-img-btn')
  .addEventListener('click', handleImageButton);



function handleImageButton() {
  let input = document.querySelector('input.temp');
  input = input ? input : document.createElement('input');
  input.className = 'temp';
  input.type = 'file';
  input.accept = '.gif, .jpg, .jpeg, .png';
  input.setAttribute('aria-hidden', true);
  document.querySelector('#chat-form').appendChild(input);
  input.addEventListener('change', handleImageInput);
  input.click();
}





 function handleImageInput(event) {
  event.preventDefault();
  const image = event.target.files[0];
  const metadata = {
    kind: 'image',
    name: image.name,
    size: image.size,
    timestamp: Date.now(),
    type: image.type,
    text: `📷 ${image.name}`
  };
  appendMessage('self', '#chat-log', metadata, image);

  const payload = {metadata: metadata , file: image}; 
   AppenedOrQueueMessage(payload,$peer);
  event.target.remove();
} 


function appendMessage(sender, log_element, message, image = null) {
  const log = document.querySelector(log_element);
  const li = document.createElement('li');
  li.className = sender;
  
  if (message.text) {
    li.innerText = message.text;
  } else if (message.name) {
    li.innerText = `📷 ${message.name}`;
  } else {
    li.innerText = '';
  }
  
  li.dataset.timestamp = message.timeStamp || message.timestamp || Date.now();
  
  if (image) {
    const img = document.createElement('img');
    img.src = URL.createObjectURL(image);
    img.onload = function() {
      URL.revokeObjectURL(this.src);
    };
    li.classList.add('img');
    li.appendChild(img);
  }
  
  log.appendChild(li);
  scrollToEnd(log);
}

function ReciveFiles(file_channel){
const chunks =[]; // <-- غير من Chunks إلى chunks
let metadata ;
let bytesRecived =0;

file_channel.onmessage= function({data}){
if(typeof data=== 'string' &&data.startsWith('{')){

  metadata = JSON.parse(data);
}else{


  bytesRecived += data.size ? data.size :data.byteLength;
  chunks.push(data); 
  
 if (bytesRecived === metadata.size) {
  const image = new Blob(chunks, { type: metadata.type }); 
  const response = {
    id: metadata.timestamp,
    timestamp: Date.now(),
  };
  
  metadata.text = `📷 ${metadata.name || 'Image'}`;
  appendMessage('peer', '#chat-log', metadata, image);
  
  try {
    file_channel.send(JSON.stringify(response));
  } catch(e) {
    QueueMessage(response);
  }
}
  
}


}

}

function scrollToEnd(el) {
  if (el.scrollTo) {
    el.scrollTo({
      top: el.scrollHeight,
      behavior: 'smooth',
    });
  } else {
    el.scrollTop = el.scrollHeight;
  }
}



function sendFile(peer, payload) {
  const { metadata, file } = payload; 
  const file_channel =
    peer.connection.createDataChannel(`${metadata.kind}-${metadata.name}`);
  const chunk = 16 * 1024; 
  file_channel.onopen = async function() {
    if (!peer.features ||
      ($self.features.binaryType !== peer.features.binaryType)) {
      file_channel.binaryType = 'arraybuffer';
    }
    
    const data = file_channel.binaryType ===
      'blob' ? file : await file.arrayBuffer(); 
    file_channel.send(JSON.stringify(metadata));

    for (let i = 0; i < metadata.size; i += chunk) {
      file_channel.send(data.slice(i, i + chunk));
    }
  };
  
  file_channel.onmessage = function(event){ 
    handleRespons(JSON.parse(event.data));
    file_channel.close();
  };
}



/////////////////////////////////
const urlParams = new URLSearchParams(window.location.search);

function sendRoomToDotNet() {
    const urlParams = new URLSearchParams(window.location.search);
    
    // -------------------

    const currentRoomUrl = window.location.href; 

    if (!window.location.hash || window.location.hash === '#') return;


    fetch('https://localhost:7032/api/VedioCallConnectionLayer/save-room', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            roomUrl: currentRoomUrl,
        })
    })
    .then(async response => {
        const data = await response.json();
        if (response.ok) {
            console.log("Success:", data.message);
        } else {
            console.error("Server Error:", data);
        }
    })
    .catch(err => console.error("Network Error:", err));
}

