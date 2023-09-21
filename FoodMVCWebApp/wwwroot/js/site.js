function createMap(addresses) {
    var GoogleMapOptions = {
        center: new google.maps.LatLng(addresses[0].latitude, addresses[0].longitude),
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var infoWindow = new google.maps.InfoWindow();
    var map = new google.maps.Map(document.getElementById("googleMapContainer"), GoogleMapOptions);

    for (i = 0; i < addresses.length; i++) {
        var data = addresses[i]

        var myLatlng = new google.maps.LatLng(data.latitude, data.longitude);

        var address = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: data.title
        });

        (function (address, data) {
            google.maps.event.addListener(address, "click", function (e) {
                infoWindow.setContent(data.name);
                infoWindow.open(map, address);
            });
        })(address, data);
    }
}