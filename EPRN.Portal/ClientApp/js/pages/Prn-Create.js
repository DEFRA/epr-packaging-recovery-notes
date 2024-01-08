export default (() => {

    $(document).ready(function () {
        $('#sort').on('change', function () {

            var dropDownValue = $(this).val();

            $('.prnTables').removeClass('govuk-!-display-none');

            switch (dropDownValue) {
                case 'reprocessor':
                    $('.exporter').addClass('govuk-!-display-none');
                    break;
                case 'exporter':
                    $('.reprocessor').addClass('govuk-!-display-none');
                    break;
            };

        });
    })
       
})();