'use strict';
angular.module('angle')
    .controller('UsersController', [
        '$scope', '$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', "$filter", "ngTableParams", "$timeout", "ngTableDataService", "$http", "$state", 'Notify', 'ngDialog',
        function($scope, $resource, DTOptionsBuilder, DTColumnDefBuilder, $filter, ngTableParams, $timeout, ngTableDataService, $http, $state, Notify, ngDialog) {
            var vm = this;
            $scope.date = new Date();
            $http.post('/users/getlist').then(function(response) {
                //  var Api = $resource('/order/list?format=json');
                var data = response.data.Results;
                $scope.alldate = data;
                vm.tableParams5 = new ngTableParams({
                    page: 1, // show first page
                    count: 10, // count per page
                    sorting: {
                        Id: 'asc' // initial sorting
                    }
                }, {
                    total: 0, // length of data
                    counts: [], // hide page counts control
                    getData: function($defer, params) {
                        ngTableDataService.getData3($defer, params, $scope.alldate);
                    }
                });
            });
            $scope.viewUser = function(id) {
                $state.go('app.user', { "id": id });

            }
            $scope.new = function() {
                $state.go("app.user", {});
            };

            $scope.lockUser = function(user) {
                var olddate = user.LockedDate;
                if (user.LockedDate) {
                    user.LockedDate = '';
                } else {
                    user.LockedDate = '2099-1-1';
                }
                $http.post("/users/update", { UserAuth: user }).success(function(data) {
                    $timeout(function() {
                        Notify.alert(
                            '操作成功..',
                            { status: 'success' }
                        );
                    }, 100);
                }).error(function() {
                    user.LockedDate = olddate;
                    $timeout(function() {
                        Notify.alert(
                            '操作失败..',
                            { status: 'fail' }
                        );
                    }, 100);
                });
            };
            $scope.new = function() {
                $scope.ec = { Id: 0 };
                ngDialog.openConfirm({
                        template: 'modalDialogId2',
                        className: 'ngdialog-theme-default',
                        preCloseCallback: function(value) {
                            return true;
                        },
                        scope: $scope
                    })
                    .then(function() {
                        $http.post("/users/update", {"UserAuth": $scope.ec}).then(function (response) {
                            if ($scope.ec.Id === 0) {
                                $scope.ec.Id = response.data;
                                $scope.table.tableParams5.data.splice($scope.table.tableParams5.data.length, 0, $scope.ec);
                            }
                            $timeout(function() {
                                Notify.alert(
                                    '添加用户' + $scope.ec.DisplayName+ '成功.初始密码:123456',
                                    { status: 'success' }
                                );
                            }, 100);
                        });
                    }, function(value) {
                    });
            };

            $scope.removeUser = function(index, id) {
                $http.post("/users/del", { "Id": id }).success(function() {
                    $scope.table.tableParams5.data.splice(index, 1);
                    $timeout(function() {
                        Notify.alert(
                            '删除成功...',
                            { status: 'success' }
                        );
                    }, 100);
                }).error(function(data) {
                    Notify.alert(
                        '删除失败...',
                        { status: 'fail' }
                    );
                }, 100);
            };
        }
    ]);