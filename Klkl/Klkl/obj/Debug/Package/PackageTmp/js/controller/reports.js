'use strict';
var app = angular.module('angle');
app.controller('ProductReport', [
    '$scope', '$http', '$filter', function($scope, $http, $filter) {
        function loadData() {
            $http.post('/report/product', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
                .then(function(res) {
                    // filter
                    $scope.gridOptions1.rowData = res.data;
                    $scope.gridOptions1.api.onNewRows();

                    //$scope.Dkje = $scope.sum($scope.gridOptions1.rowData, "Dk");
                    //$scope.Fhzje = $scope.sum($scope.gridOptions1.rowData, "Zje");
                    //$scope.Fyje = $scope.sum($scope.gridOptions1.rowData, "Fy");
                    //$scope.Yfje = $scope.sum($scope.gridOptions1.rowData, "Yf");
                    //   $scope.Rcf = $scope.sum($scope.gridOptions1.rowData, "Rcf");
                });
        }

        var d = new Date();

        $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
        $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
        loadData();
        var columnDefsFilter = [
            { displayName: '产品系列', field: 'Category', width: 100, filter: 'set' },
            { displayName: '产品名称', field: 'Name', width: 100, filter: 'set' },
            { displayName: '规格', field: 'Size', width: 100, filter: 'set' },
            { displayName: '销量', field: 'SellNum', width: 100, filter: 'number' },
            { displayName: '销量金额', field: 'SellAmount', width: 100, filter: 'number' }
        ];
        $scope.open = function($event, o) {
            $event.preventDefault();
            $event.stopPropagation();
            if (o === 1) {
                $scope.opened1 = true;
            } else
                $scope.opened2 = true;

        };
        $scope.gridOptions1 = {
            columnDefs: columnDefsFilter,
            rowData: null,
            enableFilter: true,
            enableSorting: true,
            ready: function(api) {
                api.sizeColumnsToFit();
            },
            i18n: "zh-cn"

        };

        $scope.Search = function() {
            loadData();
        };

    }
]);
app.controller('CustomerReport', [
        '$scope', '$http', '$filter', function ($scope, $http, $filter) {
            function loadData() {
                $http.post('/report/customer', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
               .then(function (res) {
                   // filter
                   $scope.gridOptions1.rowData = res.data;
                   $scope.gridOptions1.api.onNewRows();

                   //$scope.Dkje = $scope.sum($scope.gridOptions1.rowData, "Dk");
                   //$scope.Fhzje = $scope.sum($scope.gridOptions1.rowData, "Zje");
                   //$scope.Fyje = $scope.sum($scope.gridOptions1.rowData, "Fy");
                   //$scope.Yfje = $scope.sum($scope.gridOptions1.rowData, "Yf");
                   //   $scope.Rcf = $scope.sum($scope.gridOptions1.rowData, "Rcf");
               });
            }
            var d = new Date();

            $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
            $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
            loadData();
            var columnDefsFilter = [
                { displayName: '渠道', field: 'Khqd', width: 100, filter: 'set' },
                { displayName: '客户名称', field: 'Khmc', width: 100, filter: 'set' },
                { displayName: '联系人', field: 'Lxr', width: 100, filter: 'set' },
                { displayName: '联系电话', field: 'Lxdh', width: 100, filter: 'text' },
                { displayName: '区域', field: 'Qy', width: 100, filter: 'set' },
                { displayName: '订单数量', field: 'OrderNum', width: 100, filter: 'number' },
                { displayName: '订单总金额', field: 'OrderAmount', width: 100, filter: 'number' }

            ];
            $scope.open = function ($event, o) {
                $event.preventDefault();
                $event.stopPropagation();
                if (o === 1) {
                    $scope.opened1 = true;
                }
                else
                    $scope.opened2 = true;

            };
            $scope.gridOptions1 = {
                columnDefs: columnDefsFilter,
                rowData: null,
                enableFilter: true,
                enableSorting: true,
                ready: function (api) {
                    api.sizeColumnsToFit();
                },
                i18n: "zh-cn"

            };

            $scope.Search = function () {
                loadData();
            };

        }
    ]);