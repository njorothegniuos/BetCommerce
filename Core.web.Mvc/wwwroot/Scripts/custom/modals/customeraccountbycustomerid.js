$(document).ready(function () {

    function CustomerAccountGrid(id) {

        var customerAccountGrid;

        if (customerAccountGrid != undefined) {
            customerAccountGrid.fnDestroy();
        };

        customerAccountGrid = $("#customerAccountGrid").DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": "/Accounts/CustomerAccount/AccountsByCustomerId/" + id + "",
            "sServerMethod": "POST",
            // Callback settings
            "fnRowCallback": function(nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                var createdDate = new Date(parseInt(aData.CreatedDate.replace("/Date(", "").replace(")/", ""), 10));

                var h = ("0" + createdDate.getHours()).slice(-2);

                var m = ("0" + createdDate.getMinutes()).slice(-2);

                var s = ("0" + createdDate.getSeconds()).slice(-2);

                var dt_Created = $.datepicker.formatDate("dd/mm/yy", createdDate) + " " + h + ":" + m + ":" + s;

                aData.CreatedDate = dt_Created;

                $("td:eq(7)", nRow).html(dt_Created);

                return nRow;
            },
            "aoColumns": [
                { "mDataProp": "CustomerName", "sTitle": "Customer Name", "bSortable": false },
                { "mDataProp": "CustomerSerialNumber", "sTitle": "Serial #", "bSortable": false },
                { "mDataProp": "PrimaryAccountNumber", "sTitle": "PAN", "bSortable": false },
                { "mDataProp": "CurrencyName", "sTitle": "Currency", "bSortable": false },
                { "mDataProp": "RecordStatusDesc", "sTitle": "Record Status", "bSortable": false },
                { "mDataProp": "DailyThreshold", "sTitle": "Threshold", "bSortable": false },
                { "mDataProp": "CreatedBy", "sTitle": "Created By", "bSortable": false },
                { "mDataProp": "CreatedDate", "sTitle": "Created Date" },
                { "mDataProp": "CreatedDate", "sTitle": "Action", "sWidth": "30%", "bSortable": false }
            ],
            "aaSorting": [[6, "desc"]],
            "columnDefs": [{ className: "text-right", targets: [5] }],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            buttons: [
                { extend: "copy" },
                { extend: "csv" },
                { extend: "excel", title: "ExampleFile" },
                { extend: "pdf", title: "ExampleFile" },
                {
                    extend: "print",
                    customize: function(win) {
                        $(win.document.body).addClass("white-bg");
                        $(win.document.body).css("font-size", "10px");

                        $(win.document.body).find("table")
                            .addClass("compact")
                            .css("font-size", "inherit");
                    }
                }
            ]
        });
    }

    
});