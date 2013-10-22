function LayerListViewModel(argument) {
    var self = this;

    // LayerListViewModel Properties

    self.Layers = ko.observableArray(argument.Layers)
    self.SelectedLayer = ko.observable(self.Layers[0])

    // LayerListViewModel Methods

    self.LayerSelectedChange = function (eventArgs) {
        self.SelectedLayer.LayerSelected()
    }


    // LayerListViewModel EventHandlers

    self.SelectedRegion.subscribe(self.LayerSelectedChange);
}

function LayerViewModel(argument) {
    var self = this;

    // LayerViewModel properties

    self.Name = ko.observable(argument.Name)
    self.FusionTableId = ko.observable(argument.FusionTableId)
    self.ChildLayer = o.observableArray(argument.ChildLayer)

    // LayerViewModel methods

    self.LayerSelected = function () {
        // body...
    }
}