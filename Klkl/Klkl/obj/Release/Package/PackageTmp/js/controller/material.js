'use strict';
angular.module('angle')
    .controller('materialController', ['$scope', '$stateParams', '$http', '$filter', '$state', 'editableOptions', 'editableThemes', 'Notify', '$timeout', 'ngDialog', function ($scope, $stateParams, $http, $filter, $state, editableOptions, editableThemes, Notify, $timeout, ngDialog) {
        $http.post("/material/getlist").then(function (response) {
            $scope.Materials = response.data.Materials;
            $scope.MaterialTypes = response.data.MaterialTypes;
        });

        $scope.addMaterial = function () {
            $scope.inserted = {
                ID: 0
            };
            $scope.Materials.push($scope.inserted);
        };
        $scope.saveMaterial = function (material) {
            $http.post("/material/update", { "Material": material }).then(function (response) {
                material.ID = response.data;
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
        $scope.removeMaterial = function (index, id) {
            if (id === 0) {
                $scope.Materials.splice(index, 1);
            } else {
                ngDialog.openConfirm({
                    template: 'modalDialogId',
                    className: 'ngdialog-theme-default'
                }).then(function (value) {
                    $http.post("/material/del", { "Id": id }).then(function (response) {
                        $scope.Materials.splice(index, 1);
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


        $scope.showMaterialTypeName = function (material) {
            var selected = $filter('filter')($scope.MaterialTypes, { ID: material.TypeID });
            return ($scope.MaterialTypes && selected.length) ? selected[0].Name : '未设置';
        };
        $scope.showMaterialTypeUnit = function (material) {
            var selected = $filter('filter')($scope.MaterialTypes, { ID: material.TypeID });
            return ($scope.MaterialTypes && selected.length) ? selected[0].Unit : '未设置';
        };


        $scope.addMaterialType = function () {
            $scope.inserted = {
                ID: 0
            };
            $scope.MaterialTypes.push($scope.inserted);
        };
        $scope.saveMaterialType = function (materialType) {
            $http.post("/materialType/update", { "MaterialType": materialType }).then(function (response) {
                materialType.ID = response.data;
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
        $scope.removeMaterialType = function (index, id) {
            if (id === 0) {
                $scope.MaterialTypes.splice(index, 1);
            } else {
                ngDialog.openConfirm({
                    template: 'modalDialogId',
                    className: 'ngdialog-theme-default'
                }).then(function (value) {
                    $http.post("/materialType/del", { "Id": id }).then(function (response) {
                        $scope.MaterialTypes.splice(index, 1);
                    });
                }, function (reason) {
                });
            }
        };
    }]);