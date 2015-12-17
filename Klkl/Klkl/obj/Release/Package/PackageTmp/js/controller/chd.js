'use strict';
angular.module('angle')
    .controller('ChdCtrl', [
        '$scope', '$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', "$filter", "ngTableParams", "$timeout", "ngTableDataService", "$http", "$state", "Notify","$stateParams",
        function ($scope, $resource, DTOptionsBuilder, DTColumnDefBuilder, $filter, ngTableParams, $timeout, ngTableDataService, $http, $state, Notify, $stateParams) {
            var vm = this;

            $http.get('/order/' + $stateParams.id).then(function(response) {
                //  var Api = $resource('/order/list?format=json');
                $scope.Order = response.data.Order;
                for (var i = 0; i < $scope.Order.OrderGoodses.length; i++) {
                    $scope.Order.OrderGoodses[i].Xh = i + 1;
                }
                var data = $scope.Order.OrderGoodses;
                
                $scope.alldate = data;
                vm.tableParams3 = new ngTableParams({
                    page: 1, // show first page
                    count: 10, // count per page
                    sorting: {
                        ID: 'asc' // initial sorting
                    }
                }, {
                    total: 0, // length of data
                    counts: [], // hide page counts control
                    getData: function($defer, params) {
                        ngTableDataService.getData2($defer, params, $scope.alldate);
                    }
                });
            });

            $scope.SumRemark = function(goods) {
                var s = "";
                angular.forEach(goods, function(value, key) {
                    s += value.Remark + "  ";
                });
                return s;
            };
            $scope.print=function() {
                window.print();
            }
        }
    ]);