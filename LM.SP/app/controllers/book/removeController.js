(function() {

    "use strict";

    var app = angular.module("app");

    //controllers
    app.controller("bookremoveController", bookremoveCtrl);

    //injections
    bookremoveCtrl.$inject = ["$rootScope", "$scope", "$stateParams", "bookService", "utilityService"];

    function bookremoveCtrl($rootScope, $scope, $stateParams, bookService, utilityService) {

        // initialize your users data
        (function() {

            $rootScope.title = "Deleting confirmation book...";

            bookService.remove($stateParams.id)
                .then(function(response) {

                    $rootScope.message = "Book deleted successfully.";

                    utilityService.redirectTo("book/list");

                })
                .catch(function(response) {
                    $rootScope.error = utilityService.throwErrors(response);
                });

        })();

    }

})();