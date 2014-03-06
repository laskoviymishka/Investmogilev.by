function ProjectListViewModel(container, mapViewModel) {
    var self = this
    container.GetAllGeoJson(SetDataAllGeoJson);
    // ProjectListViewModel properties

    self.AllGeoJsonProjects = ko.observableArray();
    self.ShowingProjects = ko.observableArray();
    self.Types = ko.observableArray();
    self.Tags = ko.observableArray();
    self.Types.push("GreenField");
    self.Types.push("UnUsedBuilding");
    self.Perechen = ko.observable("Проекты перечня");

    // ProjectListViewModel initializer
    function SetDataAllGeoJson(argument) {
        if (argument.length > 0) {
            for (var i = 0; i < argument.length; i++) {
                self.AddTags(argument[i].Tags);
                self.AllGeoJsonProjects.push(new GeoJsonProjectViewModel(argument[i]));
                self.ShowingProjects.push(new GeoJsonProjectViewModel(argument[i]));
            }
            mapViewModel.SetProjects(self.ShowingProjects());
        };
    }

    self.UpdateFilter = function (projectsFilterViewModel) {
        var isPerechen = projectsFilterViewModel.SelectedPerechen().length > 0;
        self.ShowingProjects.removeAll();
        for (var j = 0; j < self.AllGeoJsonProjects().length; j++) {
            if (self.IsProjectInFilter(self.AllGeoJsonProjects()[j], projectsFilterViewModel.SelectedTypes(), projectsFilterViewModel.SelectedTags(), !isPerechen)) {
                self.ShowingProjects.push(self.AllGeoJsonProjects()[j]);
            }
        };
        mapViewModel.SetProjects(self.ShowingProjects());
    };

    self.IsProjectInFilter = function (project, types, tags, isPerechen) {
        for (var j = 0; j < types.length; j++) {
            if (project.Type().toLowerCase() == types[j].Name().toLowerCase()) {
                for (var j = 0; j < tags.length; j++) {
                    for (var i = 0; i < project.Tags().length; i++) {
                        if (project.Tags()[i].toLowerCase() == tags[j].Name().toLowerCase()) {
                            if (isPerechen) {
                                return !project.Perechen();
                            } else {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    };

    self.AddTags = function (tags) {
        if (tags != null && tags.length > 0) {
            for (var i = 0; i < tags.length; i++) {
                if (checkIfExistInArray(tags[i], self.Tags())) {
                    self.Tags.push(tags[i]);
                }
            }
        }
    };

    var checkIfExistInArray = function (tag, tags) {
        for (var j = 0; j < tags.length; j++) {
            if (tags[j] == tag) {
                return false;
            }
        }
        return true;
    };
}

function GeoJsonProjectViewModel(argument) {
    var self = this;

    // GeoJsonProjectViewModel properties;
    self._id = ko.observable(argument._id);
    self.Name = ko.observable(argument.Name);
    self.Description = ko.observable(argument.Description);
    self.Type = ko.observable(argument.Type);
    self.Lng = ko.observable(argument.Lng);
    self.Lat = ko.observable(argument.Lat);
    self.Tags = ko.observableArray();
    self.Perechen = ko.observable(argument.Perechen);

    if (argument.Tags != null && argument.Tags.length > 0) {
        for (var i = 0; i < argument.Tags.length; i++) {
            self.Tags.push(argument.Tags[i]);
        }
    }
}


function ProjectsFilterViewModel(projectListViewModel) {
    var self = this;
    var prlVm = projectListViewModel;

    self.SelectedTypes = ko.observableArray();
    self.AllTypes = ko.observableArray();
    self.SelectedTags = ko.observableArray();
    self.AllTags = ko.observableArray();
    self.SelectedPerechen = ko.observableArray();
    self.AllPerechen = ko.observableArray();

    self.FilterChanged = function () {
        prlVm.UpdateFilter(self);
    };

    self.TypeClick = function (argument) {
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
            self.SelectedTypes.remove(match);
        }
    };

    self.TagClick = function (argument) {
        var match = ko.utils.arrayFirst(self.SelectedTags(), function (item) {
            if (argument.Name() == item.Name()) {
                return true;
            } else {
                return false;
            }
        });

        if (!match) {
            self.SelectedTags.push(argument);
        } else {
            self.SelectedTags.remove(match);
        }
    };

    self.PerechenClick = function (argument) {
        var match = ko.utils.arrayFirst(self.SelectedPerechen(), function (item) {
            if (argument.Name() == item.Name()) {
                return true;
            } else {
                return false;
            }
        });

        if (!match) {
            self.SelectedPerechen.push(argument);
        } else {
            self.SelectedPerechen.remove(match);
        }
        self.FilterChanged();
    };

    self.UpdatePerechen = function () {
        self.SelectedPerechen.push(new FilterTypeViewModel("perechen", "Проекты вне перечня", "button success", self, false, true));
        self.AllPerechen.push(new FilterTypeViewModel("perechen", "Проекты вне перечня", "button success", self, false, true));
    };
    self.UpdateTags = function () {
        self.SelectedTags.removeAll();
        self.AllTags.removeAll();
        for (var i = 0; i < prlVm.Tags().length; i++) {
            self.SelectedTags.push(new FilterTypeViewModel(prlVm.Tags()[i], prlVm.Tags()[i], "button success", self, true, false));
            self.AllTags.push(new FilterTypeViewModel(prlVm.Tags()[i], prlVm.Tags()[i], "button success", self, true, false));
        }
    };
    self.UpdateTypes = function () {
        self.SelectedTypes.removeAll();
        self.AllTypes.removeAll();

        for (var i = 0; i < prlVm.Types().length; i++) {
            self.SelectedTypes.push(new FilterTypeViewModel(prlVm.Types()[i], prlVm.Types()[i], "button success", self, false, false));
            self.AllTypes.push(new FilterTypeViewModel(prlVm.Types()[i], prlVm.Types()[i], "button success", self, false, false));
        }
    };

    prlVm.Tags.subscribe(self.UpdateTags);
    prlVm.Types.subscribe(self.UpdateTypes);
    self.UpdateTags();
    self.UpdateTypes();
    self.UpdatePerechen();
    self.SelectedTypes.subscribe(self.FilterChanged);
    self.SelectedTags.subscribe(self.FilterChanged);
}

function FilterTypeViewModel(name, displayName, imgName, projectsFiletrs, isTag, isPerechen) {
    var self = this;
    var parent = projectsFiletrs;
    self.Name = ko.observable(displayName);
    var checkeStyle = name + " " + imgName;
    var unCheckeStyle = name + " button";
    self.checked = ko.observable(checkeStyle);
    self.imgSource = "img/clear/" + self.Name() + ".png";
    self.isTag = isTag;
    self.isPerechen = isPerechen;
    self.TypeClick = function () {
        if (self.isTag) {
            parent.TagClick(self);
        } else {
            parent.TypeClick(self);
        }

        if (self.isPerechen) {
            parent.PerechenClick(self);
        }
        if (self.checked() == checkeStyle) {
            self.checked(unCheckeStyle);
        } else {
            self.checked(checkeStyle);
        }
    };
}
