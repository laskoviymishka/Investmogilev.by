function ProjectListViewModel (container, mapViewModel) {
	var self = this
	container.GetAllGeoJson(SetDataAllGeoJson)

	// ProjectListViewModel properties

	self.AllGeoJsonProjects = ko.observableArray()

	// ProjectListViewModel initializer
	function SetDataAllGeoJson(argument) {
		if (argument.length > 0) {
			for(var i = 0; i< argument.length; i++){
				self.AllGeoJsonProjects.push(new GeoJsonProjectViewModel(argument[i]))
			}	
			mapViewModel.SetProjects(self.AllGeoJsonProjects())	
		};
	}
}
function GeoJsonProjectViewModel (argument) {
	var self = this

	// GeoJsonProjectViewModel properties
	self._id = ko.observable(argument._id)
	self.Name = ko.observable(argument.Name)
	self.Description = ko.observable(argument.Description)
	self.Type = ko.observable(argument.Type)
	self.Lng = ko.observable(argument.Lng)
	self.Lat = ko.observable(argument.Lat)
}

