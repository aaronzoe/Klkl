'use strict';
angular.module('angle')
    .controller('OrderController', [
        '$scope', '$stateParams', '$http', '$filter', '$state', 'editableOptions', 'editableThemes', 'Notify', '$timeout', function($scope, $stateParams, $http, $filter, $state, editableOptions, editableThemes, Notify, $timeout) {

            editableOptions.theme = 'bs3';
            $http.get("/order/" + $stateParams.id).then(function(response) {
                $scope.Order = response.data.Order;
                $scope.Costs = response.data.Costs;
                $scope.Goodses = response.data.Goodses;
                $scope.Customers = response.data.Customers;
                $scope.Khdbs = response.data.Khdbs;
                $scope.Categories = response.data.Categories;
                if ($stateParams.id) {
                    $scope.SelectKhmc = $filter('filter')($scope.Customers, { "Khmc": $scope.Order.Khmc })[0];
                } else {
                    $scope.SelectKhmc = $scope.Customers[0];
                }
              
                if ($scope.Order.Khdb) {
                    $scope.SelectKhdb = $filter('filter')($scope.Khdbs, { "DisplayName": $scope.Order.Khdb })[0];
                
                } else {
                    
                }
                $scope.$watch('Order', function() {
                    $scope.isChanged = false;
                }, true);
                //   $scope.SelectKhmc = $scope.Order.Khmc;
            });


            $scope.save = function() {

                $scope.Order.Khmc = $scope.SelectKhmc.Khmc;
                if ($scope.SelectKhdb) {
                    $scope.Order.Khdb = $scope.SelectKhdb.DisplayName;
                    $scope.Order.UserID = $scope.SelectKhdb.Id;
                }
                $http.post("/order/update", { "Order": $scope.Order }).then(function(response) {
                    $scope.Order.ID = response.data.ID;
                    $scope.Order.OrderID = response.data.OrderID;
                    $scope.isChanged = true;
                    $timeout(function() {
                        Notify.alert(
                            '保存订单成功..',
                            { status: 'success' }
                        );
                    }, 100);
                    //  $scope.orderTemp = angular.copy($scope.Order);
                });
            };
            $scope.cancel = function() {
                $state.go("app.orders");
            };

            $scope.showCategory = function(ordergoods) {
                if (ordergoods.GoodsID && $scope.Categories.length) {

                    var selected = $filter('filter')($scope.Categories, function(m) {
                        return $filter('filter')(m.Materials, { "ID": material.MaterialID }).length > 0;
                    });
                    return selected.length && selected.length > 0 ? selected[0].Name : '未设置';
                } else {
                    return "未设置";
                }
            };
            $scope.showMaterial = function(material) {
                if (material.MaterialID && $scope.Materials.length) {
                    var selected = $filter('filter')($scope.Materials, { "ID": material.MaterialID });
                    return selected.length ? selected[0].Name : '未设置';
                } else {
                    return "未设置";
                }
            };
            $scope.addOrderGoods = function() {
                $scope.inserted = {
                    OrderID: $scope.Order.ID
                };
                $scope.Order.OrderGoodses.push($scope.inserted);
            };
            $scope.saveOrderGoods = function(orderGoods, data) {
                orderGoods.GoodsID = data.Goods.ID;
                orderGoods.Amount = data.Num * data.Price;
                orderGoods.Size = data.Goods.Size;
                $http.post("/order/updateordergoods", { "OrderGoods": orderGoods }).then(function(response) {
                    orderGoods.ID = response.data;
                    orderGoods.Name = data.Goods.Name;
                    orderGoods.Category = data.Goods.Category;
                    $timeout(function() {

                        Notify.alert(
                            '保存订单商品成功..',
                            { status: 'success' }
                        );

                    }, 100);
                });
            };
            $scope.removeOrderGoods = function(index, id) {
                $http.post("/order/delordergoods", { "ID": id }).then(function(response) {
                    $scope.Order.OrderGoodses.splice(index, 1);
                });
            };

            $scope.$on('ngGridEventStartCellEdit', function() {
            //    console.log(1);
                //elm.focus();
                //elm.select();
            });

            $scope.addOrderCost = function() {
                $scope.inserted = {
                    OrderID: $scope.Order.ID,
                    CostID: $scope.Costs[0].ID
                };
                if (!$scope.Order.OrderCosts) {
                    $scope.Order.OrderCosts = [];
                }
                $scope.Order.OrderCosts.push($scope.inserted);
            };
            $scope.saveOrderCost = function(orderCost, data) {
                orderCost.CostID = data.Cost.ID;
                orderCost.NAME = data.Cost.Name;
                $http.post("/order/updateordercost", { "OrderCost": orderCost }).then(function(response) {
                    orderCost.ID = response.data;
                    $timeout(function() {

                        Notify.alert(
                            '保存订单费用成功..',
                            { status: 'success' }
                        );

                    }, 100);
                });
            };
            $scope.removeOrderCost = function(index, id) {
                $http.post("/order/delordercost", { "ID": id }).then(function(response) {
                    $scope.Order.OrderCosts.splice(index, 1);
                });
            };

            $scope.showCostName = function(cost) {
                var selected = $filter('filter')($scope.Costs, { ID: cost.CostID });
                return ($scope.Costs && selected.length) ? selected[0].Name : '未设置';
            };
            $scope.SelectK = function() {
                $scope.Order.Lxr = $scope.SelectKhmc.Lxr;
                $scope.Order.Lxdh = $scope.SelectKhmc.Lxdh;
                $scope.Order.Khqd = $scope.SelectKhmc.Khqd;
                $scope.Order.Shdz = $scope.SelectKhmc.Shdz;
                $scope.Order.AreaName = $scope.SelectKhmc.Qy;
            }
            $scope.pop = function() {
                // Service usage example
                $timeout(function() {

                    Notify.alert(
                        'This is a custom message from notify..',
                        { status: 'success' }
                    );

                }, 100);
            };

            $scope.GetAmount = function(d) {
                return d.Num * d.Price;
            }

            $scope.GetPrices = function(d) {
                var prices = [];
                if (d.Price1 && d.Price1 !== "") {
                    //   prices.push({ Name: '单价1:' + d.Price1, Value: d.Price1 });
                    prices.push(d.Price1);
                }
                if (d.Price2 && d.Price2 !== "") {
                    prices.push(d.Price2);
                    // prices.push({ Name: '单价2:' + d.Price2, Value: d.Price2 });
                }
                if (d.Price3 && d.Price3 !== "") {
                    prices.push(d.Price3);
                    //  prices.push({ Name: '单价3:' + d.Price3, Value: d.Price3 });
                }
                //   console.log(prices);
                return prices;
            }
        }
    ]);

angular.module('angle').directive('enterAsTab', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                event.preventDefault();

                var savebtn = element.find('button')[0];
                if (savebtn.title==="Save") {
                    savebtn.click();
                }
                $("#newOrderGoods").click();
                //var nexttr = element.next("tr");
                //if (!nexttr) {

                //}
                //   console.log(element.find('button')[0]);
                //    var elementToFocus = element.next("tr").find('submit')[0];

                //if (angular.isDefined(elementToFocus))
                //    elementToFocus.focus();
            }
        });
    };
});

angular.module('angle').directive('enterAsTab2', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                event.preventDefault();

                var savebtn = element.find('button')[0];
                if (savebtn.title === "Save") {
                    savebtn.click();
                }
                $("#newCost").click();
                //var nexttr = element.next("tr");
                //if (!nexttr) {

                //}
                //   console.log(element.find('button')[0]);
                //    var elementToFocus = element.next("tr").find('submit')[0];

                //if (angular.isDefined(elementToFocus))
                //    elementToFocus.focus();
            }
        });
    };
});