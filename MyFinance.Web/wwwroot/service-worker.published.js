// Este arquivo é gerado automaticamente pelo Blazor no Build de Produção
self.importScripts('./service-worker-assets.js');
self.addEventListener('install', event => event.waitUntil(onInstall(event)));
self.addEventListener('activate', event => event.waitUntil(onActivate(event)));
self.addEventListener('fetch', event => event.respondWith(onFetch(event)));

const cacheNamePrefix = 'offline-cache-';
const cacheName = `${cacheNamePrefix}${self.assetsManifest.version}`;

async function onInstall(event) {
    // Baixa todos os arquivos (DLLs, CSS, JS) para o cache
    const assetsRequests = self.assetsManifest.assets
        .map(asset => new Request(asset.url, { integrity: asset.hash, cache: 'no-cache' }));
    await caches.open(cacheName).then(cache => cache.addAll(assetsRequests));
}

async function onFetch(event) {
    if (event.request.method !== 'GET') return fetch(event.request);

    // Tenta encontrar no cache primeiro, se não tiver, vai na rede
    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(event.request);
    return cachedResponse || fetch(event.request);
}