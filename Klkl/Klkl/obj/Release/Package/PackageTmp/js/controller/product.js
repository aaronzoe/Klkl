'use strict';
angular.module('angle')
    .controller('productController', ['$scope', '$stateParams', '$http', '$filter', '$state', 'editableOptions', 'editableThemes', 'Notify', '$timeout', function ($scope, $stateParams, $http, $filter, $state, editableOptions, editableThemes, Notify, $timeout) {
        $http.get("/product/" + $stateParams.id).then(function(response) {
            $scope.Response = response.data;
            if ($stateParams.id) {
                $scope.SelectCategory = $filter('filter')(response.data.Categories, { "Name": response.data.Goods.Category })[0];
}
            $scope.Materials = response.data.Materials;
            $scope.MaterialTypes = response.data.MaterialTypes;
        });
        $scope.save = function () {
            console.log($scope.SelectCategory);
            $scope.Response.Goods.Category = $scope.SelectCategory.Name;
            $http.post("/product/save", { "Goods": $scope.Response.Goods }).then(function(response) {
                $scope.Response.Goods.ID = response.data.ID;
            });
        };
        $scope.cancel = function() {
            $state.go("app.products");
        };

        $scope.showMaterialType = function (material) {
            if (material.MaterialID && $scope.MaterialTypes.length) {
             
                var selected = $filter('filter')($scope.MaterialTypes, function (m) {
                    return $filter('filter')(m.Materials, { "ID": material.MaterialID }).length > 0;
                });
                return selected.length && selected.length>0 ? selected[0].Name : '未设置';
            } else {
                return "未设置";
            }
        };
        $scope.showMaterial = function (material) {
            if (material.MaterialID && $scope.Materials.length) {
                var selected = $filter('filter')($scope.Materials, { "ID": material.MaterialID });
                return selected.length ? selected[0].Name : '未设置';
            } else {
                return "未设置";
            }
        };
        $scope.addMaterial = function () {
            $scope.inserted = {
                ID: $scope.Response.Goods.Materials.length+1,
                Num: 1
            };
            $scope.Response.Goods.Materials.push($scope.inserted);
        };
        $scope.saveMaterial = function () {
            $http.post("/product/save", { "Goods": $scope.Response.Goods }).then(function (response) {
                $scope.Response.Goods.ID = response.data.ID;
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
        $scope.removeMaterial = function (index) {
            $scope.Response.Goods.Materials.splice(index, 1);
            $http.post("/product/save", { "Goods": $scope.Response.Goods }).then(function (response) {
                $scope.Response.Goods.ID = response.data.ID;
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

        $scope.SelectK = function (d) {
            $scope.Response.Goods.Category = d.Name;
        }
    }]);