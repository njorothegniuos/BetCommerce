var currentRow = 0;

function showChargesLookup(cr) {
    currentRow = cr;
    $('#ChargesLookupModal').modal('show');
}

$(document).ready(function () {

    var chargesLookupGrid = $('#ChargesLookupGrid').DataTable({
        "sPaginationType": "full_numbers",
        "bProcessing": true,
        "bServerSide": true,
        "responsive": true,
        //"bJQueryUI": true,
        "sAjaxSource": "/administration/charges",
        "sServerMethod": "POST",
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

            var createdDate = new Date(parseInt(aData.CreatedDate.replace("/Date(", "").replace(")/", ""), 10));

            var h = ("0" + createdDate.getHours()).slice(-2);

            var m = ("0" + createdDate.getMinutes()).slice(-2);

            var s = ("0" + createdDate.getSeconds()).slice(-2);

            var dt_Created = $.datepicker.formatDate('dd/mm/yy', createdDate) + " " + h + ":" + m + ":" + s;

            aData.CreatedDate = dt_Created;

            var minimimCharge = aData.MinimumCharge.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

            $('td:eq(2)', nRow).html(minimimCharge).css('text-align', 'right');

            var maximumCharge = aData.MaximumCharge.toFixed(2).replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,");

            $('td:eq(3)', nRow).html(maximumCharge).css('text-align', 'right');

            var viewUrl = '<a  href="/administration/charges/view/' + aData.Id + '" class = "btn btn-primary"">' + 'View</a>';

            var editUrl = '<a  href="/administration/charges/edit/' + aData.Id + '" class = "btn btn-primary"">' + 'Edit</a>';

            $('td:eq(5)', nRow).html(dt_Created);

            $('td:eq(6)', nRow).html(viewUrl + ' ' + editUrl);

            return nRow;
        },
        "aoColumns": [
            { "mDataProp": "Name", "sTitle": "Name", "bSortable": false },
            { "mDataProp": "TransactionTypeDesc", "sTitle": "Trx Type", "bSortable": false },
            { "mDataProp": "MinimumCharge", "sTitle": "Minimum Charge", "bSortable": false },
            { "mDataProp": "MaximumCharge", "sTitle": "Maximum Charge", "bSortable": false },
            { "mDataProp": "IsEnabled", "sTitle": "Is Enabled?", "bSortable": false },
            { "mDataProp": "CreatedDate", "sTitle": "Created Date" },
            { "mDataProp": "CreatedDate", "sTitle": "Action", "sWidth": "10%", "bSortable": false }],
        "aaSorting": [[4, "desc"]],
        dom: '<"html5buttons"B>lTfgitp',
        buttons: [
           
            { extend: 'csv' },
            { extend: 'excel', title: 'Charges list' },
            { extend: 'pdf', title: 'Charges list' },
            {
                extend: 'print',
                customize: function (win) {
                    $(win.document.body).addClass('white-bg');
                    $(win.document.body).css('font-size', '10px');

                    $(win.document.body).find('table')
                        .addClass('compact')
                        .css('font-size', 'inherit');
                }
            }
        ]
    });

    $('#ChargesLookupGrid tbody').on('click', 'tr', function () {

        var data = chargesLookupGrid.row(this).data();

        $('#CustomerChargeMapping_' + currentRow + '__ChargeId').val(data.Id);
        $('#CustomerChargeMapping_' + currentRow + '__ChargeName').val(data.Name);
        $('#CustomerChargeMapping_' + currentRow + '_CommissionCharge').val(data.Name);
        $('#CustomerChargeMapping_' + currentRow + '__ChargeTransactionType').val(data.TransactionType);
        $('#CustomerChargeMapping_' + currentRow + '__ChargeTransactionTypeDesc').val(data.TransactionTypeDesc);

        $('#ChargeId').val(data.Id);
        $('#ChargeName').val(data.Name);
        $('#CommissionCharge').val(data.Name);
        $('#ChargeTransactionType').val(data.TransactionType);
        $('#ChargeTransactionTypeDesc').val(data.TransactionTypeDesc);


        $('#ChargesLookupModal').modal('hide');

        addGraduatedScales()
    });

});