$(document).ready(function () {
    if (document.getElementById("lang").value == 2) {
        document.getElementById("note").classList.add("rtl");
    }
    else {
        document.getElementById("note").classList.add("ltr");
    }
    document.getElementById("urlval").value = window.document.location;
    document.getElementById("sabtnazar").style.display = "block";
    document.getElementById("sabtvirayesh").style.display = "none";
    var identityname = document.getElementById("identityname").value;
    var musicid = document.getElementById("musicid").value;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    $.ajax({
        type: "post",
        dataType: 'json',
        url: "/Account/IsLiked",
        data: {
            "identityname": identityname, "musicid": musicid, "__RequestVerificationToken": token
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
    $.ajax({
        type: "post",
        dataType: 'json',
        url: "/Account/IsLikedText",
        data: {
            "identityname": identityname, "musicid": musicid, "__RequestVerificationToken": token
        },
        success: function (data) {
            if (data == true) {
                document.getElementById("star").classList.remove("glyphicon-star-empty");
                document.getElementById("star").classList.add("glyphicon-star");
            }
            else {
                document.getElementById("star").classList.remove("glyphicon-star");
                document.getElementById("star").classList.add("glyphicon-star-empty");
            }
        }
    })
    $.ajax({
        type: "post",
        dataType: 'json',
        url: "/Home/ViewCount",
        data: {
            "musicid": musicid, "__RequestVerificationToken": token
        },
        success: function (data) {
            document.getElementById("viewCount").innerText = data;
        }
    })
});
$(document).on("click", function (event) {
    var $trigger = $(".dropdown");
    if ($trigger !== event.target && !$trigger.has(event.target).length) {
        $(".dropdown-menu").slideUp("fast");
    }
});
function CopyClip() {
    var textarea = document.createElement('textarea');
    textarea.textContent = document.getElementById("urlval").value;
    document.body.appendChild(textarea);

    var selection = document.getSelection();
    var range = document.createRange();
    range.selectNode(textarea);
    selection.removeAllRanges();
    selection.addRange(range);

    //alert('url copied successfully.', document.execCommand('copy'));
    selection.removeAllRanges();

    document.body.removeChild(textarea);
}
function Mousein(it) {
    it.style.color = "black";
}
function Mouseout(it) {
    it.style.color = "gray";
}
function NotLogged() {
    alert("Plese Login.");
}
function ToggleHeart(userid, musicid) {
    var d = document.getElementById("heart");
    var b = d.classList.contains("glyphicon-heart-empty");
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    if (b == true) {
        //byd like she
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/LikeSong",
            data: {
                "userid": userid, "musicid": musicid, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (data == true) {
                    d.classList.remove("glyphicon-heart-empty");
                    d.classList.add("glyphicon-heart");
                    document.getElementById("likes").innerText = document.getElementById("likes").innerText * 1 + 1;
                }
            }
        })
    }
    else {
        //like brdshte she
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/UnLikeSong",
            data: {
                "userid": userid, "musicid": musicid, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (data == true) {
                    d.classList.remove("glyphicon-heart");
                    d.classList.add("glyphicon-heart-empty");
                    document.getElementById("likes").innerText = document.getElementById("likes").innerText * 1 - 1;
                }
            }
        })
    }
}
function TogglrStar(userid, musicid) {
    var d = document.getElementById("star");
    var b = d.classList.contains("glyphicon-star-empty");
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    if (b == true) {
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/LikeText",
            data: {
                "userid": userid, "musicid": musicid, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (data == true) {
                    d.classList.remove("glyphicon-star-empty");
                    d.classList.add("glyphicon-star");
                }
            }
        })
    }
    else {
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/UnLikeText",
            data: {
                "userid": userid, "musicid": musicid, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (data == true) {
                    d.classList.remove("glyphicon-star");
                    d.classList.add("glyphicon-star-empty");
                }
            }
        })
    }
}
function SabtNazar(id, type) {
    var replied = document.getElementById("replied").value;
    var comment = document.getElementById("comment").value;
    var token = document.getElementsByName("__RequestVerificationToken")[0].value;
    var subject = window.document.title;
    var subjectid = document.getElementById("musicid").value;
    if (comment.toString().length == 0) {
        alert("Please write your comment first.");
    }
    else {
        if (type == 0) {
            $.ajax({
                type: "post",
                url: "/Account/Comment",
                dataType: "json",
                data: {
                    "id": id, "comment": comment, "subject": subject, "subjectid": subjectid, "replied": replied,
                    "__RequestVerificationToken": token
                },
                success: function (data) {
                    if (data.replied == 0) {
                        //alert(data.username)
                        //alert(data.userId)
                        var str =
                            '<div class="row" id="' + data.id + '">' +
                            '<div class="col-lg-2 col-md-2 col-sm-3 col-xs-3 ">' +
                            '<img id="img' + data.id + '" style="width:80px;height:80px;" class="img-circle pull-left" src="' + data.userpic + '" />' +
                            '</div>' +
                            '<div class="col-lg-9 col-md-9 col-sm-8 col-xs-8 thumbnail" style="background-color:papayawhip;margin:7px auto;">' +
                            '<b class="samerow">نام کاربری</b>: <p class="samerow" id="pname' + data.id + '">' + data.username + '</p><br />' +
                            '<b  class="samerow">متن</b>: <p  class="samerow" id="ptext' + data.id + '" style="font-size:smaller"> ' + data.text + '</p>' +
                            '<br />' +
                            '<p id="ptime' + data.id + '" class="samerow" style="font-size:smaller;color:gray">' + data.createDateString + '</p>' +
                            '<span onclick="Reply(' + data.id + ')" onmousemove="Mousein(this)" onmouseout="Mouseout(this)" id="reply" style="color:gray" class="samerow glyphicon glyphicon-reply pull-left">reply</span>' +
                            '</div>' +
                            '</div>' +
                            '<div class="row">' +
                            '<div class="col-lg-2 col-md-2 col-sm-3 col-xs-3 "></div>' +
                            '<div class="col-lg-9 col-md-9 col-sm-8 col-xs-8">' +
                            '<button id="virayesh1' + data.id + '" onclick="Virayesh(' + data.id + ',' + data.replied + ')" class="btn-sm btn btn-primary pull-left">Edit</button>' +
                            '<button id="hazf1' + data.id + '" onclick="Hazf(' + data.id + ',' + data.replied + ')" class="btn-sm btn btn-danger pull-left">Delete</button>' +
                            '</div>' +
                            '<div class="col-lg-1 col-md-1 col-sm-1 col-xs-1"></div>' +
                            '</div>';
                        //$("#commentha").prepend(str).html();
                        $("#body").load("#commentha");
                    }
                    else {
                        var str2 =
                            '<div class="row" id="repliedComment' + data.id + '">' +
                            '<div class="col-lg-1 col-md-1 col-sm-1 col-xs-1 "></div>' +
                            '<div class="col-lg-2 col-md-2 col-sm-4 col-xs-4 ">' +
                            '<img style="width:80px;height:80px;" class="img-circle pull-left" src="' + data.userpic + '" />' +
                            '</div>' +
                            '<div class="col-lg-7 col-md-7 col-sm-6 col-xs-6 thumbnail" style="background-color:antiquewhite;margin:7px auto">' +
                            '<b  class="samerow">username</b>: <p  class="samerow">' + data.username + '</p>' +
                            '<br />' +
                            '<b  class="samerow">text</b>: <p  class="samerow" id="repcomtext' + data.id + '" style="font-size:smaller">' + data.text + '</p>' +
                            '<br />' +
                            '<p style="font-size:smaller;color:gray">' + data.createDateString + '</p>' +
                            '</div>' +
                            '<div class="col-lg-1 col-md-1 col-sm-1 col-xs-1 "></div>' +
                            '</div>' +
                            '<div class="row" id="editdelete' + data.id + '">' +
                            '<div class="col-lg-3 col-md-3 col-sm-5 col-xs-5 ">' +
                            '</div>' +
                            '<div class="col-lg-7 col-md-7 col-sm-6 col-xs-6">' +
                            '<button id="virayesh2' + data.id + '" onclick="Virayesh(' + data.id + ',' + data.replied + ')" class="btn-sm btn btn-primary pull-left">Edit</button>' +
                            '<button id="hazf2' + data.id + '" onclick="Hazf(' + data.id + ',' + data.replied + ')" class="btn-sm btn btn-danger pull-left">Delete</button>' +
                            '</div>' +
                            '<div class="col-lg-1 col-md-1 col-sm-1 col-xs-1 "></div>' +
                            '</div>';
                        //$("#replied" + data.replied).prepend(str2);
                        $("#body").load("#replied" + data.replied);
                    }
                    document.getElementById("comment").value = "";
                    document.getElementById("replied").value = 0;
                    document.getElementById("replyWarning").innerHTML = "";
                    document.getElementById("commentid").value = 0;
                    document.getElementById("sabtnazar").style.display = "block";
                    document.getElementById("sabtvirayesh").style.display = "none";
                }
            });

        }
        if (type == 1) {
            var commentid = document.getElementById("commentid").value;
            $.ajax({
                type: "post",
                dataType: 'json',
                url: "/Account/EditComment",
                data: {
                    "commentid": commentid, "__RequestVerificationToken": token, "comment": comment
                },
                success: function (data) {
                    if (data.replied == 0) {
                        document.getElementById("ptext" + commentid).innerHTML = data.text;
                    }
                    else {
                        document.getElementById("repcomtext" + commentid).innerHTML = data.text;
                    }
                    document.getElementById("commentid").value = 0;
                    document.getElementById("comment").value = "";
                    document.getElementById("sabtnazar").style.display = "block";
                    document.getElementById("sabtvirayesh").style.display = "none";
                    document.getElementById("replyWarning").innerHTML = "";
                }
            })
        }
    }
}
function Reply(id) {
    document.getElementById("replied").value = id;
    document.getElementById("comment").value = "";
    document.getElementById("Nazarat").scrollIntoView();
    document.getElementById("replyWarning").innerHTML = "Please write your reply.";
}
function Enseraf() {
    document.getElementById("comment").value = "";
    document.getElementById("replyWarning").innerHTML = "";
    document.getElementById("replied").value = 0;
    document.getElementById("sabtnazar").style.display = "block";
    document.getElementById("sabtvirayesh").style.display = "none";
}
function Virayesh(id, repparentid) {
    alert(repparentid)
    document.getElementById("replyWarning").innerHTML = "Please complete your reply first.";
    document.getElementById("sabtvirayesh").style.display = "block";
    document.getElementById("sabtnazar").style.display = "none";
    document.getElementById("replied").value = repparentid;
    document.getElementById("Nazarat").scrollIntoView();
    document.getElementById("commentid").value = id;
    if (repparentid == 0) {
        document.getElementById("comment").value = document.getElementById("ptext" + id).innerHTML;
        alert(document.getElementById("comment").value)
    }
    else {
        document.getElementById("comment").value = document.getElementById("repcomtext" + id).innerHTML;
        alert(document.getElementById("comment").value)
    }
}
function Hazf(id, repparentid) {
    if (confirm("Are you sure you want to delete your comment?")) {
        var token = document.getElementsByName("__RequestVerificationToken")[0].value;
        $.ajax({
            type: "post",
            dataType: 'json',
            url: "/Account/DeleteComment",
            data: {
                "id": id, "__RequestVerificationToken": token
            },
            success: function (data) {
                if (repparentid == 0) {
                    if (data == true) {
                        document.getElementById("ptext" + id).innerHTML = "Deleted Commnet.";
                        document.getElementById("pname" + id).innerHTML = "";
                    }
                    else {
                        $("#" + id).remove();
                        $("#editdel" + id).remove();
                        $("#virayesh1" + id).remove();
                        $("#hazf1" + id).remove();
                    }
                }
                else {
                    $("#repliedComment" + id).remove();
                    $("#editdelete" + id).remove();
                    $("#virayesh2" + id).remove();
                    $("#hazf2" + id).remove();
                }
            }
        })
    }
}
