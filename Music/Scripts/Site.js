function Home() {
    window.document.location = "/Home/Index";
}
function Login() {
    window.document.location = "/Login";
}
function page(id) {
    window.document.location = "/Home/SongPage/" + id;
}
function singerPage(id) {
    window.document.location = "/Home/SingerSongsPage/" + id;
}
function Pagination(id) {
    var pageid = document.getElementById("pageid");
    var id2 = document.location;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    $.ajax({
        type: "Post",
        url: "/Home/ListPage",
        dataType: 'json',
        data: { "id": id, "id2": id2, "pageid": pageid, "__RequestVerificationToken": token },
        success: function (data) {
            document.getElementById("pg").innerHTML = "";
            for (var i = 0; i < data.length; i++) {
                document.getElementById("pg").innerHTML +=
                    '<div class="thumbnail col-lg-3 col-md-3 col-sm-5 col-xs-5" style="margin-right: 10px;border: 1px; width: 186px; height: 210px; background-color:lightgray; opacity: 0.7">' +
                    '<img src="' + data[i].pic + '" style="height:100px;width:120px" />' +
                    '<br />' +
                    '<b>Song:' + data[i].name + '</b><br />' +
                    '<b>Singer:' + data[i].singerName + '</b><br />' +
                    '<span class="glyphicon glyphicon-share" aria-hidden="true"></span><p><a href="/Home/SongPage/' + data[i].Id + '">More info...</a></p>' +
                    '</div>';
            }
        }
    })
}
function PaginationSong(id) {
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    $.ajax({
        type: "Post",
        url: "/Home/SingersPage",
        dataType: 'json',
        data: { "id": id, "__RequestVerificationToken": token },
        success: function (data) {
            document.getElementById("pgs").innerHTML = "";
            for (var i = 0; i < data.length; i++) {
                document.getElementById("pgs").innerHTML +=
                    '<div class="pages thumbnail col-lg-3 col-md-3 col-sm-5 col-xs-5" style="margin-right:10px;border: 1px; width: 190px; height: 210px; background-color: rosybrown; opacity: 0.7">' +
                    '<img src="/pics/microfono.jpg" style="height:100px;width:140px" />' +
                    '<br />' +
                    '<b>Singer:' + data[i].name + '</b><br />' +
                    '<b>Genre:' + data[i].genreName + '</b><br />' +
                '<a href="/Home/SingerSongsPage/' + data[i].Id + '"><p style="color:darkslategray">View List Of Songs</p></a>' +
                    '</div>';
            }
        }
    })
}
function GoRight(id) {
    var avali = document.getElementsByClassName("PagingButtons")[0].value;
    var x1 = '<span class="glyphicon glyphicon-chevron-right" onclick="GoRight(' + id + ')" style="color:white"></span>'
    var x2 = '<span class="glyphicon glyphicon-chevron-left" onclick="GoLeft(' + id + ')" style="color:white"></span>'
    if (avali == 1) {
        return;
    }
    else {
        if (id - 3 >= 3) {
            var str = "";
            str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + avali - 1 + ')">' + avali - 1 + '</button>';
            str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + avali - 2 + ')">' + avali - 2 + '</button>';
            str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + avali - 3 + ')">' + avali - 3 + '</button>';
            document.getElementById("btnha").innerHTML = x1 + str + x2;
        }
        else {
            var count = id - 3;
            if (count > 0) {
                var str = "";
                for (var i = 1; i < count; i++) {
                    str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + avali - 1 + ')">' + avali - 1 + '</button>';
                }
                document.getElementById("btnha").innerHTML = str + x2;
            }
        }

    }
}
function GoLeft(id) {
    var akhari = document.getElementsByClassName("PagingButtons")[2].value;
    var x1 = '<span class="glyphicon glyphicon-chevron-right" onclick="GoRight(' + id + ')" style="color:white"></span>'
    var x2 = '<span class="glyphicon glyphicon-chevron-left" onclick="GoLeft(' + id + ')" style="color:white"></span>'
    if (akhari == 3) {
        return;
    }
    else {
        if (id - 3 >= 3) {
            var str = "";
            str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + akhari + 1 + ')">' + akhari + 1 + '</button>';
            str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + akhari + 2 + ')">' + akhari + 2 + '</button>';
            str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + akhari + 3 + ')">' + akhari + 3 + '</button>';
            document.getElementById("btnha").innerHTML = x1 + str + x2;
        }
        else {
            var count = id - 3;
            if (count > 0) {
                var str = "";
                for (var i = 1; i < count; i++) {
                    str += '<button class="btn btn-default btn-sm center PagingButtons" style="color:white;background-color:gray" onclick="Pagination(' + akhari + 1 + ')">' + akhari + 1 + '</button>';
                }
                document.getElementById("btnha").innerHTML = x1 + str;
            }
        }
    }
}
function ToggleHeart(userid, singerid) {
    var d = document.getElementById("heart");
    var b = d.classList.contains("glyphicon-heart-empty");
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    if (b == true) {
        //byd like she
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/LikeSinger",
            data: {
                "userid": userid, "singerid": singerid, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (data == true) {
                    d.classList.remove("glyphicon-heart-empty");
                    d.classList.add("glyphicon-heart");
                }
            }
        })
    }
    else {
        //like brdshte she
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/UnLikeSinger",
            data: {
                "userid": userid, "singerid": singerid, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (data == true) {
                    d.classList.remove("glyphicon-heart");
                    d.classList.add("glyphicon-heart-empty");
                }
            }
        })
    }
}
function Register() {
    window.document.location = "/Register";
}
function Empty() {
    document.getElementById("EmailLogin").value = "";
    document.getElementById("PasswordLogin").value = "";
    document.getElementById("RememberMe").value = "";
}