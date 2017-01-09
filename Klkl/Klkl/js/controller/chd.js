'use strict';
angular.module('angle')
    .controller('ChdCtrl', [
        '$scope', '$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', "$filter", "ngTableParams", "$timeout", "ngTableDataService", "$http", "$state", "Notify","$stateParams",
        function ($scope, $resource, DTOptionsBuilder, DTColumnDefBuilder, $filter, ngTableParams, $timeout, ngTableDataService, $http, $state, Notify, $stateParams) {
            var vm = this;
         
            $http.get('/order/' + $stateParams.id).then(function(response) {
                //  var Api = $resource('/order/list?format=json');
                $scope.Order = response.data.Order;
              //  $scope.Order.OrderGoodses = response.data.Goodses;

               // console.log($scope.Order);
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
                        ngTableDataService.getData3($defer, params, $scope.alldate);
                    }
                });
            });

            $scope.SumRemark = function(goods) {
                var s = "";
                angular.forEach(goods, function (value, key) {
                    if (value.Remark) {
                        s += value.Remark + "  ";
                    }
                  
                });
                return s;
            };
            $scope.print=function() {
                window.print();
            }

            $scope.DzAmount = function (orders) {
                if (!orders) {
                    return 0;
                }
                var amount = 0;
                for (var i = 0; i < orders.length; i++) {
                    amount += orders[i].Dz * orders[i].Price;
                }
                return amount;
            }


            $scope.Hpzhje = function (orders) {
                if (!orders) {
                    return 0;
                }
                var amount = 0;
                for (var i = 0; i < orders.length; i++) {
                    var good = orders[i];
                    if (good.Type===2) {
                        amount += good.Amount;
                    }
                }
                return amount;
            }

            $scope.Hpdzje = function (orders) {
                if (!orders) {
                    return 0;
                }
                var amount = 0;
                for (var i = 0; i < orders.length; i++) {
                    var good = orders[i];
                    if (good.Type === 2) {
                        amount += good.Dz * good.Price;
                    }
                }
                return amount;
            }

            $scope.floatplus = function (a, b) {
            
                return parseFloat(a) + parseFloat(b);
            }
        }
    ]);