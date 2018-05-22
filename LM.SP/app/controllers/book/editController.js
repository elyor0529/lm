(function() {

    "use strict";

    var app = angular.module("app");

    //controllers
    app.controller("bookeditController", bookeditCtrl);

    //injections
    bookeditCtrl.$inject = ["$rootScope", "$scope", "$stateParams", "bookService", "utilityService", "fileService","bootstrap3ElementModifier"];

    function bookeditCtrl($rootScope,$scope,$stateParams,bookService,utilityService,fileService,bootstrap3ElementModifier) {

        bootstrap3ElementModifier.enableValidationStateIcons(false);

        $scope.files = [];
        $scope.bookData = {};
        $scope.downloadUrl = utilityService.getInstance().baseAddress + "/api/file/download";
        $scope.selectFile = function(file) {

            if (file) {

                if (_.find($scope.files, function(f) { return f.name === file.name; })) {
                    $window.alert("This file already exists!");

                    return;
                }

                $scope.files = [file];
            }
        };
        $scope.removeFile = function() {
            $scope.files = [];
            $scope.bookData.picturePath = "";
        };
        $scope.deleteFile = function(id) {

            bookService.removePicture(id)
                .then(function(response) {
                    $scope.bookData.picturePath = "";
                })
                .catch(function(response) {
                    $rootScope.error = utilityService.throwErrors(response);
                });

        };
        $scope.addAuthor = function() {
            var author = {
                firstName: "",
                lastName: "",
                id: 0
            };

            $scope.bookData.authors.push(author);
        };
        $scope.removeAuthor = function(index) {

            $scope.bookData.authors.splice(index, 1);
        };
        $scope.deleteAuthor = function(index, id) {

            bookService.removeAuthor(id)
                .then(function(response) {

                    $scope.removeAuthor(index);

                })
                .catch(function(response) {
                    $rootScope.error = utilityService.throwErrors(response);
                });

        };
        $scope.edit = edit;

        $(document)
            .on("change",
                ".btn-file :file",
                function() {
                    var input = $(this),
                        numFiles = input.get(0).files ? input.get(0).files.length : 1,
                        label = input.val().replace(/\\/g, "/").replace(/.*\//, "");
                    input.trigger("fileselect", [numFiles, label]);
                });

        // initialize your users data
        (function() {

            $rootScope.title = "Edit books";

            bookService.details($stateParams.id)
                .then(function(response) {

                    $scope.bookData = response.data;
                })
                .catch(function(response) {
                    $rootScope.error = utilityService.throwErrors(response);
                });

        })();

        function edit() {

            if ($scope.files.length > 0) {

                fileService.upload($scope.files)
                    .then(function(files) {

                            angular.forEach(files,
                                function(file) {
                                    $scope.bookData.picturePath = file;
                                });

                            doPut();

                        },
                        function(reason) {

                            // Error callback where reason is the value of the first rejected promise
                            console.error(reason);
                        });
            } else {
                doPut();
            }

            function doPut() {

                bookService.edit($scope.bookData)
                    .then(function(response) {
                        $rootScope.message = "Books successfully saved.";

                        utilityService.redirectTo("book/list");
                    })
                    .catch(function(response) {
                        $rootScope.error = utilityService.throwErrors(response);

                        if ($scope.files.length > 0) {
                            $scope.bookData.picturePath = $scope.files[0].name;
                        }
                    });
            }

        }

    }

})();