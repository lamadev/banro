var app = angular.module('app', ['ngRoute']);
app.factory("FactoryHome", function ($http, $q,$httpParamSerializerJQLike) {
   
    var MyFactorySuccusale = {
        rootServer: "/",
        setSuccursale: function (succ) {
            var promise_succ = $q.defer();
            var url =MyFactorySuccusale.rootServer+"Home/AddSuccursale";
            $http.post(url, succ)
                 .success(function (data, status) {
                     promise_succ.resolve(JSON.stringify(data));
                 })
                 .error(function (data, status) {
                     promise_succ.reject("Server not found");
                 })
            return promise_succ.promise;
        },
        getListDepartement: function (id_succ) {
          //  alert("ID:" + id_depart);
            var promise_depart = $q.defer();
            var url = MyFactorySuccusale.rootServer + "ListDepartement/" + id_succ;
            $http.get(url)
                 .success(function (data, status) {
                     promise_depart.resolve(data);
                 })
                .error(function (data, status) {
                    promise_depart.reject(data);
                })
            return promise_depart.promise;
        },
        setUpdateCompany: function (succ) {
            var promise_succ = $q.defer();
            var url = MyFactorySuccusale.rootServer+"Home/UpdateSuccursale";
            console.log(url);
            $http.post(url, succ)
                 .success(function (data, status) {
                     promise_succ.resolve(data);
                 })
                .error(function (data, status) {
                    promise_succ.reject("Server not found");
                });
            return promise_succ.promise;
            //$http.post(url, succ)
            //     .success(function (data, status) {
            //         promise_succ.resolve(data);
            //     })
            //    .error(function (data, status) {
            //        promise_succ.reject("Server not response");
            //    });
            //return promise_succ.promise;
            //console.log("JSON :",JSON.stringify(succ));
        },
        setUpdateDepartement: function (depart) {
            var promise_depart = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/UpdateDepartement";
            $http.post(url, depart)
                 .success(function (data, status) {
                     promise_depart.resolve(data);
                 })
                .error(function (data, status) {
                    promise_depart.reject("Server not response");
                });
            return promise_depart.promise;
        },
        getListSuccursales: function () {
            var promise_getter = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/getListSuccursales";
            $http.get(url)
                 .success(function (data, status) {
                     promise_getter.resolve(data);
                 })
                .error(function (data, status) {
                    promise_getter.reject(data);
                });
            return promise_getter.promise;
        },
        setEmployed: function (employed) {
            //console.log("Data received :", employed);
            var promise_employed = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/AddEmployed";
            $http.post(url, employed)
                 .success(function (data, status) {
                     promise_employed.resolve(data);
                 })
                .error(function (data, status) {
                    console.log("Error :", data);
                    promise_employed.reject(data);
                });

            return promise_employed.promise;
        },
        getParent: function (name) {
            var promise_child = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Parents/" + name;
            $http.get(url)
                 .success(function (data, status) {
                     promise_child.resolve(data);
                 }, function (data, status) {
                     promise_child.reject(data);
                 });
            return promise_child.promise;
        },
        setChildren: function (child) {
            var promise_children = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/AddChildren";
            $http.post(url,child)
                 .success(function (data, status) {

                     promise_children.resolve(data);

                 }, function (data, status) {
                     promise_children.reject(data);
                 });
            return promise_children.promise;
        },
        setUpdateChildren: function (child) {
            var promise_update = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/UpdateChildren";
            $http.post(url, child)
                 .success(function (data, status) {

                     promise_update.resolve(data);

                 }, function (data, status) {
                     promise_update.reject(data);
                 });

            return promise_update.promise;
        },
        setUpdateEmployed:function(employed){
            var promise_employed = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/UpdateEmployed";
            $http.post(url, employed)
                 .success(function (data, status) {
                     promise_employed.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_employed.reject(data);
                 });
            return promise_employed.promise;
        },
        getListDepartements:function(){
            var promise_allDep = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/getListDepartments";
            $http.get(url)
                 .success(function (data, status) {
                     promise_allDep.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_allDep.reject(data);
                 });
            return promise_allDep.promise;
        },
        getConjointList: function (conjoint) {
            var promise_conjoint = $q.defer();
            var url = MyFactorySuccusale.rootServer + "conjointlist/" + conjoint;
            $http.get(url)
                 .success(function (data, status) {
                     promise_conjoint.resolve(data);
                 })
                 .error(function (data, status) {
                     console.log(data);
                     promise_conjoint.reject(data);
                 });
            return promise_conjoint.promise;
        },
        setConjoint: function (conjoint) {
            var promise_conjoint = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/AddConjoint";
            $http.post(url, conjoint)
                 .success(function (data, status) {
                     promise_conjoint.resolve(data);
                 })
                .error(function (data, status) {
                    promise_conjoint.reject(data);
                });
            return promise_conjoint.promise;
        },
        setVisitor: function (visitor) {
            var promise_visitor = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/AddVisitor";
            $http.post(url, visitor)
                 .success(function (data, status) {
                     promise_visitor.resolve(data);
                 })
                .error(function (data, status) {
                    promise_visitor.reject(data);
                });
            return promise_visitor.promise;
        },
        getListHealths: function () {
            var promise_healths = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/getListHealth";
            $http.get(url)
                 .success(function (data, status) {
                     promise_healths.resolve(data);
                 })
                .error(function (data, status) {
                    promise_healths.reject(data);
                });
            return promise_healths.promise;
        },

        getListHealthsTab: function () {
            var promise_healths = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/getListHealthTab";
            $http.get(url)
                 .success(function (data, status) {
                     promise_healths.resolve(data);
                 })
                .error(function (data, status) {
                    promise_healths.reject(data);
                });
            return promise_healths.promise;
        },
        getListEmployed: function (name) {
            var promise_employed = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getListEmployed/" + name;
            $http.get(url)
                 .success(function (data, status) {
                     promise_employed.resolve(data);
                 })
                .error(function (data, status) {
                    promise_employed.reject(data);
                });
            return promise_employed.promise;
        },
        setBonCommand: function (BCommand) {
            var promise_Bcommand = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/setBonCommand/";
            $http.post(url,BCommand)
                 .success(function (data, status) {
                     promise_Bcommand.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_Bcommand.reject(data);
                 });
            return promise_Bcommand.promise;
        },
        getListBonCommand: function (name, category) {
            var promise_list_bon = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getlistforcommand/" + name + "?category=" + category;
            $http.get(url)
                 .success(function (data, status) {
                     promise_list_bon.resolve(data);
                 })
                .error(function (data, status) {
                    promise_list_bon.reject(data);
                });
            return promise_list_bon.promise;
        },
        getlistBcommandForFacture:function(name){
            var promise_BCmd = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getlistbcommand/";
            $http.get(url)
                 .success(function (data, status) {
                     promise_BCmd.resolve(data);
                 })
                  .error(function (data, status) {
                      promise_BCmd.reject(data);
                  });
            return promise_BCmd.promise;
        },
        getlistVoucherDependents:function(){
            var promise_BCmd = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getlistvoucherdependents";
            $http.get(url)
                 .success(function (data, status) {
                     promise_BCmd.resolve(data);
                 })
                  .error(function (data, status) {
                      promise_BCmd.reject(data);
                  });
            return promise_BCmd.promise;
        },
        getlistVoucherVisitor: function () {
            var promise_BCmd = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getlistvouchervisitor";
            $http.get(url)
                 .success(function (data, status) {
                     promise_BCmd.resolve(data);
                 })
                  .error(function (data, status) {
                      promise_BCmd.reject(data);
                  });
            return promise_BCmd.promise;
        },
        getlistVoucherCasual: function () {
            var promise_BCmd = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getlistvouchercasual";
            $http.get(url)
                 .success(function (data, status) {
                     promise_BCmd.resolve(data);
                 })
                  .error(function (data, status) {
                      promise_BCmd.reject(data);
                  });
            return promise_BCmd.promise;
        },
        getlistVoucherContactor: function () {
            var promise_BCmd = $q.defer();
            var url = MyFactorySuccusale.rootServer + "getlistvouchercontractor";
            $http.get(url)
                 .success(function (data, status) {
                     promise_BCmd.resolve(data);
                 })
                  .error(function (data, status) {
                      promise_BCmd.reject(data);
                  });
            return promise_BCmd.promise;
        },
        setHospitalNewer:function(object){
            var promise_BCmd = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/ModifyHospital";
            $http.post(url,object)
                 .success(function (data, status) {
                     promise_BCmd.resolve(data);
                 })
                  .error(function (data, status) {
                      promise_BCmd.reject(data);
                  });
            return promise_BCmd.promise;
        },
        setSendFileCSV: function (csv) {
            var promise_csv = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/getCSV";
            $http.post(url, csv)
                 .success(function (data, status) {
                     promise_csv.resolve(data);
                 })
                .error(function (data, status) {
                    promise_csv.reject(data);
                });
            return promise_csv.promise;
        },
        getReprotingService: function () {
            var promise_reporting = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/getReportingSystem";
            $http.get(url)
                 .success(function (data, status) {
                     promise_reporting.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_reporting.reject(data);
                 });
            return promise_reporting.promise;
        },
        setExoprtCSV: function (tabData) {
            var promise_export = $q.defer();
            var url = MyFactorySuccusale.rootServer+"exportation";
            $http.post(url, tabData)
                 .success(function (data, status) {
                     promise_export.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_export.reject();
                 });
            //console.log("Data Exports Linker :", url);
            return promise_export.promise;
        },
        setExoprtPDF: function (tabData) {
            var promise_export = $q.defer();
            var url = MyFactorySuccusale.rootServer + "exportPDF";
            $http.post(url, tabData)
                 .success(function (data, status) {
                     promise_export.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_export.reject();
                 });
            //console.log("Data Exports Linker :", url);
            return promise_export.promise;
        },
        getListCasuals:function(){
            var promise_casual = $q.defer();
            var url = MyFactorySuccusale.rootServer + "searchcasual";
            console.log(url);
            $http.get(url)
                 .success(function (data, status) {
                     promise_casual.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_casual.reject(data);
                 });
            //console.log("Data Exports Linker :", url);
            return promise_casual.promise;
        },
        getExportMVI: function (object) {
            var promise_export = $q.defer();
            var url = MyFactorySuccusale.rootServer + "excelMCI";
            $http.get(url)
                 .success(function (data, status) {
                     promise_export.resolve(data);
                 })
                 .error(function (data, status) {
                     promise_export.reject();
                 });
            //console.log("Data Exports Linker :", url);
            return promise_export.promise;

        },
        getUsersAccount:function (succursale) {
            var promise_users = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/listusersaccount/" + succursale;
            $http.get(url)
                          .success(function (data, status) {
                              promise_users.resolve(data);
                          })
                          .error(function (data, status) {

                          });

            return promise_users.promise;

        },
        getUsersAccountExist: function (succursale) {
            var promise_users = $q.defer();
            var url = MyFactorySuccusale.rootServer + "Home/listusersaccountexist/" + succursale;
            $http.get(url)
                          .success(function (data, status) {
                              promise_users.resolve(data);
                          })
                          .error(function (data, status) {

                          });

            return promise_users.promise;

        },
        UpdateVisitor: function (object) {
            var promise_sender = $q.defer();
            var url=MyFactorySuccusale.rootServer + "Home/UpdateVisitor/";
            $http.post(url,object).success(function (data) {
                promise_sender.resolve(data);
            })
            .error(function (error) {
                promise_sender.reject(error);
            });
            return promise_sender.promise;

        }


        
    }
    return MyFactorySuccusale;
});