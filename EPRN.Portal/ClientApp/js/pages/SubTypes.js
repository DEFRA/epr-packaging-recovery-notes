export default (() => {
    $(document).ready(function () {
        // Initial visibility setup
        toggleCustomPercentageVisibility();

        // Event handler for radio button change
        $('input[type=radio][name="SelectedWasteSubTypeId"]').change(function () {
            toggleCustomPercentageVisibility();
        });
    })

    function toggleCustomPercentageVisibility() {
        var selectedRadio = $('input[type=radio][name="SelectedWasteSubTypeId"]:checked');
        var customPercentageInput = $('input[type=text][name="CustomPercentage"]');
        var customPercentageDiv = $('#custom-percentage-div');

        if (selectedRadio.length > 0) {
            var showCustomPercentage = selectedRadio.data('custompercentage') === true;
            customPercentageDiv.toggle(showCustomPercentage);
            var errorSummary = $('.govuk-error-summary');
            // Clear the value and validation errors if hidden
            if (!showCustomPercentage) {
                customPercentageInput.val('');
                errorSummary.addClass('govuk-visually-hidden');

            } else {
                if (validationErrorsExist()) {
                    errorSummary.removeClass('govuk-visually-hidden');
                }
            }
        } else {
            // If no radio is selected, hide the custom percentage input and clear the value
            customPercentageDiv.hide();
            customPercentageInput.val('');
        }
    }

    function validationErrorsExist() {
        var errorList = $('.govuk-list.govuk-error-summary__list');
        var liElements = errorList.find('li');

        return !((liElements.length === 1 && liElements.text().trim() === '') || liElements.length === 0);
    }
})();