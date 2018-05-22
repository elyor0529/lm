(function() {

    "use strict";

    var app = angular.module("app");

    //controllers
    app.controller("bookaddController", bookaddCtrl);

    //injections
    bookaddCtrl.$inject = ["$rootScope", "$scope", "$window", "bookService", "fileService", "bootstrap3ElementModifier", "utilityService"];

    function bookaddCtrl($rootScope, $scope, $window, bookService, fileService, bootstrap3ElementModifier, utilityService) {

        bootstrap3ElementModifier.enableValidationStateIcons(false);

        $scope.files = [];
        $scope.bookData = {
            authors: [
                {
                    firstName: "",
                    lastName: ""
                }
            ],
            title: "",
            countOfPages: 0,
            publisher: "",
            year: new Date().getFullYear(),
            isbn: "",
            picturePath: ""
        };
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
        $scope.addAuthor = function() {
            var author = {
                firstName: "",
                lastName: ""
            };

            $scope.bookData.authors.push(author);
        };
        $scope.removeAuthor = function(index) {

            $scope.bookData.authors.splice(index, 1);
        };
        $scope.add = add;

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

            $rootScope.title = "Add book";

        })();

        function add() {

            if ($scope.files.length > 0) {

                fileService.upload($scope.files)
                    .then(function(files) {

                            angular.forEach(files,
                                function(file) {
                                    $scope.bookData.picturePath = file;
                                });

                            doPost();

                        },
                        function(reason) {

                            // Error callback where reason is the value of the first rejected promise
                            console.error(reason);
                        });
            } else {
                doPost();
            }


            function doPost() {

                bookService.add($scope.bookData)
                    .then(function(response) {
                        $rootScope.message = "Book successfully saved.";

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