function createMap(addresses) {
    var GoogleMapOptions = {
        center: new google.maps.LatLng(addresses[0].latitude, addresses[0].longitude),
        zoom: 10,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    var infoWindow = new google.maps.InfoWindow();
    var map = new google.maps.Map(document.getElementById("googleMapContainer"), GoogleMapOptions);

    for (i = 0; i < addresses.length; i++) {
        var data = addresses[i]

        var myLatlng = new google.maps.LatLng(data.latitude, data.longitude);

        var address = new google.maps.Marker({
            position: myLatlng,
            map: map,
            title: data.title
        });

        (function (address, data) {
            google.maps.event.addListener(address, "click", function (e) {
                infoWindow.setContent(data.name);
                infoWindow.open(map, address);
            });
        })(address, data);

    }
}

function createChart() {
    $.ajax({
        url: '/api/Chart/JsonCategoriesList',
        type: 'GET',
        success: function (getdata) {
            console.log(getdata);
            var clabels = [];
            var levels = [];
            let counts = [];
            var datasets = [];
            $.each(getdata, function (index, item) {
                clabels.push(item.title);
                item.difficultyLevelDTOs.sort(function (a, b) { return a.id - b.id });
                $.each(item.difficultyLevelDTOs, function (difindex, diflevel) {
                    if (index == 0) {
                        levels.push(diflevel.name);

                    }
                    if (counts[difindex] === undefined) {
                        counts[difindex] = [];
                    }
                    counts[difindex][index] = diflevel.dishesCount;
                });

            });
            $.each(levels, function (index, item) {
                var dataset = {};
                dataset["label"] = item;
                dataset["data"] = counts[index];
                datasets.push(dataset);
            });

            const config = {
                type: 'bar',
                data: {
                    labels: clabels,
                    datasets: datasets
                },
                options: {
                    plugins: {
                        title: {
                            display: true,
                            text: 'Dishes by category'
                        },
                    },
                    responsive: true,
                    scales: {
                        x: {
                            stacked: true,
                        },
                        y: {
                            stacked: true,
                            beginAtZero: true
                        }
                    }
                }
            };

            const ctx = document.getElementById('myChart');
            new Chart(ctx, config);
            console.log(datasets);
        }
    });
}

function CategoryAutocomplete() {
    $("#CategoryName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dishes/GetCategoryTitles",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.title, val: item.id };
                    }))

                }
            })
        },
        select: function (e, i) {
            console.log(i.item.val);
            $("#CategoryId").val(i.item.val);
        },
        delay: 500,
        minLength: 3
    });
}

function DifficultyLevelAutocomplete() {
    $("#DifficultyLevelName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dishes/GetDifficultyLevelsNames",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.name, val: item.id };
                    }))

                }
            })
        },
        select: function (e, i) {
            console.log(i.item.val);
            $("#DifficultyLevelId").val(i.item.val);
        },
        delay: 500,
        minLength: 3
    });
}

function CuisineCountryTypeAutocomplete() {
    $("#CuisineCountryTypeName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Dishes/GetCuisineCountryTypesNames",
                type: "POST",
                dataType: "json",
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.name, val: item.id };
                    }))

                }
            })
        },
        select: function (e, i) {
            console.log(i.item.val);
            $("#CuisineCountryTypeId").val(i.item.val);
        },
        delay: 500,
        minLength: 3
    });
}