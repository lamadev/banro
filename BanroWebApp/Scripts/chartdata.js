var app = angular.module('appchart',['ngRoute']);
app.controller('chart', function ($scope, $http) {
    var url = document.location.href.split('/')[4];
    console.log('URL:',url)
    if (url === 'ViewDepartments') {
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
    if (url === 'ViewFacilities') {
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
    if (url ==='ViewVouchers') {
        $http.get("/api/voucherbeneficiairies").then(function (response) {
            console.log('data :', response.data);
            $scope.vouchersPerBeneficiairies = response.data;
            $scope.chartUnified();

            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: response.data[0],
                    datasets: [{
                        label: ' Vouchers graphic',
                        data: response.data[1],
                        backgroundColor: [
                            'rgb(11, 137, 188)'
                            
                        ],
                        borderColor: [
                            'darkgreen'
                            
                        ],
                        borderWidth: 3
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

        $http.get("/api/voucherperdepartment").then(function (response) {

            console.log('data :', response.data);
            $scope.vouchersPerDepartment = response.data;
            $scope.chartUnified();

            var myChart = new Chart(ctx2, {
                type: 'line',
                data: {
                    labels: response.data[0],
                    datasets: [{
                        label: ' Vouchers per department',
                        data: response.data[1],
                        backgroundColor: [
                            'rgb(194, 251 , 153)'

                        ],
                        borderColor: [
                            'darkgreen'

                        ],
                        borderWidth: 3
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
    $scope.ctr = 0;
    $scope.chartUnified=function(){
        $scope.ctr += 1;
        if ($scope.ctr == 2) {
            var config = {
                type: 'pie',
                data: {
                    datasets: [
                     /* Outer doughnut data starts*/
                    {
                        data: [
                          10,
                          20,
                          30
                        ],
                        backgroundColor: [
                          "rgb(255, 0, 0)", // red
                          "rgb(0, 255, 0)", // green
                          "rgb(0, 0, 255)", //blue
                        ],
                        label: 'Doughnut 1'
                    },
                    /* Outer doughnut data ends*/
                    /* Inner doughnut data starts*/
                    {
                        data: [
                          45,
                          25,
                          11
                        ],
                        backgroundColor: [
                          "rgb(255, 0, 0)", // red
                          "rgb(0, 255, 0)", // green
                          "rgb(0, 0, 255)", //blue
                        ],
                        label: 'Doughnut 2'
                    }
                    /* Inner doughnut data ends*/
                    ],
                    labels: [
                      "Info 1",
                      "Info 2",
                      "Info 3"
                    ]
                },
                options: {
                    responsive: true,
                    legend: {
                        position: 'top',
                    },
                    title: {
                        display: true,
                        text: 'Chart.js Doughnut Chart'
                    },
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    },
                    tooltips: {
                        callbacks: {
                            label: function (item, data) {
                                console.log(data.labels, item);
                                return data.datasets[item.datasetIndex].label + ": " + data.labels[item.index] + ": " + data.datasets[item.datasetIndex].data[item.index];
                            }
                        }
                    }
                }
            };
            var myChart = new Chart(ctx3, config);
        }
    }
})

