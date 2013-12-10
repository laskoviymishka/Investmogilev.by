var test
var map
var projects
var selectedRegion = ''
    $.ajax({
        url: 'http://investmogilev.azurewebsites.net/InvestProjects/ProjectGeoJson?callback=jQuery110205763436721172184_1381677318690&_=1381677318691',
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


function getProjectGeoData() {
    $.ajax({
        url: 'http://tserakhau.cloudapp.net//InvestProjects/ProjectGeoJSON',
        type: "GET",
        dataType: "json",
        success: function (data) {
        }
    });
}
function ShowMap (argument) {
    $('#MainContainer').show();
    $('#RegionView').hide();
    $('#radioMap').attr("class","icon-radio-checked");
    $('#radioRegion').attr("class","icon-radio-unchecked");
}

function ShowRegion (argument) {
    $('#MainContainer').hide();
    $('#RegionView').show();
    $('#radioMap').attr("class","icon-radio-unchecked");
    $('#radioRegion').attr("class","icon-radio-checked");
}
$(window).resize(function(){
  alert('Размеры окна браузера изменены.');
});
$(window).load(function () {
    var height = window.innerHeight
    $('#googft-mapCanvas').height(height-50);
});
