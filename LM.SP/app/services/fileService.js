(function () {

    "use strict";

    var app = angular.module("app");

    //factories
    app.factory("fileService", fileSrvc);

    //injections
    fileSrvc.$inject = ["$q", "$http", "localStorageService", "utility", "Upload"];

    function fileSrvc($q, $http, localStorageService, utility, Upload) {

        var serviceFactory = {};

        serviceFactory.upload = upload;

        return serviceFactory;

        function upload(files) {
            var defer = $q.defer();

            Upload.upload({
                url: utility.baseAddress + "/api/file/upload",
                data: {
                    file: files
                }
            })
                .then(function (resp) {

                    defer.resolve(resp.data);
                },
                    function (resp) {

                        defer.reject(resp.status);
                    },
                    function (resp) {
                        var progressPercentage = parseInt(100.0 * resp.loaded / resp.total);

                        console.warn("Progress " + progressPercentage + "%");
                    });

            return defer.promise;
        }

    }

})();