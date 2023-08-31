// change password input to text
function showPassword() {
    var x = document.getElementById("password");
    var y = document.getElementById("passwordconf");
    if (y) {

        if (x.type === "password" || y.type === "password") {
            x.type = "text";
            y.type = "text";
        } else {
            x.type = "password";
            y.type = "password";
        }
    } else {
        x.type === "password" ? x.type = "text" : x.type = "password";
    }
}


//toast notification
function closeAlert() {
    document.getElementById("alert-box").style.display = "none";
}


//show end date if not currently working
function hideEndDate() {
    var checkbox = document.getElementById("AlreadyWorking");
        if (checkbox.checked) {
            // hide end date
            document.getElementById("EmpEndDate").style.display = "none";
        } else {
            // show end date
            document.getElementById("EmpEndDate").style.display = "block";
        }
}

//show end date if not currently working
function hideMessagInput() {
    var checkbox = document.getElementById("ApproveEmployee");
        if (checkbox.checked) {
            // hide end date
            document.getElementById("EmpRejectMessageInput").style.display = "none";
        } else {
            // show end date
            document.getElementById("EmpRejectMessageInput").style.display = "block";
        }
}

// tab navigation
function nextButton() {
    const nextTabLinkEl = $('.nav-tabs .active').closest('li').next('li').find('a')[0];
    console.log(nextTabLinkEl)
    nextTabLinkEl.classList.remove("disabled");
    const nextTab = new bootstrap.Tab(nextTabLinkEl);
    nextTab.show();
};
function prevButton() {
    const prevTabLinkEl = $('.nav-tabs .active').closest('li').prev('li').find('a')[0];
    const prevTab = new bootstrap.Tab(prevTabLinkEl);
    prevTab.show();
};



//delete action from modal
function deleteConf(id) {
    $('#RecordIdDelete').val(id);
    $("#DeleteModalCenter").modal()
}

//approve action from modal
function approveModal(id) {
    $('#ApproveId').val(id);
    $("#ApproveModalCenter").modal()
}

//reject action from modal
function rejectModal(id) {
    $('#RejectEmpId').val(id);
    $("#RejectModalCenter").modal()
}
//
function inactivemodal(id) {
    $('#inactiveid').val(id);
    $("#InActiveuserModal").modal()
}
function activemodal(id) {
    console.log("ddd "+id)
    $('#activeid').val(id);
    $("#ActiveuserModal").modal()
}

//file upload placeholder
const profilepicture = document.getElementById('profilePictureHolder');
const profile = document.getElementById('ProfilePicture');

if (profilepicture && profile) {

    profilepicture.addEventListener('click', function () {
        profile.click();
    });

    profile.addEventListener('change', function () {
        var file = this.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            profilepicture.src = e.target.result;
        };

        reader.readAsDataURL(file);
        if (file) {
            const reader = new FileReader();

            reader.onload = function (e) {
                profilePictureHolder.src = e.target.result;
            };

            reader.readAsDataURL(file);
        } else {

        }
    });
}





//employee validation first tab
function submitFirstTab() {
    var pattern = /^[a-zA-Z\s]{3,30}$/;
    var currentDate = new Date();

    var fields = document.getElementsByClassName("name-input");
    var dropdowns = document.getElementsByClassName("first-dropdown");

    var isLoopValid = true;
    var isgenderValid = false;
    var isdobValid = false;
    var isppValid = false;
    var isdropdownValid = false;


    for (var i = 0; i < fields.length; i++) {
        var input = fields[i].value;
        var inputclass = fields[i];
        var error = fields[i].nextElementSibling;

        if (!input.match(pattern)) {
            error.textContent = "Please enter between 3 and 30 alphabetic characters.";
            inputclass.classList.add("is-invalid");
            isLoopValid = false;
        } else {
            isLoopValid = true;
            error.textContent = "";
            inputclass.classList.remove("is-invalid");

        }
    }
    console.log(dropdowns.length)
    for (var i = 0; i < dropdowns.length; i++) {
        var input = dropdowns[i].value;
        var inputclass = dropdowns[i];
        var error = dropdowns[i].nextElementSibling;

        if (!input) {
            error.textContent = "This field is required.";
            inputclass.classList.add("is-invalid");
            isdropdownValid = false;
        } else {
            isdropdownValid = true;
            error.textContent = "";
            inputclass.classList.remove("is-invalid");

        }
    }
    var gender = document.querySelector('input[name="Gender"]:checked');
    if (!gender) {
        isgenderValid = false
        document.getElementById("GenderValidation").textContent = "Please select gender."
    } else {
        isgenderValid = true
        document.getElementById("GenderValidation").textContent = ""
    }

    var dob = new Date(document.getElementById('DateofBirth').value);
    var age = currentDate.getFullYear() - dob.getFullYear();
    if (!(age >= 18 && age <= 60)) {
        isdobValid = false
        document.getElementById("DateofBirthValidation").textContent = "Please select date >= 18 and <= 60."
    } else {
        isdobValid = true
        document.getElementById("DateofBirthValidation").textContent = ""
    }

    const alreadySelected = document.getElementById('FileAlreadyExist');
    var pp = document.getElementById('ProfilePicture').files[0];
    if (!pp && !alreadySelected) {
        isppValid = false
        document.getElementById("ProfilePictureValidation").textContent = "Please insert passport size profile photo."
    } else {
        isppValid = true
        document.getElementById("ProfilePictureValidation").textContent = ""
    }

    if (isLoopValid && isdobValid && isgenderValid && isppValid && isdropdownValid) {
        nextButton()
    }
}

