$(document).ready(function () {

    function CountryLookupGrid() {

        var CountryLookupGrid;

        if (CountryLookupGrid != undefined) {
            CountryLookupGrid.fnDestroy();
        };

        CountryLookupGrid = $('#countryLookupGrid').DataTable({
            "bretrieve": true,
            "sPaginationType": "full_numbers",
            "bProcessing": true,
            "bServerSide": true,
            "info": false,
            "sAjaxSource": '/administration/Routes/countries',
            "sServerMethod": "POST",
            // Callback settings
            "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {

                var createdDate = new Date(parseInt(aData.createdDate.replace("/Date(", "").replace(")/", ""), 10));
                var h_createdDate = ("0" + createdDate.getHours()).slice(-2);
                var m_createdDate = ("0" + createdDate.getMinutes()).slice(-2);
                var s_createdDate = ("0" + createdDate.getSeconds()).slice(-2);

                var dt_Created = $.datepicker.formatDate('dd/mm/yy', createdDate) + " " + h_createdDate + ":" + m_createdDate + ":" + s_createdDate;
                aData.CreatedDate = dt_Created;

                $('td:eq(3)', nRow).html(dt_Created);

                return nRow;
            },
            // Display settings
            "aoColumns": [
                { "mDataProp": "name", "sTitle": "Name", "bSortable": false },
                { "mDataProp": "alphaCode2", "sTitle": "Alpha Code", "bSortable": false },
                { "mDataProp": "numericCode", "sTitle": "NumericCode", "bSortable": false },
                { "mDataProp": "createdDate", "sTitle": "Action", "sWidth": "10%", "bSortable": false }],
            "aaSorting": [[3, "desc"]],
            "responsive": true,
            "dom": '<"html5buttons"B>lTfgitp',
            "buttons": []
        });

        $('#countryLookupGrid tbody').on('click', 'tr', function () {

            var data = CountryLookupGrid.row(this).data();

            $('input[name=CountryId]').val(data.Id);
            $('input[name=CountryName]').val(data.Name);

            //$.get("/Lookup/FindCurrenciesByCountryId/" + data.Id, function (response, status) {

            //    if (response[0] != null) {
            //        $('input[name=CurrencyId]').val(response[0].Id);
            //        $('input[name=CurrencyName]').val(response[0].Name);
            //        $('input[name=CurrencyCode]').val(response[0].Code);
            //    }

            //});


            $('#CountryLookupModal').modal('hide');
        });
    }

    CountryLookupGrid();
});