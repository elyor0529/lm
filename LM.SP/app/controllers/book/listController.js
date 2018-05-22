(function () {

    "use strict";

    var app = angular.module("app");

    //controllers
    app.controller("booklistController", booklistCtrl);

    //injections
    booklistCtrl.$inject = ["$rootScope", "$scope", "$stateParams", "$window", "$http", "$q", "bookService", "utilityService", "bootstrap3ElementModifier"];

    function booklistCtrl($rootScope, $scope, $stateParams, $window, $http, $q, bookService, utilityService, bootstrap3ElementModifier) {

        bootstrap3ElementModifier.enableValidationStateIcons(false);

        $scope.page = $stateParams.page;
        $scope.search = $stateParams.search;
        $scope.searching = searching;
        $scope.downloadUrl = utilityService.getInstance().baseAddress + "/api/file/download";
        $scope.list = list;
        $scope.sorting = sorting;
        $scope.reset = reset;
        $scope.sort = $stateParams.sort;
        $scope.column = $stateParams.column;
        $scope.saveBook = saveBook;
        $scope.saveAuthor = saveAuthor;

        // initialize your users data
        (function () {

            $rootScope.title = "Books";

            list();

        })();

        function reset() {
            $scope.search = "";

            list();
        }

        function searching() {

            $window.location.href = "#/book/list?page=" + $scope.page + "&search=" + $scope.search + "&column=" + $scope.column + "&sort=" + $scope.sort;
        }

        function sorting(column) {

            $scope.column = column;

            if ($scope.sort === "asc") {
                $scope.sort = "desc";
            } else {
                $scope.sort = "asc";
            }
        }

        function saveBook(id, column, data) {
            var d = $q.defer();

            $http.post(utilityService.getInstance().baseAddress + "/api/book/save", { id: id, column: column, data: data })
                .success(function (res) {
                    res = res || {};

                    console.log(res);

                    if (res.status === "ok") {
                        d.resolve();
                    } else {
                        d.resolve(res.msg);
                    }

                })
                .error(function (e) {
                    console.error(e);

                    d.reject("Server error!");
                });

            return d.promise;
        }

        function saveAuthor(id, authorId, column, data) {
            var d = $q.defer();

            $http.post(utilityService.getInstance().baseAddress + "/api/book/save", { id: id, authorId: authorId, column: column, data: data })
                .success(function (res) {
                    res = res || {};

                    console.log(res);

                    if (res.status === "ok") {
                        d.resolve();
                    } else {
                        d.resolve(res.msg);
                    }

                })
                .error(function (e) {
                    console.error(e);

                    d.reject("Server error!");
                });

            return d.promise;
        }

        function list() {
            bookService.list($scope.page, $scope.search, $scope.column, $scope.sort)
                .then(function (response) {

                    $scope.bookData = response.data.items;
                    $scope.pager = response.data.pager;

                })
                .catch(function (response) {
                    $rootScope.error = utilityService.throwErrors(response);
                });
        }
    }

})();