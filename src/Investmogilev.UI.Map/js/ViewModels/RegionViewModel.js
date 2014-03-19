function RegionViewModel(repo) {
    var self = this;

    // RegionViewModel properties

    self.RegionName = ko.observable();
    self.EnglishName = ko.observable();
    self.AdditionalInfo = ko.observableArray();
    self.Parametrs = ko.observableArray();
    self.id = ko.observable();

    // RegionViewModel methods

    self.Set = function(argument) {
        self.RegionName(argument.RegionName);
        self.EnglishName(argument.EnglishName);
        self.id(argument._id);
        self.Parametrs.removeAll();
        if (argument.Parametrs != null && argument.Parametrs.length > 0) {
            for (var i = 0; i < argument.Parametrs.length; i++) {
                var Parametr = new ParametrViewModel();
                Parametr.Set(argument.Parametrs[i]);
                self.Parametrs.push(Parametr);
            };
        }
        if (argument.MoreInfo != null && argument.MoreInfo.length > 0) {
            for (var i = 0; i < argument.MoreInfo.length; i++) {
                var AddInfo = new AdditionalInfo();
                AddInfo.Set(argument.Parametrs[i]);
                self.Parametrs.push(AddInfo);
            };
        }
    };
}

var parametrId = 0;

function ParametrViewModel() {
    var self = this;

    // ParametrViewModel properties

    self.Id = parametrId++;
    self.Name = ko.observable();
    self.Values = ko.observableArray();
    self.ChildParametrs = ko.observableArray();
    self.IntegralValue = ko.observable();
    self.DependAbsoluteValues = ko.observableArray();
    self.DependValues = ko.observableArray();
    self.AbsoluteValues = ko.observableArray();


    self.ValuesData = ko.observableArray();
    self.DependAbsoluteValuesData = ko.observableArray();
    self.DependValuesData = ko.observableArray();
    self.AbsoluteValuesData = ko.observableArray();

    // ParametrViewModel methods

    self.Set = function(argument) {
        self.Name(argument.ParametrName);
        self.IntegralValue(argument.IntegralValue);
        self.Values.removeAll();
        self.ChildParametrs.removeAll();
        self.DependAbsoluteValues.removeAll();
        self.DependValues.removeAll();
        self.AbsoluteValues.removeAll();
        if (argument.AbsoluteValues != null && argument.AbsoluteValues.length > 0) {
            for (var i = 0; i < argument.AbsoluteValues.length; i++) {
                self.AbsoluteValues.push(new ValueViewModel(argument.AbsoluteValues[i]));
            };
        }

        if (argument.DependAbsoluteValues != null && argument.DependAbsoluteValues.length > 0) {
            for (var i = 0; i < argument.DependAbsoluteValues.length; i++) {
                self.DependAbsoluteValues.push(new ValueViewModel(argument.DependAbsoluteValues[i]));
            };
        }

        if (argument.DependValues != null && argument.DependValues.length > 0) {
            for (var i = 0; i < argument.DependValues.length; i++) {
                self.DependValues.push(new ValueViewModel(argument.DependValues[i]));
            };
        }

        if (argument.Values != null && argument.Values.length > 0) {
            for (var i = 0; i < argument.Values.length; i++) {
                self.Values.push(new ValueViewModel(argument.Values[i]));
            };
        }

        if (argument.ChildParametrs != null && argument.ChildParametrs.length > 0) {
            for (var i = 0; i < argument.ChildParametrs.length; i++) {
                var Child = new ParametrViewModel();
                Child.Set(argument.ChildParametrs[i]);
                self.ChildParametrs.push(Child);
            };
        }
    };

    self.ShowChilds = function(argument) {
        var id = '#' + self.Name().replace(/ /g, '');
        $(id).toggle("roll");
        for (var i = 0; i < self.ChildParametrs().length; i++) {
            var childId = '#' + self.ChildParametrs()[i].Id + '>div';
            var chart = $(childId).dxChart('instance');
            var renderOptions = {
                force: true
            };
            chart.render(renderOptions);
        };
    };
}

function ValueDataViewModel(argument) {
    var self = this;

    // ValueViewModel properties

    self.Value = argument;
}

function ValueViewModel(argument) {
    var self = this;

    // ValueViewModel properties

    self.Year = argument[0];
    self.Value = argument[1];
}

function AdditionalInfo() {
    var self = this;

    // AdditionalInfo properties


    // AdditionalInfo methods

    self.Set = function(argument) {
    };
}

function RegionViewModelRepository() {
    var GetRegionDataApiLink = linkToSite + '/api/regionapi/get/';
    this.FillRegionViewModel = function(id, callback) {
        var getLink = GetRegionDataApiLink;
        if (id != '') {
            for (var i = 0; i < dataForRegions.length; i++) {
                if (dataForRegions[i].RegionName == id) {
                    callback(dataForRegions[i]);
                };
            };
            getLink += id;
        } else {
            callback(dataForRegions[0]);
        }
    };
}

var regionRepository = new RegionViewModelRepository();