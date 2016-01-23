'use strict';
var app = angular.module('angle', ['agGrid']);
app.controller('ProductReport', [
    '$scope', '$http', '$filter', function($scope, $http, $filter) {
        var columnDefsFilter = [
            { headerName: '产品系列', field: 'Category', width: 100, filter: 'set' },
            { headerName: '产品名称', field: 'Name', width: 300, filter: 'set' },
            { headerName: '规格', field: 'Size', width: 100, filter: 'set' },
            { headerName: '销量', field: 'SellNum', width: 100, filter: 'number' },
            { headerName: '销量金额', field: 'SellAmount', width: 100, filter: 'number' }
        ];        $scope.pageSize = '15';        $scope.gridOptions = {
            // note - we do not set 'virtualPaging' here, so the grid knows we are doing standard paging
            enableSorting: true,
            enableFilter: true,
            enableColResize: true,
            columnDefs: columnDefsFilter,
            rowHeight: 33,
            headerHeight: 33
        };        $scope.onPageSizeChanged = function () {
            createNewDatasource();
        };
        var allOfTheData;
        $scope.loadData = function () {
            $http.post('/report/product', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
                .then(function (res) {
                    allOfTheData = res.data;
                    createNewDatasource();
                });

        };
        var d = new Date();        $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
        $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
        $scope.loadData();
        function createNewDatasource() {
            if (!allOfTheData) {
                // in case user selected 'onPageSizeChanged()' before the json was loaded
                return;
            }

            var dataSource = {
                //rowCount: ???, - not setting the row count, infinite paging will be used
                pageSize: parseInt($scope.pageSize), // changing to number, as scope keeps it as a string
                getRows: function (params) {
                    // this code should contact the server for rows. however for the purposes of the demo,
                    // the data is generated locally, a timer is used to give the experience of
                    // an asynchronous call
                    setTimeout(function () {
                        // take a chunk of the array, matching the start and finish times
                        var rowsThisPage = allOfTheData.slice(params.startRow, params.endRow);
                        // see if we have come to the last page. if we have, set lastRow to
                        // the very last row of the last page. if you are getting data from
                        // a server, lastRow could be returned separately if the lastRow
                        // is not in the current page.
                        var lastRow = -1;
                        if (allOfTheData.length <= params.endRow) {
                            lastRow = allOfTheData.length;
                        }
                        params.successCallback(rowsThisPage, lastRow);
                    }, 50);
                }
            };
            $scope.gridOptions.api.setDatasource(dataSource);

        }
    }
]);
app.controller('CustomerReport', [
        '$scope', '$http', '$filter', function ($scope, $http, $filter) {
           
            var columnDefsFilter = [
                { displayName: '渠道', field: 'Khqd', width: 100, filter: 'set' },
                { displayName: '客户名称', field: 'Khmc', width: 300, filter: 'set' },
                { displayName: '联系人', field: 'Lxr', width: 200, filter: 'set' },
                { displayName: '联系电话', field: 'Lxdh', width: 150, filter: 'text' },
                { displayName: '区域', field: 'Qy', width: 100, filter: 'set' },
                { displayName: '订单数量', field: 'OrderNum', width: 100, filter: 'number' },
                { displayName: '订单总金额', field: 'OrderAmount', width: 100, filter: 'number' }

            ];
            $scope.pageSize = '15';            $scope.gridOptions = {
                // note - we do not set 'virtualPaging' here, so the grid knows we are doing standard paging
                enableSorting: true,
                enableFilter: true,
                enableColResize: true,
                columnDefs: columnDefsFilter,
                rowHeight: 33,
                headerHeight: 33
            };            $scope.onPageSizeChanged = function () {
                createNewDatasource();
            };
            var allOfTheData;
            $scope.loadData = function () {
                $http.post('/report/customer', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
                    .then(function (res) {
                        allOfTheData = res.data;
                        createNewDatasource();
                    });

            };
            var d = new Date();            $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
            $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
            $scope.loadData();
            function createNewDatasource() {
                if (!allOfTheData) {
                    // in case user selected 'onPageSizeChanged()' before the json was loaded
                    return;
                }

                var dataSource = {
                    //rowCount: ???, - not setting the row count, infinite paging will be used
                    pageSize: parseInt($scope.pageSize), // changing to number, as scope keeps it as a string
                    getRows: function (params) {
                        // this code should contact the server for rows. however for the purposes of the demo,
                        // the data is generated locally, a timer is used to give the experience of
                        // an asynchronous call
                        setTimeout(function () {
                            // take a chunk of the array, matching the start and finish times
                            var rowsThisPage = allOfTheData.slice(params.startRow, params.endRow);
                            // see if we have come to the last page. if we have, set lastRow to
                            // the very last row of the last page. if you are getting data from
                            // a server, lastRow could be returned separately if the lastRow
                            // is not in the current page.
                            var lastRow = -1;
                            if (allOfTheData.length <= params.endRow) {
                                lastRow = allOfTheData.length;
                            }
                            params.successCallback(rowsThisPage, lastRow);
                        }, 50);
                    }
                };
                $scope.gridOptions.api.setDatasource(dataSource);

            }

        }
    ]);