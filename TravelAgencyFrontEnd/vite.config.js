import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { viteStaticCopy } from 'vite-plugin-static-copy';
import path from 'path';

// https://vitejs.dev/config/

//hmr disable websocket

export default defineConfig({
    plugins: [
        viteStaticCopy({
            targets: [
                {
                    src: './src/assets',
                    dest: './src',
                },
            ],
        }),
        vue()
    ],
    server: {
        port: 3000,
        https: false,
        //hmr: false,
        hmr: true,
        //hmr: {
        //    host: "https://localhost",
        //    port: 3000,
        //    protocol: "wss",
        //},
    },
    build: {
        chunkSizeWarningLimit: 1600
    }
})
