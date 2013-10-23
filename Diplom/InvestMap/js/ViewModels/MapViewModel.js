﻿function MapViewModel (map) {
	var self = this

	// MapViewModel properties
	self.Css = ko.observable("col-md-9")
	self.FusionTableFrom = ko.observable("1Kl6NI6pi4SuX8dF6NRUg3fLGoQJMrhmDQ2F-6aA")
	self.FusionTableStyleId = ko.observable(3)
	self.FusionTableTemlateId = ko.observable(5)
	self.ProjectList = ko.observableArray()
	self.Markers = ko.observableArray()
	self.Layer = ko.observable()


	self.gLayer = new google.maps.FusionTablesLayer({
        map: map,
        heatmap: { enabled: false },
        query: {
            select: "col2",
            from: self.FusionTableFrom(),
            where: "col5 \x3e\x3d 1"
        },
        options: {
            styleId: self.FusionTableStyleId(),
            templateId: self.FusionTableTemlateId()
        },
        suppressInfoWindows: true
    });

	// MapViewModel methods
	self.SetProjects = function (argument) {
		for (var i = 0; i < argument.length; i++) {
			var marker = new MarkerViewModel(argument[i])
			marker.InitMarker(map)
			google.maps.event.addListener(marker.gMarker, 'click', function (e) {
	            for (var i = 0; i < argument.length; i++) {
	                if (argument[i].Lat() == e.latLng.lb && argument[i].Lng() == e.latLng.mb) {
	                    self.OpenPopUp(argument[i]._id(), argument[i].Type())
	                }
	            }
	        });
			self.Markers.push(marker)
		};
	}

	self.SetLayer = function (argument) {
		self.Layer = argument
		self.FusionTableTemlateId(self.Layer.TemplateId())
		self.FusionTableStyleId(self.Layer.StyleId())
	}

	self.UpdateLayer = function () {
		self.gLayer.setOptions({
            styleId: self.FusionTableStyleId(),
            templateId: self.FusionTableTemlateId()
        })
	}

	self.OpenPopUp = function (e, type) {
	    $('#popupContainer').html('');
	    linkToDetails = 'http://tserakhau.cloudapp.net//InvestProjects/PopUpDetails' + type.toString() + '/' + e.toString()
		loadStart()
	    $.ajax({
	        url: linkToDetails,
	        dataType: 'html',
	        success: function (data) {
	        	loadEnd()
	    		Avgrund.show("#project-popup");
	            $('#popupContainer').html(data);
	        }
	    });
	}

	//MapViewModel subscribes
	self.FusionTableTemlateId.subscribe(self.UpdateLayer);
	self.FusionTableStyleId.subscribe(self.UpdateLayer);

	// bounds of the desired area

	self.Initialize = function () {
		var allowedBounds = new google.maps.LatLngBounds(
		  new google.maps.LatLng(52.613327, 27.438599 ),
		  new google.maps.LatLng(54.429782,32.788027)
		);
		var boundLimits = {
			maxLat : allowedBounds.getNorthEast().lat(),
			maxLng : allowedBounds.getNorthEast().lng(),
			minLat : allowedBounds.getSouthWest().lat(),
			minLng : allowedBounds.getSouthWest().lng()
		};

		var lastValidCenter = map.getCenter();
		var newLat, newLng;
		google.maps.event.addListener(map, 'center_changed', function() {
			center = map.getCenter();
			if (allowedBounds.contains(center)) {
				// still within valid bounds, so save the last valid position
				lastValidCenter = map.getCenter();
				return;
			}
			newLat = lastValidCenter.lat();
			newLng = lastValidCenter.lng();
			if(center.lng() > boundLimits.minLng && center.lng() < boundLimits.maxLng){
				newLng = center.lng();
			}
			if(center.lat() > boundLimits.minLat && center.lat() < boundLimits.maxLat){
				newLat = center.lat();
			}
			map.panTo(new google.maps.LatLng(newLat, newLng));
		});
		var lastValidZoom = 7;
		google.maps.event.addListener(map, 'zoom_changed', function() {
			if (map.getZoom() < 7){
				map.setZoom(lastValidZoom);
			} else{
				lastValidZoom = map.getZoom();
			}
	   	});
		google.maps.event.addListener(self.gLayer, 'click', function (e) {
	        alert(e.row.Name.value)
	    });
	}
}

function MarkerViewModel (argument) {
	var self = this

	// MarkerViewModel porperties

	self.Lat = argument.Lat()
	self.Lng = argument.Lng()
	self.Title = argument.Name()
	self.Type = argument.Type
	self.gMarker

	// MarkerViewModel initializer

	self.InitMarker = function (map) {
		var _icon = ""
		switch (self.Type()) {
		   case "GreenField":
		       _icon = "img/constructioncrane.png"
		      break
		   case "BrownField":
		      _icon = "img/factory.png"
		      break
		   case "UnusedBuilding":
		       _icon = "img/office-building.png"
		      break
		   default:
		       _icon = "img/factory.png"
		      break
		}
		self.gMarker = new google.maps.Marker({
            position: new google.maps.LatLng(self.Lat, self.Lng),
            map: map,
            title: self.Title,
            icon: _icon
        });
	}
}
var pr = new ProjectRepository()
var mapvm = new MapViewModel(map)
var plvm  = new ProjectListViewModel(pr,mapvm)
var layerListViewModel = new LayerListViewModel(dataForLayers, mapvm)
mapvm.Initialize()
ko.applyBindings(layerListViewModel, document.getElementById("LayerView"));
ko.applyBindings(mapvm, document.getElementById("MapView"));