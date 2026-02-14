process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
import http from "http";
import ws from "websocket";
import redis from "redis";
import axios from "axios";

const APPID = process.env.APPID || "Server1";
const PORT = process.env.PORT || 8080;
const REDIS_HOST = process.env.REDIS_HOST || "chat_redis"; 
const DOTNET_API_URL = process.env.DOTNET_API_URL || "http://dotnet-api:5076/api/ChatSync/sync";

const redisConfig = {
    host: REDIS_HOST,
    port: 6379,
    retry_strategy: (options) => {
        if (options.error && options.error.code === 'ECONNREFUSED') {
            console.error(`[${APPID}] Redis refused connection. Retrying...`);
        }
        return Math.min(options.attempt * 100, 3000);
    }
};

const subscriber = redis.createClient(redisConfig);
const publisher = redis.createClient(redisConfig);
const bufferClient = redis.createClient(redisConfig);

subscriber.on("error", (err) => console.error(`[${APPID}] Redis Sub Error:`, err));
publisher.on("error", (err) => console.error(`[${APPID}] Redis Pub Error:`, err));

const WebSocketServer = ws.server;
const httpserver = http.createServer();
const websocket = new WebSocketServer({ httpServer: httpserver });

httpserver.listen(PORT, () => {
    console.log(`🚀 Server ${APPID} is flying on port ${PORT}`);
    console.log(`🔗 Connecting to Redis at ${REDIS_HOST}:6379`);
});

const rooms = new Map();

subscriber.on("message", (channel, message) => {
    try {
        const roomId = channel.split(":")[2];
        const roomConnections = rooms.get(roomId);
        if (!roomConnections) return;

        for (const con of roomConnections) {
            con.send(message);
        }
    } catch (ex) {
        console.error("ERR::" + ex);
    }
});

const syncToDotNet = async () => {
    bufferClient.lrange("chat_buffer", 0, -1, async (err, items) => {
        if (err) return console.error("Redis Lrange Error:", err);
        
        if (items && items.length > 0) {
            const messagesBatch = items.map(item => JSON.parse(item));
            try {
                const response = await axios.post(DOTNET_API_URL, messagesBatch);
                if (response.status === 200) {
                    console.log(`✅ [${APPID}] Synced ${items.length} messages to .NET`);
                    bufferClient.ltrim("chat_buffer", items.length, -1);
                }
            } catch (error) {
                console.error(`❌ [${APPID}] Sync failed:`, error.message);
            }
        }
    });
};

setInterval(syncToDotNet, 60000);

websocket.on("request", (request) => {
    const con = request.accept(null, request.origin);
    let userRoomId = null;

    con.on("message", (message) => {
        if (message.type !== 'utf8') return;
        
        const data = JSON.parse(message.utf8Data);

        if (data.type === "join") {
            userRoomId = data.roomId.toString();
            if (!rooms.has(userRoomId)) {
                rooms.set(userRoomId, new Set());
                subscriber.subscribe(`chat:room:${userRoomId}`);
            }
            rooms.get(userRoomId).add(con);
            console.log(`👤 User joined room: ${userRoomId} on ${APPID}`);
            return;
        }

        if (data.type === "chat") {
            if (!userRoomId) {
                con.send(JSON.stringify({ error: "Join a room first" }));
                return;
            }

            const msgForRedis = {
                senderId: data.senderId,
                senderName: data.senderName,
                roomId: userRoomId,
                content: data.content,
                timestamp: new Date().toISOString()
            };

            publisher.publish(`chat:room:${userRoomId}`, JSON.stringify(msgForRedis));
            bufferClient.rpush("chat_buffer", JSON.stringify(msgForRedis));
        }
    });

    con.on("close", () => {
        if (userRoomId && rooms.has(userRoomId)) {
            rooms.get(userRoomId).delete(con);
            console.log(`🚪 User left room: ${userRoomId}`);
            if (rooms.get(userRoomId).size === 0) {
                subscriber.unsubscribe(`chat:room:${userRoomId}`);
                rooms.delete(userRoomId);
            }
        }
    });
});