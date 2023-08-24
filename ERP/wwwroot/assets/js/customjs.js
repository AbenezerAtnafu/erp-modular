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



//file upload placeholder
const profilepicture = document.getElementById('profilePictureHolder');
if (profilepicture) {
    profilepicture.addEventListener('click', function () {
        document.getElementById('ProfilePicture').click();
    });

    document.getElementById('ProfilePicture').addEventListener('change', function () {
        var file = this.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            document.getElementById('profilePictureHolder').src = e.target.result;
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

    var isLoopValid = true;
    var isgenderValid = false;
    var isdobValid = false;
    var isppValid = false;


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
    if (!(age >=18)) {
        isdobValid = false
        document.getElementById("DateofBirthValidation").textContent = "Please select date >= 18."
    } else {
        isdobValid = true
        document.getElementById("DateofBirthValidation").textContent = ""
    }

    var pp = document.getElementById('ProfilePicture').files[0];
    if (!pp) {
        isppValid = false
        document.getElementById("ProfilePictureValidation").textContent = "Please insert passport size profile photo."
    } else {
        isppValid = true
        document.getElementById("ProfilePictureValidation").textContent = ""
    }

    if (isLoopValid && isdobValid && isgenderValid && isppValid) {
        nextButton()
    }
}


//second tab validation
function submitSecondTab() {
    var phonepattern = /^[0-9]{9}$/;
    var addresspattern = /^[a-zA-Z\s]{2,30}$/;

    var phonefields = document.getElementsByClassName("phone-input");
    var addressfields = document.getElementsByClassName("address-input");
    var isLoopValid = false;
    var isaddressValid = false;


    for (var i = 0; i < phonefields.length; i++) {
        var input = phonefields[i].value;
        var inputclass = phonefields[i];
        var error = phonefields[i].nextElementSibling;

        if (!input.match(phonepattern)) {
            error.textContent = "Phone number must start with 9 or 7 and 9 characters long.";
            inputclass.classList.add("is-invalid");
            isLoopValid = false;
        } else {
            isLoopValid = true;
            error.textContent = "";
            inputclass.classList.remove("is-invalid");
        }
    }

    for (var i = 0; i < addressfields.length; i++) {
        var input = addressfields[i].value;
        var inputclass = addressfields[i];
        var error = addressfields[i].nextElementSibling;

        if (!input.match(addresspattern)) {
            error.textContent = "Phone number must start with 9 or 7 and 9 characters long.";
            inputclass.classList.add("is-invalid");
            isaddressValid = false;
        } else {
            isaddressValid = true;
            error.textContent = "";
            inputclass.classList.remove("is-invalid");
        }
    }


    if (isLoopValid && isaddressValid) {
        console.log("ggg")
        nextButton()
    }
}

//third tab validation
function submitThirdTab() {
    var tinpattern = /^[0-9]{10}$/;
    var accountpattern = /^[0-9]{4,30}$/;

    var istinValid = false
    var isbankValid = false


    var tin = document.getElementById('TinNumber').value;
    if (!tin.match(tinpattern)) {
        istinValid = false
        document.getElementById("TinNumber").classList.add("is-invalid");
        document.getElementById("TinNumberValidation").textContent = "Tin number should be exactly 10 digits."
    } else {
        istinValid = true
        document.getElementById("TinNumber").classList.remove("is-invalid");
        document.getElementById("TinNumberValidation").textContent = ""
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

    if (isbankValid && istinValid) {
        nextButton()
    }
}


//submit employee registration
function submitfinalTab() {
    var pattern = /^[a-zA-Z\s]{2,30}$/;
    document.getElementById("CreateEmpForm").submit();
}

//submit modal
function submitApproveModal() {
    var pattern = /^[a-zA-Z\s]{2,30}$/;

    if (document.getElementById("ApproveEmployee").checked) {
        document.getElementById("EmployeeApproveForm").submit();

    } else {
        if (!document.getElementById('EmpRejectMessage').value.match(pattern)) {
            isbankValid = false
            document.getElementById("EmpRejectMessage").classList.add("is-invalid");
            document.getElementById("EmpRejectMessageValidation").textContent = "Account number should at least be 4 or max 30 digits."
        } else {
            document.getElementById("EmpRejectMessage").classList.remove("is-invalid");
            document.getElementById("EmpRejectMessageValidation").textContent = ""
            isbankValid = true
        }
        isbankValid ? setTimeout(() => { document.getElementById("EmployeeApproveForm").submit() }, 400)  : '';
    }
    
}


// show table actions
function showTableMenu() {
    const tbl = document.getElementById("TableMenu");
    if (tbl.classList.contains("d-none")) {
        tbl.classList.remove("d-none");
    } else {
        tbl.classList.add("d-none");
    }
}

const tblmenu = document.getElementById("TableMenu");
if (tblmenu) {
        console.log("vlid here")
    if (tblmenu.classList.contains("d-none")) {
        console.log("yes here")
    } else {
        console.log("no here")
        document.addEventListener('mousedown', function (event) {
            tblmenu.addEventListener('mousedown', function (event) {
                if (!event.target === box) {
                    tblmenu.classList.add("d-none");
                }
            });
        });
    }
}