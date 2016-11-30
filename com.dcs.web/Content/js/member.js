function AddMember(action, currentUserRole, role) {
    var result = false;
    $.ajax({
        url: action,
        type: "POST",
        async: false,
        data: {
            Name: $("#member-form input[name='Name']").val(),
            RoleCode: role,
            ParentName: role == currentUserRole ? null : $("#member-form select[name='parent']").find("option:selected").text()
        },
        success: function (data) {
            if (data.state == "error") {
                $(".alert-addmember").show();
                $(".alert-addmember span").text(data.message);
                result = false;
            }
            else if (data.state == "success") {
                var member = {
                    Name: data.data.Name,
                    Account: data.data.Account,
                    Role: data.data.RoleCode,
                    ParentName: data.data.ParentName,
                    Cocount: data.data.Cocount,
                    Apcount: data.data.Apcount,
                    Ascount: data.data.Ascount
                };
                var html = template("member-table", member);
                $("table tbody").append(html);

                result = true;
            }
        },
        error: function () {
            $(".alert-addmember").show();
            $(".alert-addmember span").text("系统错误，添加用户失败");
            result = false;
        }

    });

    return result;
}

function UpdateMember(action, account, name, currentUserRole, role, parentName) {
    var result = false;

    if (currentUserRole == 1) {
        parentName = null;
    }

    $.ajax({
        url: action,
        type: "POST",
        async: false,
        data: {
            Account: account,
            Name: name,
            RoleCode: role,
            ParentName: role == currentUserRole ? null : parentName
        },
        success: function (data) {
            if (data.state == "error") {
                $(".alert-updatemember").show();
                $(".alert-updatemember #span-message").text(data.message);

                result = false;
            }
            else if (data.state == "success") {
                result = true;
            }
        },
        error: function () {
            $(".alert-updatemember").show();
            $(".alert-updatemember #span-message").text("系統錯誤，修改用戶信息失敗");

            result = false;
        }
    });

    return result;
}

function DeleteMember(action, account) {
    var result = false;
    $.ajax({
        url: action,
        type: "POST",
        async: false,
        data: {
            Account: account
        },
        success: function (data) {
            if (data.state == "error") {
                $(".alert-deletemember").show();
                $(".alert-deletemember span").html("刪除該賬號失敗");
                result = false;
            }
            else if (data.state == "success") {
                $("table tbody").each(function () {
                    console.log($(this).children().eq(1).text());
                    if ($(this).children().eq(1).text() == account) {
                        console.log($(this));
                        $(this).remove();
                    }
                    result = true;
                });
            }
        },
        error: function () {
            $(".alert-deletemember").show();
            $(".alert-deletemember span").html("系統錯誤，無法刪除該賬號");
            result = false;
        }
    });
    return result;
}