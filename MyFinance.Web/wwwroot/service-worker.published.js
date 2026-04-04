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

    // O segredo do PWA SPA: Se o usuário estiver navegando para qualquer 
    // rota da aplicação (ex: /dashboard, /lancamentos), nós forçamos 
    // a entrega do index.html. O roteador do Blazor assume a partir daí.
    const shouldServeIndexHtml = event.request.mode === 'navigate';
    const requestToFetch = shouldServeIndexHtml ? 'index.html' : event.request;

    const cache = await caches.open(cacheName);
    const cachedResponse = await cache.match(requestToFetch);

    // Se estiver no cache (e o index.html ESTARÁ), ele retorna na hora.
    // Se não (uma imagem nova, por exemplo), tenta a rede.
    return cachedResponse || fetch(event.request);
}