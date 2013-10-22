var test
var map
var projects
var selectedRegion = ''
function initialize() {
    $.ajax({
        url: 'http://tserakhau.cloudapp.net//InvestProjects/ListOfProjects?callback=jQuery110205763436721172184_1381677318690&_=1381677318691',
        dataType: 'html',
        success: function (data) {
            //$('#moduleProjecs').html(data);
        }
    });
    google.maps.visualRefresh = true;
    map = new google.maps.Map(document.getElementById('googft-mapCanvas'), {
        center: new google.maps.LatLng(53.572885053173536, 30.412414184570274),
        zoom: 8,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    map.controls[google.maps.ControlPosition.RIGHT_BOTTOM].push(document.getElementById('googft-legend'));

    layer = new google.maps.FusionTablesLayer({
        map: map,
        heatmap: { enabled: false },
        query: {
            select: "col2",
            from: "1EOvuz_bSq6mL7_FdZuzfLER0ZRJ32Jke3RRh2wA",
            where: "col5 \x3e\x3d 1"
        },
        options: {
            styleId: 2,
            templateId: 2
        },
        suppressInfoWindows: true
    });
    google.maps.event.addListener(layer, 'click', function (e) {
        if (selectedRegion == e.row.Имя.value) {
            UnSelectRegion()
            map.setCenter(new google.maps.LatLng(53.572885053173536, 30.412414184570274))
            map.setZoom(8)
            selectedRegion = ''
        } else {
            selectedRegion = e.row.Имя.value
            RegionSelected(selectedRegion)
            map.setCenter(e.latLng)
            map.setZoom(9)
        }
    });
    getProjectGeoData()
}
google.maps.event.addDomListener(window, 'load', initialize);

function RegionSelected(regionName) {
    document.getElementById("mid").className = "col-md-7"
    document.getElementById("right").className = "col-md-3"
    document.getElementById("right").innerText = regionName
    linkToRegionInfo = 'http://localhost:20483/RegionInfo/GetRegionInfo/' + regionName
    $.get({
        type: "GET",
        url: linkToRegionInfo,
        dataType: 'raw',
        success: function (html) {
            alert(html)
            ocument.getElementById("right").innerText = html
        },
        error: function (e) {
            alert(e)
        }
    });

}
function UnSelectRegion() {
    document.getElementById("mid").className = "col-md-10"
    document.getElementById("right").className = "hidden"
}

function SetLayer(layerName) {
    if (layerName == 'integral') {
        layer.setOptions({
            styleId: 2,
            templateId: 2
        })
        HideCheckedLayers()
        document.getElementById("integralSelected").className = "badge"
        return
    }
    if (layerName == 'demographic') {
        layer.setOptions({
            styleId: 3,
            templateId: 4
        })
        HideCheckedLayers()
        document.getElementById("demographicSelected").className = "badge"
        return
    }
    SetLayer('integral')
}

function HideCheckedLayers() {
    document.getElementById("agroSelected").className = "badge hidden"
    document.getElementById("promSelected").className = "badge hidden"
    document.getElementById("demographicSelected").className = "badge hidden"
    document.getElementById("integralSelected").className = "badge hidden"
}
function openPopUp(e, type) {
    $('#popupContainer').html('');
    Avgrund.show("#default-popup");
    $('#popupContainer')
    linkToDetails = 'http://tserakhau.cloudapp.net//InvestProjects/PopUpDetails' + type.toString() + '/' + e.toString()
    $.ajax({
        url: linkToDetails,
        dataType: 'html',
        success: function (data) {
            $('#popupContainer').html(data);
        }
    });
}
function closeDialog() {
    Avgrund.hide();
}

function getProjectGeoData() {
    $.ajax({
        url: 'http://tserakhau.cloudapp.net//InvestProjects/ProjectGeoJSON',
        type: "GET",
        dataType: "json",
        success: function (data) {
            addMarkers(data);
        }
    });
}
function addMarkers(data){
    projects = data;
    for (var i = 0; i < projects.length; i++) {
        var marker = new google.maps.Marker({
            position: new google.maps.LatLng(projects[i].Lat, projects[i].Lng),
            map: map,
            title: projects[i].Name,
        });
        google.maps.event.addListener(marker, 'click', function (e) {
            for (var i = 0; i < projects.length; i++) {
                if (new google.maps.LatLng(projects[i].Lat, projects[i].Lng).toString() == e.latLng.toString()) {
                    openPopUp(projects[i]._id, projects[i].Type)
                }
            }
        });
    }
}