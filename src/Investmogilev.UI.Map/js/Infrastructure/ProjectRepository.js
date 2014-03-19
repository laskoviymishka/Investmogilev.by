function ProjectRepository() {
    var getGeoJsonString = linkToSite + '/InvestProjects/ProjectGeoJSON?take=20&page=0';
    var ret = [];

    this.GetAllGeoJson = function(callback) {
        PopulatePage(callback, 0);
    };

    function PopulatePage(callback, page) {
        var link = linkToSite + '/InvestProjects/ProjectGeoJSON?take=20&page=' + page;
        $.ajax({
            url: link,
            type: "GET",
            dataType: "json",
            success: function(data) {
                for (var i = 0; i < data.Data.length; i++) {
                    ret.push(data.Data[i]);
                }
                if (data.Take * (data.Page + 1) < data.Total) {
                    PopulatePage(callback, ++page);
                } else {
                    callback(ret);
                }
            }
        });
    }

    function AgregateData(data, keyWord) {
        var result = [];
        var counter = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i].Type == keyWord) {
                result[counter++] = data[i];
            }
        };
        return result;
    }

}