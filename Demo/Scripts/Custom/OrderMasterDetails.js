$(document).ready(function () {
    setDate();
    getdata();

    $('form[id="frmItemDetails"]').validate({
        rules: {
            ItemName: 'required',
            Qty: 'required',
            Rate: 'required',
        },
        messages: {
            ItemName: 'This field is required',
            Qty: 'This field is required',
            Rate: 'This field is required'
        },
        submitHandler: function (form) {
            form.submit();
        }
    });
});

function getDueDate() {
    var mdate = document.getElementById('dteMDate').value;
    var duedays = document.getElementById('txtDueDays').value;

    var date = new Date(mdate);
    var newdate = new Date(date);

    newdate.setDate(newdate.getDate() + parseInt(duedays));

    var dd = ("0" + newdate.getDate()).slice(-2);
    var mm = ("0" + (newdate.getMonth() + 1)).slice(-2);
    var y = newdate.getFullYear();


    var someFormattedDate = y + "-" + (mm) + "-" + (dd);
    $('#dteDueDate').val(someFormattedDate);
}

function setDate() {
    var now = new Date();

    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);

    var today = now.getFullYear() + "-" + (month) + "-" + (day);

    $('#dteMDate').val(today);
    $('#dteDueDate').val(today);
}

var orderItems = [];

$("#btnAdd").click(function () {
    AddItemIntoList();
});

function AddItemIntoList() {
    if ($("#frmItemDetails").valid()) {
        orderItems.push({
            SerialNo: $('#txtSNo').val().trim(),
            ItemName: $('#txtItemName').val().trim(),
            Qty: parseInt($('#txtQty').val().trim()),
            Rate: parseFloat($('#txtRate').val().trim()),
            Amount: parseInt($('#txtAmount').val().trim())
        });
        if (orderItems.length > 0) {
            createTable(orderItems);
        }
        else {
            $('#orderItems').html('');
        }
    }
}

function createTable(orderItems) {
    var total = 0;
    for (var i = 0; i < orderItems.length; i++) {
        total += orderItems[i].Amount;
    }
    var $table = $('<table/>');
    $table.append('<thead><tr><th>SNo</th><th>Item</th><th>Qty</th><th>Rate</th><th>Amount</th><th></th></tr></thead>');
    var $tbody = $('<tbody/>');
    $.each(orderItems, function (i, val) {
        var $row = $('<tr/>');
        $row.append($('<td/>').html(i + 1));
        $row.append($('<td/>').html(val.ItemName));
        $row.append($('<td/>').html(val.Qty));
        $row.append($('<td/>').html(val.Rate));
        $row.append($('<td/>').html(val.Amount));
        var $remove = $('<a href="#" onclick="removeItem(' + i + ')">Remove</a>');

        $row.append($('<td/>').html($remove));
        $tbody.append($row);
    });
    var $totalrow = $('<tr/>');
    $totalrow.append($('<td/>').html(''));
    $totalrow.append($('<td/>').html(''));
    $totalrow.append($('<td/>').html('Total'));
    $totalrow.append($('<td/>').html(''));
    $totalrow.append($('<td/>').html(total));
    $tbody.append($totalrow);

    $table.append($tbody);
    $('#orderItems').html($table);

    var nextSNo = orderItems.length + 1;
    $("#txtSNo").val(nextSNo);
    ClearItemsDetails();
}

function removeItem(Index) {
    orderItems.splice(Index, 1);
    createTable(orderItems);
}

function getTotalAmt() {
    var totalAmt = 0;
    var qty = parseInt($('#txtQty').val().trim());
    var rate = parseInt($('#txtRate').val().trim());
    if (qty && rate) {
        totalAmt = qty * rate;
    }
    $('#txtAmount').val(totalAmt);
}

function ClearItemsDetails() {
    $('#txtItemName').val('');
    $('#txtQty').val('');
    $('#txtRate').val('');
    $('#txtAmount').val('');
}

$("#btnSubmit").click(function () {

    var data = {
        Code: $('#txtCode').val().trim(),
        MDate: $('#dteMDate').val().trim(),
        DueDays: $('#txtDueDays').val().trim(),
        DueDate: $('#dteDueDate').val().trim(),
        Party: $('#txtParty').val().trim(),
        OrderDetails: orderItems
    }

    $.ajax({
        url: '/OrderMaster/SaveOrder',
        type: "POST",
        data: JSON.stringify(data),
        dataType: "JSON",
        contentType: "application/json",
        success: function (response) {
            if (response.status == true) {
                alert(response.message);
                orderItems = [];
                $("#orderItems").empty();
                //getdata();
            }
            else {
                alert('Failed');
            }
        },
        error: function () {
            alert('Error. Please try again.');
        }
    });
});

function getdata() {
    $.ajax({
        type: "POST",
        dataType: "json",
        //data: "{'Code':'" + code + "','Type':'" + '4' + "','TableName':'SafeMaster','SerialColumnName':'SafeCode','TypeColumnName':'','CodeType':''}",
        url: "/OrderMaster/GetData",
        success: function (data) {
            debugger
            var datatableVariable = $('#orderItemTable').DataTable({
                data: data.Data,
                columns: [
                    { 'data': 'Code' },
                    {
                        'data': 'MDate', 'render': function (date) {
                            var date = new Date(parseInt(date.substr(6)));
                            var month = date.getMonth() + 1;
                            return date.getDate() + "/" + month + "/" + date.getFullYear();
                        }
                    },
                    { 'data': 'Party' }

                ]
            });
            $('#orderItemTable tfoot th').each(function () {
                var placeHolderTitle = $('#orderItemTable thead th').eq($(this).index()).text();
                $(this).html('<input type="text" class="form-control input input-sm" placeholder = "Search ' + placeHolderTitle + '" />');
            });
            datatableVariable.columns().every(function () {
                var column = this;
                $(this.footer()).find('input').on('keyup change', function () {
                    column.search(this.value).draw();
                });
            });
        }
    });
}

