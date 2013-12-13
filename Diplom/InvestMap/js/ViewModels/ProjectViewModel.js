function ProjectListViewModel(container, mapViewModel) {
    var self = this
    container.GetAllGeoJson(SetDataAllGeoJson);
    // ProjectListViewModel properties

    self.AllGeoJsonProjects = ko.observableArray();
    self.ShowingProjects = ko.observableArray();
    self.Tags = ko.observableArray();
    self.Tags.push("BrownField");
    self.Tags.push("GreenField");
    self.Tags.push("UnUsedBuilding");


    // ProjectListViewModel initializer
    function SetDataAllGeoJson(argument) {
        if (argument.length > 0) {
            for (var i = 0; i < argument.length; i++) {
                self.AddTags(argument[i].Tags)
                self.AllGeoJsonProjects.push(new GeoJsonProjectViewModel(argument[i]));
                self.ShowingProjects.push(new GeoJsonProjectViewModel(argument[i]));
            }
            mapViewModel.SetProjects(self.ShowingProjects())
        };
    }

    self.UpdateFilter = function (projectsFilterViewModel) {
        self.ShowingProjects.removeAll()
        for (var j = 0; j < self.AllGeoJsonProjects().length; j++) {
            if (self.IsProjectInFilter(self.AllGeoJsonProjects()[j], projectsFilterViewModel.SelectedTypes())) {
                self.ShowingProjects.push(self.AllGeoJsonProjects()[j]);
            }
        };
        mapViewModel.SetProjects(self.ShowingProjects())
    }

    self.IsProjectInFilter = function (project, filters) {
        for (var j = 0; j < filters.length; j++) {
            if (project.Type().toLowerCase() == filters[j].Name().toLowerCase()) {
                return true;
            };

            for (var i = 0; i < project.Tags().length; i++) {
                if (project.Tags()[i].toLowerCase() == filters[j].Name().toLowerCase()) {
                    return true;
                }
            }
        }

        return false;
    }

    self.AddTags = function (tags) {
        if (tags != null && tags.length > 0) {
            for (var i = 0; i < tags.length; i++) {
                if (checkIfExistInArray(tags[i], self.Tags())) {
                    self.Tags.push(tags[i]);
                }
            }
        }
    }

    var checkIfExistInArray = function (tag, tags) {
        for (var j = 0; j < tags.length; j++) {
            if (tags[j] == tag) {
                return false;
            }
        }
        return true;
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
    self.Tags = ko.observableArray();

    if (argument.Tags != null && argument.Tags.length > 0) {
        for (var i = 0; i < argument.Tags.length; i++) {
            self.Tags.push(argument.Tags[i]);
        }
    }
}


function ProjectsFilterViewModel(projectListViewModel) {
    var self = this
    var prlVM = projectListViewModel

    self.SelectedTypes = ko.observableArray()
    self.AllTypes = ko.observableArray()

    self.FilterChanged = function (argument) {
        prlVM.UpdateFilter(self)
    }

    self.ItemClick = function (argument) {
        var match = ko.utils.arrayFirst(self.SelectedTypes(), function (item) {
            if (argument.Name() == item.Name()) {
                return true;
            } else {
                return false;
            }
        });

        if (!match) {
            self.SelectedTypes.push(argument);
        } else {
            self.SelectedTypes.remove(match)
        }
    }

    self.UpdateTags = function () {
        self.SelectedTypes.removeAll();
        self.AllTypes.removeAll();

        for (var i = 0; i < prlVM.Tags().length; i++) {
            self.SelectedTypes.push(new FilterViewModel(prlVM.Tags()[i], "button success", self))
            self.AllTypes.push(new FilterViewModel(prlVM.Tags()[i], "button success", self))
        }
    }

    prlVM.Tags.subscribe(self.UpdateTags);
    self.UpdateTags();
    self.SelectedTypes.subscribe(self.FilterChanged);
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