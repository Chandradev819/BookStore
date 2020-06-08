$(function () {
    $("#Product_CategoryId").change(function () {
        $.getJSON("/Admin/Product/GetSubCategory", { catId: $("#Product_CategoryId").val() }, function (d) {
            var row = "";
            row = "<option value=" + "0" + ">" + "Select SubCategory" + "</option>"
            $("#Product_SubCategoryId").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });

            $("#Product_SubCategoryId").html(row);
        })
    })
    $("#Product_SubCategoryId").change(function () {
        $.getJSON("/Admin/Product/GetMiniCategory", { subCateId: $("#Product_SubCategoryId").val() }, function (d) {
            var row = "";
            row = "<option value=" + "0" + ">" + "Select MiniCategory" + "</option>"
            $("#Product_SubSubId").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";
            });
            $("#Product_SubSubId").html(row);
        })
    })
})