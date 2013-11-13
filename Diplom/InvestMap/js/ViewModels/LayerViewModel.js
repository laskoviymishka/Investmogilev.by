var test
var emptyLayer = {
	    "Name": " ",
        "StyleId": 32,
        "TemplateId": 0, 
        "Css": "white", 
}
function LayerListViewModel (container, mapViewModel) {
	var _container = container
	var self = this;

	// LayerListViewModel properties

	self.Layers = ko.observableArray();
	self.SelectedLayerName = ko.observable(_container[0].Name)
	self.SelectedLayerCss = ko.observable(_container[0].Css)
	self.SelectedLayer = new LayerViewModel(_container[0]);

	//LayerListViewModel methods

	self.AddLayer = function (layer) {
		self.Layers.push(new LayerViewModel(layer))
	}

	// LayerListViewModel initializer
	for(var i = 0; i < _container.length; i++){
		self.Layers.push(new LayerViewModel(_container[i]))
	}

	self.SelectLayer = function (value) {
		self.SelectedLayerName(value.Name())
		self.SelectedLayerCss(value.Css())
		self.SelectedLayer = value
		mapViewModel.SetLayer(self.SelectedLayer)
	}

	self.LayerRemove = function () {
       	self.SelectLayer(new LayerViewModel(emptyLayer))
	}
}

function LayerViewModel (argument) {
	var self = this	
	// LayerViewModel properties

	self.Name = ko.observable(argument.Name)
	self.Childs = ko.observableArray()
	self.StyleId = ko.observable(argument.StyleId)
	self.TemplateId = ko.observable(argument.TemplateId)
	self.Css = ko.observable(argument.Css)

	// LayerViewModel initializer

	if(argument.Childs != null){
		for(var i = 0; i < argument.Childs.length; i++){
			self.Childs.push(new LayerViewModel(argument.Childs[i]))
		}	
	}

	// LayerViewModel methods

	self.SelectLayer = function () {
		layerListViewModel.SelectLayer(self)
	}
}

