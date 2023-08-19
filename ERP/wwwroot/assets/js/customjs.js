// change password input to text
function showPassword() {
    var x = document.getElementById("password");
    var y = document.getElementById("passwordconf");
    if (x.type === "password" || y.type === "password") {
        x.type = "text";
        y.type = "text";
    } else {
        x.type = "password";
        y.type = "password";
    }
}


//toast notification
function closeAlert() {
    document.getElementById("alert-box").style.display = "none";
}
setTimeout(closeAlert, 6000)


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

// tab navigation
function nextButton() {
    const nextTabLinkEl = $('.nav-tabs .active').closest('li').next('li').find('a')[0];
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
    $("#exampleModalCenter").modal()
}
