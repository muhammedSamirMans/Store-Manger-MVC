//what happens when page is ready
$(document).ready(function () {
    $("#Supplier").val("");
    $("#StoreName").val("");
    var finaltotal = 0;
    var finalremaning = 0;
    //when choose a store from the store list it will load the dropdown list of items that this store have.
    $("#StoreName").change(function () {
        var storeID = $(this).val();
        $.ajax({
            type: "post",
            url: "../Purchases/GetItemsList/" + storeID,
            contentType: "html",
            success: function (response) {
                $("#ItemName").empty();
                $("#UnitName").empty();
                $("#Price").val("");
                $("#ItemName").append(response);
                $("#ItemName").val("");
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        })
    })
    //what happens when change the Items DropDownList (load the units of the choosen Item).
    $("#ItemName").change(function () {
        var itemID = $(this).val();
        $.ajax({
            type: "post",
            url: "../Purchases/GetUnitsList/" + itemID,
            contentType: "html",
            success: function (response) {
                $("#UnitName").empty();
                $("#Price").val("");
                $("#UnitName").append(response);
                $("#UnitName").val("");
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        })

    })
    //when we choose the uint we will calc the price depend on selected unit
    $("#UnitName").change(function () {
        var precent = $(this).val();
        $.ajax({
            url: "../Purchases/GetItembyID/" + $("#ItemName").val(),
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (data) {
                //get the price of main or sub unit of item
                $("#Price").val(data.SillingPrice * precent);
            },
            error: function () {
                alert("Failed! Please try again.");
            }
        })
    })
    //when we change the Payed amount the remaining will be clac automatic using ttotal.
    $("#Payed").change(function () {
        var payed = $(this).val();
        var cheektotal = parseFloat($("#TTotal").text());
        //cheeking if total is >= 0
        if (cheektotal > 0 || cheektotal == 0) {
            var remain = parseFloat($("#TTotal").text()) - parseFloat(payed);
            finalremaning = remain;
            $("#Remaining").text(remain);
        }

    })
})
//The Actions that will happened when click Add Button
$("body").on("click", "#btnAdd", function () {
    //get the value of inputs and define it in variable
    var storeName = $("#StoreName");
    var itemName = $("#ItemName");
    var unitName = $("#UnitName");
    var quantity = $("#Quantity");
    var price = $("#Price");
    //var data = new Array();
    //data.push($("#StoreName").val());
    //data.push($("#ItemName").val());
    var item = {};
    //item.BillID = billid,
    item.StoreID = storeName.val();
    item.ItemID = itemName.val();
    item.Quantity = quantity.val() + unitName.val();
    item.Price = price.val();
    item.AddedTax = 0;
    item.Total = 0;
  
            //get the price of main or sub unit of item
            //Validation for Item details inputs that will be cheecked when add button clicked.
            $(".error").remove();
            if (!$("#ItemName").val()) {
                $('#ItemName').after('<span class="error" style="color:red;">This field is required</span>');
            }
            if (!$("#StoreName").val()) {
                $('#StoreName').after('<span class="error" style="color:red;">This field is required</span>');
            }

            if (!$("#UnitName").val()) {
                $('#UnitName').after('<span class="error" style="color:red;">This field is required</span>');
            }
            if (!$("#Quantity").val()) {
                $('#Quantity').after('<span class="error" style="color:red;">This field is required</span>');
            }
            if ($("#Quantity").val() < 0 || $("#Quantity").val() == 0) {
                $('#Quantity').after('<span class="error" style="color:red;">This field must be postive and bigger than 0</span>');
            }
            if (!$("#Price").val()) {
                $('#Price').after('<span class="error" style="color:red;">This field is required</span>');
            }
            if ($('#Price').val() == 0 || $('#Price').val() < 0) {
                $('#Price').after('<span class="error" style="color:red;">This field must be postive and bigger than 0</span>');
            }
            if ($("#ItemName").val() && $("#StoreName").val() && $("#UnitName").val() && $("#Quantity").val() && $("#Price").val() && $("#Quantity").val() > 0 && $('#Price').val() > 0) {
                //ajax requist to get the selected item detials to calc the table data
                $.ajax({
                    url: "../Purchases/GetItembyID/" + $("#ItemName").val(),
                    type: "GET",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (data) {
                        //cheecking repeated StoreID and ItemID
                                var tBody = $("#tblSkills > TBODY")[0];
                                //Add Row.
                                var row = tBody.insertRow(-1);
                                //Add StoreID
                                cell = $(row.insertCell(-1));
                                cell.html(storeName.val());
                                //Add ItemID
                                cell = $(row.insertCell(-1));
                                cell.html(itemName.val());
                                //Add unit precent
                                cell = $(row.insertCell(-1));
                                cell.html(unitName.val() * quantity.val());
                                //Add Price
                                cell = $(row.insertCell(-1));
                                cell.html(price.val() / unitName.val());
                                //Add Added Tax
                                var puretotal = (unitName.val() * quantity.val() * price.val() / unitName.val());
                                var totalafteraddedtax = (parseFloat(puretotal + (puretotal * (parseFloat(data.AddedTax) / 100))));
                                cell = $(row.insertCell(-1));
                                cell.html((puretotal * (parseFloat(data.AddedTax) / 100)));
                                //Add Total
                                cell = $(row.insertCell(-1));
                                cell.html(totalafteraddedtax);
                                //Add Remove Button cell.
                                cell = $(row.insertCell(-1));
                                var btnRemove = $("<input/>");
                                btnRemove.attr("type", "button");
                                btnRemove.attr("onclick", "Remove(this);");
                                btnRemove.attr("class", "btn btn-danger btn-sm");
                                btnRemove.val("x");
                                cell.append(btnRemove);
                                //Clear the TextBoxes.
                                storeName.val("");
                                itemName.empty();
                                unitName.empty();
                                quantity.val("");
                                price.val("");

                                var total = 0;
                                $("#tblSkills  TBODY TR").each(function () {
                                    var row = $(this);
                                    total = parseFloat(total) + parseFloat(row.find("TD").eq(5).html());
                                });
                                finaltotal = total;
                                finalremaning = total;
                                $("#TTotal").text(total);
                                $("#Payed").val("");
                                $("#Remaining").text(total);
                    },
                    error: function () {
                        alert("Please Check all values becouse They all required.");
                    }
                });
            }

      
});
///Remove item if you pressed the x button
function Remove(button) {
    //Determine the reference of the Row using the Button.
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    var value = $("TD", row).eq(5).html();
    if (confirm("Do you want to delete:" + name)) {
        //Get the reference of the Table.
        var table = $("#tblSkills")[0];
        $("#TTotal").text(parseFloat($("#TTotal").text()) - parseFloat(value));
        $("#Payed").val("");
        $("#Remaining").text($("#TTotal").text());
        if (parseFloat($("#TTotal").text()) < 0 || parseFloat($("#TTotal").text()) == 0) {
            $("#TTotal").text("");
            $("#Remaining").text("");
        }
        //Delete the Table row using it's Index.
        table.deleteRow(row[0].rowIndex);
    }
};
//the code will run on clicking the save button
$("body").on("click", "#btnSave", function () {
    //Loop through the Table rows and build a JSON array.
    var items = new Array();
    $("#tblSkills > TBODY TR").each(function () {
        var row = $(this);
        var item = {};
        //item.BillID = billid,
        item.StoreID = row.find("TD").eq(0).html();
        item.ItemID = row.find("TD").eq(1).html();
        item.Quantity = row.find("TD").eq(2).html();
        item.Price = row.find("TD").eq(3).html();
        item.AddedTax = row.find("TD").eq(4).html();
        item.Total = row.find("TD").eq(5).html();
        items.push(item);
    });
    //Validation for required inputs
    $(".error").remove();
    //if (!$("#BillNumber").val()) {
    //    $('#BillNumber').after('<span class="error" style="color:red;">This field is required</span>');
    //}
    //if (!$("#Date").val()) {
    //    $('#Date').after('<span class="error" style="color:red;">This field is required</span>');
    //}
    if (!$("#Supplier").val()) {
        $('#Supplier').after('<span class="error" style="color:red;">This field is required</span>');
    }
    if (!parseFloat($("#TTotal").text())) {
        $('#TTotal').after('<span class="error" style="color:red;">This field is required</span>');
    }
    if (!$("#Payed").val()) {
        $('#Payed').after('<span class="error" style="color:red;">This field is required</span>');
    }
    else if ($("#Payed").val() < 0 || $("#Payed").val() > parseFloat($("#TTotal").text())) {
        $('#Payed').after('<span class="error" style="color:red;">This field is must be less that total value and postive.</span>');
    }
    if (!finalremaning) {
        $('#Remaining').after('<span class="error" style="color:red;">This field is required</span>');
    }
    if (finalremaning < 0) {
        $('#Remaining').after('<span class="error" style="color:red;">This field must be postive</span>');
    }
    if (  $("#Supplier").val() && parseFloat($("#TTotal").text()) && $("#Payed").val() && (finalremaning || finalremaning == 0 || parseFloat($("#Remaining").text()) > 0) && ($("#Payed").val() < finaltotal || $("#Payed").val() == finaltotal) && ($("#Payed").val() == 0 || $("#Payed").val() > 0)) {
        //$("#BillNumber").val() &&$("#Date").val() && Collect the Bill info in object to send it as jason
        //var ftotal = 0;
        //for (var item in items)
        //{
        //    alert(item.Total.val());
        //      ftotal = ftotal + parseFloat(item.Total);
        //}
        var billobj = {

            //BillNumber: $('#BillNumber').val(),
            //BillDate: $('#Date').val(),
            SupplierID: $('#Supplier').val(),
            Total: parseFloat($("#TTotal").text()),
            Payed: $('#Payed').val(),
            Remain: parseFloat($("#Remaining").text())
        };
        //create an object that contain the billinfo object and billitemsdetails
        var BillInfo = {
            Bill: billobj,
            Items: items
        }
        //using ajax to post the object to controler to add it if it valiad
        $.ajax({
            contentType: "application/json",
            type: "POST",
            method: "POST",
            url: "../Purchases/InsertBill/",
            data: JSON.stringify(BillInfo),
            success: function (sid) {
                //alert if bill inserted or not
                if (sid == true) {
                    alert("bill number is reapeted and your one not inserted please try agian with defrente bill number!! cheeckall inputs!");
                }
                else if (sid == false) {
                    alert("Bill Inserted succufully!!");
                    location.reload();
                }

            },
            error: function () {
                alert("Please Check all values and try again.");
            }
        });
    }
    else {
        alert("there are not valiad inputs please cheeck all inputs amd try agian.")
    }
});