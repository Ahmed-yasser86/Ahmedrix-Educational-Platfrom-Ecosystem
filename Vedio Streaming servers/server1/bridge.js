const http = require('http').createServer();
const io = require('socket.io')(http, {
    cors: { origin: "*", methods: ["GET", "POST"] }
});
const ffmpeg = require('fluent-ffmpeg');
const { PassThrough } = require('stream');


io.on('connection', (socket) => {
    console.log('✅ Teacher Connected to Bridge');

    let ffmpegProcess = null;
    let inputStream = null;

    socket.on('start-stream', (streamKey) => {
        console.log(`🎬 Starting FFmpeg for Stream Key: ${streamKey}`);
        
        inputStream = new PassThrough();

        ffmpegProcess = ffmpeg(inputStream)
            .inputFormat('webm')
            .inputOptions(['-re'])
            .outputOptions([
                '-c:v libx264',
                '-preset ultrafast',
                '-tune zerolatency',
                '-pix_fmt yuv420p',
                '-c:a aac',
                '-f flv'
            ])
            .output(`rtmp://nms_server1:1936/live/${streamKey}`) 
            .on('start', (command) => {
                console.log(`🚀 Streaming started to NMS Container on port 1936`);
            })
            .on('error', (err) => {
                console.error('❌ FFmpeg Error:', err.message);
            });

        ffmpegProcess.run();
    });

    socket.on('video-chunk', (data) => {
        if (inputStream && inputStream.writable) {
            inputStream.write(data);
        }
    });

    socket.on('disconnect', () => {
        console.log('🛑 Teacher disconnected');
        if (ffmpegProcess) ffmpegProcess.kill();
        if (inputStream) inputStream.end();
    });
});

http.listen(3001, () => {
    console.log('🚀 Bridge Server ready on port 3001');
});