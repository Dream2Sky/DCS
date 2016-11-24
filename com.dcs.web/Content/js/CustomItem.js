// 添加自定义项
function AddCustomItem(action, customItemName) {
    $.ajax({
        url: action,
        type: "POST",
        data: {
            customItemName: customItemName
        },
        success: function (data) {
            if (data.state == "error") {
                $(".alert-customItem").show();
                $(".alert-customItem span").html(data.message);
            }
            else if (data.state == "success") {
                var customitemList = {
                    itemList: JSON.parse("[" + data.data + "]")
                };

                var html = template("customerItemTemplate", customitemList);
                $("#form-CustomItems table").append(html);
            }
        },
        error: function () {
            $(".alert-customItem").show();
            $(".alert-customItem span").html("系统错误，添加自定义项失败");
        }
    });
}

// 修改自定义项
function UpdateCustomItem(action, customItemName, customItemContent) {
    var result = false;
    $.ajax({
        url: action,
        type: "POST",
        async: false,
        data: {
            customItemName: customItemName,
            customItemContent: customItemContent
        },
        success: function (data) {
            if (data.state == "error") {
                $("#updateModal .alert-updateCustomItem").show();
                $("#updateModal .alert-updateCustomItem span").html(data.message);

                result = false;
            }
            else if (data.state == "success") {
                result = true;
            }
        },
        error: function () {
            $("#updateModal .alert-updateCustomItem").show();
            $("#updateModal .alert-updateCustomItem span").html("系统错误，修改自定义项失败");

            result = false;
        }
    });

    return result;
}

// 删除自定义项
function DeleteCustomItem(action, customItemName) {
    var result = false;

    $.ajax({
        url: action,
        type: "POST",
        async: false,
        data: {
            customItemName: customItemName
        },
        success: function (data) {
            if (data.state == "error") {
                $("#deleteModal .alert-deletecustomItem").show();
                $("#deleteModal .alert-deletecustomItem span").html(data.message);

                result = false;
            }
            else if (data.state == "success") {
                result = true;
            }
        },
        error: function () {
            $("#deleteModal .alert-deletecustomItem").show();
            $("#deleteModal .alert-deletecustomItem span").html("系统错误，删除自定义项失败");

            result = false;
        }
    });

    return result;
}