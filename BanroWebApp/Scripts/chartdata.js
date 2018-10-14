var app = angular.module('appchart',['ngRoute']);
app.controller('chart', function ($scope, $http) {
    var url = document.location.href.split('/')[4];
    console.log('URL:',url)
    if (url == 'ViewDepartments') {
        $http.get("/api/employeecharts").then(function (response) {
            console.log('data :', response.data);


            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: response.data[0],
                    datasets: [{
                        label: ' Employees',
                        data: response.data[1],
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }, function (error) {
            console.log('Error: ', error)
        });
    }
    if (url == 'ViewFacilities') {
        $http.get("/api/voucherschart").then(function (response) {
            console.log('data :', response.data);


            var myChart = new Chart(ctx, {
                type: 'radar',
                data: {
                    labels: response.data[0],
                    datasets: [{
                        label: 'vouchers',
                        data: response.data[1],
                        backgroundColor: [
                            'rgba(255, 99, 132, 0.2)',
                            'rgba(54, 162, 235, 0.2)',
                            'rgba(255, 206, 86, 0.2)',
                            'rgba(75, 192, 192, 0.2)',
                            'rgba(153, 102, 255, 0.2)',
                            'rgba(255, 159, 64, 0.2)'
                        ],
                        borderColor: [
                            'rgba(255,99,132,1)',
                            'rgba(54, 162, 235, 1)',
                            'rgba(255, 206, 86, 1)',
                            'rgba(75, 192, 192, 1)',
                            'rgba(153, 102, 255, 1)',
                            'rgba(255, 159, 64, 1)'
                        ],
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    }
                }
            });
        }, function (error) {
            console.log('Error: ', error)
        });
    }
})

