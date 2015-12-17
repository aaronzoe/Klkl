'use strict';
angular.module('angle')
    .controller('UserController', ['$scope', "$http", "$state", "$stateParams", 'Notify', '$timeout', 'editableOptions', 'editableThemes',
  function ($scope, $http, $state, $stateParams, Notify, $timeout, editableOptions, editableThemes) {


      editableOptions.theme = 'bs3';

      editableThemes.bs3.inputClass = 'input-sm';
      editableThemes.bs3.buttonsClass = 'btn-sm';
      editableThemes.bs3.checklistClass = 'btn-sm';
      editableThemes.bs3.submitTpl = '<button type="submit" class="btn btn-success"><span class="fa fa-check"></span></button>';
      editableThemes.bs3.cancelTpl = '<button type="button" class="btn btn-default" ng-click="$form.$cancel()">' +
                                       '<span class="fa fa-times text-muted"></span>' +
                                     '</button>';

      $http.post("/user/", { Id: $stateParams.id }).success(function(data) {
          $scope.User = data.User;
          $scope.Roles = data.Roles;
      });

      $scope.showRoles=function() {
          var selected = [];
          if ($scope.User) {
              angular.forEach($scope.Roles, function (s) {

                  if ($scope.User.Roles.indexOf(s.RoleName) >= 0) {
                      selected.push(s.RoleName);
                  }
              });
          }
          return selected.length ? selected.join(', ') : '未设置';
      }

      $scope.saveUser=function() {
          $http.post("/users/update", { UserAuth: $scope.User }).success(function (data) {
              $scope.User.Id = data;
              $timeout(function () {
                  Notify.alert(
                      '保存成功..',
                      { status: 'success' }
                  );
              }, 100);
          });
      }
      $scope.savePassWord = function (psw) {
         
          $http.post("/users/changepsw", { Id: $scope.User.Id, PassWord: psw }).success(function (data) {
              $scope.User.Id = data;
              $timeout(function () {
                  Notify.alert(
                      '保存成功..',
                      { status: 'success' }
                  );
              }, 100);
          });
      }
      $scope.cancel = function () {
          $state.go("app.users");
      };
  }]);