//second tab validation
function submitSecondTab() {
    var phonepattern = /^(\d{7}|\d{9})$/;
    var addresspattern = /^[a-zA-Z\s]{2,30}$/;
    var kebelepattern = /^[a-zA-Z\s]{2,20}$/;

    var dropdownfields = document.getElementsByClassName("second-dropdown");
    var phonevalid = false;
    var altphonevalid = false;
    var intphonevalid = false;
    /*var addressfield = false;*/
    var isLoopValid = false;
    var iskebeleValid = false;


    for (var i = 0; i < dropdownfields.length; i++) {
        var input = dropdownfields[i].value;
        var inputclass = dropdownfields[i];
        var error = dropdownfields[i].nextElementSibling;

        if (!input) {
            error.textContent = "This field is required.";
            inputclass.classList.add("is-invalid");
            isLoopValid = false;
        } else {
            isLoopValid = true;
            error.textContent = "";
            inputclass.classList.remove("is-invalid");
        }
    }


    /*var address = document.getElementById('PrimaryAddress').value;
    if (!address.match(addresspattern)) {
        addressfield = false
        document.getElementById("PrimaryAddressValidation").textContent = "Please enter between 3 and 30 alphabetic characters."
    } else {
        addressfield = true
        document.getElementById("PrimaryAddressValidation").textContent = ""
    }*/

    var phone = document.getElementById('PhoneNumber').value;
    if (!phone.match(phonepattern)) {
        phonevalid = false
        document.getElementById("PhoneNumberValidation").textContent = "Phone number must start with 9 or 7 and 9 characters long."
    } else {
        phonevalid = true
        document.getElementById("PhoneNumberValidation").textContent = ""
    }

    var kebele = document.getElementById('Kebele');
    if (kebele.value) {
        if (!kebele.value.match(kebelepattern)) {
            iskebeleValid = false
            document.getElementById("kebeleValidation").textContent = "Please enter between 2 and 20 alphabetic characters."
        } else {
            iskebeleValid = true
            document.getElementById("kebeleValidation").textContent = ""
        }
    } else {
        iskebeleValid = true
        document.getElementById("kebeleValidation").textContent = ""
    }

    var altphone = document.getElementById('AlternativePhoneNumber');
    if (altphone.value) {
        console.log(" i have value...")
        if (!altphone.value.match(phonepattern)) {
            altphonevalid = false
            document.getElementById("AlternativePhoneNumberValidation").textContent = "Phone number must start with 9 or 7 and 9 characters long."
        } else {
            altphonevalid = true
            document.getElementById("AlternativePhoneNumberValidation").textContent = ""
        }
    } else {
        altphonevalid = true
        document.getElementById("AlternativePhoneNumberValidation").textContent = ""
    }

    var intphone = document.getElementById('InternalPhoneNumber');
    if (intphone.value) {
        if (!intphone.value.match(phonepattern)) {
            intphonevalid = false
            document.getElementById("InternalPhoneNumberValidation").textContent = "Phone number must start with 9 or 7 and 9 characters long."
        } else {
            intphonevalid = true
            document.getElementById("InternalPhoneNumberValidation").textContent = ""
        }
    } else {
        intphonevalid = true
        document.getElementById("InternalPhoneNumberValidation").textContent = ""
    }



    if (isLoopValid && altphonevalid && phonevalid && iskebeleValid && intphonevalid) {
        console.log("ggg")
        nextButton()
    }
   
}

//third tab validation
function submitThirdTab() {
    var tinpattern = /^[0-9]{10}$/;
    var penpattern = /^[0-9]{4}$/;
    var accountpattern = /^[0-9]{4,30}$/;

    var istinValid = false
    var isbankValid = false
    var penValid = false


    var tin = document.getElementById('TinNumber');
    if (tin.value) {
        if (!tin.value.match(tinpattern)) {
            istinValid = false
            document.getElementById("TinNumber").classList.add("is-invalid");
            document.getElementById("TinNumberValidation").textContent = "Tin number should be exactly 10 digits."
        } else {
            istinValid = true
            document.getElementById("TinNumber").classList.remove("is-invalid");
            document.getElementById("TinNumberValidation").textContent = ""
        }
    } else {
        istinValid = true
        document.getElementById("TinNumberValidation").textContent = ""
    }

    var pension = document.getElementById('PensionNumber');
    if (pension.value) {
        if (!pension.value.match(penpattern)) {
            penValid = false
            document.getElementById("PensionNumberValidation").textContent = "Pension number should be 4 digits only."
        } else {
            penValid = true
            document.getElementById("PensionNumberValidation").textContent = ""
        }
    } else {
        penValid = true
        document.getElementById("PensionNumberValidation").textContent = ""
    }

    var bank = document.getElementById('BankNumber').value;
    if (!bank.match(accountpattern)) {
        isbankValid = false
        document.getElementById("BankNumber").classList.add("is-invalid");
        document.getElementById("BankNumberValidation").textContent = "Account number should at least be 4 or max 30 digits."
    } else {
        isbankValid = true
        document.getElementById("BankNumber").classList.remove("is-invalid");
        document.getElementById("BankNumberValidation").textContent = ""
    }

    if (isbankValid && istinValid && penValid) {
        nextButton()
    }
}


