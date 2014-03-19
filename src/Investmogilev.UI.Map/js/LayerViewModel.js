var ttest;

function LayerListViewModel(container, mapViewModel) {
    var _container = container;
    var self = this;

    // LayerListViewModel properties

    self.Layers = ko.observableArray();
    self.SelectedLayer = ko.observable(new LayerViewModel(_container[0]));

    //LayerListViewModel methods

    self.AddLayer = function(layer) {
        self.Layers.push(new LayerViewModel(layer));
    }; // LayerListViewModel initializer
    for (var i = 0; i < _container.length; i++) {
        self.Layers.push(new LayerViewModel(_container[i]));
    }

    self.SelectedLayer.subscribe(self.SelectLayer);
    self.SelectLayer = function(value) {
        self.SelectedLayer().Name(value.Name());
        mapViewModel.SetLayer(value);
    };
}

function LayerViewModel(argument) {
    var self = this; // LayerViewModel properties

    self.Name = ko.observable(argument.Name);
    self.Childs = ko.observableArray();
    self.StyleId = ko.observable(argument.StyleId);
    self.TemplateId = ko.observable(argument.TemplateId);
    self.Css = ko.observable(argument.Css); // LayerViewModel initializer

    if (argument.Childs != null) {
        for (var i = 0; i < argument.Childs.length; i++) {
            self.Childs.push(new LayerViewModel(argument.Childs[i]));
        }
    }

    // LayerViewModel methods

    self.SelectLayer = function() {
        layerListViewModel.SelectLayer(self);
    };
}