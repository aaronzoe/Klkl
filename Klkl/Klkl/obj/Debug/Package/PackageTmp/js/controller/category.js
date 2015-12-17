'use strict';
angular.module('angle')
    .controller('categoryController', ['$scope', '$stateParams', '$http', '$filter', '$state', 'editableOptions', 'editableThemes', 'Notify', '$timeout', 'ngDialog', function ($scope, $stateParams, $http, $filter, $state, editableOptions, editableThemes, Notify, $timeout, ngDialog) {
        $http.get("/categories").then(function (response) {
            $scope.Categories = response.data;
        });
       
        $scope.add = function () {
            $scope.inserted = {
                ID:0
        };
            $scope.Categories.push($scope.inserted);
        };
        $scope.save = function (cate) {
            $http.post("/category/update", { "Category": cate }).then(function (response) {
                cate.ID = response.data;
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
            if (id ===0) {
                $scope.Categories.splice(index, 1);
            } else {
                ngDialog.openConfirm({
                    template: 'modalDialogId',
                    className: 'ngdialog-theme-default'
                }).then(function(value) {
                    $http.post("/category/del", { "Id": id }).then(function(response) {
                        $scope.Categories.splice(index, 1);
                    });
                }, function(reason) {
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