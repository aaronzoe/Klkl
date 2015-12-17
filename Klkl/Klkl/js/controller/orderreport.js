'use strict';
angular.module('angle')
    .controller('OrderReportController', [
        '$scope', '$http', '$filter', function ($scope, $http, $filter) {
            function loadData() {
                $http.post('/order/getorderreport', { Dt1: $scope.dt1, Dt2: $scope.dt2 })
               .then(function (res) {
                   // filter
                   $scope.gridOptions1.rowData = res.data;
                   $scope.gridOptions1.api.onNewRows();

                   $scope.Dkje = $scope.sum($scope.gridOptions1.rowData, "Dk");
                   $scope.Fhzje = $scope.sum($scope.gridOptions1.rowData, "Zje");
                   $scope.Fyje = $scope.sum($scope.gridOptions1.rowData, "Fy");
                   $scope.Yfje = $scope.sum($scope.gridOptions1.rowData, "Yf");
                //   $scope.Rcf = $scope.sum($scope.gridOptions1.rowData, "Rcf");
                    });
            }
            var d = new Date();
     
            $scope.dt1 = $filter('date')(d, 'yyyy-01-01');
            $scope.dt2 = $filter('date')(d, 'yyyy-12-31');
            loadData();
            var columnDefsFilter = [
                //{
                //    displayName: 'Athlete',
                //    field: 'athlete',
                //    width: 150,
                //    filter: 'set',
                //    filterParams: { cellHeight: 20, values: irishAthletes }
                //},
                //{ displayName: 'Age', field: 'age', width: 90, filter: 'number' },
                //{ displayName: 'Country', field: 'country', width: 120 },
                //{ displayName: 'Year', field: 'year', width: 90 },
                //{ displayName: 'Date', field: 'date', width: 110 },
                //{ displayName: 'Sport', field: 'sport', width: 110 },
                //{ displayName: 'Gold', field: 'gold', width: 100, filter: 'number' },
                //{ displayName: 'Silver', field: 'silver', width: 100, filter: 'number' },
                //{ displayName: 'Bronze', field: 'bronze', width: 100, filter: 'number' },
                //{ displayName: 'Total', field: 'total', width: 100, filter: 'number' }
                { displayName: '订单号', field: 'OrderID', width: 100, filter: 'text', enableCellEdit: false, cellTemplate: '/order/'+ },
                { displayName: '客户渠道', field: 'Khqd', width: 100, filter: 'text' },
                { displayName: '客户名称', field: 'Khmc', width: 100, filter:'set'},
                { displayName: '联系人', field: 'Lxr', width: 100, filter: 'text' },
                { displayName: '联系电话', field: 'Lxdh', width: 100, filter: 'text' },
                { displayName: '区域', field: 'AreaName', width: 100, filter: 'set' },
                { displayName: '发货状态', field: 'Zt', width: 100 },
                { displayName: '打款金额', field: 'Dk', width: 100, filter: 'number' },
                { displayName: '发货总金额', field: 'Zje', width: 100, filter: 'number' },
                { displayName: '费用金额', field: 'Fy', width: 100, filter: 'number' },
                { displayName: '运费金额', field: 'Yf', width: 100, filter: 'number' },
                { displayName: '备注', field: 'Remark', filter: 'text' },
                { displayName: '录入时间', field: 'CreateAt', filter: 'text' }
            ];
            $scope.open = function ($event,o) {
                $event.preventDefault();
                $event.stopPropagation();
                if (o===1) {
                    $scope.opened1 = true;
                }
                else
                    $scope.opened2 = true;
          
            };
            $scope.gridOptions1 = {
                columnDefs: columnDefsFilter,
                rowData: null,
                enableFilter: true,
                enableSorting:true,
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