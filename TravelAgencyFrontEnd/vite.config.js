import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import { viteStaticCopy } from 'vite-plugin-static-copy';
import path from 'path';

// https://vitejs.dev/config/

//hmr disable websocket
function externalCSSPlugin() {
    return {
        name: 'external-css',
        transformIndexHtml: {
            enforce: 'post',
            transform(html, ctx) {
                return [
                    {
                        tag: 'link',
                        attrs: {
                            id: "color-scheme",
                            rel: 'stylesheet',
                            type: 'text/css',
                            href: '/src/assets/css/schemes/sky-net.min.css',
                        },
                        injectTo: ctx.server ? 'body-prepend' : 'head',
                    },
                ]
            },
        },
    }
}

export default defineConfig({
    plugins: [externalCSSPlugin(),
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
