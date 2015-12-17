'use strict';
angular.module('angle')
    .controller('costController', ['$scope', '$stateParams', '$http', '$filter', '$state', 'editableOptions', 'editableThemes', 'Notify', '$timeout', 'ngDialog', function ($scope, $stateParams, $http, $filter, $state, editableOptions, editableThemes, Notify, $timeout, ngDialog) {
        $http.post("/cost/getlist").then(function (response) {
            $scope.Costs = response.data;
        });

        $scope.add = function () {
            $scope.inserted = {
                ID: 0
            };
            $scope.Costs.push($scope.inserted);
        };
        $scope.save = function (cost) {
            $http.post("/cost/update", { "Cost": cost }).then(function (response) {
                cost.ID = response.data;
                $timeout(function () {
                    Notify.alert(
                        '保存成功..',
                        { status: 'success' }
                    );

                }, 100);
            });
            //   console.log('Saving user: ' + id);
            // return $http.post('/saveUser', data);

        };
        $scope.remove = function (index, id) {
            if (id === 0) {
                $scope.Costs.splice(index, 1);
            } else {
                ngDialog.openConfirm({
                    template: 'modalDialogId',
                    className: 'ngdialog-theme-default'
                }).then(function (value) {
                    $http.post("/cost/del", { "Id": id }).then(function (response) {
                        $scope.Costs.splice(index, 1);
                    });
                }, function (reason) {
                });
            }
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