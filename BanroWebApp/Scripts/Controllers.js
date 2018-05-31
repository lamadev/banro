app.controller('CtrlHome', function (FactoryHome, $scope, $timeout,$http) {
    console.log("Okay Good");
    $scope.idAuthSucc="";
    $scope.roleUser = "";
    $scope.categrieBenef = [
         
        "Employee","Dependents","Visitor","Casual","Contractor"
    ];
  
    try {
        $scope.roleUser=document.querySelector('#roleSpan').innerHTML.toString().trim();
        $scope.idAuthSucc=document.querySelector('#spanSucc').innerHTML.toString().trim();
        //alert("Role User :"+$scope.roleUser);
        //alert("ID Succ :"+$scope.idAuthSucc);
    } catch (e) {
    
    }
    $scope.imageBasic = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAQQAAAC0CAYAAABytVLLAAAK4klEQVR4Xu2bB4sVyxZGyyxizmLAhAFzzvrbzVkUc44ooo45p3e/hjr0OzOjnzr3eR7fKrhczsw+3bXX7loVehzW19f3vdAgAAEI/ENgGELgOYAABCoBhMCzAAEIdAggBB4GCEAAIfAMQAAC/QmwQuCpgAAEWCHwDEAAAqwQeAYgAIEfEGDLwOMBAQiwZeAZgAAE2DLwDEAAAmwZeAYgAAGHAGcIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAhhBSaNCHgEEAIDiViIBBCACGEFJo0IeAQQAgOJWIgEEIAIYQUmjQh4BBACA4lYiAQQgAh9Fih37x5Uy5cuFA+fvxYRowYUSZPnlyWLl1axo4d2+np58+fy9WrV8uzZ8/Kt2/fyvjx48uyZcvKpEmTOjHv378v58+fL/r/9+/fy8SJE8vq1avL6NGjfyvj+/fvl7t375bZs2c3/Wm3V69elevXrzf3UtO91J92n4e6P7+VBF/6KQGE8FNE/7uA169fl+PHjw94w+3btzcD/+vXr+XgwYPly5cv/eI2b97cCESD7+jRo40s2m3kyJFl165dZdSoUb+UlK534sSJIhHNnDmzrF27tvP9vr6+cubMmX7XGzZsWNm9e3cZM2bMkPfnlzpP8C8RQAi/hOvfDT558mR5+fJlcxMNbA3At2/fNp81+2/ZsqXcvHmz3L59u/nZtGnTigb548ePm88zZswo69atK5cuXSoPHz7s/EzyeP78efN5yZIlZdGiRVYi9+7da+6lftQ2a9assmbNms7ndp8lC8XWe82dO7esXLlyyPpjdZqgPyKAEP4I39B9Wcv6I0eONLOpVgJaEehnhw4darYPWuprxtVMrW2FPu/Zs6doJpYAJJIpU6aU5cuXd74zbty4smPHjmalcODAgWZ1oeX8qlWryuXLlxuZDB8+vNlK6DraqkgeitOgl3yqWAYTglYikpbuvWnTpqbP+/fvb64jQeg6NYfB+rN169ahA8mV/ogAQvgjfEP3ZQ1CDfZPnz6V+fPnl8WLFzcX1/agCkGS0ODSANdqYOrUqUXbjAkTJpR58+Y1g1rfrzFz5sxpBr/asWPHOiLRtkH3qqsPSURikCTU6uDW7yUa/U5nFpr9B1shaBsi+ej+urb6qDy0GvlZfyQ63YP29wkghL9fg0F7oK2ADgbVNLNv3Lixmem7zwb0e8lAwtCevca0hXDlypXy4MGDzkpDA1crku5raWDu27evOdBstyqUbiFotaLfdTf1R9dRc/qDEHrjQUQIvVGHfr3QjKyT/dq09NaZQVsI+qyVxYsXL5owbTV0sFhjVqxY0awc1G7cuFHu3LnTiKMe9mk7oO1Gu23YsKG5T3cbTAi3bt0q+m+gpsNHrWLc/vRoKaK6hRB6rNwfPnxo3jS0D/LWr19fpk+f3uzL6+Bqz9RaouvVn84EtGw/fPhwM/O33wicO3euPHnypHkVqC2DxKCmVcK7d+86QtEqY6A2kBDa/dH5gM4Q1G8dNEpUOufYuXNnp89Of3qsHHHdQQg9VHINJg3m+kpRe3nN2HU53R6A+luAhQsXNr2/du1a0RuBum2QUCSE9pahHv7Vw0ld8+nTp+Xs2bP/RaC+unRWCFqZnDp1qglt90evIfU6slsIP+tPD5UitisIoYdKXweSuqQ9vLYJmmnVNNg1w9bVgM4Ktm3b1gx8zd6SRZ39NUNrxaDvaDVQD/p0nfpqUvE6sKzXrxh0OKi3F917+oFWCFrNSGB6s6Dtit4W6F51hSMh7N27t9PnH/Wnh8oQ3RWE0CPlH2yA1u5JEDqk08zbPavXGP114IIFC8qjR4/KxYsXB8ysnhG05aPv6XCwvmJsz+T1IgMJQSLQ4Nd3B2p6y6A3GE5/eqQM8d1ACD3yCLS3AwN1qb3319sCvTVotzr46s8GOuyrh4ztpb6uq32+7l9fD+oaWn3odWa3ELploW3O6dOn+0lBf5Sk+9Wzih/1p0dKQDe0Ev1nxvkOif8/AvrbBP2dgAacDvS0hehuWtLXf8ugJf3v/jsGh476oj5pq6F76YDzb/bH6TMx/QkgBJ4KCECgQwAh8DBAAAIIgWcAAhBgy8AzAAEI/IAAWwYeDwhAgC0DzwAEIMCWgWcAAhBgy8AzAAEIOAQ4Q3AoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHAEJwKBEDgRACCCGk0KQJAYcAQnAoEQOBEAIIIaTQpAkBhwBCcCgRA4EQAgghpNCkCQGHwH8AFmb1VN5FaGoAAAAASUVORK5CYII=";
    //console.log(@ViewBag.Title);
    //console.log("Controller CtrlHome");
    var pictureEmployed = document.querySelector('#pictureEmployed');
    $scope.browserPictureChild = function () {
        //   alert("OK");
        var browser = document.createElement('input');
        browser.type = "file"; 
        browser.click();
        var fReader = new FileReader();
        
        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            hidden.value = fReader.result;
            picture.src = fReader.result;
          
           
        } 
    }
     
    
    $scope.browserPicture = function () {
       
        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var fReader = new FileReader();

        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        //var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            //hidden.value = fReader.result;
            pictureEmployed.src = fReader.result;
            console.log(fReader.result);

        }
    } 

    $scope.browserPictureChilding = function () {
        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var fReader = new FileReader();

        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        //var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            //hidden.value = fReader.result;
            document.querySelector('#pictureChilding').src = fReader.result;

        }
    }
    $scope.getDepartement = function () {
        
        FactoryHome.getListDepartement($scope.code_succ).then(function (response) {
            $scope.listDeparts = [];
            $scope.listDeparts = response;
        }, function (error) {
            alert("Error :" + error);
        });
    }
    $scope.selectedDepartement = function (depart) {
        // alert('Selected');
        $scope.hide_code_depart = depart.C_id;

    }
    $scope.hornel = function () {
        
    }
    $scope.ModificationSuccursale = function () {
        var id = document.querySelector('#id_succ');
        var name = document.querySelector('#name_succ');
        var phone = document.querySelector('#phone_succ');
        var adresse = document.querySelector('#address_succ');
        var displaying = document.querySelector("#alertSucc");
        var idCompany = document.querySelector("#id_Company");
        var ctr = 0;
        //alert(id.value);
        var succ = {
            C_id: id.value,
            C_company:idCompany.value,
            C_name: name.value,
            C_phone: phone.value,
            C_adresse: adresse.value
        };
        FactoryHome.setUpdateCompany(succ).then(function (response) {
           // alert(response);
            if (response.toString().trim() == "200") {
               // alert("OKAY");
                //alert(succ.C_id.toString().length);
                displaying.style = "display:normal";
                angular.forEach($scope.ListSuccursales, function (value, key) {

                    if (value.id == succ.C_id) {
                        value.name = succ.C_name;
                        value.address = succ.C_adresse;
                        value.phone = succ.C_phone;
                        delete $scope.ListSuccursales[key];
                        $scope.ListSuccursales[key] = value;
                        //console.log(JSON.stringify($scope.ListSuccursales));

                    }

                })
            } else {
                console.log("RESPONSE UPDATE :", response);
            }
        }, function (error) {
            alert("Error :" + error);
        });
    }
    $scope.selectedDepartement = function () {
        // alert(cbodep);
    }
    $scope.changeDepartement= function () {
        // alert(document.querySelector("#cbo_depart").value);
    }

    var cboDepart = document.querySelector("#cbo_depart");
    if (cboDepart!=null) {
        cboDepart.onchange = function () {
            //var departDisabled = document.querySelector('#depDisabled');
            var index = document.querySelector("#cbo_depart").selectedIndex;
            $scope.id_depart = $scope.ListDepartementBySucc[index].id_depart;
            $scope.id_succ = $scope.ListDepartementBySucc[index].code_Succ;
            var deapartSaved = document.querySelector('#depSave');

            deapartSaved.placeholder = document.querySelector("#cbo_depart").value;
        }
    }
   
    $scope.updateDepartement = function () {
        // alert("OK");
        var departement = {
            "C_id":$scope.id_depart,
            "C_id_depart": $scope.updatedep,
            "C_id_succ": $scope.id_succ
        }
        //  console.log(JSON.stringify(departement));
        FactoryHome.setUpdateDepartement(departement).then(function (response) {
            if (response=="200") {
                document.querySelector("#alertdep").style = "display:normal";
                angular.forEach($scope.ListDepartementBySucc, function (value, key) {
                    if (value.id_depart==departement.C_id) {
                        value.code_depart = departement.C_id_depart;
                        delete $scope.ListDepartementBySucc[key];
                        $scope.ListDepartementBySucc[key] = value;
                    }
                });
            }
        }, function (error) {
            console.log(error)
        });
    }
    $scope.getSuccursales = function () {
        if ($scope.roleUser != "user") {
            //  console.log("List Succursale Run : UserRun");

            FactoryHome.getListSuccursales().then(function (response) {
                $scope.ListSuccursales = response;
                console.log("Succursales :", JSON.stringify($scope.ListSuccursales));
            }, function (error) {
                console.log(error);
            });
        } else {
            //console.log("List Succursale Run :", $scope.roleUser);

            FactoryHome.getListSuccursales().then(function (response) {
                $scope.ListSuccursales = response;
                console.log("Succursales :", JSON.stringify($scope.ListSuccursales));
            }, function (error) {
             console.log(error);
            });
        }
        
    }
    $scope.getListDepartements = function () {
        FactoryHome.getListDepartements().then(function (response) {
            $scope.getAllsDepartments = response;
            $scope.getAllsDepartments = response;
            console.log("Departments :", JSON.stringify(response));
        }, function (error) {

        });
        if ($scope.roleUser == "user") {
            FactoryHome.getListDepartement($scope.idAuthSucc).then(function (response) {
                $scope.ListDepartementBySucc = response;
                $scope.ListDepartementBySuccA = response;
                $scope.ListDepartementBySucc2 = response; 
            }, function (error) {

            });
         

        }
        
    }
    $scope.getOneSuccursale = function (data) {
       
        var displaying = document.querySelector("#alertSucc");
        displaying.style = "display:none";
        var btnModal = document.querySelector('#modal');
        $scope.ListSuccursales.forEach(function (elt) {
            if (elt.id == data) {
                // alert(elt.name);
                document.querySelector('#id_succ').value = elt.id;
                document.querySelector('#myModalLabel').innerHTML = elt.name;
                document.querySelector('#id_Company').value = elt.idCompany;
                document.querySelector('#name_succ').value = elt.name;
                document.querySelector('#phone_succ').value = elt.phone;
                document.querySelector('#address_succ').value = elt.address;

                btnModal.click();
               
            }
        })
        
    }

    $scope.getOneSuccursaleByDepartement = function (data) {
        var departDisabled = document.querySelector('#depDisabled');
        var deapartSaved = document.querySelector('#depSave');
        //alert(data);
        var btnModal = document.querySelector('#modal');
        var titleEntreprise = document.querySelector('#myModalLabel');
        $scope.ListSuccursales.forEach(function (elt) {
            if (elt.id==data) {
                titleEntreprise.innerHTML = elt.name;
            }
        })
        FactoryHome.getListDepartement(data).then(function (response) {

            $scope.ListDepartementBySucc = response;
            if ($scope.ListDepartementBySucc.length>0) {
                var departDisabled = document.querySelector('#depDisabled');
                var deapartSaved = document.querySelector('#depSave');

                departDisabled.style = "display:none";
                deapartSaved.style = "display:normal";
                deapartSaved.value = "";
                deapartSaved.placeholder = $scope.ListDepartementBySucc[0].code_depart;
                $scope.id_depart = $scope.ListDepartementBySucc[0].id_depart;
                $scope.id_succ = $scope.ListDepartementBySucc[0].code_Succ;
                // alert($scope.id_succ);
                document.querySelector("#alertdep").style = "display:none";
            } else {
                var departDisabled = document.querySelector('#depDisabled');
                var deapartSaved = document.querySelector('#depSave');
                document.querySelector("#alertdep").style = "display:none";
                departDisabled.style = "display:normal";
                deapartSaved.style = "display:none";
            }
            btnModal.click();
        }, function (error) {

        });
        
    }
    $scope.refreshing = function () {
        document.location.reload();
    }
    $scope.employedSearch = "";
    $scope.getUrlToBase64=function(picture){
        var canvas = document.createElement('canvas');
        canvas.width = picture.width;
        canvas.height = picture.height;
        var ctx = canvas.getContext("2d");
        ctx.drawImage(picture, 0, 0);
        return canvas.toDataURL("image/png");
    }
    $scope.isBtnVisible = false;
    $scope.tabPartner = [];
    $scope.udpateEmployed = function () {
        if ($scope.maxChildEmployed>0 && $scope.maxChildPartner>0) {
            document.querySelector('#btnTransfert').click();
        } else {
            //console.log("ID EMPLOYED:", $scope.idEmployed);
            //console.log(JSON.stringify($scope.ListSuccursales));
            if ($scope.idSuccEmployed == "") {
                var findEmployed = false;
                var findPartner = false;
                angular.forEach($scope.ListSuccursales, function (succursale, keySucc) {
                    if (succursale.name == $scope.cbo_succ) {
                        $scope.idSuccursale = succursale.id;
                        angular.forEach($scope.getAllsDepartments, function (department, keyDepart) {
                            if (department.name == document.querySelector('#cbo_departEmployed').value) {
                                $scope.idDepartEmployed = department.id;
                            }
                        });

                    }


                });
                console.log("Succ Employed :", $scope.idSuccursale);
                console.log("Depart Employed :", $scope.idDepartEmployed);

            }
            if ($scope.cbo_succPart == undefined) {
                if ($scope.tabPartner.length > 0) {
                    angular.forEach($scope.ListSuccursales, function (succursale, keySucc) {
                        if ($scope.tabPartner[0].ID_Succursale == succursale.name) {

                            $scope.tabPartner[0].ID_Succursale = succursale.id;
                            angular.forEach($scope.getAllsDepartments, function (department, keyDepart) {
                                if (department.name == $scope.tabPartner[0].ID_Departement) {
                                    $scope.tabPartner[0].ID_Departement = department.id;
                                }
                            });
                        }
                    });
                    console.log("Succ Partner :", $scope.tabPartner[0].ID_Succursale);
                    console.log("Depart Partner :", $scope.tabPartner[0].ID_Departement);

                }

            }

            if ($scope.tabPartner.length > 0) {
                $scope.partner = $scope.tabPartner[0];
            } else {
                $scope.partner = {
                            name:''
                };
                $scope.partner.Childs = [];
            }
            $scope.dateEmployed =document.querySelector('#cboday').value.toString().trim() + '/' + $scope.cbomonth.toString().trim() + '/' + document.querySelector('#cboyear').value.toString().trim()
            
            try {
                $scope.partner.account_system = document.querySelector('#cboastatusPartner').value;
            } catch (e) {
                $scope.partner.account_system = ($scope.partner.account_system == "Enabled" ? "1" : "0");
            }
            $scope.Employed = {
                id: $scope.idEmployed,
                Matricule:$scope.idnumberBene,
                name: $scope.nameBene,
                sex: $scope.sexBene,
                phone: $scope.phoneBene,
                CivilStatus: document.querySelector("#cboEtatCivil").value,
                ID_Succursale: $scope.idSuccursale,
                ID_Departement: $scope.idDepartEmployed,
                datenaiss: $scope.dateEmployed,
                picture: document.querySelector('#pictureEmployed').src,
                childs: $scope.tabChild,
                partner: $scope.partner,
                status: false,
                account_system: ($scope.accountEmployee == "Enabled" ? "1" : "0")


            };
          
            console.log(JSON.stringify($scope.Employed));
            if ($scope.partnerStatus == true) {
                $scope.Employed.status = true;
            }
            
            // console.log("TAB CHILDS :", JSON.stringify($scope.Employed.childs));
            var base64 = $scope.getUrlToBase64(document.querySelector('#pictureEmployed'));
            // console.log("Base64", base64);
            console.log(JSON.stringify('New info partner:',$scope.partner))
            FactoryHome.setUpdateEmployed($scope.Employed).then(function (response) {
                if (response.toString().trim() == "200") {
                    $scope.isModify = true;
                }
                console.log("RESPONSE :", response);

            }, function (error) {
                console.log("Error :", error);
            });
            
        }
        console.log("Object :", JSON.stringify($scope.Employed.picture));
    }
    $scope.isModify = false;
    $scope.btnAddPartner = true;
    $scope.EtatCivil = function () {
        if ($scope.cboEtatCivil != "Single") {
            $scope.btnAddPartner = true;
        } else {
            $scope.btnAddPartner = false;
        }
    }
    $scope.maxChildEmployed = 0;
    $scope.maxChildPartner = 0;
    $scope.isVisibilityTransfert = false;

    $scope.transfertPartner = function () {
        angular.forEach($scope.tabChild, function (value, key) {
           
            value.parent = $scope.EmployedSelected.partner.id;
            $scope.EmployedSelected.partner.Childs.push(value);
           
            //delete $scope.tabChild[key];
        });
        $scope.tabChild = [];
        $scope.isVisibilityTransfert = true;


       
    }
    $scope.transfertEmployed = function () {
        //     console.log("Partner Childs:",JSON.stringify($scope.EmployedSelected.partner.Childs));
        angular.forEach($scope.EmployedSelected.partner.Childs, function (value, key) {
           
            value.parent = $scope.employedSearch.id;
            $scope.tabChild.push(value);
            //delete $scope.tabChild[key];
        });
        $scope.EmployedSelected.partner.Childs = [];
        console.log("TabChilds Update:", JSON.stringify($scope.tabChild));
        $scope.isVisibilityTransfert = true;

    }
    $scope.transfertValidation = function () {
      
        $scope.maxChildEmployed = $scope.tabChild.length;
        $scope.maxChildPartner = $scope.EmployedSelected.partner.Childs.length;
        //alert("Length Employed :" + $scope.maxChildEmployed);
        //alert("Length Partner :" + $scope.maxChildPartner);
        document.querySelector('#btnTransfert').click();
    }
    $scope.isAccountStatus = false;
    $scope.selectedEmployed = function (employed) {
       
        $scope.isBtnVisible = true;
        $scope.isAccountStatus = true;
        try {
            document.querySelector('#cboastatus').value = (employed.account_system == "Disabled" ? "0" : "1");
        } catch (e) {

        }
        if (employed.account_system == "Disabled") {
            alert('This employee is disabled');
            try {
                 document.querySelector('#btnUpdateEmployed').disabled = true;
            document.querySelector('#btnaddPartner').disabled = true;
            document.querySelector('#btnaddPartner2').disabled = true;
            document.querySelector('#btnaddSearchPartner').disabled = true;
            document.querySelector('#btnChild').disabled = true;
            $scope.linkDisabled = true;
            
            } catch (e) {

            }
           
            
            
        } else {
            try {
                 document.querySelector('#btnUpdateEmployed').disabled = false;
            document.querySelector('#btnaddPartner').disabled = false;
            document.querySelector('#btnaddPartner2').disabled = false;
            document.querySelector('#btnaddSearchPartner').disabled = false;
            document.querySelector('#btnChild').disabled = false;
            $scope.linkDisabled = false;
            
            } catch (e) {

            }

           
        }
        
        var dateNaiss = employed.datenaiss.toString().split('/');
        //console.log("$scope.isBtnVisible =", $scope.isBtnVisible);
        $scope.employedSearch = employed;
        $scope.idEmployed = employed.id;
         console.log(JSON.stringify(employed));
        console.log("ID EMPL:", employed.ID_Succursale);
        angular.forEach($scope.ListSuccursales, function (value, key) {
            if (value.name == employed.ID_Succursale) {
                $scope.idCompany = value.id;
            }
        });
        $scope.listTest = [];
        var xy = 0;
        var cbcodepart = document.querySelector('#cbo_departEmployed');

        angular.forEach($scope.getAllsDepartments, function (value, key) {
            if (value.idSucc == $scope.idCompany) {
                var object = {
                    "code_Succ": $scope.idCompany,
                    "id_depart": value.id,
                    "code_depart":value.name
                }
                if (value.name==employed.ID_Departement) {
                    xy = key;
                }
                $scope.listTest.push(object);
                var option = document.createElement('option');
                option.text =value.name;
                cbcodepart.add(option);
             
            }

        });
        cbcodepart.selectedIndex = xy+1;
        $scope.ListDepartementBySuccA = $scope.listTest;
        $scope.cbomonth = dateNaiss[1];
        var cbcod = document.querySelector('#cboday');
        var cby = document.querySelector('#cboyear');
   
        for (var i = 1; i <= 31; i++) {
            var t = i.toString();
            (t.length == 1) ? t = "0" + i : t;
            if (dateNaiss[0] == t) {
          
                cbcod.selectedIndex = t;
         
            } 
        }
        for (var i = 0; i <cby.length; i++) {
          
           
            if (dateNaiss[2] == cby[i].text) {
               
                cby.selectedIndex = i;
            }

        }
        $scope.accountEmployee = employed.account_system;
        $scope.idnumberBene = employed.Matricule;
        $scope.nameBene = employed.name;
        $scope.phoneBene = employed.phone;
        $scope.sexBene = employed.sex;
        $scope.cbo_succ = employed.ID_Succursale;
        $scope.cboEtatCivil = employed.CivilStatus;
        //$scope.cbo_depart = employed.ID_Departement;
        document.querySelector('#pictureEmployed').src =FactoryHome.rootServer+ "Images/"+employed.picture;
        // console.log("IDDEP:" + employed.ID_Departement.length + ", IDDEP OPT:" + option.text.length);
        document.querySelector('#btnCloseIt').click();
        console.log(FactoryHome.rootServer);
        $scope.tabChild = employed.Childs;
        $scope.tabPartner = [];
        console.log("Partener Object:",JSON.stringify(employed.partner))
        $scope.maxChildEmployed = employed.Childs.length;
        if (employed.partner!=null) {
            $scope.tabPartner.push(employed.partner);
            $scope.maxChildPartner = employed.partner.Childs.length;

        }
        document.querySelector('#cboEtatCivil').selectedIndex = 2;
        $scope.IsSearch = true;
        document.querySelector('#btnaddSearchPartner').style = "float:right;margin-top:-.5em;display:normal;";
        $scope.EmployedSelected = employed;
       
        //alert("Size Employed:" + employed.Childs.length);
        //alert("Size Partner:" + employed.partner.Childs.length);
        
        if ($scope.maxChildEmployed > 0 && $scope.maxChildPartner > 0) {
            document.querySelector('#btnTransfert').click();
        }
    }

    // BENEFICIAIRE PROCESS
    $scope.seachEmployed = function (categ) {
        if (categ == "Master") {
            var parametre = $scope.nameEmployed.trim();
            FactoryHome.getListEmployed(parametre).then(function (response) {
                $scope.tabSearchEmployed = response;

                 console.log(JSON.stringify($scope.tabSearchEmployed));
            }, function (error) {
                console.log(error);
            })
        }
    }

    // ROUTERS PROCESS

    $scope.actionRouters = function () {
        // alert($scope.category);
        switch ($scope.category.toString().trim()) {
            case 'Employee':
                
                document.location.href = "#/employed";
                break;
            case 'Enfant':
                document.location.href = "#/children";
                document.querySelector('#category').value = "Enfant";
                break;
            case 'Conjoint':
                document.location.href = "#/partner";
                //document.querySelector('#category').value = "Enfant";
                break;
            case 'Visitor':
                document.location.href = "#/visitor";

                break;
           
            default:

        }
    }
    $scope.isVisibility = false;
    $scope.controlsSucc = function (categ) {
        switch (categ) {
            case 'yes':
                if ($scope.roleUser="user") {
                    $scope.cbo_succPart2 = $scope.idAuthSucc;
                }
                document.querySelector('#group-succ').style = "display:normal";
                document.querySelector('#group-depart').style = "display:normal";
                document.querySelector('#group-mat').style = "display:normal";
                break;
            case'no':
                document.querySelector('#group-succ').style = "display:none";
                document.querySelector('#group-depart').style = "display:none";
                if ($scope.roleUser!="user") {
                    $scope.cbo_succPart = undefined;
                    $scope.cbo_departPart = undefined;
                } else {
                    $scope.cbo_departPart = undefined;
                    $scope.cbo_succPart2 = undefined;

                }
                break;
            case 'yes2':
                document.querySelector('#group-succ2').style = "display:normal";
                document.querySelector('#group-depart2').style = "display:normal";
                break;
            case 'no2':
                document.querySelector('#group-succ2').style = "display:none";
                document.querySelector('#group-depart2').style = "display:none";
                $scope.cbo_succ = undefined;
                $scope.cbo_departMaster = undefined
                break;

        }
    }
    $scope.tabChild = [];
    $scope.pictureChild = "";
    $scope.isEditable = false;
    $scope.indexTabChild = 0;
    $scope.isVisible = false;
    // ADD CHILD
    $scope.ClearChild = function () {
        $scope.titleChild = "Add Child";
        $scope.nameChild = "";
        $scope.dateChild = "";
        $scope.sexChild = "--Select Sex--";
        $scope.statusChild = undefined;
        document.querySelector('#pict_child').src = $scope.imageBasic
    }
    $scope.AddChild = function (categ) {

        if ($scope.nameChild != "" && $scope.sexChild != "" && $scope.pictureChild != "") {
            //  alert($scope.idEmployed);
            $scope.dateChild = document.querySelector('#cbodayChild').value.toString().trim() + '/' + document.querySelector('#cbomonthChild').value.toString().trim() + '/' + document.querySelector('#cboyearChild').value.toString().trim();
            var dtNow=new Date();
            var dtChild = $scope.dateChild.toString().split('/');
            var dt = new Date(parseInt(dtChild[2]), parseInt(dtChild[1])-1, parseInt(dtChild[0]));
            if (dt < dtNow) {
                if ($scope.isEditable == false) {
                    $scope.child = {
                        name: $scope.nameChild,
                        datenais: $scope.dateChild,
                        sex: $scope.sexChild,
                        picture: $scope.pictureChild,
                        parent: $scope.idEmployed,
                        status: $scope.statusChild,
                        account_system:$scope.statusChild2
                    };

                    $scope.tabChild.push($scope.child);
                    //  console.log(JSON.stringify($scope.tabChild));

                    switch (categ) {
                        case 'new':
                       
                            $scope.nameChild = "";
                            $scope.dateChild = "";
                            $scope.sexChild = "--Select Sex--";
                            document.querySelector('#pict_child').src = $scope.imageBasic;
                            $scope.statusChild = undefined;
                            $scope.cbodayChild = undefined;
                            $scope.cbomonthChild = undefined;
                            $scope.cboyearChild = undefined;
                            //document.querySelector('#cbodayChild').selectedIndex = 0;
                            //document.querySelector('#cbomonthChild').selectedIndex = 0;
                            //document.querySelector('#cboyearChild').selectedIndex = 0;
                            break;
                        default:
                            document.querySelector('#btnFermer').click();
                            $scope.nameChild = "";
                            $scope.dateChild = "";
                            $scope.sexChild = "--Select Sex--";
                            document.querySelector('#pict_child').src = $scope.imageBasic;
                            $scope.statusChild = undefined;
                            break;

                    }
                    $timeout(function () {
                        $scope.isVisible = true;
                    }, 3000);
                    $scope.isVisible = false;

                } else {
                    angular.forEach($scope.tabChild, function (value, key) {
                        if (key == $scope.indexTabChild) {
                            $scope.child = {
                                id: value.id,
                                name: $scope.nameChild,
                                datenais: $scope.dateChild,
                                sex: $scope.sexChild,
                                picture: value.picture,
                                status: $scope.statusChild,
                                parent: $scope.idEmployed,
                                account_system: $scope.statusChild2
                            };
                            if ($scope.pictureChild!=undefined) {
                                $scope.child.picture = $scope.pictureChild;
                            }

                            delete $scope.tabChild[key];
                            $scope.tabChild[key] = $scope.child;
                            console.log("Child found", $scope.child);
                            switch (categ) {
                                case 'new':
                                    $scope.nameChild = "";
                                    $scope.dateChild = "";
                                    $scope.sexChild = "--Select Sex--";
                                    document.querySelector('#pict_child').src = $scope.imageBasic;
                                    break;
                                default:
                                    document.querySelector('#btnFermer').click();
                                    $scope.nameChild = "";
                                    $scope.dateChild = "";
                                    $scope.sexChild = "--Select Sex--";
                                    document.querySelector('#pict_child').src = $scope.imageBasic;
                                    break;

                            }
                        }
                    });
                    $scope.isEditable = false;
                }

            } else {
                alert('Check your date');
            }
          
           


        } else {
             alert("The fields is empty");
        }
    }

    $scope.listStatus = ["active","inactive"];
    $scope.listStatusChild =new Array();
    $scope.editChild = function (children) {
        $scope.titleChild = "Update Child";
        $scope.indexTabChild = $scope.tabChild.indexOf(children);
        $scope.isEditable = true;
        $scope.nameChild = children.name;
        var dateNaissChild = children.datenais.split('/');
        $scope.sexChild = children.sex;
        $scope.cbomonthChild = dateNaissChild[1];
        $scope.statusChild2 = children.account_system == "Enabled" ? "1" : "0";
        
        
      
        var cbcod = document.querySelector('#cbodayChild');
        var cby = document.querySelector('#cboyearChild');

        for (var i = 1; i <= 31; i++) {
            var t = i.toString();
            (t.length == 1) ? t = "0" + i : t;
            if (dateNaissChild[0] == t) {
                cbcod.selectedIndex = t;
            }
        }
        for (var i = 0; i < cby.length; i++) {


            if (dateNaissChild[2] == cby[i].text) {

                cby.selectedIndex = i;
            }

        }

        switch (children.status.trim()) {
            case 'active':
                $scope.listStatus = ["active", "inactive"];
                $scope.listStatusChild.push($scope.listStatus[0]);
                $scope.listStatusChild.push($scope.listStatus[1]);
                $scope.listStatus = $scope.listStatusChild;

                break;
            case 'inactive':
                $scope.listStatus = ["active", "inactive"];
                $scope.listStatusChild.push($scope.listStatus[0]);
                $scope.listStatusChild.push($scope.listStatus[1]);
                $scope.listStatus = $scope.listStatusChild;

                break;
            default:
                $scope.listStatus = ["active", "inactive"];
                $scope.listStatusChild.push($scope.listStatus[1]);
                $scope.listStatusChild.push($scope.listStatus[0]);
                $scope.listStatus = $scope.listStatusChild;

                break;

        }
        $scope.statusChild = children.status.trim();
        //if ($scope.statusChild==$scope.listStatus[0]) {
        //    alert('Equals');
        //} else {
        //    alert("SCOPE:" + $scope.statusChild.length + ",TAB:" + $scope.listStatus[0]);
        //}
       
        if ($scope.employedSearch=="") {
          //  document.querySelector('#pict_child').src=children.picture;
        } else {
            document.querySelector('#pict_child').src = "../../Images/" + children.picture;
            $scope.pictureChild = "../../Images/" + children.picture;

        }
        if ($scope.pictureChild != undefined) {
            document.querySelector('#pict_child').src = $scope.pictureChild;
        } else {
            //document.querySelector('#pict_child').src = "../../Images/" + children.picture;
           //$scope.pictureChild = "../../Images/" + children.picture;
        }
        // console.log(children.picture);
        document.querySelector('#btnChild2').click();
    }


    $scope.browserPictureChild = function () {
        //  alert("Employed");
        var pict_child = document.querySelector('#pict_child');
        //   alert("OK");
        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var fReader = new FileReader();

        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            $scope.pictureChild = fReader.result;
            pict_child.src = fReader.result;

        }
    }

    $scope.browserPicturePartner = function () {
        var picturePartner = document.querySelector('#picturePartner');
        //   alert("OK");
        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var fReader = new FileReader();

        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        // var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            $scope.picturePartner = fReader.result;
            picturePartner.src = fReader.result;

        }
    }

    $scope.selectedEmployedConjoint = function (employed) {
        
        $scope.Partner = {
            id:employed.id,
            name: employed.name,
            sex: employed.sex,
            phone: employed.phone,
            datenaiss: employed.datenais,
            ID_Succursale: employed.ID_Succursale,
            ID_Departement: employed.ID_Departement,
            picture: employed.picture,
            childs: employed.Childs,
            partner: employed.partner
        };
        //alert($scope.Partner.childs.length);
        //alert($scope.tabChild.length);
        $scope.EmployedSelected = {
            name: $scope.employedSearch.name,
            partner: {
                name: employed.name,
                Childs: employed.Childs
            }
        };
        $scope.maxChildEmployed = $scope.tabChild.length;
        $scope.maxChildPartner = $scope.Partner.childs.length;
        if ($scope.tabPartner.length==0) {
            $scope.tabPartner.push($scope.Partner);
        } else {
            $scope.tabPartner[0]=$scope.Partner;
        }
        // console.log("Value Visibility :", $scope.isBtnVisible);
        console.log("PART:", JSON.stringify($scope.Partner));
        document.querySelector('#btnCloseIt2').click();
        if ($scope.maxChildEmployed > 0 && $scope.maxChildPartner > 0) {
            document.querySelector('#btnTransfert').click();
        }
    }
    $scope.seachEmployedConjoint = function (categ) {
        if (categ=='Master') {
            var parameter = $scope.nameEmployedConjoint.trim();
            // console.log(parameter.length);
            FactoryHome.getConjointList(parameter).then(function (response) {
                $scope.tabSearchEmployedConjoint = response;
                //  $scope.title = "Employé(es) trouvé(s) ";
                // $scope.rowsConjoints = response.length;
                console.log(JSON.stringify(response));
            }, function (error) {

            });
        } else {

        }
    }
    $scope.Partner = {};
    //ADD EMPLOYED
    $scope.clearAddPartner = function () {
        if ($scope.isClear==true) {
            $scope.namePartner = undefined;
            $scope.phonePartner = undefined;
            $scope.sexPartner = undefined;
            $scope.datenaisPartner = undefined;
            $scope.controlsSucc('no');
            document.querySelector('#optionsRadios2').checked = "true";
            
        }
    }
    $scope.base = "BEGIN";
    $scope.fBase = function () {
        console.log($scope.tbase);
    }

    $scope.isClear = false;
    $scope.editPartner = function (conjoint) {
       
        $scope.titleConjoint = "Modify Partner";
        if ($scope.IsSearch==true) {
            $scope.namePartner = conjoint.name;
            $scope.sexPartner = conjoint.sex;
            $scope.phonePartner = conjoint.phone;
            $scope.datenaisPartner = conjoint.datenaiss;
            $scope.account_system = conjoint.account_system;
           
            document.querySelector('#picturePartner').src = "../../Images/" + conjoint.picture;
            $scope.idnumberSpouse = conjoint.Matricule;
            var dateNaiss3 = conjoint.datenaiss.toString().trim().split('/');
        
            $scope.cbomonthSpouse = dateNaiss3[1];
            var cbcod3 = document.querySelector('#cbodaySpouse');
            var cby3 = document.querySelector('#cboyearSpouse');

            for (var i = 1; i <= 31; i++) {
                var t = i.toString();
                (t.length == 1) ? t = "0" + i : t;
                if (dateNaiss3[0] == t) {
                    cbcod3.selectedIndex = t;
                }
            }
            for (var i = 0; i < cby3.length; i++) {


                if (dateNaiss3[2] == cby3[i].text) {

                    cby3.selectedIndex = i;
                }

            }
            console.log("Partner Editable :", JSON.stringify(conjoint));
            console.log("Image :", conjoint.picture);
            $scope.nameSucc = "";
            if ($scope.roleUser=="user") {
                angular.forEach($scope.ListSuccursales, function (value, key) {
                    if (value.name==conjoint.ID_Succursale) {
                        if (value.id==$scope.idAuthSucc) {
                            $scope.nameSucc = value.id;
                        }
                    }
                });
               
                    document.querySelector('#btnClose').style = "display:normal;";
                    document.querySelector('#controlChoice').style = "display:normal;";
                    $scope.cbo_succPart2 = $scope.idAuthSucc;
                    document.querySelector('#optionsRadios1').checked = "true";
                    var option = document.createElement('option');
                    option.text = conjoint.ID_Departement;
                    var cboP = document.querySelector('#cbo_departPart');
                    cboP.add(option);
                    cboP.selectedIndex = 1;
          //          $scope.controlsSucc('yes');
      
                 //   document.querySelector('#controlChoice').style = "display:none;";
                
            } else {
                if (conjoint.ID_Succursale != null) {
                    $scope.cbo_succPart = conjoint.ID_Succursale;
                    document.querySelector('#optionsRadios1').checked = "true";
                    var option = document.createElement('option');
                    option.text = conjoint.ID_Departement;
                    var cboP = document.querySelector('#cbo_departPart');
                    cboP.add(option);
                    cboP.selectedIndex = 1;
                    $scope.controlsSucc('yes');
                } else {
                    document.querySelector('#optionsRadios2').checked = "false";
                    $scope.cbo_succPart = undefined;
                    $scope.cbo_departPart = undefined;
                    $scope.controlsSucc('no');

                }
            }
         
            
        }
        $scope.isClear = false;
        document.querySelector('#btnaddPartner2').click();
    }
    $scope.addEmployed = function () {
        console.log('Data Picture:')
        if ($scope.nameBene != undefined && $scope.sexBene != undefined && $scope.cbo_succ != undefined && $scope.cbo_depart != undefined && $scope.cboday != undefined && $scope.cbomonth != undefined && $scope.cboyear != undefined) {
            if (document.querySelector('#pictureEmployed').src == $scope.imageBasic) {
                alert('Please add a picture');
            } else {
                if ($scope.roleUser != "user") {
                    $scope.cbo_succ = undefined;
                } else {
                    $scope.idSuccEmployed = $scope.cbo_succ;
                }
                console.log("Partner :", JSON.stringify($scope.Partner))
                $scope.dateEmployed = $scope.cboday.trim() + '/' + $scope.cbomonth.trim() + '/' + $scope.cboyear.trim();
                $scope.Employed = {
                    name: $scope.nameBene,
                    sex: $scope.sexBene,
                    phone: $scope.phoneBene,
                    ID_Succursale: $scope.idSuccEmployed,
                    ID_Departement: $scope.idDepartEmployed,
                    datenaiss: $scope.dateEmployed,
                    picture: document.querySelector('#pictureEmployed').src,
                    childs: $scope.tabChild,
                    partner: $scope.Partner,
                    CivilStatus: $scope.cboEtatCivil,
                    Matricule: $scope.idnumberBene

                };
                $scope.nameBene = undefined;
                $scope.sexBene = undefined;
                $scope.phoneBene = "";
                $scope.idnumberBene = undefined;
                $scope.cbo_depart = undefined;
                $scope.dateEmployed = undefined;
                $scope.tabChild = [];
                $scope.Partner = {};
                $scope.tabPartner = [];
                $scope.cboEtatCivil = undefined;
                document.querySelector('#pictureEmployed').src = $scope.imageBasic;
                // console.log(JSON.stringify($scope.Employed));
                document.querySelector('#alertWarnning').style = "display:normal;text-align:center";

                console.log(JSON.stringify($scope.Employed));
                FactoryHome.setEmployed($scope.Employed).then(function (response) {

                    if (response.toString().trim() == "200") {

                        document.querySelector('#alertWarnning').style = "display:none";
                        document.querySelector('#alertSuccess').style = "display:normal;text-align:center";
                        $timeout(function () {
                            document.querySelector('#alertSuccess').style = "display:none";

                        }, 3000);

                    }
                }, function (error) {
                    console.log(error);
                });
            }
          
        } else {
            alert('Field(s) is empty...');
        }
       
    }
    //ADD PARTNERS
    $scope.visibleAddPartner = false;
    $scope.EditConjoint = function () {
        
        if ($scope.namePartner.trim() != "" && $scope.sexPartner.trim() != "" && $scope.datenaisPartner.trim() != "") {
            document.querySelector('#btnClose').style = "display:none";
            $scope.conjoint = $scope.tabPartner[0];
          
            $scope.conjoint.name = $scope.namePartner;
            $scope.conjoint.sex = $scope.sexPartner;
            $scope.conjoint.phone = ($scope.phonePartner==undefined?"":$scope.phonePartner);
            $scope.conjoint.datenaiss = document.querySelector('#cbodaySpouse').value.toString().trim() + '/' + document.querySelector('#cbomonthSpouse').value.toString().trim() + '/' + document.querySelector('#cboyearSpouse').value.toString().trim();
            $scope.datenaisPartner == document.querySelector('#cbodaySpouse').value.toString().trim() + '/' + document.querySelector('#cbomonthSpouse').value.toString().trim() + '/' + document.querySelector('#cboyearSpouse').value.toString().trim();
            $scope.conjoint.ID_Succursale = $scope.idSuccPartner;
            $scope.conjoint.ID_Departement = $scope.idDepartPartner;
            //$scope.conjoint.account_system = document.querySelector('#cboastatusPartner').value;
            $scope.conjoint.Matricule = $scope.idnumberSpouse;
            if ($scope.picturePartner!=undefined) {
                $scope.conjoint.picture = $scope.picturePartner;
            } else {
               // alert('PHOTO NON DEFINI');
            }
            $scope.tabPartner[0] = $scope.conjoint
            document.querySelector('#btnaddPartner2').click();
            console.log("PARTNER :", JSON.stringify($scope.tabPartner));
        } else {
            alert("Remplissez le(s) champ(s) vide(s)....");
        }
        
    }
    $scope.cleanConjoint = function () {
        $scope.titleConjoint="Add Partner"
        $scope.visibleAddPartner = true;
        $scope.namePartner = undefined;
        $scope.sexPartner = undefined;
        $scope.phonePartner = undefined;
        $scope.datenaisPartner = undefined;
        $scope.cbo_succPart = undefined;
        $scope.cbo_departPart = undefined;
        document.querySelector('#picturePartner').src = $scope.imageBasic;
        document.querySelector('#btnClose').style = "display:normal;";
        document.querySelector('#controlChoice').style = "display:normal;";
    };
    $scope.AddPartner = function () {
    
        
        $scope.datenaisPartner = document.querySelector('#cbodaySpouse').value.toString().trim() + '/' + document.querySelector('#cbomonthSpouse').value.toString().trim() + '/' + document.querySelector('#cboyearSpouse').value.toString().trim();
        $scope.Partner = {
            id: '',
            Matricule: $scope.idnumberSpouse,
            name: $scope.namePartner,
            sex: $scope.sexPartner,
            phone: $scope.phonePartner,
            datenaiss: $scope.datenaisPartner,
            ID_Succursale: $scope.idSuccPartner,
            ID_Departement: $scope.idDepartPartner,
            picture: document.querySelector('#picturePartner').src,
            childs: [],
            partner: {},
            account_system:'1'

        };
        if ($scope.tabPartner.length==0) {
            $scope.tabPartner.push($scope.Partner);
            $scope.partnerStatus = true;
            
        } else {
            $scope.tabPartner[0] = $scope.Partner;
          

        }
        // console.log(JSON.stringify($scope.Partner));
        document.querySelector('#btnaddPartner').style="display:none;";
        document.querySelector('#btnClosePartner').click();
        // alert("LEGNTH :" + $scope.tabPartner.length);
    }
    $scope.idSuccEmployed = '';
    $scope.idDepartEmployed = '';
    $scope.idSuccPartner = '';
    $scope.idDepartPartner = '';
    // GET DEPARTMENTS BY SUCCURSAL
    $scope.getDepartementBySucc = function (categ) {
        
        console.log("category", categ);
        var index = 0;
        var data = '';
        switch (categ) {
            case 'Employed':
                index = document.querySelector('#cbo_succ').selectedIndex;
                $scope.idSuccEmployed = $scope.ListSuccursales[index].id;
                data = $scope.idSuccEmployed;
                break;
            case 'Partner':
                index = document.querySelector('#cbo_succPart').selectedIndex;
                $scope.idSuccPartner = $scope.ListSuccursales[index].id;
                data = $scope.idSuccPartner;
                break;
            case 'Partner-master':
                index = document.querySelector('#cbo_succ').selectedIndex;
                $scope.idSuccPartner = $scope.ListSuccursales[index].id;
                data = $scope.idSuccPartner;
                break;
            case 'Bcmd':
                console.log(JSON.stringify($scope.ListSuccursales));
                index = document.querySelector('#cbo_succ_BCmd').selectedIndex;
                console.log("INDEX :", index);
                $scope.idSuccBCmd = ($scope.ListSuccursales[index-1].id);
                data = $scope.idSuccBCmd;
                console.log("Id getter :", $scope.idSuccBCmd);
                break;
            case 'Bcmd2':
                console.log(JSON.stringify($scope.ListSuccursales));
                index = document.querySelector('#cbo_succ_BCmd2').selectedIndex;
                $scope.idSuccBCmd = ($scope.ListSuccursales[index - 1].id);
                data = $scope.idSuccBCmd;
                console.log("Id getter :", $scope.idSuccBCmd);
                break;
            default:
                break;
        }
       
        // alert();
       
        
        FactoryHome.getListDepartement(data).then(function (response) {
           
            switch (categ) {
                case 'Employed':
                    console.log("Id Succ Employed :", data);
                    $scope.ListDepartementBySucc = response;
                    console.log("List Succ:", JSON.stringify(response));
                    break;
                case 'Partner':
                    console.log("Id Suc Partner:", data);
                    $scope.ListDepartementBySucc2 = response;
                    console.log("List Succ:", JSON.stringify(response));
                    break;
                case 'Partner-master':
                    console.log("Id Suc Partner:", data);
                    $scope.listdepartementbysucc = response;
                    console.log("List Departement:", JSON.stringify(response));
                    break;
                case 'Bcmd':
                    console.log("Id Succ BCmd :", data);
                    $scope.listdepartementbyBCmd = response;
                    console.log("List Department :", JSON.stringify($scope.listdepartementbyBCmd));
                    break;
                case 'Bcmd2':
                    console.log("Id Succ BCmd :", data);
                    $scope.listdepartementbyBCmd2 = response;
                    console.log("List Department :", JSON.stringify($scope.listdepartementbyBCmd2));
                    break;
                default:
                    break;

            }

        }, function (error) {

        });
    }

    $scope.beneficiaireDepartement = function (categ) {
        var index = 0;
        switch (categ) {
            case 'Employed':
                if ($scope.ListDepartementBySuccA!=undefined) {
                    index = document.querySelector('#cbo_departEmployed').selectedIndex;
                    $scope.idDepartEmployed = $scope.ListDepartementBySuccA[index].id_depart;
                    console.log("Employed ID DAPERT:", $scope.idDepartEmployed);
                    console.log(JSON.stringify($scope.ListDepartementBySuccA));
                } else {
                    index = document.querySelector('#cbo_departEmployed').selectedIndex;
                    $scope.idDepartEmployed = $scope.ListDepartementBySucc[index].id_depart;
                    console.log("Employed ID DAPERT:", $scope.idDepartEmployed);
                    console.log(JSON.stringify($scope.ListDepartementBySucc));
                }
                
                
                break;
            case 'Partner':
                index = document.querySelector('#cbo_departPart').selectedIndex;
                //$scope. = $scope.ListDepartementBySucc[index].id;
                $scope.idDepartPartner = $scope.ListDepartementBySucc2[index].id_depart;
                console.log("Partner :", $scope.idDepartEmployed);
                console.log(JSON.stringify($scope.ListDepartementBySucc2));
                break;
            case 'Partner-master':
                index = document.querySelector('#cbo_departMaster').selectedIndex;
                $scope.idDepartEmployed = $scope.listdepartementbysucc[index].id_depart;
                console.log("Partner Master :", $scope.idDepartEmployed);
                console.log(JSON.stringify($scope.listdepartementbysucc));
                break;
            default:

        }
    }
    // CHILDREN PROCESS
    $scope.Children = function () {
        if ($scope.cbo_child== "Ajouter enfant(s)") {
            document.querySelector('#modalChild').click();
        } else {
            //alert($scope.cbo_child);
        }
    }
    $scope.addChildren = function () {
        
        if ($scope.nameChildren != undefined && $scope.sexChildren != undefined && $scope.datenaisChildren!=undefined) {
            $scope.userChild = {
                name: $scope.nameChildren,
                sex: $scope.sexChildren,
                datenais: $scope.datenaisChildren,
                parent: $scope.SelectParent._idparent,
                picture: document.querySelector('#pictureChilding').src
            };
            FactoryHome.setChildren($scope.userChild).then(function (response) {
                if (response == "200") {
                    $scope.message = "Enregistrement effectuée avec succès";
                    document.querySelector("#alertSucc").style = "display:normal;text-align:center;";
                    console.log("Success :", response);
                } else {
                    console.log("Middle error :", response);
                    document.querySelector("#alertSucc").style = "display:normal;text-align:center;";
                    $scope.parentFound = response;
                    $scope.message="Une prise en charge existe pour cet enfant,voulez-vous la remplacer?"
                    document.querySelector('#btnReplacePC').style = "display:normal;float:right;margin-top:-0.5em;";
                }
            }, function (error) {
                console.error("ERROR :", error);
            });
           
        } else {
            alert("Remplissez le(s) champ(s)...");
        }
    }
    $scope.seachParent = function (categ) {
        if (categ == "Master") {
            var parameter= $scope.searchText.trim();
            console.log(parameter.length);
            FactoryHome.getParent(parameter).then(function (response) {
                $scope.tabViewerParentSearch = response;
                $scope.title = "Parent trouvé(s) ";
                $scope.rowsParents = response.length;
            }, function (error) {

            });
        } else {

        }
    }
    $scope.selectedParent = function (parent) {
        document.querySelector('#viewTables').style = "display:none;";
        document.querySelector('#viewProfil').style = "display:normal;width:100%;";
        $scope.imageParentSelected =parent._photo;
        $scope.nameParentSelected =parent._name;

    }
    $scope.returnTable = function () {
        document.querySelector('#viewTables').style = "display:normal;width:100%;";
        document.querySelector('#viewProfil').style = "display:none";
        console.log("Clickable");
    }
    $scope.tabParentSender = [];
    $scope.AddParent = function () {
        if ($scope.tabParentSender.length==0) {
            $scope.tabParentSender.push($scope.SelectParent);
            document.querySelector('#btnClose').click();
        } else {
            $scope.tabParentSender[0]=$scope.SelectParent;
            document.querySelector('#btnClose').click();

        }
        console.log(JSON.stringify($scope.tabParentSender));
    }
    $scope.choiceParent = function (parent) {
        $scope.SelectParent = $scope.tabViewerParentSearch[$scope.tabViewerParentSearch.indexOf(parent)];
        console.log(JSON.stringify($scope.SelectParent));
    }
    $scope.ReplaceParent = function () {
        FactoryHome.setUpdateChildren($scope.userChild).then(function (response) {
            console.log(response);
        }, function (error) {
            console.log("Error :", error);
        });
    }
    // PROCESS CONJOINT
    $scope.seachConjoint = function (categ) {
        switch (categ) {
            case 'Master':
                var parameter = $scope.searchTextconj.trim();
                console.log(parameter.length);
                FactoryHome.getConjointList(parameter).then(function (response) {
                    $scope.tabViewerConjointSearch = response;
                    $scope.title = "Employé(es) trouvé(s) ";
                    $scope.rowsConjoints = response.length;
                    console.log(JSON.stringify(response));
                }, function (error) {

                });
                break;
            default:

        }
    }
    $scope.choiceConjoint = function (partner) {
        $scope.SelectConjoint = $scope.tabViewerConjointSearch[$scope.tabViewerConjointSearch.indexOf(partner)];
        console.log(JSON.stringify($scope.SelectConjoint));
    }
    $scope.tabConjointSender = [];
    $scope.addSenderConjoint = function () {
        if ($scope.tabConjointSender.length==0) {
            $scope.tabConjointSender.push($scope.SelectConjoint);
            document.querySelector('#btnClose').click();
        } else {
            $scope.tabConjointSender[0] = $scope.SelectConjoint;
            document.querySelector('#btnClose').click();

        }
        console.log(JSON.stringify($scope.tabConjointSender));
    }
    $scope.addConjoint = function () {
       
        if ($scope.nameConjoint != undefined && $scope.sexConjoint != undefined && $scope.datenaisConjoint != null && document.querySelector('#pict_conjoint').src!=$scope.imageBasic) {
            $scope.conjoint = {
                name: $scope.nameConjoint,
                sex: $scope.sexConjoint,
                phone: $scope.phoneConjoint,
                //,
                datenais: $scope.datenaisConjoint,
                idSuccursale: $scope.idSuccPartner,
                idDepartement: $scope.idDepartEmployed,
                conjoint: $scope.SelectConjoint._id,
                picture: document.querySelector('#pict_conjoint').src
            };
            //console.log(JSON.stringify($scope.conjoint));
            FactoryHome.setConjoint($scope.conjoint).then(function (response) {
                if (response == "200") {
                    $scope.nameConjoint = "";
                    $scope.sexConjoint = "";
                    $scope.phoneConjoint = "";
                    $scope.datenaisConjoint = "";
                    $scope.cbo_succ = "";
                    $scope.tabConjointSender = [];
                    $scope.cbo_departMaster = "";
                    $scope.searchTextconj = "";
                    $scope.tabViewerConjointSearch = [];
                    document.querySelector('#pict_conjoint').src = $scope.imageBasic;
                    document.querySelector('#alertSucc').style = "display:normal;text-align:center;";
                    console.log("CODE HTTP:", response);
                    $timeout(function () {
                        document.querySelector('#alertSucc').style = "display:none";

                    },3000)

                } else {
                    console.log(response);
                }
            }, function (error) {
                console.log("CODE ERROR:", error);
            });
        } else {
            alert("Remplissez le(s) champ(s)....");
        }
    }
    $scope.browserPictureConjoint = function () {
        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var fReader = new FileReader();

        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        //var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            //hidden.value = fReader.result;
            document.querySelector('#pict_conjoint').src = fReader.result;

        }
    }

    //ADD VISITOR
    $scope.browserPictureVisitor = function () {
        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var fReader = new FileReader();

        browser.onchange = function () {
            fReader.readAsDataURL(browser.files[0]);
        }
        //var hidden = document.querySelector('#Img');
        fReader.onloadend = function () {
            //hidden.value = fReader.result;
            document.querySelector('#pict_visitor').src = fReader.result;

        }
    }


    $scope.addVisitor = function () {
        console.log(document.querySelector('#idVisitor').value);
        if ($scope.nameVisitor != undefined && $scope.sexVisitor != undefined && $scope.phoneVisitor != undefined) {
            if (document.querySelector('#pict_visitor').src == $scope.imageBasic) {
                alert('Please add a picture')
            } else {
                $scope.userVisitor = {
                    name: $scope.nameVisitor,
                    sex: $scope.sexVisitor,
                    phone: $scope.phoneVisitor,
                    picture: document.querySelector('#pict_visitor').src,
                    idVisitor: document.querySelector('#idVisitor').value,
                    Cause: $scope.causeVisitor,
                    CompanyVisitor: $scope.companyVisitor
                };
                FactoryHome.setVisitor($scope.userVisitor).then(function (response) {
                    if (response == "200") {
                        $scope.nameVisitor = "";
                        $scope.sexVisitor = "";
                        $scope.phoneVisitor = "";
                        $scope.causeVisitor = "";
                        $scope.companyVisitor = "";
                        document.querySelector('#pict_visitor').src = $scope.imageBasic;
                        document.querySelector('#alertSucc').style = "display:normal;text-align:center;";
                        $timeout(function () {
                            document.querySelector('#alertSucc').style = "display:none;";

                        }, 3000);
                        console.log("CODE HTTP", response);
                    } else {
                        console.log("OTHER CODE", response);
                    }
                }, function (error) {
                    console.log("ERROR", error);
                });
            }
           
        } else {
            alert("Fields are empty...");
        }
    }
    $scope.isVisible = false;
    
    //HEALTHS
    $scope.getHealth = function (health) {
        var index = $scope.tabListHealths.indexOf(health);
        document.querySelector('#btnModif').click();
        $scope.nameHealth = health.C_name;
        $scope.adresseHealth = health.adresse;
        $scope.phoneHealth = health.C_phone;
        $scope.idCentre = health.C_id_centre;
        $scope.isVisible = false;
    }
    $scope.ModifiyHospital = function () {
     
        var object = {
            C_id_centre: $scope.idCentre,
            C_name: $scope.nameHealth,
            C_phone: $scope.phoneHealth,
            adresse: $scope.adresseHealth,
            C_status_system: document.querySelector('#statusAccount').value
        };
        FactoryHome.setHospitalNewer(object).then(function (response) {
            console.log(response)
            if (response.toString().trim() == "200") {
                //angular.forEach($scope.tabListHealths, function (value, key) {
                //    if (value.C_id_centre==$scope.idCentre) {
                //        delete $scope.tabListHealths[key];
                //        $scope.tabListHealths[key] = object;
                //    }
                //});
                $scope.isVisible = true;
                window.setTimeout(function () {
                    window.location.reload();
                },2000)
            }
        }, function (error) {
            console.log(error);
        });
    }

    $scope.isVisibleTableEmployed = false;
    $scope.isVisibleTablePartner = false;
    $scope.isVisibleTableChild = false;
    $scope.viewMarital = false;
    $scope.cboVoucherCasualCompany = true;
    var routes = document.location;
    var indexerLink=routes.toString().lastIndexOf('/');
    var routeViewer = routes.toString().substring((indexerLink + 1));
    $scope.loadHealth = false;
    console.log(routeViewer);
    switch (routeViewer) {
        case 'addlogger':
            document.querySelector('#username').value = "";
            document.querySelector('#pwd1').value = "";

            break;
        case 'SearchSuccursale':
            //console.log("Linker :", routeViewer);
            $scope.getSuccursales();
            $scope.getListDepartements();
            break;
        case 'SearchDepartement':
            $scope.getSuccursales();
            $scope.getListDepartements();
            break;
        case 'employed':
            $scope.category = "Employee";
             $scope.arrayDay = [];
            for (var i = 1; i <=31; i++) {
                var value = i.toString();
                if (value.length==1) {
                    value = "0" + value;
                }
                $scope.arrayDay.push(value);
              

            }
            $scope.arrayYear = [];
            for (var i = 1865; i <= new Date().getFullYear()-18; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayYear.push(value);


            }

            $scope.arrayYearChild = [];
            var fixeDate = new Date().getFullYear() - 150;
            for (var i = fixeDate; i <= new Date().getFullYear(); i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayYearChild.push(value);


            }

            if ($scope.roleUser == "user") {
                $scope.cbo_succ = $scope.idAuthSucc;
                
                // alert($scope.idAuthSucc);
            }
            $scope.getSuccursales();
            $scope.getListDepartements();

            break;
        case 'children':
            console.log("Enfant");
            $scope.category = "Enfant";

            break;
        case 'partner':
            console.log("Conjoint");
            $scope.category = "Conjoint";
            $scope.getSuccursales();
            $scope.getListDepartements();
            break;
        case 'visitor':
            console.log('Visiteur');
            $scope.category = "Visitor";
            break;

        case 'SearchHealths':
            console.log("Routing", routeViewer);
            FactoryHome.getListHealthsTab().then(function (response) {
                $scope.tabListHealths = response;
            }, function (error) {
                console.log("Error", error);
            });
            break;
        case 'employedcommand':
           // var cbday = document.querySelector('#cbodayDeb');
            //var opt = document.createElement('option');
            $scope.typeUser = "Employé";
            $scope.arrayDay = [];
            for (var i = 1; i <= 31; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayDay.push(value);

              //  opt.text = value;
               // cbday.add(opt);
            }
            $scope.arrayYear = [];
            for (var i = 2010; i <= new Date().getFullYear() ; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayYear.push(value);


            }
            $scope.viewMarital = true;
            $scope.titleBonCommand = "Employee ";
            $scope.categoryClient = "Employee";
            //  $scope.titleBonCommand = "Bon de commande pour employé(e)";
            $scope.getSuccursales();
            $scope.getListDepartements();
            $scope.cboCategBeneficiaire = "Employee";
            if ( $scope.loadHealth ==false) {
                $scope.loadHealth = true;
            
                FactoryHome.getListHealths().then(function (response) {
                    $scope.tabListHealths = response;
                    console.log("LIST HOSPITALS:", JSON.stringify($scope.tabListHealths));

                }, function (error) {
                    console.log("Error", error);
                });
            }
            $scope.isVisibleTableEmployed = true;
            $scope.isVisibleTablePartner = false;
            $scope.isVisibleTableChild = false;
            //alert("Lama Hornel");

            break;
        case 'partnercommand':
            $scope.typeUser = "Epoux(se)";
            $scope.arrayDay = [];
            for (var i = 1; i <= 31; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayDay.push(value);


            }
            $scope.arrayYear = [];
            for (var i = 2010; i <= new Date().getFullYear() ; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayYear.push(value);


            }
            $scope.viewMarital = true;

            $scope.titleBonCommand = "Spouse";
            $scope.categoryClient = "Spouse";
            // $scope.titleBonCommand = "Bon de commande pour employé(e)";
            $scope.cboCategBeneficiaire = "Spouse";
            if ($scope.loadHealth == false) {
                $scope.loadHealth = true;

                FactoryHome.getListHealths().then(function (response) {
                    $scope.tabListHealths = response;
                    console.log("LIST HOSPITALS:", JSON.stringify($scope.tabListHealths));

                }, function (error) {
                    console.log("Error", error);
                });
            }

            $scope.isVisibleTableEmployed = false;
            $scope.isVisibleTablePartner = true;
            $scope.isVisibleTableChild = false;

            break;
        

        case 'childcommand':
            $scope.typeUser = "Enfant";
            $scope.arrayDay = [];
            for (var i = 1; i <= 31; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayDay.push(value);


            }
            $scope.arrayYear = [];
            for (var i = 2010; i <= new Date().getFullYear(); i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayYear.push(value);


            }

            
            $scope.viewMarital = false;

            $scope.titleBonCommand = "Child";
            $scope.categoryClient = "enfant";
            // $scope.titleBonCommand = "Bon de commande pour employé(e)";
            $scope.cboCategBeneficiaire = "Child";
            if ($scope.loadHealth == false) {
                $scope.loadHealth = true;

                FactoryHome.getListHealths().then(function (response) {
                    $scope.tabListHealths = response;
                    console.log("LIST HOSPITALS:", JSON.stringify($scope.tabListHealths));

                }, function (error) {
                    console.log("Error", error);
                });
            }

            $scope.isVisibleTableEmployed = false;
            $scope.isVisibleTablePartner = false;
            $scope.isVisibleTableChild = true;

            break;

        case 'visitorcommand':
            $scope.viewMarital = true;

           // document.querySelector('#voucherSearch').placeholder = "Write name a visitor";
            $scope.titleBonCommand = "Visitor";
            $scope.categoryClient = "Visitor";
            $scope.typeUser = "Visiteur";
            $scope.arrayDay = [];
            for (var i = 1; i <= 31; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayDay.push(value);


            }
            $scope.arrayYear = [];
            for (var i = 2010; i <= new Date().getFullYear() ; i++) {
                var value = i.toString();
                if (value.length == 1) {
                    value = "0" + value;
                }
                $scope.arrayYear.push(value);


            }
            // $scope.titleBonCommand = "Bon de commande pour employé(e)";
            $scope.cboCategBeneficiaire = "Visitor";
            if ($scope.loadHealth == false) {
                $scope.loadHealth = true;

                FactoryHome.getListHealths().then(function (response) {
                    $scope.tabListHealths = response;
                    console.log("LIST HOSPITALS:", JSON.stringify($scope.tabListHealths));

                }, function (error) {
                    console.log("Error", error);
                });
            }

            $scope.isVisibleTableEmployed = false;
            $scope.isVisibleTablePartner = false;
            $scope.isVisibleTableChild = false;
            $scope.isVisibleTableVisitor = true;

            break;
        case 'ReportingSystem':
            FactoryHome.getReprotingService().then(function (response) {
                $scope.TabReporting = response;
                //$scope.getDateReporting();
                console.log(JSON.stringify($scope.TabReporting));
            }, function (error) {
                console.log("Error:", error);
            });
            break;
        case 'VoucherCasual':
            if ($scope.roleUser=="user") {
                $scope.cboVoucherCasualCompany=false
           //     alert($scope.roleUser);
            } else {
                $scopecboVoucherCasualCompany = true;
               // alert($scope.roleUser);
            }
            $scope.ListCasualOld = [];
            FactoryHome.getListCasuals().then(function (response) {
                $scope.ListCasual = response;
                $scope.ListCasualOld = response;
                console.log("Casual LIST:", JSON.stringify($scope.ListCasual));
            }, function (error) {
                console.log("Error Casual:", error);
            });
            FactoryHome.getListSuccursales().then(function (response) {
                $scope.ListSuccursales = response;
                console.log("Succursales Casual:", JSON.stringify($scope.ListSuccursales));
            }, function (error) {
                console.log("Error Casual:", error);
            });
            FactoryHome.getListHealths().then(function (response) {
                console.log("LIST HOSPITAL  :", JSON.stringify(response));
                $scope.tabListHealths = response;
            }, function (error) {
                console.log("Error", error);
            });
            break;
        case 'AddFacture':
            if ($scope.roleUser=="user") {
                FactoryHome.getListDepartement($scope.idAuthSucc).then(function (response) {
                    console.log("DEPARTEMENT :",JSON.stringify(response));
                    $scope.listdepartementbyBCmd = response;
                    $scope.listdepartementbyBCmd2 = response;

                }, function (error) {

                })
            }
            $scope.getSuccursales();

            FactoryHome.getlistVoucherDependents().then(function (response) {
                $scope.TabDependents = response;
                console.log("Vouchers Depends :", JSON.stringify(response));
            }, function (error) {
                console.log("Error List Deps :", error);
            });

            FactoryHome.getlistVoucherVisitor().then(function (response) {
                $scope.TabVisitorVouchers = response;
                console.log("Vouchers Visitor :", JSON.stringify(response));
            }, function (error) {
                console.log("Error Visitor :", error);
            });
            FactoryHome.getlistVoucherCasual().then(function (response) {
                $scope.TabCasualVouchers = response;
                console.log("Vouchers Casual :", JSON.stringify(response));
            }, function (error) {
                console.log("Error Casual :", error);
            });
            FactoryHome.getlistVoucherContactor().then(function (response) {
                console.log("LIST CONTRACTOR :", JSON.stringify(response));
                $scope.TabContractorVouchers = response;
            }, function (error) {
                console.log(" ERROR CONTRACTOR :", error);
            });
            break;
        default:
            break;

    }
    //PRINTABLE PROCESS
    //$scope.savePayment = function () {
    //    if ($scope.nameSearchPaie != undefined && $scope.categoryPay != undefined && $scope.amountPay != undefined) {
    //        $scope.paymentObject = {
    //            name: $scope.nameSearchPaie,
    //            slice: $scope.categoryPay,
    //            object: $scope.categoryTranche,
    //            amount: $scope.amountPay
    //        };
    //        factoryStudent.setPayment($scope.paymentObject).then(function (response) {

    //            // console.log("Response :",JSON.stringify(response));
    //            if (response._IDPAY != undefined) {
    //                document.querySelector('#lblsuccess2').className = "alert alert-success";
    //                document.querySelector('#lblError').className = "alert alert-success hide";
    //                //console.log(JSON.stringify(response));
    //                $scope.tablePayment.push(response);
    //            }
    //            //console.log("Response payment:",response);
    //        }, function (error) {
    //            console.log("error :", error);
    //        });
    //        // console.log("Object pay:",$scope.paymentObject);
    //    } else {
    //        document.querySelector('#lblsuccess2').className = "alert alert-success hide";
    //        document.querySelector('#lblError').className = "alert alert-success";
    //    }

    //}

   
    $scope.actionRoutersBCmd = function () {
        //alert($scope.cboCategBeneficiaire.toString().trim());
        switch ($scope.cboCategBeneficiaire.toString().trim()) {
            case 'Employee':
                
                document.location.href = "#/employedcommand";
                
                $scope.cboCategBeneficiaire = "Employee";
                if ($scope.loadHealth==false) {
                    $scope.loadHealth = true;
                   
                }
                
                // document.querySelector('#btnEmployed').click();
                // $scope.btnEmployed.click();
                //$scope.Bon3 = "Lama Hornel";
                
                break;
            case 'Spouse':
                //   $scope.titleBonCommand = "Bon de commande pour conjoint(e)";
                document.location.href = "#/partnercommand";
                $scope.cboCategBeneficiaire = "Spouse";
            
                if ($scope.loadHealth == false) {
                    $scope.loadHealth = true;
                 
                }
                break;
            case 'Child':
                // $scope.titleBonCommand = "Bon de commande pour employé(e)";
                document.location.href = "#/childcommand";
                $scope.cboCategBeneficiaire = "Child";
              
                if ($scope.loadHealth == false) {
                    $scope.loadHealth = true;
             
                }
                break;
            default:
                document.location.href = "#/visitorcommand";
                $scope.cboCategBeneficiaire = "Visitor";

                if ($scope.loadHealth == false) {
                    $scope.loadHealth = true;

                }
                break;

        }
    }
    $scope.whereSuccursale = function (name) {
       
        angular.forEach($scope.ListSuccursales, function (value, key) {

            if (name==value.name.toString().trim()) {
                $scope.idSuccursale = value.id;
                angular.forEach($scope.getAllsDepartments, function (value, key) {
                    if (value.idSucc == $scope.idSuccursale) {
                        $scope.idDepartment = value.id;
                    }
                });
            }
        });

        
    }
    $scope.EmployeeScope = "";
    $scope.selectedEmployedBCmd = function (employed,index) {
       
        switch ($scope.cboCategBeneficiaire.toString().trim()) {
            case 'Employee':
                if (employed.account_system=="Disabled") {
                    alert("This employee is disabled");
                } else {
                    $scope.EmployeeScope = employed.name;

                    $scope.employedBC = employed;
                    $scope.entreprise = employed.ID_Succursale;
                    $scope.nameEmployedBC = employed.name;
                    $scope.cboEtatCivilBC = employed.CivilStatus;
                    $scope.matrAgent = employed.Matricule;
                    $scope.whereSuccursale(employed.ID_Succursale);
                    $scope.departement = employed.ID_Departement;
                    var old = employed.datenaiss.toString().trim().substring((employed.datenaiss.toString().trim().lastIndexOf('/') + 1));
                    old = new Date().getFullYear() - old;
                    console.log("OLD EMPLOYED :" + old);
                    $scope.oldEmployed = old;
                    document.querySelector('#pictureEmployedBC').src = "../images/" + employed.picture;
                    $scope.picturePrinter = "../images/" + employed.picture;
                    $scope.idPrinter = employed.id;
                    console.log("Employed Bon:", JSON.stringify(employed));
                    console.log("Id :", $scope.idPrinter);
                }
                break;
            case 'Spouse':
                $scope.employedBC = employed.partner;
                $scope.EmployeeScope = employed.name;
                if (employed.partner.account_system=="Disabled") {
                    alert("This account is Disabled");
                } else {
            
                    if (employed.partner.ID_Succursale == null) {
                  
                        $scope.entreprise = employed.ID_Succursale;
                        $scope.matrAgent = employed.Matricule;
                        $scope.nameEmployedBC = employed.partner.name;
                        $scope.cboEtatCivilBC = employed.partner.CivilStatus;
                        $scope.whereSuccursale(employed.ID_Succursale);
                        $scope.departement = employed.ID_Departement;
                        $scope.idDepartement = "_________________";
                        var old = employed.partner.datenaiss.toString().trim().substring((employed.partner.datenaiss.toString().trim().lastIndexOf('/') + 1));
                        old = new Date().getFullYear() - old;
                        $scope.oldEmployed = old;
                        document.querySelector('#pictureEmployedBC').src = "../images/" + employed.partner.picture;
                        $scope.picturePrinter = "../images/" + employed.partner.picture;

                        console.log("Employed Bon:", JSON.stringify(employed));

                    } else {
                        alert('Spouse Employee');
                        $scope.employedBC = employed;
                        $scope.entreprise = employed.partner.ID_Succursale;
                        $scope.nameEmployedBC = employed.partner.name;
                        $scope.cboEtatCivilBC = employed.partner.CivilStatus;
                        //$scope.matrAgent = employed.partner.Matricule;
                        $scope.matrAgent = employed.Matricule;
                        $scope.whereSuccursale(employed.partner.ID_Succursale);
                        $scope.departement = employed.partner.ID_Departement;
                        var old = employed.partner.datenaiss.toString().trim().substring((employed.partner.datenaiss.toString().trim().lastIndexOf('/') + 1));
                        old = new Date().getFullYear() - old;
                        console.log("OLD EMPLOYED :" + old);
                        $scope.oldEmployed = old;
                        document.querySelector('#pictureEmployedBC').src = "../images/" + employed.partner.picture;
                        $scope.picturePrinter = "../images/" + employed.partner.picture;

                        console.log("Employed Bon:", JSON.stringify(employed));
                    }
                }
                break;
            case 'Child':

                $scope.statut = employed.Childs[index].status.toString().trim();
                if ($scope.statut=="inactive") {
                    alert('The Child was disabled by an administrator for some reason.\n Please contact an administrator or enabled the child to proceed')
                } else {

                    if (employed.Childs[index].account_system == "Enabled") {
                        $scope.EmployeeScope = employed.name;
                        $scope.matrAgent = employed.Matricule;
                        $scope.picturePrinter = employed.Childs[index].picture;
                        $scope.employedBC = employed.Childs[index];
                        $scope.entreprise = employed.ID_Succursale;
                        $scope.nameEmployedBC = employed.Childs[index].name;
                        $scope.cboEtatCivilBC = "Single";
                        $scope.matrAgent = employed.Matricule;
                        $scope.whereSuccursale(employed.ID_Succursale);
                        $scope.departement = employed.ID_Departement;
                        $scope.idDepartement = "________________________";

                        var old = employed.Childs[index].datenais.toString().trim().substring((employed.Childs[index].datenais.toString().trim().lastIndexOf('/') + 1));
                        old = new Date().getFullYear() - old;
                        console.log("OLD EMPLOYED :" + old);
                        $scope.oldEmployed = old;
                        document.querySelector('#pictureEmployedBC').src = "../../images/" + employed.Childs[index].picture;
                        $scope.picturePrinter = "../images/" + employed.Childs[index].picture;

                        console.log("Employed Bon:", JSON.stringify(employed));
                    } else {
                        alert('The Child was disabled by an administrator for some reason.\n Please contact an administrator or enabled the child to proceed');
                    }
                }
                
                break;
            case "Visitor":
                
                if (employed.status=="Enabled") {
                    $scope.picturePrinter = employed.picture;
                    $scope.employedBC = { id: employed.Uid };
                    $scope.entreprise = employed.ComapnyName;
                    $scope.nameEmployedBC = employed.name;
                    $scope.cboEtatCivilBC = "___________";
                    $scope.matrAgent = "______________________";
                    $scope.whereSuccursale(employed.idVisitor);
                    $scope.departement = "__________________________";
                    $scope.idDepartement = "________________________";
                    $scope.yypeUser = "Visiteur";
                    var old = '0';
                    console.log("OLD EMPLOYED :" + employed.Uid);
                    $scope.oldEmployed = old;
                    document.querySelector('#pictureEmployedBC').src = "../../images/" + employed.picture;

                    console.log("Employed Bon:", JSON.stringify(employed));
                } else {
                    alert("This visitor is disabled");
                }
                break;
            default:

        }
        document.querySelector('#btnEmployed').click();
    }
    $scope.entreprise = "NAMOYA MINING SA";
    $scope.isVisiblePrintable = false;
    $scope.isVisiblebcEmployed = true;
    $scope.addBonCommand = function () {
        
        if (
                $scope.idHospital != "" && $scope.cboApprouve != undefined
            ) {
            //if (document.querySelector('#tmotif2').value!="") {
            //    $scope.tmotif = document.querySelector('#tmotif2').value;
            //}
            
            $scope.datedeb = document.querySelector('#cbomonthDeb').value.toString().trim() + '/' + document.querySelector('#cbodayDeb').value.toString().trim() + '/' + document.querySelector('#cboyearDeb').value.toString().trim();
            $scope.datefin ='12/31/2019'
            $scope.bonCommand = {
                C_id_bon: 0,
                C_id_bene: $scope.employedBC.id,
                C_datedeb: $scope.datedeb,
                C_datefin: $scope.datefin,
                C_nameDoctor: '',
                C_approuve: $scope.cboApprouve,
                C_id_centre: $scope.idHospital,
                C_motif: document.querySelector('#cboservices').value.toString().trim()
            };
            console.log($scope.bonCommand);
            FactoryHome.setBonCommand($scope.bonCommand).then(function (response) {
                var result = parseInt(response.toString().trim());
                
                if (typeof (result) == "number") {
                    $scope.idVoucher = result;
                    document.querySelector('#alertSuccessBC').style = "display:normal;text-align:center";
                    $scope.healthCenter = $scope.cboCenterHealth;
                    $timeout(function () {
                        // document.location.href = "#/printablecommand";
                        $scope.isVisiblePrintable = true;
                        $scope.isVisiblebcEmployed = false;
                    }, 3000);
                    console.log("Object Command:", JSON.stringify($scope.bonCommand));
                    
                }

            }, function (error) {
                console.log("Error :", error);
            });
        } else {
            alert("Remplissez le(s) Champ(s) vide(s).....");
        }
      
    }
    $scope.CenterHealth = function () {
        var _nameHealth = $scope.cboCenterHealth.toString().trim();
        angular.forEach($scope.tabListHealths, function (value, key) {
            if (value.C_name==_nameHealth) {
                $scope.idHospital = value.C_id_centre;
            }
        });
        console.log("ID HOSPITAL :", $scope.idHospital);
    }
    $scope.printerView = true;
    $scope.Printable = function () {
        window.onafterprint = function () {
            $scope.printerView = true;
            document.querySelector("#wrappermenubar").style = "display:normal";
            window.document.querySelector('#headerBC').style = "display:normal";
            document.querySelector("#wrapper").id ="page-wrapper";
            document.querySelector('#btnPrint').style = "display:normal";
            document.location.reload();

        }
        
        $scope.printerView = false;
        document.querySelector("#wrappermenubar").style = "display:none";
        window.document.querySelector('#headerBC').style = "display:none";
        document.querySelector("#page-wrapper").id = "wrapper";
        document.querySelector('#btnPrint').style = "display:none"; 
        window.print();
       

    }
    $scope.SearchBonCommand = function (categ) {
       
        if (categ == "Master") {
            var name = $scope.nameEmployed.toString().trim();
            var category = $scope.cboCategBeneficiaire.toString().trim();
            //alert("Name :" + name);
           //alert("222Category :" + category);

            FactoryHome.getListBonCommand(name, category).then(function (response) {
                
                $scope.tabSearchEmployed = response;
                console.log("LIST FOUND SPOUSE :",JSON.stringify(response));
                //if (category=="Child") {
                //    if ($scope.tabSearchEmployed.length == 0) {
                //        alert("Currently deactivated because 18/25 Old");
                //    } else {
                //        $scope.oldChild = "";
                //    }
                //}
                
                //console.log("TAB :", JSON.stringify(response));

            }, function (error) {
                console.log(error);
            })
        }
    }
    $scope.getFileFacture = function () {

        var browser = document.createElement('input');
        browser.type = "file";
        browser.click();
        var pict = document.querySelector('#fileUpload');
        var tArea = document.querySelector('#fileBenef');
        var fRead = new FileReader();
        browser.onchange = function () {
            pict.value = browser.files[0].name;
            fRead.readAsDataURL(browser.files[0]);
        }
        fRead.onloadend = function () {
            tArea.value = fRead.result;
        }
    }
    $scope.tableForEmployed = false;
    $scope.tableForPartner = false;
    $scope.tableChildren = false;
    $scope.loading = true;
    $scope.tabHealthBCmd = [];
    $scope.btnDisabled = true;
    $scope.SearchBCmdForFacture = function () {


        $scope.tabHealthBCmd = [];
        $scope.categFacture = "Employee";
        $scope.isVisibleTabFactureEmployee = true;
        $scope.isVisibleTabFactureDep = false;
        $scope.isVisibleTabVisitor = false;
        $scope.isVisibleTabCasual = false;
        $scope.isVisibleTabContractor = false;
        document.querySelector("#cbofacture").value = "Employee";
        try {
           // document.querySelector("#cbofacture").value = "Employee";
//            $scope.cboFacture = "Employee";
            document.querySelector('#fileBenef').value = "";
            document.querySelector('#alertDep').style = "display:none";
        } catch (e) {
            console.log("Error :", e);
        }
        
        FactoryHome.getlistBcommandForFacture("").then(function (response) {
            console.log('response Server :', JSON.stringify(response));
          
            $scope.archiveSearch = response;
            $scope.btnDisabled = false;
            $scope.loading = false;
            
            angular.forEach(response, function (value, key) {
                if ($scope.tabHealthBCmd.length==0) {
                    $scope.tabHealthBCmd.push(value.idHealth);
                } else {
                    var indexed = $scope.tabHealthBCmd.indexOf(value.idHealth);
                    if (indexed<0) {
                        $scope.tabHealthBCmd.push(value.idHealth);

                    }
                    console.log("LENGHT TABHELATH :", $scope.tabHealthBCmd.length);
                    console.log("HEALTH ADDED :", $scope.tabHealthBCmd[key]);
                }
                if (value.Employed != undefined && value.Child == undefined) {
                    $scope.tableForEmployed = true;
                    $scope.tableForPartner = false;
                    $scope.tableChildren = false;
                   // $scope.idBon = value.id;
                    $scope.tabBCmdFacture = response;

                } else if (value.Partner != undefined) {
                    $scope.tableForEmployed = false;
                    $scope.tableForPartner = true;
                    $scope.tableChildren = false;
                    $scope.tabBCmdFacture = response;
                    //  $scope.idBon = value.id;

                } else {

                    $scope.tableForEmployed = false;
                    $scope.tableForPartner = false;
                    $scope.tableChildren = true;
                    $scope.tabBCmdFacture = response;
                    // $scope.idBon = value.id;n
                }
            });
        }, function (error) {
            $scope.loading = false;
            $scope.loading2 = true;
          
          //  console.log("Error :", error);
        });
        
        console.log("List Health :", $scope.tabHealthBCmd.length);
    }
    $scope.selectedEmployedfacture = function (employed) {
       
        
        if ($scope.tableForEmployed==true) {
            $scope.nameBenef = employed.Employed.name;
            $scope.phoneBenef = employed.Employed.phone;
            $scope.SuccBenef = employed.Employed.ID_Succursale;
            $scope.departBenef = employed.Employed.ID_Departement;
            $scope.idBon = employed.id;
            document.querySelector('#btnSearchCmd').click();
        } else if($scope.tableForPartner==true) {
            $scope.nameBenef = employed.Partner.name;
            $scope.phoneBenef = employed.Partner.phone;
            $scope.SuccBenef = employed.Partner.ID_Succursale;
            $scope.departBenef = employed.Partner.ID_Departement;
            $scope.idBon = employed.id;
            document.querySelector('#btnSearchCmd').click();
        } else {
            $scope.nameBenef = employed.Child.name;
            $scope.phoneBenef = employed.Child.phone;
            $scope.SuccBenef = employed.Child.ID_Succursale;
            $scope.departBenef = employed.Child.ID_Departement;
            $scope.idBon = employed.id;
            document.querySelector('#btnSearchCmd').click();
        }
        $scope.tableForEmployed = false;
        $scope.tableForPartner = false;
        $scope.tableChildren = false;
    }
    try{
        document.querySelector('#btnSearchFile').onclick = function () {
            var browser = document.createElement('input');
            browser.type = "file";

            browser.click();
            var fReaderFile = new FileReader();

            browser.onchange = function () {
                var extension = browser.files[0].name.split('.');
                if (extension[1]=="csv") {
                    if (browser.files[0] != null) {
                        fReaderFile.readAsText(browser.files[0], 'UTF-8');
                        var alertImport = document.querySelector('#alertImport');
                        var spanMessage = document.querySelector('#messageImport');
                        alertImport.className = "alert alert-success";
                        fReaderFile.onloadend = function(e) {
                            console.log(fReaderFile.result)
                            FactoryHome.setSendFileCSV(fReaderFile.result).then(function (response) {
                                if (response.toString().trim() == "200") {
                                    spanMessage.innerHTML = " Exportation Successfull ";
                                    window.setTimeout(function () {
                                        document.location.reload();
                                    }, 2000)
                                }
                            }, function (error) {
                                console.log("Error:", error);
                            })
                        }
                        
                    }
                } else {
                    alert("File not supported, (CSV FORMAT)");
                }
                
            }
        
        }
    } catch (e) {

    }
    $scope.TabReportingDate = new Array();

    $scope.getDateReporting = function () {
        
        if ($scope.TabReporting.length > 0) {
            $scope.TabReportingDate = [];
            var date_begin = document.querySelector('#datedeb').value.toString().trim();
            var date_end = document.querySelector('#datefin').value.toString().trim();
            
            var date_beginSplitter = date_begin.toString().split('/');
            var date_endSplitter = date_end.toString().split('/');
            console.log("Date1:", date_begin.toString());
            console.log("Date2:", date_end.toString());
            var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
            var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
            console.log("Formatter :", formatter);
       
            var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1]-1), parseInt(date_beginSplitter[0]));
            var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1]-1), parseInt(date_endSplitter[0]));
            console.log("Date1 :", date1.toLocaleDateString());
            console.log("Date2 :", date2.toLocaleDateString());
            var date3 = new Date(2017, 2, 12);
          
            //if (date1.toLocaleDateString() > date2.toLocaleDateString()) {
            //console.log("Date Superior 1:", date1.toLocaleDateString());
            
            //} else {
            //    console.log("Date Superior 2:", date2.toLocaleDateString());

            //}
            //if (date3 >= date1.toLocaleDateString() && date3 <= date2.toLocaleDateString()) {
            //    console.log("Good :",date3);
            //} else {
            //    console.log("No thing:",date3);
            //}

            if (date1<date2) {
                angular.forEach($scope.TabReporting, function (value, key) {
                    var objectCurrent = {};
                    var currentFactures = [];
                    var currentDependecies = [];
                    console.log("Key", value.name + "=>");
                    if ($scope.TabReporting[key].Facturations != null) {
                        angular.forEach($scope.TabReporting[key].Facturations, function (facture, keyF) {
                            var dateSplitter = facture.datecmd.toString().trim().split('/');
                            var dateFound = new Date(parseInt(dateSplitter[2]), parseInt(dateSplitter[1] - 1), parseInt(dateSplitter[0]));
                            if (dateFound >= date1 && dateFound <= date2) {
                                currentFactures.push(facture);
                            }

                        });
                        if (currentFactures.length > 0) {
                            objectCurrent = value;
                            objectCurrent.Facturations = currentFactures;

                            $scope.TabReportingDate.push(objectCurrent);
                        }

                    }
                    if ($scope.TabReporting[key].dependecies != null) {
                        angular.forEach($scope.TabReporting[key].dependecies, function (facture, keyD) {
                            var dateSplitter = facture.datecmd.toString().trim().split('/');
                            var dateFound = new Date(parseInt(dateSplitter[2]), parseInt(dateSplitter[1] - 1), parseInt(dateSplitter[0]));
                            if (dateFound >= date1 && dateFound <= date2) {
                                currentDependecies.push(facture);
                            }
                        });

                        if (currentDependecies.length > 0) {
                            objectCurrent = value;
                            objectCurrent.Facturations = currentDependecies;

                            $scope.TabReportingDate.push(objectCurrent);
                        }
                    }
                });
                console.log(JSON.stringify($scope.TabReportingDate));
                // $scope.exemple = [{ "CtrBon": 2, "nbreDepencies": 0, "id": "1036", "name": "Levi Mwema", "sex": "M", "phone": "0897689002", "CivilStatus": "Marié", "picture": "126.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "SQL DEVELOPER", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 30, "idHealth": "CS LISEKWA", "idBene": 1036, "datecmd": "31/01/2017", "dateValidation": "01/02/2010", "nameDoctor": "Bertin Lofoyo", "approuve": null, "motif": "crise de malaria", "Employed": null, "Child": null, "Partner": null, "cout": 100, "nameAuthor": null, "dateFacture": "12/12/2016" }] }, { "CtrBon": 1, "nbreDepencies": 0, "id": "3125", "name": "Edmond Okosa", "sex": "M", "phone": "089657890", "CivilStatus": "Marié", "picture": "972.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "VISUAL STUDIO TEAM", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 21, "idHealth": "CS AKRAM", "idBene": 3125, "datecmd": "01/02/2017", "dateValidation": "01/02/2010", "nameDoctor": "Jean nhfdyf", "approuve": null, "motif": null, "Employed": null, "Child": null, "Partner": null, "cout": 0, "nameAuthor": null, "dateFacture": "" }] }, { "CtrBon": 7, "nbreDepencies": 0, "id": "3140", "name": "Grace Mukendi", "sex": "M", "phone": "0894848494", "CivilStatus": "Marié", "picture": "658.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "SQL DEVELOPER", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 29, "idHealth": "CS LISEKWA", "idBene": 3140, "datecmd": "31/01/2017", "dateValidation": "01/02/2010", "nameDoctor": "Bertin Lofoyo", "approuve": null, "motif": "crise de malaria", "Employed": null, "Child": null, "Partner": null, "cout": 0, "nameAuthor": null, "dateFacture": "" }] }, { "CtrBon": 7, "nbreDepencies": 0, "id": "3140", "name": "Grace Mukendi", "sex": "M", "phone": "0894848494", "CivilStatus": "Marié", "picture": "658.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "SQL DEVELOPER", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 29, "idHealth": "CS LISEKWA", "idBene": 3140, "datecmd": "31/01/2017", "dateValidation": "01/02/2010", "nameDoctor": "Bertin Lofoyo", "approuve": null, "motif": "crise de malaria", "Employed": null, "Child": null, "Partner": null, "cout": 0, "nameAuthor": null, "dateFacture": "" }] }, { "CtrBon": 7, "nbreDepencies": 0, "id": "3140", "name": "Grace Mukendi", "sex": "M", "phone": "0894848494", "CivilStatus": "Marié", "picture": "658.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "SQL DEVELOPER", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 29, "idHealth": "CS LISEKWA", "idBene": 3140, "datecmd": "31/01/2017", "dateValidation": "01/02/2010", "nameDoctor": "Bertin Lofoyo", "approuve": null, "motif": "crise de malaria", "Employed": null, "Child": null, "Partner": null, "cout": 0, "nameAuthor": null, "dateFacture": "" }] }, { "CtrBon": 7, "nbreDepencies": 0, "id": "3140", "name": "Grace Mukendi", "sex": "M", "phone": "0894848494", "CivilStatus": "Marié", "picture": "658.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "SQL DEVELOPER", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 29, "idHealth": "CS LISEKWA", "idBene": 3140, "datecmd": "31/01/2017", "dateValidation": "01/02/2010", "nameDoctor": "Bertin Lofoyo", "approuve": null, "motif": "crise de malaria", "Employed": null, "Child": null, "Partner": null, "cout": 0, "nameAuthor": null, "dateFacture": "" }] }, { "CtrBon": 7, "nbreDepencies": 0, "id": "3140", "name": "Grace Mukendi", "sex": "M", "phone": "0894848494", "CivilStatus": "Marié", "picture": "658.jpg", "datenaiss": "12/10/1978", "ID_Succursale": "MICROSOFT INC.", "ID_Departement": "SQL DEVELOPER", "partner": null, "status": false, "Childs": null, "dependecies": [], "Facturations": [{ "id": 29, "idHealth": "CS LISEKWA", "idBene": 3140, "datecmd": "31/01/2017", "dateValidation": "01/02/2010", "nameDoctor": "Bertin Lofoyo", "approuve": null, "motif": "crise de malaria", "Employed": null, "Child": null, "Partner": null, "cout": 0, "nameAuthor": null, "dateFacture": "" }] }];
                //console.log($scope.exemple);
                //console.log("Found Date:",$scope.tabTrier.length);
                if ($scope.TabReportingDate.length > 0) {
                    $scope.TableDataVisible = false;
                    $scope.TableDataDateVisible = true;

                } else {
                    alert("Datas not found");
                }
            } else {
                alert("La date 2 doit etre superieure à la date 1");
            }

        }
    }
    $scope.TableDataVisible = true;
    $scope.TableDataDateVisible = false;
    $scope.EmployedFacturation = function (data) {
        
        $scope.isVisibilityTabFacturation = false;
        $scope.isVisibilityTabDependecies = false;
        if ($scope.TabReporting.length>0) {
            $scope.TabReportEmployed = {};
            $scope.totalPay = 0.0;
            angular.forEach($scope.TabReporting, function (value, key) {
                if (value.id == data) {

                    $scope.TabReportEmployed = value;

                }
            });
            $scope.totalPayIndividual = 0.0;
            $scope.totalPayDependecies = 0.0;
            console.log("Tab Employed :", JSON.stringify($scope.TabReportEmployed));
            if ($scope.TabReportEmployed.Facturations!=null) {
                $scope.isVisibilityTabFacturation = true;
                $scope.currentBeneficiaire = $scope.TabReportEmployed.name;
                angular.forEach($scope.TabReportEmployed.Facturations, function (value, key) {
                    $scope.totalPayIndividual +=parseFloat(value.cout);
                });
            }
            if ($scope.TabReportEmployed.dependecies.length>0) {
               
                $scope.isVisibilityTabDependecies = true;
                $scope.currentBeneficiaire = $scope.TabReportEmployed.name + " and dependecies";
                console.log("Dependecies :", JSON.stringify($scope.TabReportEmployed.dependecies));
                angular.forEach($scope.TabReportEmployed.dependecies, function (value, key) {
                    $scope.totalPayDependecies +=parseFloat(value.cout);
                });
            }
            document.querySelector('#btnModalReporting').click();
        } else {
            alert("Vidé");
        }
    }
    $scope.viewTabReporting = function () {
        $scope.TableDataVisible = true;
        $scope.TableDataDateVisible = false;
    }
    $scope.isVisibleTabFactureEmployee = true;
    $scope.isVisibleTabFactureDep = false;
    $scope.isVisibleTabVisitor = false;
    $scope.isVisibleTabCasual = false;
    $scope.isVisibleTabContractor = false;
    $scope.getter = function () {
        alert("get up");
    }
    $scope.categFacture = "";
    $scope.btnInvoice = false;
    $scope.changeFacture = function () {
        $scope.categFacture = $scope.cbofacture.toString().trim();
       // alert($scope.categFacture);
        switch ($scope.categFacture) {
            case 'Employee':
                $scope.btnInvoice = true;
                $scope.isVisibleTabFactureEmployee = true;
                $scope.isVisibleTabFactureDep = false;
                $scope.isVisibleTabVisitor = false;
                $scope.isVisibleTabCasual = false;
                $scope.isVisibleTabContractor = false;
                $scope.SearchBCmdForFacture();
                var tabRows = document.querySelector('#idTabCout');
                while (tabRows.firstChild) {
                    tabRows.removeChild(tabRows.firstChild);
                }
                document.querySelector('#TDepart').innerHTML = "Department";
                break;

            case 'Dependents':
                $scope.btnInvoice =true;
                $scope.isVisibleTabFactureEmployee = false;
                $scope.isVisibleTabFactureDep = true;
                $scope.isVisibleTabVisitor = false;
                $scope.isVisibleTabCasual = false;
                $scope.isVisibleTabContractor = false;
                $scope.tabBCmdFacture = [];
                $scope.tabBCmdFacture = $scope.TabDependents;
                var tabRows = document.querySelector('#idTabCout');
                while (tabRows.firstChild) {
                    tabRows.removeChild(tabRows.firstChild);
                }
                document.querySelector('#TDepart').innerHTML = "Department";
                break;
            case 'Visitor':
                $scope.btnInvoice =true;
                $scope.isVisibleTabFactureEmployee = false;
                $scope.isVisibleTabFactureDep = false;
                $scope.isVisibleTabVisitor = true;
                $scope.isVisibleTabCasual = false;
                $scope.isVisibleTabContractor = false;
                $scope.tabBCmdFacture = [];
                $scope.tabBCmdFacture = $scope.TabVisitorVouchers;
                var tabRows = document.querySelector('#idTabCout');
                while (tabRows.firstChild) {
                    tabRows.removeChild(tabRows.firstChild);
                }
                document.querySelector('#TDepart').innerHTML = "Hospital";
                break;

            case 'Casual':
                $scope.btnInvoice = false;
                $scope.isVisibleTabFactureEmployee = false;
                $scope.isVisibleTabFactureDep = false;
                $scope.isVisibleTabVisitor = false;
                $scope.isVisibleTabCasual = true;
                $scope.isVisibleTabContractor = false;
                $scope.tabBCmdFacture = [];
                $scope.tabBCmdFacture = $scope.TabCasualVouchers;
                var tabRows = document.querySelector('#idTabCout');
                while (tabRows.firstChild) {
                    tabRows.removeChild(tabRows.firstChild);
                }
                document.querySelector('#TSex').innerHTML = "Date voucher";
                document.querySelector('#TDepart').style = "display:none";
                document.querySelector('#DateV').style = "display:none";
                break;
            case 'Contractor':
                $scope.btnInvoice = false;
                $scope.isVisibleTabFactureEmployee = false;
                $scope.isVisibleTabFactureDep = false;
                $scope.isVisibleTabVisitor = false;
                $scope.isVisibleTabCasual = false;
                $scope.isVisibleTabContractor = true;
                $scope.tabBCmdFacture = [];
                $scope.tabBCmdFacture = $scope.TabContractorVouchers;
                var tabRows = document.querySelector('#idTabCout');
                while (tabRows.firstChild) {
                    tabRows.removeChild(tabRows.firstChild);
                }
                document.querySelector('#TSex').innerHTML = "Date voucher";
                document.querySelector('#TDepart').style = "display:none";
                document.querySelector('#DateV').style = "display:none";
                break;
            default:

        }
    }
    $scope.tabDateSearch = [];
    $scope.dater = "10/02/2010";
    $scope.TAB = $scope.dater.toString().split('/');
    console.log("Day:" + $scope.TAB[0] + " Month:" + $scope.TAB[1] + " Year:" + $scope.TAB[2]);
    try {
        var date_begin = document.querySelector('#datedeb');
        var date_end = document.querySelector('#datefin');
        date_begin.onchange = function () {
            if (date_begin.value!="" && date_end.value!="") {
                
            }
            if (date_begin.value == "" && date_end.value == "") {
                alert("Back it");
            }
        }

        date_end.onchange=function(){
            if (date_begin.value != "" && date_end.value != "") {
                //  alert("Search!!!");
            }
            if (date_begin.value == "" && date_end.value == "") {
                alert("Back it");
            }
        }
    } catch (e) {

    }
    $scope.getPrintReporting = function () {
        window.onafterprint = function () {
            
            
            alert("Printable Success");
            document.location.reload();
        }


        $scope.printerView = false;
        document.querySelector("#wrappermenubar").style = "display:none";
        window.document.querySelector('#headerReporting').style = "display:none";
        document.querySelector("#page-wrapper").id = "wrapper";
        document.querySelector('#btnPrint').style = "display:none";
        document.querySelector('#headerDate').style = "display:none";
        document.querySelector('#btnFind').style = "display:none";
        var table = $('#dataTables-example').DataTable();
        var data = table.rows()
                        .data();
        //    alert("Lenght :" + data.length);
        //  console.log("Data :", JSON.stringify(data));
        //  alert("Length Search :" + table.rows({ page: 'current' }).nodes().length);
     

        $scope.printData = [];
        var max = table.rows({ page: 'current' }).nodes().length;
        for (var i = 0; i < max; i++) {
            var current = data[i];
            for (var j = 0; j < 1; j++) {
                var object = {
                    id: current[0],
                    name: current[1],
                    sex: current[2],
                    phone: current[3],
                    ID_Succursal: current[4],
                    ID_Departement: current[5],
                    ctrFact: current[6],
                    ctrdependecies: current[7]
                }
                $scope.printData.push(object);
            }

        }
        
        console.log("Length PrintData:", JSON.stringify($scope.printData));
        document.querySelector('#tablebasic').style = "display:none";
        document.querySelector('#tableprint').style = "display:normal";
        //$scope.printData = table.rows({ page: 'current' }).nodes();
        // console.log(table.rows({ page: 'current' }).nodes().data());

        window.setTimeout(function () {
            window.print();
        }, 3000);
    }
    var date_begin = "";
    var date_end = "";
    try {
        
        document.querySelector('#dateCmd1').onchange = function () {
            
            try {
                if (document.querySelector('#dateCmd1').value.toString().trim() != "") {
                    date_begin = document.querySelector('#dateCmd1').value.toString().trim();
                    // var date_end = document.querySelector('#datefin').value.toString().trim();

                    var date_beginSplitter = date_begin.toString().split('/');
                    //var date_endSplitter = date_end.toString().split('/');
                    console.log("Date1:", date_begin.toString());
                    //console.log("Date2:", date_end.toString());
                    //var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
                    date_begin = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
                    //       console.log("Formatter :", formatter);
                    $scope.dateCmd1 = date_begin.toLocalDateString();
                    //var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
                    //var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
                    console.log("Date1 :", $scope.dateCmd1.toLocaleDateString());
                    
                    //console.log("Date2 :", date2.toLocaleDateString());
                } else {
                    console.log("date 1: Vide");

                }
            } catch (e) {

            }
            
        }
        

        document.querySelector('#dateCmd2').onchange = function () {   
            try {
                if (document.querySelector('#dateCmd2').value.toString().trim() != "") {
                    date_end = document.querySelector('#dateCmd2').value.toString().trim();
                    // var date_end = document.querySelector('#datefin').value.toString().trim();

                    var date_endSplitter = date_end.toString().split('/');
                    //var date_endSplitter = date_end.toString().split('/');
                    console.log("Date2:", date_end.toString());
                    //console.log("Date2:", date_end.toString());
                    //var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
                    date_end = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
                    //       console.log("Formatter :", formatter);
                    $scope.dateCmd2 = date_end.toLocalDateString();
                    //var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
                    //var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
                    console.log("Date2 :", $scope.dateCmd2.toLocaleDateString());
                    //console.log("Date2 :", date2.toLocaleDateString());
                    // $scope.getRangeDate();
                } else {
                    console.log("date 2: Vide");
                }
            } catch (e) {

            }
        }
    } catch (e) {

    }
    $scope.lama = function () {
        if (date_begin != "" && date_end != "") {
           
            if (date_begin < date_end) {
               
                
                angular.forEach($scope.tabBCmdFacture, function (value, key) {

                    var currentSplitterDate = value.datecmd.toString().split('/');
                    var currentDate = new Date(parseInt(currentSplitterDate[2]), parseInt(currentSplitterDate[1] - 1), parseInt(currentSplitterDate[0]));
                    console.log("Date Trie :", currentDate.toLocaleDateString("en-US"));
                    console.log("Date Begin :", date_begin.toLocaleDateString("en-US"));
                    console.log("Date End :", date_end.toLocaleDateString("en-US"));
                    if (currentDate >= date_begin && currentDate <= date_end) {
                        $scope.tabSearchCmd.push(value);
                    }
                });
                $scope.tabBCmdFacture = [];
                //console.log("Lenght tabCmdFacture :", $scope.tabBCmdFacture.length);
                $scope.tabBCmdFacture = $scope.tabSearchCmd;
                //console.log("Trie value :", $scope.tabSearchCmd.length);
                $scope.tabSearchCmd = [];

            } else {
                alert('Date End is Superior at Date 1');
            }
        }
        if ($scope.NameEmployeeSearch!=undefined) {
            $scope.tabCurrent = [];
            var _name = $scope.NameEmployeeSearch.toString().trim().toLowerCase();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.Employed.name.toString().toLowerCase();
              

                if (_currentValue.includes(_name)) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
            $scope.tabCurrent = [];
        }

        if ($scope.nameCmd != undefined) {
            //$scope.tabBCmdFacture = [];
            $scope.tabCurrent = [];
            var _name = $scope.nameCmd.toString().trim().toLowerCase();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = "";
                if ($scope.categFacture == "Employee") {
                    _currentValue = value.Employed.name.toString().toLowerCase();
                }
                if ($scope.categFacture="Dependents") {
                    _currentValue = value.nameAuthor.toString().toLowerCase();
                }
                
                if (_currentValue.includes(_name)) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }
                 
            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
            $scope.tabCurrent = [];

            // console.log("Found Name:", JSON.stringify($scope.tabBCmdFacture));
        }
        if ($scope.idCmd!=undefined) {
            $scope.tabCurrent = [];
            var _idCmd = $scope.idCmd.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.id.toString().trim();
                if (_currentValue == _idCmd) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }
        
        if ($scope.idSexe!=undefined) {
            $scope.tabCurrent = [];
            var _idSexe = $scope.idSexe.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = "";
                if ($scope.categFacture=="Employee") {
                    _currentValue=value.Employed.sex.toString().trim();
                }
                if ($scope.categFacture=="Dependents") {
                    _currentValue = value.sexeAuthor.toString().trim();
                }
                if (_currentValue == _idSexe) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }

        if ($scope.phoneSearch != undefined) {
            $scope.tabCurrent = [];
            var _phoneSearch = $scope.phoneSearch.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.Employed.phone.toString().trim();
                if (_currentValue == _phoneSearch) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }

        if ($scope.cbo_succ_BCmd!=undefined ) {
            $scope.tabCurrent = [];
            var _cbo_succ_Search = $scope.cbo_succ_BCmd.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.Employed.ID_Succursale.toString().trim();
                if (_currentValue == _cbo_succ_Search) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }
        if ($scope.cbo_succ_BCmd2 != undefined) {
            $scope.tabCurrent = [];
            var _cbo_succ_Search = $scope.cbo_succ_BCmd2.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.Employed.ID_Succursale.toString().trim();
                if (_currentValue == _cbo_succ_Search) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }

        if ($scope.cbo_depart_BCmd != undefined) {
            $scope.tabCurrent = [];
            var _cbo_depart_Search = $scope.cbo_depart_BCmd.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.Employed.ID_Departement.toString().trim();
                if (_currentValue == _cbo_depart_Search) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }
        if ($scope.cbo_depart_BCmd2 != undefined) {
            $scope.tabCurrent = [];
            var _cbo_depart_Search = $scope.cbo_depart_BCmd2.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.Employed.ID_Departement.toString().trim();
                if (_currentValue == _cbo_depart_Search) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }

        if ($scope.cbo_health_BCmd != undefined) {
            $scope.tabCurrent = [];
            var _cbo_health_Search = $scope.cbo_health_BCmd.toString().trim();
            angular.forEach($scope.tabBCmdFacture, function (value, key) {
                var _currentValue = value.idHealth.toString().trim();
                if (_currentValue == _cbo_health_Search) {
                    $scope.tabCurrent.push(value);
                    console.log("Data found :", JSON.stringify(value));
                    console.log("Name found :", _currentValue);
                } else {
                    console.log("Data not found:Error");
                }

            });
            $scope.tabBCmdFacture = [];
            $scope.tabBCmdFacture = $scope.tabCurrent;
        }

    }
    
    $scope.tabSearchCmd = [];
    $scope.archiveSearch = [];
    $scope.ListBCmd = [];

    $scope.resetCmd = function () {
        $scope.idCmd = undefined;
        $scope.dateCmd1 = undefined;
        document.querySelector('#dateCmd1').value = "";
        $scope.dateCmd2 = undefined;
        document.querySelector('#dateCmd2').value = "";
        $scope.nameCmd = undefined;
        $scope.idSexe = undefined;
        $scope.phoneSearch = undefined;
        $scope.cbo_succ_BCmd = undefined;
        $scope.cbo_succ_BCmd2 = undefined;
        $scope.cbo_depart_BCmd = undefined;
        $scope.cbo_depart_BCmd2 = undefined;
        $scope.cbo_health_BCmd = undefined;
        //        $scope.
        $scope.tabBCmdFacture = [];
        switch ($scope.categFacture) {
            case 'Employee':
                $scope.tabBCmdFacture = $scope.archiveSearch;
                break;
            case 'Dependents':
                $scope.tabBCmdFacture = $scope.TabDependents;
                break;
            default:

        }
        
        
    }
    var ctr = 1;
    var dataInputTab = [];
    var idBonTab = [];

    $scope.cancelValidate = function () {
        var tbodyCout = document.querySelector('#idTabCout');
        document.querySelector('#sendFacture').disabled = true;
        $('#idTabCout').remove();
        document.querySelector('#validate').click();
    }
    $scope.currentFilter = [];
    $scope.checkAllInvoiceDependents = function () {
        var tbodyCout = document.querySelector('#idTabCout');
        var checkAll = document.querySelector('#check_dep');
        if (checkAll.checked==true) {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#d' + employed.id).checked = true;
                var tbodyCout = document.querySelector('#idTabCout');
                _added = false;
                //  alert("Selected Cmd");
                if ($scope.ListBCmd.length == 0) {
                    // alert('Vide');
                    $scope.ListBCmd.push(employed);
                    idBonTab.push(employed.id);
                    _added = true;

                } else {
                    //   alert("Rempli");
                    var index = $scope.ListBCmd.indexOf(employed);

                    if (index > -1) {
                        $scope.ListBCmd.splice(index, 1);
                        _added = false;
                    } else {
                        $scope.ListBCmd.push(employed);
                        _added = true;
                        idBonTab.push(employed.id);

                    }
                    //  alert($scope.ListBCmd);


                }
                //alert(employed.Employed.name);
                if (_added) {
                    //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                    //  alert($scope.categFacture);
                    document.querySelector('#sendFacture').disabled = false;
                    //alert("Button actived...");
                    var value = {};
                    value = employed;
                    var inputCout = document.createElement('input');
                    inputCout.type = "text";
                    inputCout.className = "form-control";
                    var row = document.createElement('td');
                    var cols = document.createElement('tr');
                    row.innerHTML = value.id;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);


                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.name;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    } else if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.sex;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout)
                        tbodyCout.appendChild(cols);

                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Departement;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.datecmd;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                    var validate_input = true;
                    var dataValidate = {};
                    inputCout.onchange = function () {

                        if (isNaN(parseInt(inputCout.value))) {
                            inputCout.style = "border:1px solid red;";
                            validate_input = false;
                        } else {
                            inputCout.style = "border:1px solid silver;";
                            validate_input = true;

                            if (dataInputTab.length == 0) {
                                dataValidate.id = ctr;
                                dataValidate.cout = parseFloat(inputCout.value);
                                dataValidate.categorie = $scope.categFacture;
                                dataInputTab.push(dataValidate);
                                ctr++;
                                // alert("Ctr :" + ctr);
                            } else {
                                dataValidate.cout = parseFloat(inputCout.value);
                                //alert("No Empty");
                                var index = dataInputTab.indexOf(dataValidate);
                                if (index > -1) {

                                    dataInputTab[index] = dataValidate;
                                } else {
                                    dataValidate.id = ctr;
                                    dataInputTab.push(dataValidate);
                                    ctr++;
                                }
                            }
                            //alert("Okay");
                        }
                        console.log("State Validate :", JSON.stringify(dataInputTab));
                    }

                    _added = false;
                    console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
                }

            })
        } else {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#d' + employed.id).checked = false;
                var tbodyCout = document.querySelector('#idTabCout');
                tbodyCout.innerHTML = "";
            });
        }
    }

    $scope.checkAllInvoiceVisitors = function () {
        var tbodyCout = document.querySelector('#idTabCout');
        var checkAll = document.querySelector('#check_vis');
        if (checkAll.checked == true) {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#v' + employed.id).checked = true;
                var tbodyCout = document.querySelector('#idTabCout');
                _added = false;
                //  alert("Selected Cmd");
                if ($scope.ListBCmd.length == 0) {
                    // alert('Vide');
                    $scope.ListBCmd.push(employed);
                    idBonTab.push(employed.id);
                    _added = true;

                } else {
                    //   alert("Rempli");
                    var index = $scope.ListBCmd.indexOf(employed);

                    if (index > -1) {
                        $scope.ListBCmd.splice(index, 1);
                        _added = false;
                    } else {
                        $scope.ListBCmd.push(employed);
                        _added = true;
                        idBonTab.push(employed.id);

                    }
                    //  alert($scope.ListBCmd);


                }
                //alert(employed.Employed.name);
                if (_added) {
                    //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                    //  alert($scope.categFacture);
                    document.querySelector('#sendFacture').disabled = false;
                    //alert("Button actived...");
                    var value = {};
                    value = employed;
                    var inputCout = document.createElement('input');
                    inputCout.type = "text";
                    inputCout.className = "form-control";
                    var row = document.createElement('td');
                    var cols = document.createElement('tr');
                    row.innerHTML = value.id;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);


                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.name;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    } else if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.sex;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout)
                        tbodyCout.appendChild(cols);

                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Departement;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.datecmd;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                    var validate_input = true;
                    var dataValidate = {};
                    inputCout.onchange = function () {

                        if (isNaN(parseInt(inputCout.value))) {
                            inputCout.style = "border:1px solid red;";
                            validate_input = false;
                        } else {
                            inputCout.style = "border:1px solid silver;";
                            validate_input = true;

                            if (dataInputTab.length == 0) {
                                dataValidate.id = ctr;
                                dataValidate.cout = parseFloat(inputCout.value);
                                dataValidate.categorie = $scope.categFacture;
                                dataInputTab.push(dataValidate);
                                ctr++;
                                // alert("Ctr :" + ctr);
                            } else {
                                dataValidate.cout = parseFloat(inputCout.value);
                                //alert("No Empty");
                                var index = dataInputTab.indexOf(dataValidate);
                                if (index > -1) {

                                    dataInputTab[index] = dataValidate;
                                } else {
                                    dataValidate.id = ctr;
                                    dataInputTab.push(dataValidate);
                                    ctr++;
                                }
                            }
                            //alert("Okay");
                        }
                        console.log("State Validate :", JSON.stringify(dataInputTab));
                    }

                    _added = false;
                    console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
                }

            })
        } else {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#v' + employed.id).checked = false;
                var tbodyCout = document.querySelector('#idTabCout');
                tbodyCout.innerHTML = "";
            });
        }
    }
    $scope.checkAllInvoiceCasuals = function () {
        var tbodyCout = document.querySelector('#idTabCout');
        var checkAll = document.querySelector('#check_ca');
        if (checkAll.checked == true) {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#ca' + employed.id).checked = true;
                var tbodyCout = document.querySelector('#idTabCout');
                _added = false;
                //  alert("Selected Cmd");
                if ($scope.ListBCmd.length == 0) {
                    // alert('Vide');
                    $scope.ListBCmd.push(employed);
                    idBonTab.push(employed.id);
                    _added = true;

                } else {
                    //   alert("Rempli");
                    var index = $scope.ListBCmd.indexOf(employed);

                    if (index > -1) {
                        $scope.ListBCmd.splice(index, 1);
                        _added = false;
                    } else {
                        $scope.ListBCmd.push(employed);
                        _added = true;
                        idBonTab.push(employed.id);

                    }
                    //  alert($scope.ListBCmd);


                }
                //alert(employed.Employed.name);
                if (_added) {
                    //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                    //  alert($scope.categFacture);
                    document.querySelector('#sendFacture').disabled = false;
                    //alert("Button actived...");
                    var value = {};
                    value = employed;
                    var inputCout = document.createElement('input');
                    inputCout.type = "text";
                    inputCout.className = "form-control";
                    var row = document.createElement('td');
                    var cols = document.createElement('tr');
                    row.innerHTML = value.id;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);


                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.name;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    } else if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.sex;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout)
                        tbodyCout.appendChild(cols);

                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Departement;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.datecmd;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                    var validate_input = true;
                    var dataValidate = {};
                    inputCout.onchange = function () {

                        if (isNaN(parseInt(inputCout.value))) {
                            inputCout.style = "border:1px solid red;";
                            validate_input = false;
                        } else {
                            inputCout.style = "border:1px solid silver;";
                            validate_input = true;

                            if (dataInputTab.length == 0) {
                                dataValidate.id = ctr;
                                dataValidate.cout = parseFloat(inputCout.value);
                                dataValidate.categorie = $scope.categFacture;
                                dataInputTab.push(dataValidate);
                                ctr++;
                                // alert("Ctr :" + ctr);
                            } else {
                                dataValidate.cout = parseFloat(inputCout.value);
                                //alert("No Empty");
                                var index = dataInputTab.indexOf(dataValidate);
                                if (index > -1) {

                                    dataInputTab[index] = dataValidate;
                                } else {
                                    dataValidate.id = ctr;
                                    dataInputTab.push(dataValidate);
                                    ctr++;
                                }
                            }
                            //alert("Okay");
                        }
                        console.log("State Validate :", JSON.stringify(dataInputTab));
                    }

                    _added = false;
                    console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
                }

            })
        } else {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#ca' + employed.id).checked = false;
                var tbodyCout = document.querySelector('#idTabCout');
                tbodyCout.innerHTML = "";
            });
        }
    }
    $scope.checkAllInvoiceContractor = function () {
        var tbodyCout = document.querySelector('#idTabCout');
        var checkAll = document.querySelector('#check_co');
        if (checkAll.checked == true) {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#co' + employed.id).checked = true;
                var tbodyCout = document.querySelector('#idTabCout');
                _added = false;
                //  alert("Selected Cmd");
                if ($scope.ListBCmd.length == 0) {
                    // alert('Vide');
                    $scope.ListBCmd.push(employed);
                    idBonTab.push(employed.id);
                    _added = true;

                } else {
                    //   alert("Rempli");
                    var index = $scope.ListBCmd.indexOf(employed);

                    if (index > -1) {
                        $scope.ListBCmd.splice(index, 1);
                        _added = false;
                    } else {
                        $scope.ListBCmd.push(employed);
                        _added = true;
                        idBonTab.push(employed.id);

                    }
                    //  alert($scope.ListBCmd);


                }
                //alert(employed.Employed.name);
                if (_added) {
                    //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                    //  alert($scope.categFacture);
                    document.querySelector('#sendFacture').disabled = false;
                    //alert("Button actived...");
                    var value = {};
                    value = employed;
                    var inputCout = document.createElement('input');
                    inputCout.type = "text";
                    inputCout.className = "form-control";
                    var row = document.createElement('td');
                    var cols = document.createElement('tr');
                    row.innerHTML = value.id;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);


                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.name;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    } else if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.sex;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout)
                        tbodyCout.appendChild(cols);

                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Departement;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.datecmd;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                    var validate_input = true;
                    var dataValidate = {};
                    inputCout.onchange = function () {

                        if (isNaN(parseInt(inputCout.value))) {
                            inputCout.style = "border:1px solid red;";
                            validate_input = false;
                        } else {
                            inputCout.style = "border:1px solid silver;";
                            validate_input = true;

                            if (dataInputTab.length == 0) {
                                dataValidate.id = ctr;
                                dataValidate.cout = parseFloat(inputCout.value);
                                dataValidate.categorie = $scope.categFacture;
                                dataInputTab.push(dataValidate);
                                ctr++;
                                // alert("Ctr :" + ctr);
                            } else {
                                dataValidate.cout = parseFloat(inputCout.value);
                                //alert("No Empty");
                                var index = dataInputTab.indexOf(dataValidate);
                                if (index > -1) {

                                    dataInputTab[index] = dataValidate;
                                } else {
                                    dataValidate.id = ctr;
                                    dataInputTab.push(dataValidate);
                                    ctr++;
                                }
                            }
                            //alert("Okay");
                        }
                        console.log("State Validate :", JSON.stringify(dataInputTab));
                    }

                    _added = false;
                    console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
                }

            })
        } else {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#co' + employed.id).checked = false;
                var tbodyCout = document.querySelector('#idTabCout');
                tbodyCout.innerHTML = "";
            });
        }
    }


    $scope.selectedcmd5 = function (employed) {

        var tbodyCout = document.querySelector('#idTabCout');
        if (document.querySelector('#co' + employed.id).checked == false) {

            for (var i = 0; i < $scope.ListBCmd.length; i++) {
                if ($scope.ListBCmd[i].id == employed.id) {
                    $scope.ListBCmd.splice(i, 1);
                    $('#idTabCout tr:eq(' + i + ')').remove();


                }
            }
            //$('#idTabCout').remove();




        } else {

            var tbodyCout = document.querySelector('#idTabCout');
            _added = false;
            //  alert("Selected Cmd");
            if ($scope.ListBCmd.length == 0) {
                // alert('Vide');
                $scope.ListBCmd.push(employed);
                idBonTab.push(employed.id);
                _added = true;

            } else {
                //   alert("Rempli");
                var index = $scope.ListBCmd.indexOf(employed);

                if (index > -1) {
                    $scope.ListBCmd.splice(index, 1);
                    _added = false;
                } else {
                    $scope.ListBCmd.push(employed);
                    _added = true;
                    idBonTab.push(employed.id);

                }
                //  alert($scope.ListBCmd);


            }
            //alert(employed.Employed.name);
            if (_added) {
                //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                //  alert($scope.categFacture);
                document.querySelector('#sendFacture').disabled = false;
                //alert("Button actived...");
                var value = {};
                value = employed;
                var inputCout = document.createElement('input');
                inputCout.type = "text";
                inputCout.className = "form-control";
                var row = document.createElement('td');
                var cols = document.createElement('tr');
                row.innerHTML = value.id;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);


                if ($scope.categFacture.toString().trim() == "Dependents") {

                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Employee") {

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.name;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Dependents") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                } else if ($scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.sex;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout)
                    tbodyCout.appendChild(cols);

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Departement;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                var row = document.createElement('td');
                row.innerHTML = value.datecmd;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);
                var validate_input = true;
                var dataValidate = {};
                inputCout.onchange = function () {

                    if (isNaN(parseInt(inputCout.value))) {
                        inputCout.style = "border:1px solid red;";
                        validate_input = false;
                    } else {
                        inputCout.style = "border:1px solid silver;";
                        validate_input = true;

                        if (dataInputTab.length == 0) {
                            dataValidate.id = ctr;
                            dataValidate.cout = parseFloat(inputCout.value);
                            dataValidate.categorie = $scope.categFacture;
                            dataInputTab.push(dataValidate);
                            ctr++;
                            // alert("Ctr :" + ctr);
                        } else {
                            dataValidate.cout = parseFloat(inputCout.value);
                            //alert("No Empty");
                            var index = dataInputTab.indexOf(dataValidate);
                            if (index > -1) {

                                dataInputTab[index] = dataValidate;
                            } else {
                                dataValidate.id = ctr;
                                dataInputTab.push(dataValidate);
                                ctr++;
                            }
                        }
                        //alert("Okay");
                    }
                    // console.log("State Validate :", JSON.stringify(dataInputTab));
                }

                _added = false;
                //  console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
            }
        }



    }
    $scope.selectedcmd4 = function (employed) {

        var tbodyCout = document.querySelector('#idTabCout');
        if (document.querySelector('#ca' + employed.id).checked == false) {

            for (var i = 0; i < $scope.ListBCmd.length; i++) {
                if ($scope.ListBCmd[i].id == employed.id) {
                    $scope.ListBCmd.splice(i, 1);
                    $('#idTabCout tr:eq(' + i + ')').remove();


                }
            }
            //$('#idTabCout').remove();




        } else {

            var tbodyCout = document.querySelector('#idTabCout');
            _added = false;
            //  alert("Selected Cmd");
            if ($scope.ListBCmd.length == 0) {
                // alert('Vide');
                $scope.ListBCmd.push(employed);
                idBonTab.push(employed.id);
                _added = true;

            } else {
                //   alert("Rempli");
                var index = $scope.ListBCmd.indexOf(employed);

                if (index > -1) {
                    $scope.ListBCmd.splice(index, 1);
                    _added = false;
                } else {
                    $scope.ListBCmd.push(employed);
                    _added = true;
                    idBonTab.push(employed.id);

                }
                //  alert($scope.ListBCmd);


            }
            //alert(employed.Employed.name);
            if (_added) {
                //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                //  alert($scope.categFacture);
                document.querySelector('#sendFacture').disabled = false;
                //alert("Button actived...");
                var value = {};
                value = employed;
                var inputCout = document.createElement('input');
                inputCout.type = "text";
                inputCout.className = "form-control";
                var row = document.createElement('td');
                var cols = document.createElement('tr');
                row.innerHTML = value.id;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);


                if ($scope.categFacture.toString().trim() == "Dependents") {

                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Employee") {

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.name;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Dependents") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                } else if ($scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.sex;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout)
                    tbodyCout.appendChild(cols);

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Departement;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                var row = document.createElement('td');
                row.innerHTML = value.datecmd;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);
                var validate_input = true;
                var dataValidate = {};
                inputCout.onchange = function () {

                    if (isNaN(parseInt(inputCout.value))) {
                        inputCout.style = "border:1px solid red;";
                        validate_input = false;
                    } else {
                        inputCout.style = "border:1px solid silver;";
                        validate_input = true;

                        if (dataInputTab.length == 0) {
                            dataValidate.id = ctr;
                            dataValidate.cout = parseFloat(inputCout.value);
                            dataValidate.categorie = $scope.categFacture;
                            dataInputTab.push(dataValidate);
                            ctr++;
                            // alert("Ctr :" + ctr);
                        } else {
                            dataValidate.cout = parseFloat(inputCout.value);
                            //alert("No Empty");
                            var index = dataInputTab.indexOf(dataValidate);
                            if (index > -1) {

                                dataInputTab[index] = dataValidate;
                            } else {
                                dataValidate.id = ctr;
                                dataInputTab.push(dataValidate);
                                ctr++;
                            }
                        }
                        //alert("Okay");
                    }
                    // console.log("State Validate :", JSON.stringify(dataInputTab));
                }

                _added = false;
                //  console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
            }
        }



    }
    $scope.selectedcmd3 = function (employed) {

        var tbodyCout = document.querySelector('#idTabCout');
        alert(document.querySelector('#v' + employed.id).checked);
        if (document.querySelector('#v' + employed.id).checked == false) {

            for (var i = 0; i < $scope.ListBCmd.length; i++) {
                if ($scope.ListBCmd[i].id == employed.id) {
                    $scope.ListBCmd.splice(i, 1);
                    $('#idTabCout tr:eq(' + i + ')').remove();


                }
            }
            //$('#idTabCout').remove();




        } else {

            var tbodyCout = document.querySelector('#idTabCout');
            _added = false;
            //  alert("Selected Cmd");
            if ($scope.ListBCmd.length == 0) {
                // alert('Vide');
                $scope.ListBCmd.push(employed);
                idBonTab.push(employed.id);
                _added = true;

            } else {
                //   alert("Rempli");
                var index = $scope.ListBCmd.indexOf(employed);

                if (index > -1) {
                    $scope.ListBCmd.splice(index, 1);
                    _added = false;
                } else {
                    $scope.ListBCmd.push(employed);
                    _added = true;
                    idBonTab.push(employed.id);

                }
                //  alert($scope.ListBCmd);


            }
            //alert(employed.Employed.name);
            if (_added) {
                //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                //  alert($scope.categFacture);
                document.querySelector('#sendFacture').disabled = false;
                //alert("Button actived...");
                var value = {};
                value = employed;
                var inputCout = document.createElement('input');
                inputCout.type = "text";
                inputCout.className = "form-control";
                var row = document.createElement('td');
                var cols = document.createElement('tr');
                row.innerHTML = value.id;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);


                if ($scope.categFacture.toString().trim() == "Dependents") {

                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Employee") {

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.name;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Dependents") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                } else if ($scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.sex;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout)
                    tbodyCout.appendChild(cols);

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Departement;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                var row = document.createElement('td');
                row.innerHTML = value.datecmd;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);
                var validate_input = true;
                var dataValidate = {};
                inputCout.onchange = function () {

                    if (isNaN(parseInt(inputCout.value))) {
                        inputCout.style = "border:1px solid red;";
                        validate_input = false;
                    } else {
                        inputCout.style = "border:1px solid silver;";
                        validate_input = true;

                        if (dataInputTab.length == 0) {
                            dataValidate.id = ctr;
                            dataValidate.cout = parseFloat(inputCout.value);
                            dataValidate.categorie = $scope.categFacture;
                            dataInputTab.push(dataValidate);
                            ctr++;
                            // alert("Ctr :" + ctr);
                        } else {
                            dataValidate.cout = parseFloat(inputCout.value);
                            //alert("No Empty");
                            var index = dataInputTab.indexOf(dataValidate);
                            if (index > -1) {

                                dataInputTab[index] = dataValidate;
                            } else {
                                dataValidate.id = ctr;
                                dataInputTab.push(dataValidate);
                                ctr++;
                            }
                        }
                        //alert("Okay");
                    }
                    // console.log("State Validate :", JSON.stringify(dataInputTab));
                }

                _added = false;
                //  console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
            }
        }



    }
    $scope.selectedcmd2 = function (employed) {

        var tbodyCout = document.querySelector('#idTabCout');
   
        if (document.querySelector('#d' + employed.id).checked == false) {

            for (var i = 0; i < $scope.ListBCmd.length; i++) {
                if ($scope.ListBCmd[i].id == employed.id) {
                    $scope.ListBCmd.splice(i, 1);
                    $('#idTabCout tr:eq(' + i + ')').remove();


                }
            }
            //$('#idTabCout').remove();




        } else {

            var tbodyCout = document.querySelector('#idTabCout');
            _added = false;
            //  alert("Selected Cmd");
            if ($scope.ListBCmd.length == 0) {
                // alert('Vide');
                $scope.ListBCmd.push(employed);
                idBonTab.push(employed.id);
                _added = true;

            } else {
                //   alert("Rempli");
                var index = $scope.ListBCmd.indexOf(employed);

                if (index > -1) {
                    $scope.ListBCmd.splice(index, 1);
                    _added = false;
                } else {
                    $scope.ListBCmd.push(employed);
                    _added = true;
                    idBonTab.push(employed.id);

                }
                //  alert($scope.ListBCmd);


            }
            //alert(employed.Employed.name);
            if (_added) {
                //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                //  alert($scope.categFacture);
                document.querySelector('#sendFacture').disabled = false;
                //alert("Button actived...");
                var value = {};
                value = employed;
                var inputCout = document.createElement('input');
                inputCout.type = "text";
                inputCout.className = "form-control";
                var row = document.createElement('td');
                var cols = document.createElement('tr');
                row.innerHTML = value.id;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);


                if ($scope.categFacture.toString().trim() == "Dependents") {

                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Employee") {

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.name;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Dependents") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                } else if ($scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.sex;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout)
                    tbodyCout.appendChild(cols);

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Departement;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                var row = document.createElement('td');
                row.innerHTML = value.datecmd;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);
                var validate_input = true;
                var dataValidate = {};
                inputCout.onchange = function () {

                    if (isNaN(parseInt(inputCout.value))) {
                        inputCout.style = "border:1px solid red;";
                        validate_input = false;
                    } else {
                        inputCout.style = "border:1px solid silver;";
                        validate_input = true;

                        if (dataInputTab.length == 0) {
                            dataValidate.id = ctr;
                            dataValidate.cout = parseFloat(inputCout.value);
                            dataValidate.categorie = $scope.categFacture;
                            dataInputTab.push(dataValidate);
                            ctr++;
                            // alert("Ctr :" + ctr);
                        } else {
                            dataValidate.cout = parseFloat(inputCout.value);
                            //alert("No Empty");
                            var index = dataInputTab.indexOf(dataValidate);
                            if (index > -1) {

                                dataInputTab[index] = dataValidate;
                            } else {
                                dataValidate.id = ctr;
                                dataInputTab.push(dataValidate);
                                ctr++;
                            }
                        }
                        //alert("Okay");
                    }
                    // console.log("State Validate :", JSON.stringify(dataInputTab));
                }

                _added = false;
                //  console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
            }
        }



    }
    $scope.selectedcmd = function (employed) {
        
        var tbodyCout = document.querySelector('#idTabCout');
        
        if (document.querySelector('#c' + employed.id).checked == false) {
            
            for (var i = 0; i < $scope.ListBCmd.length; i++) {
                if ($scope.ListBCmd[i].id == employed.id) {
                    $scope.ListBCmd.splice(i, 1);
                    $('#idTabCout tr:eq(' + i + ')').remove();
                    
               
                }
            }
            //$('#idTabCout').remove();

          

            
        } else {
         
            var tbodyCout = document.querySelector('#idTabCout');
            _added = false;
            //  alert("Selected Cmd");
            if ($scope.ListBCmd.length == 0) {
                // alert('Vide');
                $scope.ListBCmd.push(employed);
                idBonTab.push(employed.id);
                _added = true;

            } else {
                //   alert("Rempli");
                var index = $scope.ListBCmd.indexOf(employed);

                if (index > -1) {
                    $scope.ListBCmd.splice(index, 1);
                    _added = false;
                } else {
                    $scope.ListBCmd.push(employed);
                    _added = true;
                    idBonTab.push(employed.id);

                }
                //  alert($scope.ListBCmd);


            }
            //alert(employed.Employed.name);
            if (_added) {
                //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                //  alert($scope.categFacture);
                document.querySelector('#sendFacture').disabled = false;
                //alert("Button actived...");
                var value = {};
                value = employed;
                var inputCout = document.createElement('input');
                inputCout.type = "text";
                inputCout.className = "form-control";
                var row = document.createElement('td');
                var cols = document.createElement('tr');
                row.innerHTML = value.id;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);
                

                if ($scope.categFacture.toString().trim() == "Dependents") {
                    
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Employee") {
                  
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.name;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.nameAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Contractor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Dependents") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                } else if ($scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.sex;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Casual") {
                    var row = document.createElement('td');
                    row.innerHTML = value.sexeAuthor;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }

                if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout)
                    tbodyCout.appendChild(cols);

                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Departement;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.Employed.ID_Succursale;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                if ($scope.categFacture.toString().trim() == "Visitor") {
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                }
                var row = document.createElement('td');
                row.innerHTML = value.datecmd;
                cols.appendChild(row);
                cols.appendChild(inputCout);
                tbodyCout.appendChild(cols);
                var validate_input = true;
                var dataValidate = {};
                inputCout.onchange = function () {

                    if (isNaN(parseInt(inputCout.value))) {
                        inputCout.style = "border:1px solid red;";
                        validate_input = false;
                    } else {
                        inputCout.style = "border:1px solid silver;";
                        validate_input = true;

                        if (dataInputTab.length == 0) {
                            dataValidate.id = ctr;
                            dataValidate.cout = parseFloat(inputCout.value);
                            dataValidate.categorie = $scope.categFacture;
                            dataInputTab.push(dataValidate);
                            ctr++;
                            // alert("Ctr :" + ctr);
                        } else {
                            dataValidate.cout = parseFloat(inputCout.value);
                            //alert("No Empty");
                            var index = dataInputTab.indexOf(dataValidate);
                            if (index > -1) {

                                dataInputTab[index] = dataValidate;
                            } else {
                                dataValidate.id = ctr;
                                dataInputTab.push(dataValidate);
                                ctr++;
                            }
                        }
                        //alert("Okay");
                    }
                    // console.log("State Validate :", JSON.stringify(dataInputTab));
                }

                _added = false;
                //  console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
            }
        }
       
      
      
    }
    $scope.sendFacture = function () {
        var _validationSending =true;
        angular.forEach(dataInputTab, function (value, key) {
            if (isNaN(parseFloat(value.cout))) {
                _validationSending = false;
            }
        });
        if (_validationSending==false) {
            alert("Field(s) is empty...");
        } else {
            var TabCmd = [];
           // alert("Champs sont prets");
            angular.forEach(dataInputTab, function (value, key) {
                value.id = idBonTab[key];
                TabCmd.push(value);
            });
            document.querySelector('#fileBenef').value = JSON.stringify(TabCmd);
            if (document.querySelector('#fileBenef').value.toString().trim()!="") {
                document.querySelector('#btnSubmit').click();
            }
            // 
          //  console.log(JSON.stringify(TabCmd));
        }
    }
        
    
    $scope.exportationData = function () {
       
        var category = $scope.cboCategExport.toString().trim();
        if ($scope.categFacture=="Employee") {
            switch (category) {
                case 'CSV':
                    var dataCSV = "";
                    console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        dataCSV += value.id + "," + value.datecmd + "," + value.Employed.name + "," + value.Employed.sex + "," + value.Employed.phone +
                                   "," + value.Employed.ID_Succursale + "," + value.Employed.ID_Departement + "," + value.idHealth + '\r' + '\n';
                    });
                    //console.log("Export Data :", dataCSV);
                    FactoryHome.setExoprtCSV(dataCSV).then(function (response) {
                        if (response.toString().trim() != "") {
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                case 'PDF':
                    var dataPDF = [];
                    //console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        var object = {
                            idBon: value.id,
                            datecmd: value.datecmd,
                            nameEmployed: value.Employed.name,
                            sex: value.Employed.sex,
                            phone: value.Employed.phone,
                            company: value.Employed.ID_Succursale,
                            department: value.Employed.ID_Departement,
                            Health: value.idHealth,
                            Category:"Employee"
                        };

                        dataPDF.push(object);

                    });
                    console.log("Export Data :", JSON.stringify(dataPDF));
                    FactoryHome.setExoprtPDF(dataPDF).then(function (response) {
                        if (response.toString().trim() != "") {
                            //window.open(response);
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                default:

            }
        } else if($scope.categFacture=="Dependents"){
            switch (category) {
                case 'CSV':
                    var dataCSV = "";
                    console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        dataCSV += value.id + "," + value.datecmd + "," + value.Employed.name + "," + value.Employed.sex + "," + value.Employed.phone +
                                   "," + value.Employed.ID_Succursale + "," + value.Employed.ID_Departement + "," + value.idHealth + '\r' + '\n';
                    });
                    //console.log("Export Data :", dataCSV);
                    FactoryHome.setExoprtCSV(dataCSV).then(function (response) {
                        if (response.toString().trim() != "") {
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                case 'PDF':
                    var dataPDF = [];
                    //console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        var object = {
                            idBon: value.id,
                            datecmd: value.datecmd,
                            nameAuthor:value.nameAuthor,
                            nameEmployed: value.Employed.name,
                            sex: value.sexeAuthor,
                            phone: "-",
                            company: value.Employed.ID_Succursale,
                            department: value.Employed.ID_Departement,
                            Health: value.idHealth,
                            Category: "Dependents"

                        };

                        dataPDF.push(object);

                    });
                    console.log("Export Data :", JSON.stringify(dataPDF));
                    FactoryHome.setExoprtPDF(dataPDF).then(function (response) {
                        if (response.toString().trim() != "") {
                            //window.open(response);
                           document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                default:

            }
        }
        if ($scope.categFacture == "Visitor") {
            
            switch (category) {
                case 'CSV':
                    var dataCSV = "";
                    console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        dataCSV += value.id + "," + value.datecmd + "," + value.Employed.name + "," + value.Employed.sex + "," + value.Employed.phone +
                                   "," + value.Employed.ID_Succursale + "," + value.Employed.ID_Departement + "," + value.idHealth + '\r' + '\n';
                    });
                    //console.log("Export Data :", dataCSV);
                    FactoryHome.setExoprtCSV(dataCSV).then(function (response) {
                        if (response.toString().trim() != "") {
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                case 'PDF':
                    var dataPDF = [];
                    //console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        var object = {
                            idBon: value.id,
                            datecmd: value.datecmd,
                            nameAuthor: value.nameAuthor,
                            //nameEmployed: value.Employed.name,
                            sex: value.sexeAuthor,
                           // phone: "-",
                            company: value.Employed.ID_Succursale,
                       //     department: value.Employed.ID_Departement,
                            Health: value.idHealth,
                            Category: "Visitor"

                        };

                        dataPDF.push(object);

                    });
                    console.log("Export Data :", JSON.stringify(dataPDF));
                    FactoryHome.setExoprtPDF(dataPDF).then(function (response) {
                        if (response.toString().trim() != "") {
                            //window.open(response);
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                default:

            }
        }

        if ($scope.categFacture == "Casual") {

            switch (category) {
                case 'CSV':
                    var dataCSV = "";
                    console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        dataCSV += value.id + "," + value.datecmd + "," + value.Employed.name + "," + value.Employed.sex + "," + value.Employed.phone +
                                   "," + value.Employed.ID_Succursale + "," + value.Employed.ID_Departement + "," + value.idHealth + '\r' + '\n';
                    });
                    //console.log("Export Data :", dataCSV);
                    FactoryHome.setExoprtCSV(dataCSV).then(function (response) {
                        if (response.toString().trim() != "") {
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                case 'PDF':
                    var dataPDF = [];
                    //console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        var object = {
                            idBon: value.id,
                            datecmd: value.datecmd,
                            nameAuthor: value.nameAuthor,
                            //nameEmployed: value.Employed.name,
                            sex: value.sexeAuthor,
                            // phone: "-",
                            company: value.Employed.ID_Succursale,
                            //     department: value.Employed.ID_Departement,
                            Health: value.idHealth,
                            Category: "Casual"

                        };

                        dataPDF.push(object);

                    });
                    console.log("Export Data :", JSON.stringify(dataPDF));
                    FactoryHome.setExoprtPDF(dataPDF).then(function (response) {
                        if (response.toString().trim() != "") {
                            //window.open(response);
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                default:

            }
        }
        if ($scope.categFacture == "Contractor") {

            switch (category) {
                case 'CSV':
                    var dataCSV = "";
                    console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        dataCSV += value.id + "," + value.datecmd + "," + value.Employed.name + "," + value.Employed.sex + "," + value.Employed.phone +
                                   "," + value.Employed.ID_Succursale + "," + value.Employed.ID_Departement + "," + value.idHealth + '\r' + '\n';
                    });
                    //console.log("Export Data :", dataCSV);
                    FactoryHome.setExoprtCSV(dataCSV).then(function (response) {
                        if (response.toString().trim() != "") {
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                case 'PDF':
                    var dataPDF = [];
                    //console.log("Hoster :", document.location.hostname);
                    angular.forEach($scope.tabBCmdFacture, function (value, key) {
                        var object = {
                            idBon: value.id,
                            datecmd: value.datecmd,
                            nameAuthor: value.nameAuthor,
                            //nameEmployed: value.Employed.name,
                            sex: value.sexeAuthor,
                            // phone: "-",
                            company: value.Employed.ID_Succursale,
                            //     department: value.Employed.ID_Departement,
                            Health: value.idHealth,
                            Category: "Contractor"

                        };

                        dataPDF.push(object);

                    });
                    console.log("Export Data :", JSON.stringify(dataPDF));
                    FactoryHome.setExoprtPDF(dataPDF).then(function (response) {
                        if (response.toString().trim() != "") {
                            //window.open(response);
                            document.location.href = response;
                            //console.log("RESPONSE :", response);
                        }
                    }, function (error) {
                        console.log("Error Exportation :", error);
                    });
                    break;
                default:

            }
        }
       
    }
    $scope.dategroup = false;
    var exportData = {};
    $scope.exportMCI = function () {
       
        if ($scope.categoryExport!=undefined) {
            if ($scope.categoryExport.toString().trim() == "Choose") {
                var date_begin = document.querySelector('#date1Export').value.toString().trim();
                var date_end = document.querySelector('#date2Export').value.toString().trim();

                var date_beginSplitter = date_begin.toString().split('/');
                var date_endSplitter = date_end.toString().split('/');
                console.log("Date1:", date_begin.toString());
                console.log("Date2:", date_end.toString());
                var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
                var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
                console.log("Formatter :", formatter);

                var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
                var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
                console.log("Date1 :", date1.toLocaleDateString());
                console.log("Date2 :", date2.toLocaleDateString());
                document.querySelector('#btnSendMVI').click();
                //if (date1 < date2) {
                //    document.querySelector('#btnSendMVI').click();
                //} else {

                //    alert("Date 1 is not Superior of Date 2");
                //}

            } else {
                document.querySelector('#btnSendMVI').click();
            }
        } else {
            alert("Choisissez une categorie");
        }
        
    }
    $scope.rmvAction = function () {
        var date_begin = document.querySelector('#datervm1').value.toString().trim();
        var date_end = document.querySelector('#datervm2').value.toString().trim();

        var date_beginSplitter = date_begin.toString().split('/');
        var date_endSplitter = date_end.toString().split('/');
        console.log("Date1:", date_begin.toString());
        console.log("Date2:", date_end.toString());
        var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
        var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
        console.log("Formatter :", formatter);

        var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
        var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
        console.log("Date1 :", date1.toLocaleDateString());
        console.log("Date2 :", date2.toLocaleDateString());
        if (date1 < date2) {
           // document.querySelector('#btnSendMVI').click();
        } else {

            alert("Date 1 is not Superior of Date 2");
        } 
    }
    $scope.listDeparts = []; 
    $scope.changeCompany = function () {
        var id = $scope.cboCompany.toString().trim();
        FactoryHome.getListDepartement(id).then(function (response) {
           // $scope.listDeparts = [];
            $scope.listDeparts = response;
            console.log(JSON.stringify($scope.listDeparts));
            document.querySelector('#rowViewer').style="display:normal"
        }, function (error) {
            alert("Error :" + error);
        });
    }
    $scope.LibDeparts = [];
    $scope.rowdep = false;
    $scope.checkedDepartment = function (department) {
        //alert(department);
        //var dataList = document.querySelector('#dataList');
        //var checkAll = document.querySelector('#checkAll');
   
        //    if (checkAll.checked) {
        //        $scope.rowdep = true;
        //        angular.forEach($scope.listDeparts, function (p, key) {
        //            document.querySelector('#dc' + p.id_depart).checked = true;
                    
        //            $scope.LibDeparts.push(p.code_depart);
        //        })
        //        dataList.value = JSON.stringify($scope.LibDeparts);
        //        alert($scope.LibDeparts.length);
        //    } else {
        //        angular.forEach($scope.listDeparts, function (p, key) {
        //            document.querySelector('#dc' + p.id_depart).checked = false;
        //            //$scope.getListdep(p);
        //        })
        //        $scope.LibDeparts = [];
        //        dataList.value = "";
        //        $scope.rowdep = false;
        //    }

    }

    $scope.LibDeparts = [];
    $scope.CheckAllDepartment = function () {
        var check = document.querySelector('#checkAll');
        if (check.checked == true) {
            $scope.LibDeparts = [];
            $scope.listDeparts.forEach(function (elt) {
                document.querySelector('#dc' + elt.id_depart).checked = true;
                $scope.LibDeparts.push(elt.code_depart);
            })
            document.querySelector('#dataList').value = JSON.stringify($scope.LibDeparts);
        } else {
            $scope.listDeparts.forEach(function (elt) {
                document.querySelector('#dc' + elt.id_depart).checked = false;
                $scope.LibDeparts = [];
                document.querySelector('#dataList').value =""

            })
        }
    }
    $scope.checkedDepartment = function (department) {
        
        var libDepart = department.code_depart;
        if ($scope.LibDeparts.length == 0) {
            $scope.LibDeparts.push(libDepart);
            dataList.value = JSON.stringify($scope.LibDeparts);
        } else {
            var index = $scope.LibDeparts.indexOf(libDepart);
            if (index > -1) {
                $scope.LibDeparts.splice(index, 1);
                if ($scope.LibDeparts.length==0) {
                    document.querySelector('#dataList').value = "";
                } else {
                    document.querySelector('#dataList').value = JSON.stringify($scope.LibDeparts);
                }
            } else {
                $scope.LibDeparts.push(libDepart);
                document.querySelector('#dataList').value = JSON.stringify($scope.LibDeparts);
            }
        }
    }
    $scope.sendExportRMVD = function () {

         if ($scope.cboCompany != undefined && $scope.LibDeparts.length > 0) {
                document.querySelector('#rmvdepBtn').click();
            } else {
                alert("Fields is empty");
            }
        
    }
    $scope.sendExportCompany = function () {
        //var date_begin = document.querySelector('#dateFromSucc').value.toString().trim();
        //var date_end = document.querySelector('#dateToSucc').value.toString().trim();

        //var date_beginSplitter = date_begin.toString().split('/');
        //var date_endSplitter = date_end.toString().split('/');
        //console.log("Date1:", date_begin.toString());
        //console.log("Date2:", date_end.toString());
        //var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
        //var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
        //console.log("Formatter :", formatter);

        //var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
        //var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
        //console.log("Date1 :", date1.toLocaleDateString());
        //console.log("Date2 :", date2.toLocaleDateString());
        //if (date1 < date2) {
        //    alert('Good');
        //} else {

        //    alert("Date 1 is not Superior of Date 2");
        //}
    }
  
    $scope.checkdate = function () {
       // alert(document.querySelector('#dateFormEmployee').value);
        var date_begin = document.querySelector('#dateFormEmployee').value.toString().trim();
        var date_end = document.querySelector('#dateToEmployee').value.toString().trim();

        var date_beginSplitter = date_begin.toString().split('/');
        var date_endSplitter = date_end.toString().split('/');
        console.log("Date1:", date_begin.toString());
        console.log("Date2:", date_end.toString());
        var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
        var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
        console.log("Formatter :", formatter);

        var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
        var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
        console.log("Date1 :", date1.toLocaleDateString());
        console.log("Date2 :", date2.toLocaleDateString());
        if (date1 < date2) {
            //alert('Good');
            document.querySelector('#btnSubmitHMVCE').click();
        } else {

            alert("Date 1 is not Superior of Date 2");
        }
    }
    $scope.checkdate2 = function () {
         var date_begin = document.querySelector('#dateFromDependecy').value.toString().trim();
        var date_end = document.querySelector('#dateToDependecy').value.toString().trim();

        var date_beginSplitter = date_begin.toString().split('/');
        var date_endSplitter = date_end.toString().split('/');
        console.log("Date1:", date_begin.toString());
        console.log("Date2:", date_end.toString());
        var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
        var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
        console.log("Formatter :", formatter);

        var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
        var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
        console.log("Date1 :", date1.toLocaleDateString());
        console.log("Date2 :", date2.toLocaleDateString());
        if (date1 < date2) {
            //alert('Good');
            document.querySelector('#btnSubmitHMVCE2').click();
        } else {

            alert("Date 1 is not Superior of Date 2");
        }
    }
 
    // REPORTING SYSTEM
    $scope.categCompany = function () {
        //alert("OKay");
        var categ = $scope.categoryExport.toString().trim();
        if (categ!="" || categ!=undefined) {
            //alert(categ);
            switch (categ) {
                case "Choose":
                    $scope.dategroup = true;
                    
                    break;
                default:
                    $scope.dategroup = false;

                    break;

            }
        }
    }
    $scope.changeMotif = function () {
        
        if ($scope.tmotif.toString().trim()=="....") {
            document.querySelector('#ctrMotif').style = "display:initial";
        } else {
            document.querySelector('#tmotif2').value = "";
            document.querySelector('#ctrMotif').style = "display:none";

        }
    }
    $scope.resetCasual = function () {
        $scope.idVoucher = undefined;
        $scope.NameCasual = undefined;
        $scope.CompanyCasual = undefined;
        $scope.hospitalCasual = undefined;
        $scope.companyCasual = undefined;
        document.querySelector('#dateStarting').value = "";
        document.querySelector('#dateEnding').value = "";
        $scope.ListCasual = $scope.ListCasualOld;
    }
   
    $scope.searchCasual = function () {
       
        if ($scope.idVoucherCasual != undefined) {
            $scope.tabTemp = [];
            angular.forEach($scope.ListCasual, function (value, key) {
                if (value.idVoucher == $scope.idVoucherCasual.toString().trim()) {
                    $scope.tabTemp.push(value);
                }
            });
            $scope.ListCasual = [];
            $scope.ListCasual = $scope.tabTemp;

        }
        if ($scope.NameCasual!=undefined) {
            $scope.tabTemp = [];
            angular.forEach($scope.ListCasual, function (value, key) {
                if (value.NameCasual == $scope.NameCasual.toString().trim()) {
                    $scope.tabTemp.push(value);
                }
            });
            $scope.ListCasual = [];
            $scope.ListCasual = $scope.tabTemp;
        }
        if ($scope.CompanyCasual != undefined) {
            $scope.tabTemp = [];
            angular.forEach($scope.ListCasual, function (value, key) {
                if (value.CompanyCasual == $scope.CompanyCasual.toString().trim()) {
                    $scope.tabTemp.push(value);
                }
            });
            $scope.ListCasual = [];
            $scope.ListCasual = $scope.tabTemp;
        }
        if ($scope.hospitalCasual != undefined) {
          
            $scope.tabTemp = [];
            angular.forEach($scope.ListCasual, function (value, key) {
                if (value.idHospital == $scope.hospitalCasual.toString().trim()) {
                    $scope.tabTemp.push(value);
                }
            });
            $scope.ListCasual = [];
            $scope.ListCasual = $scope.tabTemp;
        }
        if ($scope.companyCasual != undefined) {
         //   alert($scope.companyCasual.toString().trim());
            $scope.tabTemp = [];
            angular.forEach($scope.ListCasual, function (value, key) {
                if (value.idCompanyVisited == $scope.companyCasual.toString().trim()) {
                    $scope.tabTemp.push(value);
                }
            });
            $scope.ListCasual = [];
            $scope.ListCasual = $scope.tabTemp;
        }
        if (document.querySelector('#dateStarting') != "" && document.querySelector('#dateEnding') != "") {
            $scope.tabTemp = [];
            var date_begin = document.querySelector('#dateStarting').value.toString().trim();
            var date_end = document.querySelector('#dateEnding').value.toString().trim();

            var date_beginSplitter = date_begin.toString().split('/');
            var date_endSplitter = date_end.toString().split('/');
            console.log("Date1:", date_begin.toString());
            console.log("Date2:", date_end.toString());
            var formatter = date_beginSplitter[0] + "/" + date_beginSplitter[1] + "/" + date_beginSplitter[2];
            var date1 = new Date(date_beginSplitter[2], date_beginSplitter[1], date_beginSplitter[0]);
            console.log("Formatter :", formatter);

            var date1 = new Date(parseInt(date_beginSplitter[2]), parseInt(date_beginSplitter[1] - 1), parseInt(date_beginSplitter[0]));
            var date2 = new Date(parseInt(date_endSplitter[2]), parseInt(date_endSplitter[1] - 1), parseInt(date_endSplitter[0]));
            console.log("Date1 :", date1.toLocaleDateString());
            console.log("Date2 :", date2.toLocaleDateString());
            var date3 = new Date(2017, 2, 12);
            if (date1<date2) {
                angular.forEach($scope.ListCasual, function (value, key) {
                    var dateSplitter = value.DateCasual.toString().trim().split('/');
                    var dateFound = new Date(parseInt(dateSplitter[2]), parseInt(dateSplitter[1] - 1), parseInt(dateSplitter[0]));
                    if (dateFound >= date1 && dateFound <= date2) {
                        $scope.tabTemp.push(value);
                    }
                });
                $scope.ListCasual = [];
                $scope.ListCasual = $scope.tabTemp;
                console.log("TEMP :",$scope.tabTemp.length);
            } else {
                //alert("No.")
            }
        }
    }
    $scope.fillCasual = function () {
        FactoryHome.getListCasuals().then(function (response) {
            $scope.ListCasual = response;
            $scope.ListCasualOld = response;
            console.log("Casual LIST:", JSON.stringify($scope.ListCasual));
        }, function (error) {
            console.log("Error Casual:", error);
        });
    }
    $scope.exportationCasual = function () {
        switch ($scope.CasualExport.toString().trim()) {
            case 'CSV':
                var dataCSV = "";
                console.log("Hoster :", document.location.hostname);
                angular.forEach($scope.tabBCmdFacture, function (value, key) {
                    dataCSV += value.id + "," + value.datecmd + "," + value.Employed.name + "," + value.Employed.sex + "," + value.Employed.phone +
                               "," + value.Employed.ID_Succursale + "," + value.Employed.ID_Departement + "," + value.idHealth + '\r' + '\n';
                });
                //console.log("Export Data :", dataCSV);
                FactoryHome.setExoprtCSV(dataCSV).then(function (response) {
                    if (response.toString().trim() != "") {
                        document.location.href = response;
                        //console.log("RESPONSE :", response);
                    }
                }, function (error) {
                    console.log("Error Exportation :", error);
                });
                break;
            case 'PDF':
                var dataPDF = [];
                //console.log("Hoster :", document.location.hostname);
                angular.forEach($scope.ListCasual, function (value, key) {
              
                    var object = {
                        idBon: value.idVoucher,
                        datecmd: value.DateCasual,
                        nameAuthor: value.NameCasual,
                        sex: value.CompanyCasual,
                        phone: value.Cause,
                        company: value.Motif,
                        department: value.idCompanyVisited,
                        Health: value.idHospital,
                        Category:"CasualInfo"
                    };

                    dataPDF.push(object);
                    console.log(JSON.stringify(dataPDF))
                });
                FactoryHome.setExoprtPDF(dataPDF).then(function (response) {
                    if (response.toString().trim() != "") {
                        //window.open(response);
                        document.location.href = response;
                        //console.log("RESPONSE :", response);
                    }
                }, function (error) {
                    console.log("Error Exportation :", error);
                });
                break;
            default:

        }
    }
    $scope.casualSelected = function (casual) {
        console.log(JSON.strcasual);
        document.querySelector('#NameCasual').value = casual.NameCasual;
        var btn = document.querySelector('#btnModalContractor');
        btn.click();
    }
    $scope.SelectPriority = function () {
        $scope.priorityUser = $scope.cbopriority;

    }
    $scope.getNameEmployee = function (obj) {


        document.querySelector('#textCurrent').innerHTML = obj.name;
        var user = {};
        $scope.ListEmployeeUsers.forEach(function (e) {
            if (e.name == obj.name) {
                user = e;
            }
        })

        $scope.idUser = user.id;
        $scope.idCompanyUser = user.idCompany;
        console.log(user)

    }
    $scope.SelectUserEmployee = function () {
        
        
    }
    $scope.SelectUserEmployeeExist = function () {
        var index = document.getElementById('cbouserEmployee').selectedIndex;
        var user = $scope.ListEmployeeUsers[index];
        $scope.idUser = user.id;
        $scope.idCompanyUser = user.idCompany;
        $scope.username = user.user;
        $scope.cbopriority = user.priority;
        document.querySelector('#selectStatut').value = user.status;
        console.log(JSON.stringify($scope.ListEmployeeUsers[index]));
        document.querySelector('#statutsSystem').value = user.status;
        document.querySelector('#pwd2').value = "";
    }
    $scope.confirmPassword = function () {
        var pwd1 = document.querySelector('#pwd1');
        var pwd2 = document.querySelector('#pwd2');
        var username = document.querySelector('#username');
        if (pwd1.value == pwd2.value && (pwd1.value != "" && pwd2.value != "")) {
            if (username.value.trim().toString() == "" || $scope.cbopriority.trim().toString()=="") {
                alert('Fields empty');
            } else {
               
                document.querySelector('#btnLogger').click();
            }
        } else {
            alert('Password is not correct');
        }
    }
    console.log("Controller Run");
    $scope.usersVisible = false;
    $scope.usersFormVisible = false;

    $scope.SelectCompanyUsers = function () {
       
        try {
            document.querySelector('#alertSubSucc').style = "display:none";
            document.querySelector('#pwd1').value = "";
            document.querySelector('#pwd2').value = "";
            document.querySelector('#username').value = "";


        } catch (e) {

        }
        var selectUser = document.getElementById('select_users');
        var textgetter = selectUser.options[selectUser.selectedIndex].value;
        var text = textgetter.toString().trim();
        var url = "listusersaccount/" + text;
        console.log("URL :", url);
        $http.get(url)
                      .success(function (data, status) {
                          console.log(" List :", JSON.stringify(data));
                          $scope.ListEmployeeUsers = data;
                          $scope.usersVisible = true;
                          $scope.usersFormVisible = true;


                      })
                      .error(function (data, status) {
                          console.error(" Error :", data);

                      });
        
        
    }
    $scope.SelectCompanyUsersExist = function () {
        
        document.querySelector('#pwd1').value = ""
       // alert(document.querySelector('#pwd1').value);

        try {
            
            document.querySelector('#alertSubSucc').style.display = "none";
            document.querySelector('#pwd1').value = "";
            document.querySelector('#pwd2').value = "";
            document.querySelector('#username').value = "";


        } catch (e) {
          //  alert(e);
        }
        //alert(document.querySelector('#pwd1').value);
        var selectUser = document.getElementById('select_users');
        var textgetter = selectUser.options[selectUser.selectedIndex].value;
        var text = textgetter.toString().trim();
        var url = "listusersaccountexist/" + text;
        console.log("URL :", url);
        $http.get(url)
                      .success(function (data, status) {
                          console.log(" List :", JSON.stringify(data));
                          $scope.ListEmployeeUsers = data;
                          $scope.usersVisible = true;
                          $scope.usersFormVisible = true;


                      })
                      .error(function (data, status) {
                          console.error(" Error :", data);

                      });


    }
    $scope.tabCost = [];
    $scope.choiceCost = function (obj) {
       // alert(obj);
        switch (obj) {
            case 'all':
                if (document.querySelector('#all').checked) {
                    document.querySelector('#check1').checked = true;
                    document.querySelector('#check2').checked = true;
                    document.querySelector('#check3').checked = true;
                    document.querySelector('#check4').checked = true;
                    $scope.tabCost = [];
                    $scope.tabCost = ['beneficiairies', 'casual', 'contractor','visitor'];
                    console.log($scope.tabCost);
                    $scope.textCostFunc($scope.tabCost)
                } else {
                    document.querySelector('#check1').checked = false;
                    document.querySelector('#check2').checked = false;
                    document.querySelector('#check3').checked = false;
                    document.querySelector('#check4').checked = false;
                    $scope.tabCost = [];
                    $scope.textCostFunc("")
                    console.log($scope.tabCost);
                }
                
                break;
            case 'beneficiairies':
               
                    if (!document.querySelector('#check1').checked) {
                        angular.forEach($scope.tabCost, function (elt, key) {
                            if (elt == "beneficiairies") {
                                //  delete $scope.tabCost[key];
                                $scope.tabCost.splice(key, 1);
                                console.log(elt)
                            }
                        });
                        $scope.textCostFunc($scope.tabCost)
                        console.log($scope.tabCost);
                    } else {
                        $scope.tabCost.push('beneficiairies');
                        $scope.textCostFunc($scope.tabCost)
                        console.log($scope.tabCost);
                    }
                  
                
                    break;


            case 'contractor':
                if (!document.querySelector('#check3').checked) {
                    angular.forEach($scope.tabCost, function (elt, key) {
                        if (elt == "contractor") {
                            //  delete $scope.tabCost[key];
                            $scope.tabCost.splice(key, 1);
                            console.log(elt)
                        }
                    });
                    $scope.textCostFunc($scope.tabCost)
                    console.log($scope.tabCost);
                } else {
                    $scope.tabCost.push('contractor');
                    $scope.textCostFunc($scope.tabCost)
                    console.log($scope.tabCost);
                   
                }


                break;
            case 'casual':

                if (!document.querySelector('#check2').checked) {
                    angular.forEach($scope.tabCost, function (elt, key) {
                        if (elt == "casual") {
                            //  delete $scope.tabCost[key];
                            $scope.tabCost.splice(key, 1);
                            console.log(elt)
                        }
                    });
                    $scope.textCostFunc($scope.tabCost)
                    console.log($scope.tabCost);
                } else {
                    $scope.tabCost.push('casual');
                    console.log($scope.tabCost);
                    $scope.textCostFunc($scope.tabCost)
                }


                break;



            case 'visitor':

                if (!document.querySelector('#check4').checked) {
                    angular.forEach($scope.tabCost, function (elt, key) {
                        if (elt == "visitor") {
                            //  delete $scope.tabCost[key];
                            $scope.tabCost.splice(key, 1);
                            console.log(elt)
                        }
                    });
                    $scope.textCostFunc($scope.tabCost)
                    console.log($scope.tabCost);
                } else {
                    $scope.tabCost.push('visitor');
                    console.log($scope.tabCost);
                    $scope.textCostFunc($scope.tabCost)
                }


                break;

            default:
                break;

        }
        
    }
    $scope.textCostFunc = function (tabs) {
        document.querySelector('#textCost').value = tabs;
    }
    $scope.datalist = function (d) {
        alert("LAMA");
    }
    $scope.updateVisitor = function () {
    
        var object = {
            id: document.querySelector('#ivisitor').value,
            name: document.querySelector('#nvisitor').value,
            gender: document.querySelector('#gvisitor').value,
            phone: document.querySelector('#pvisitor').value,
            companyvisitor: document.querySelector('#cvisitor').value,
            motif: document.querySelector('#mvisitor').value
        }
        console.log(object);
        FactoryHome.UpdateVisitor(object).then(function (data) {
            if (data.toString().trim() == "200") {
                document.location.reload();

            }
        }, function (error) {
            console.error(error);
        })
    }
                                                                        
    $scope.checkAllInvoiceEmployee=function(){
        var checkAll = document.querySelector('#check_employee');
        if (checkAll.checked==true) {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#c' + employed.id).checked = true;
                var tbodyCout = document.querySelector('#idTabCout');
                _added = false;
                //  alert("Selected Cmd");
                if ($scope.ListBCmd.length == 0) {
                    // alert('Vide');
                    $scope.ListBCmd.push(employed);
                    idBonTab.push(employed.id);
                    _added = true;

                } else {
                    //   alert("Rempli");
                    var index = $scope.ListBCmd.indexOf(employed);

                    if (index > -1) {
                        $scope.ListBCmd.splice(index, 1);
                        _added = false;
                    } else {
                        $scope.ListBCmd.push(employed);
                        _added = true;
                        idBonTab.push(employed.id);

                    }
                    //  alert($scope.ListBCmd);


                }
                //alert(employed.Employed.name);
                if (_added) {
                    //   $scope.categFacture = ($scope.categFacture.toString().trim() ? "" : "Employee");
                    //  alert($scope.categFacture);
                    document.querySelector('#sendFacture').disabled = false;
                    //alert("Button actived...");
                    var value = {};
                    value = employed;
                    var inputCout = document.createElement('input');
                    inputCout.type = "text";
                    inputCout.className = "form-control";
                    var row = document.createElement('td');
                    var cols = document.createElement('tr');
                    row.innerHTML = value.id;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);


                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.name;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.nameAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Contractor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Dependents") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    } else if ($scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.sex;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Casual") {
                        var row = document.createElement('td');
                        row.innerHTML = value.sexeAuthor;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }

                    if ($scope.categFacture.toString().trim() == "Dependents" || $scope.categFacture.toString().trim() == "Employee") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout)
                        tbodyCout.appendChild(cols);

                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Departement;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.Employed.ID_Succursale;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    if ($scope.categFacture.toString().trim() == "Visitor") {
                        var row = document.createElement('td');
                        row.innerHTML = value.datecmd;
                        cols.appendChild(row);
                        cols.appendChild(inputCout);
                        tbodyCout.appendChild(cols);
                    }
                    var row = document.createElement('td');
                    row.innerHTML = value.datecmd;
                    cols.appendChild(row);
                    cols.appendChild(inputCout);
                    tbodyCout.appendChild(cols);
                    var validate_input = true;
                    var dataValidate = {};
                    inputCout.onchange = function () {

                        if (isNaN(parseInt(inputCout.value))) {
                            inputCout.style = "border:1px solid red;";
                            validate_input = false;
                        } else {
                            inputCout.style = "border:1px solid silver;";
                            validate_input = true;

                            if (dataInputTab.length == 0) {
                                dataValidate.id = ctr;
                                dataValidate.cout = parseFloat(inputCout.value);
                                dataValidate.categorie = $scope.categFacture;
                                dataInputTab.push(dataValidate);
                                ctr++;
                                // alert("Ctr :" + ctr);
                            } else {
                                dataValidate.cout = parseFloat(inputCout.value);
                                //alert("No Empty");
                                var index = dataInputTab.indexOf(dataValidate);
                                if (index > -1) {

                                    dataInputTab[index] = dataValidate;
                                } else {
                                    dataValidate.id = ctr;
                                    dataInputTab.push(dataValidate);
                                    ctr++;
                                }
                            }
                            //alert("Okay");
                        }
                        console.log("State Validate :", JSON.stringify(dataInputTab));
                    }

                    _added = false;
                    console.log("Table Checked :", JSON.stringify($scope.ListBCmd));
                }

            })
        } else {
            $scope.tabBCmdFacture.forEach(function (employed, key) {
                document.querySelector('#c' + employed.id).checked = false;
                var tbodyCout = document.querySelector('#idTabCout');
                tbodyCout.innerHTML = "";
            });
        }

       
    }
   
})
function SelectStatus() {
    var select_statut = document.querySelector('#selectStatut');
    document.querySelector('#statutsSystem').value = select_statut.value;
}
function ValidatePassWord() {
    alert('OKAY');
    //var pwd1 = document.querySelector('#pwd1');
    //var pwd2 = document.querySelector('#pwd2');
    //if (pwd1.value==pwd2.value) {
    //    alert('OKAY')
    //} else {
    //    alert('No');
    //}
}


