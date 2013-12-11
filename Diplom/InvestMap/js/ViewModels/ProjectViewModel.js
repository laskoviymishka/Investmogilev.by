function ProjectListViewModel(container, mapViewModel) {
    var self = this
    container.GetAllGeoJson(SetDataAllGeoJson)

    // ProjectListViewModel properties

    self.AllGeoJsonProjects = ko.observableArray()
    self.ShowingProjects = ko.observableArray()

    // ProjectListViewModel initializer
    function SetDataAllGeoJson(argument) {
        if (argument.length > 0) {
            for (var i = 0; i < argument.length; i++) {
                self.AllGeoJsonProjects.push(new GeoJsonProjectViewModel(argument[i]))
                self.ShowingProjects.push(new GeoJsonProjectViewModel(argument[i]))
            }
            mapViewModel.SetProjects(self.ShowingProjects())
        };
    }

    self.UpdateFilter = function (projectsFilterViewModel) {
        self.ShowingProjects.removeAll()
        for (var i = 0; i < projectsFilterViewModel.SelectedTypes().length; i++) {
            for (var j = 0; j < self.AllGeoJsonProjects().length; j++) {
                if (self.AllGeoJsonProjects()[j].Type().toLowerCase() == projectsFilterViewModel.SelectedTypes()[i].Name().toLowerCase()) {
                    self.ShowingProjects.push(self.AllGeoJsonProjects()[j])
                };
            };
        };
        mapViewModel.SetProjects(self.ShowingProjects())
    }
}

function GeoJsonProjectViewModel(argument) {
    var self = this

    // GeoJsonProjectViewModel properties
    self._id = ko.observable(argument._id)
    self.Name = ko.observable(argument.Name)
    self.Description = ko.observable(argument.Description)
    self.Type = ko.observable(argument.Type)
    self.Lng = ko.observable(argument.Lng)
    self.Lat = ko.observable(argument.Lat)
}


function ProjectsFilterViewModel(projectListViewModel) {
    var self = this
    var prlVM = projectListViewModel
    self.SelectedTypes = ko.observableArray()
    self.AllTypes = ko.observableArray()

    self.SelectedTypes.push(new FilterViewModel("BrownField", "button success", self))
    self.SelectedTypes.push(new FilterViewModel("GreenField", "button success", self))
    self.SelectedTypes.push(new FilterViewModel("UnusedBuilding", "button success", self))

    self.AllTypes.push(new FilterViewModel("BrownField", "button success", self))
    self.AllTypes.push(new FilterViewModel("GreenField", "button success", self))
    self.AllTypes.push(new FilterViewModel("UnusedBuilding", "button success", self))

    self.FilterChanged = function (argument) {
        prlVM.UpdateFilter(self)
    }

    self.ItemClick = function (argument) {
        var match = ko.utils.arrayFirst(self.SelectedTypes(), function (item) {
            return argument.Name() == item.Name();
        });

        if (!match) {
            self.SelectedTypes.push(argument);
        } else {
            self.SelectedTypes.remove(match)
        }
    }

    self.SelectedTypes.subscribe(self.FilterChanged)
}

function FilterViewModel(name, imgName, projectsFiletrs) {
    var self = this
    var parent = projectsFiletrs
    self.Name = ko.observable(name)
    self.checked = ko.observable(imgName)

    self.FilterClick = function (argument) {
        parent.ItemClick(self)
        if (self.checked() == "button success") {
            self.checked("button")
        } else {
            self.checked("button success")
        }
    }
}