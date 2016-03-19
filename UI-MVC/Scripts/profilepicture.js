document.getElementById('profile_click').onclick = function () {
    document.getElementById('profileImage').click();
};
document.getElementById("profileImage").onchange = function () {
    var reader = new FileReader();

    reader.onload = function (e) {
        document.getElementById("profile_click").src = e.target.result;
    };

    reader.readAsDataURL(this.files[0]);
};