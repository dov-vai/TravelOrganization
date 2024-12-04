export function onLoad() {
    const MAP_DOM = 'map';
    const TILE_URL = 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png';
    const ATTRIBUTION = '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors';

    const LAT = 55.231;
    const LON = 23.807;

    const ZOOM = 8;

    const map = L.map(MAP_DOM).setView([LAT, LON], ZOOM);

    L.tileLayer(TILE_URL, {
        attribution: ATTRIBUTION
    }).addTo(map);
    
    // temporary marker for navigation showcase
    const marker = L.marker([LAT, LON]).addTo(map);
    
    marker.on('click', () => {
        window.location.href = '/Stop/1';
    });
}
