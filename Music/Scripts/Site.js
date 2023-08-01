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
                    '<div class="thumbnail col-lg-3 col-md-3 col-sm-6 col-xs-6" style="margin-right: 40px;margin-top:5px;border: 1px; width: 190px; height: 210px; background-color:lightgray; opacity: 0.7">' +
                    '<img src="' + data[i].pic + '" style="height:100px;width:120px" />' +
                    '<br />' +
                    '<b>نام اهنگ:' + data[i].name + '</b><br />' +
                    '<b>خواننده:' + data[i].singerName + '</b><br />' +
                    '<span class="glyphicon glyphicon-share" aria-hidden="true"></span><p><a href="/Home/SongPage/' + data[i].Id + '">ادامه مطلب...</a></p>' +
                    '</div>';
            }
        }
    })
}