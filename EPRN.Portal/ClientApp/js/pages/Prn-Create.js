export default (() => {

    $(document).ready(function () {
        $('#sort').on('change', function () {

            var dropDownValue = $(this).val();

            $('.prnTables').removeClass('govuk-visually-hidden');

            switch (dropDownValue) {
                case 'reprocessor':
                    $('.exporter').addClass('govuk-visually-hidden');
                    break;
                case 'exporter':
                    $('.reprocessor').addClass('govuk-visually-hidden');
                    break;
            };

        });
    })
       
})();