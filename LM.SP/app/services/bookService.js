(function () {

    "use strict";

    var app = angular.module("app");

    //factories
    app.factory("bookService", bookSrvc);

    //injections
    bookSrvc.$inject = ["$q", "$http", "localStorageService", "utility"];

    function bookSrvc($q, $http, localStorageService, utility) {

        var serviceFactory = {};

        serviceFactory.list = list;
        serviceFactory.add = add;
        serviceFactory.edit = edit;
        serviceFactory.details = details;
        serviceFactory.remove = remove;
        serviceFactory.removePicture = removePicture;
        serviceFactory.removeAuthor = removeAuthor;

        return serviceFactory;

        function list(page, search, column, sort) {

            return $http.get(utility.baseAddress + "/api/book/list?page=" + page + "&search=" + search + "&column=" + column + "&sort=" + sort)
                .then(function (response) {
                    return response;
                });
        }

        function add(bookData) {

            return $http.post(utility.baseAddress + "/api/book/add", bookData)
                .then(function (response) {
                    return response;
                });
        }

        function edit(bookData) {

            return $http.put(utility.baseAddress + "/api/book/edit", bookData)
                .then(function (response) {
                    return response;
                });
        }

        function details(id) {

            return $http.get(utility.baseAddress + "/api/book/details/" + id)
                .then(function (response) {
                    return response;
                });
        }

        function remove(id) {

            return $http.delete(utility.baseAddress + "/api/book/remove/" + id)
                .then(function (response) {
                    return response;
                });
        }

        function removePicture(id) {

            return $http.delete(utility.baseAddress + "/api/book/remove-picture/" + id)
                .then(function (response) {
                    return response;
                });
        }

        function removeAuthor(id) {

            return $http.delete(utility.baseAddress + "/api/book/remove-author/" + id)
                .then(function (response) {
                    return response;
                });
        }
    }

})();