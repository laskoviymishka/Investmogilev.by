function ProjectRepository () {
	var getGeoJsonString = 'http://tserakhau.cloudapp.net//InvestProjects/ProjectGeoJSON'

	this.GetAllGeoJson = function  (callback) {
		$.ajax({
	        url: getGeoJsonString,
	        type: "GET",
	        dataType: "json",
	        success: function (data) {
	           	callback(data)
	        }
	    });
	}

	function AgregateData (data, keyWord) {
       	var result = []
       	var counter = 0
       	for (var i = 0; i < data.length; i++) {
       		if(data[i].Type == keyWord){
       			result[counter++] = data[i]
       		}
       	};
       	return result
	}
}
