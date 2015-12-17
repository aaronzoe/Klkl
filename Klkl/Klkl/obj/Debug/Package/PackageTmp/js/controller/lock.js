'use strict';
angular.module('angle')
    .controller('lockController', ['$scope', '$stateParams', '$http', '$state', '$cookieStore', '$rootScope', function ($scope, $stateParams, $http, $state, $cookieStore, $rootScope) {
        
        $http.post("/authenticate", { provider: "logout" }).then(function (response) {
            $cookieStore.remove('currentUser');
        });

        $scope.login = function () {
            $http.post("/authenticate", { UserName: $scope.user.username, PassWord: $scope.lock.password }).success(function () {
                $cookieStore.put('currentUser', $rootScope.user);
                $state.go('app.dashboard');
            }).error(function () {
                alert('用户名或密码错误');
            });

        };
       
    }]);