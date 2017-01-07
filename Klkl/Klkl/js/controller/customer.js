'use strict';
angular.module('angle')
    .controller('CustomersController', ['$scope', '$stateParams', '$http', '$filter', '$state',   'Notify', '$timeout', 'ngDialog', 'ngTableParams','ngTableDataService',
        function ($scope, $stateParams, $http, $filter, $state,   Notify, $timeout, ngDialog, ngTableParams, ngTableDataService) {

        var vm = this;

        $http.get('/customer/list?format=json&OrderByDesc=ID').then(function (response) {
            //  var Api = $resource('/order/list?format=json');
            var data = response.data.result;
            $scope.alldate = data;
            vm.tableParams5 = new ngTableParams({
                page: 1, // show first page
                count: 10, // count per page
                sorting: {
                    ID: 'desc' // initial sorting
                }
            },
            {
                total: 0, // length of data
                counts: [], // hide page counts control
                getData: function($defer, params) {
                    ngTableDataService.getData3($defer, params, $scope.alldate);
                }
            });
        });
    
        $scope.search = function () {
            console.log(1);
            var term = vm.quickFilterText;
            if (vm.isInvertedSearch) {
                term = "!" + term;
            }
            vm.tableParams5.filter({ $: term });
            }

        $scope.remove = function (index, id) {
                ngDialog.openConfirm({
                    template: 'modalDialogId',
                    className: 'ngdialog-theme-default'
                }).then(function (value) {
                    $http.post("/customer/del", { "Id": id }).then(function (response) {
                        $scope.table.tableParams5.data.splice(index, 1);
                    });
                }, function (reason) {
                });
        };
        $scope.edit = function (customer) {
            $scope.ec=   angular.copy(customer);
            ngDialog.openConfirm({
                template: 'modalDialogId2',
                className: 'ngdialog-theme-default',
                preCloseCallback: function (value) {
                    return true;
                },
                scope: $scope
            })
     .then(function () {
         $http.post("/customer/update", { "Customer": $scope.ec }).then(function (response) {
             angular.forEach($scope.table.tableParams5.data, function (u, i) {
                 if (u.ID === $scope.ec.ID) {
                     $scope.table.tableParams5.data[i] = $scope.ec;
                    
                 }
             });
         
             $timeout(function () {
                 Notify.alert(
                     '保存成功..',
                     { status: 'success' }
                 );
             }, 100);
         });
     }, function (value) {
         console.log('rejected:' + value);

     });
        };
        $scope.new = function () {
            $scope.ec = {ID:0};
            ngDialog.openConfirm({
                template: 'modalDialogId2',
                className: 'ngdialog-theme-default',
                preCloseCallback: function (value) {
                    return true;
                },
                scope: $scope
            })
     .then(function () {
         $http.post("/customer/update", { "Customer": $scope.ec }).then(function (response) {
             if ($scope.ec.ID === 0) {
                 $scope.ec.ID = response.data;
                 $scope.table.tableParams5.data.splice(0, 0, $scope.ec);
             }
             $timeout(function () {
                 Notify.alert(
                     '保存成功..',
                     { status: 'success' }
                 );
             }, 100);
         });
     }, function (value) {
         console.log('rejected:' + value);

     });
        };
        $scope.pop = function () {
            // Service usage example
            $timeout(function () {

                Notify.alert(
                    'This is a custom message from notify..',
                    { status: 'success' }
                );

            }, 100);
        };
    }]);