$(document).ready(function () {
    var identityname = document.getElementById("identityname").value;
    var singerid = document.getElementById("sid").innerHTML;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    $.ajax({
        type: "post",
        dataType: 'json',
        url: "/Account/IsLikedSinger",
        data: {
            "identityname": identityname, "singerid": singerid, "__RequestVerificationToken": token
        },
        success: function (data) {
            if (data == true) {
                document.getElementById("heart").classList.remove("glyphicon-heart-empty");
                document.getElementById("heart").classList.add("glyphicon-heart");
            }
            else {
                document.getElementById("heart").classList.remove("glyphicon-heart");
                document.getElementById("heart").classList.add("glyphicon-heart-empty");
            }
        }
    })
});
