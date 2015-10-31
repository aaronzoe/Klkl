angular.module('angle', ['ngRoute'])
  .controller('productController', function ($scope, $stateParams, $http) {
      $http.get("/product/" + $stateParams.id).then(function (response) {
            $scope.Response = response.data;
        });
    });