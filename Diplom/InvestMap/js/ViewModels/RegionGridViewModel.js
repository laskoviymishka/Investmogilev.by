function RegionGridViewModel (argument) {
	var self = this

	self.Regions = ko.observableArray(argument.RegionName)

	for (var i = 0; i < argument.Regions.length; i++) {
		argument.Regions[i]
	};
	// body...
}

function RegionViewModel (argument) {

	var self = this

	self.Name = ko.observable(argument.RegionName)
	self.Parametrs = ko.observableArray()
	
	for (var i = 0; i < argument.ParametrViewModel.length; i++) {
		self.Parametrs.push(argument.ParametrViewModel[i])
	};
}

function ParametrsViewModel (argument) {
	var self = this

	self.ParametrName = argument.ParametrName
	self.Parentname = argument.ParentParametrName
	self.Values = ko.observableArray()

	for (var i = 0; i < argument.Values.length; i++) {
		self.Values.push(argument.Values[i])
	};
}

function ValuesViewModel (argument) {
	var self = this

	self.Year = argument.key
	self.Value = argument.value
	// body...
}