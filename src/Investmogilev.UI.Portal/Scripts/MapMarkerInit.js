var map;

function CheckAddress(e) {
    GMaps.geocode({
        address: $('#AddressName').val().trim(),
        callback: function(results, status) {
            if (status == 'OK') {
                var latlng = results[0].geometry.location;
                map.removeMarkers();
                map.setCenter(latlng.lat(), latlng.lng());
                map.setZoom(13);
                map.addMarker({
                    lat: latlng.lat(),
                    lng: latlng.lng()
                });
                document.getElementById('Address_Lat').value = latlng.lat().toString();
                document.getElementById('Address_Lng').value = latlng.lng().toString();
            }
        }
    });
};

$(document).ready(function() {
    map = new GMaps({
        el: '#map',
        lat: 53.6,
        lng: 30.623333,
        zoom: 8
    });

    map.addMarker({
        lat: document.getElementById('Address_Lat').value,
        lng: document.getElementById('Address_Lng').value
    });

    GMaps.on('click', map.map, function(event) {
        var index = map.markers.length;
        var lat = event.latLng.lat();
        var lng = event.latLng.lng();
        document.getElementById('Address_Lat').value = lat.toString();
        document.getElementById('Address_Lng').value = lng.toString();
        map.removeMarkers();
        map.setCenter(lat, lng);
        map.addMarker({
            lat: lat,
            lng: lng
        });
    });
});