function getEmployeeContractor(object) {
    try {
        
        var urlActive = document.location.toString().split('/')[4];
        console.log('URL :', urlActive);
        console.log(JSON.stringify(object));
        if (urlActive == 'VoucherContractor') {
            if (object.account_status=="0") {
                alert("Employee is disabled");
            } else {
                document.querySelector('#idEmployedContractor').value = object.id;
                document.querySelector('#nameContractor').value = object.name;
                document.querySelector('#nameContractor').style = "display:normal;"
            }
            
        } else {
            document.querySelector('#nameContractor').value = object.name;
          
            for (var i = 0; i < document.querySelector('#cboSex').length; i++) {
                if (document.querySelector('#cboSex')[i].text=object.Sexe) {
                    document.querySelector('#cboSex').selectedIndex = i;
                }

            }
            document.querySelector('#idEContractor').value = object.id;
            document.querySelector('#AddressHome').value = object.adress;
            document.querySelector('#phoneContractor').value = object.phone;
            document.querySelector('#dateborn').value = object.datenais;
           
            (object.status = "Enabled" ? (document.querySelector('#cbostatus').value = object.status) : document.querySelector('#cbostatus').value = 0)
            ;
            for (var i = 0; i < document.querySelector('#cboContractor').length; i++) {
                if (document.querySelector('#cboContractor')[i].text = object.idCompany) {
                    document.querySelector('#cboContractor').selectedIndex = i;
                }

            }
        }
        document.querySelector('#btnSenderC').style = "display:normal";
        document.querySelector('#blockStatus2').style = "display:normal";
        document.querySelector('#btnModalContractor').click();
    } catch (e) {
        console.log(e);
        document.querySelector('#btnModalContractor').click();
    }
}
function clearFormSucc() {
    document.querySelector('#nameSuccForm').value = "";
    document.querySelector('#adressSuccForm').value = "";
    document.querySelector('#phoneSuccForm').value = "";
    document.querySelector('#alertSubSucc').style = 'display:none';
}
function clearFormDepart() {
    document.querySelector('#alertDepartSub').style = 'display:none';
    document.querySelector('#idDepartForm').value = "";
}