//submit employee registration
function submitfinalTab() {
    var dropdownfields = document.getElementsByClassName("thrid-dropdown");
    var pattern = /^[a-zA-Z\s]{2,30}$/;
    var officepattern = /^[0-9]{2,10}$/;
    var isLoopValid = false
    var placeValid = false
    var officeValid = false
    var currentValid = false
    var startValid = false

    for (var i = 0; i < dropdownfields.length; i++) {
        var input = dropdownfields[i].value;
        var inputclass = dropdownfields[i];
        var error = dropdownfields[i].nextElementSibling;

        if (!input) {
            error.textContent = "This field is required.";
            inputclass.classList.add("is-invalid");
            isLoopValid = false;
        } else {
            isLoopValid = true;
            error.textContent = "";
            inputclass.classList.remove("is-invalid");
        }
    }

    var place = document.getElementById('PlaceofWork').value;
    if (!place.match(pattern)) {
        placeValid = false
        document.getElementById("PlaceofWork").classList.add("is-invalid");
        document.getElementById("PlaceofWorkValidation").textContent = "Please enter between 3 and 30 alphabetic characters."
    } else {
        placeValid = true
        document.getElementById("PlaceofWork").classList.remove("is-invalid");
        document.getElementById("PlaceofWorkValidation").textContent = ""
    }

    var office = document.getElementById('OfficeNumber');
    if (office.value) {
        if (!office.value.match(officepattern)) {
            officeValid = false
            document.getElementById("OfficeNumberValidation").textContent = "Office number should be 2 and 10 digit long."
        } else {
            officeValid = true
            document.getElementById("OfficeNumberValidation").textContent = ""
        }
    } else {
        officeValid = true
        document.getElementById("OfficeNumberValidation").textContent = ""
    }

    var startdate = document.getElementById('StartDate').value;
    if (!startdate) {
        startValid = false
        document.getElementById("StartDate").classList.add("is-invalid");
        document.getElementById("StartDateValidation").textContent = "This field is required."
    } else {
        startValid = true
        document.getElementById("StartDateValidation").textContent = ""
    }


    if (document.getElementById("AlreadyWorking").checked) {
        currentValid = true

    } else {
        if (!document.getElementById('EndDate').value) {
            currentValid = false
            document.getElementById("EndDateValidation").classList.add("is-invalid");
            document.getElementById("EndDateValidation").textContent = "This field is Required."
        } else {
            document.getElementById("EndDateValidation").classList.remove("is-invalid");
            document.getElementById("EndDateValidation").textContent = ""
            currentValid = true
        }
    }


    if (isLoopValid && placeValid && officeValid && currentValid && startValid) {
         document.getElementById("CreateEmpForm").submit();
    }
}

//submit modal
/*function submitApproveModal() {
    var pattern = /^[a-zA-Z\s]{2,30}$/;
    var validation = false;

    if (!document.getElementById('EmpRejectMessage').value.match(pattern)) {
        validation = false
        document.getElementById("EmpRejectMessage").classList.add("is-invalid");
        document.getElementById("EmpRejectMessageValidation").textContent = "Account number should at least be 4 or max 30 digits."
    } else {
        document.getElementById("EmpRejectMessage").classList.remove("is-invalid");
        document.getElementById("EmpRejectMessageValidation").textContent = ""
        validation = true
    }
    validation ? document.getElementById("RejectForm").submit()  : '';
    
}
*/

function screenShot(cardelement, dwnlink) {

    let card = document.getElementById(cardelement);
    let link = document.getElementById(dwnlink);

    // Get the dimensions of the card element
    let width = card.offsetWidth;
    let height = card.offsetHeight;

    let options = {
        width: width,
        height: height,
        scale: 4
    };

    html2canvas(card, options)
        .then(canvas => {
            link.href = canvas.toDataURL('image/png', 1.0);
            link.click(); // click on the link
        });
}


function submitRejectModal() {
    var pattern = /^[a-zA-Z\s]{2,30}$/;

    if (!document.getElementById('EmpRejectMessage').value.match(pattern)) {
        document.getElementById("EmpRejectMessage").classList.add("is-invalid");
        document.getElementById("EmpRejectMessageValidation").textContent = "Please enter between 10 and 300 alphabetic characters."
    } else {
        document.getElementById("EmpRejectMessage").classList.remove("is-invalid");
        document.getElementById("EmpRejectMessageValidation").textContent = ""
        document.getElementById("RejectFormModal").submit()
    }
}
