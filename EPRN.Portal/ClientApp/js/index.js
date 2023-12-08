﻿import jQuery from "jquery";

// these are not referenced directly, but do not remove! They are required
import validate from "jquery-validation";
import unobtrusiveValidation from "jquery-validation-unobtrusive";
import inputmask from "inputmask";

import { initAll } from 'govuk-frontend';

// @ts-ignore
window.$ = window.jQuery = jQuery;

// looks like it's unused, but it is. This is so we can hook into jquery validation
// functions and add further validation routines in
import jqueryvalidatehooks from "./common/jquery-validate-hooks";

// add imports to further files here

$(document).ready(function () {
    initAll();

    var inputs = $('input.3dp');

    // setup input masks for 3 decimal places
    Inputmask({
        regex: "^[0-9]*(\\.[0-9]{0,3})?$",
        placeholder: ""
    }).mask(inputs);
    
    $('form').addTriggersToJqueryValidate();

    $('form').formValidation(function (element, result) {
        clearSummaryErrors();
        if (!result) {
            displayValidation();
        }
    });

    $('form input').elementValidationError(function (element) {
        clearSummaryErrors();
        displayValidation();
    });

    $('form input').elementValidationSuccess(function (element) {
        
        $(element).closest('.govuk-form-group').removeClass('govuk-form-group--error');

        // find the associated error in the summary and remove
        $(`.govuk-error-summary .govuk-list.govuk-error-summary__list [data-id=${element.id}]`).remove();

        // if there are no more errors in the summary, hide it
        if ($('.govuk-error-summary .govuk-list.govuk-error-summary__list li').is(':empty'))
            $('.govuk-error-summary').addClass('govuk-visually-hidden');
    });
});

var clearSummaryErrors = function () {
    $('.govuk-error-summary .govuk-list.govuk-error-summary__list').empty();
}
var displayValidation = function () {
    $('.govuk-error-summary.govuk-visually-hidden').removeClass('govuk-visually-hidden');
    $('.field-validation-error').each(function () {
        // is this part of a radio input? If so, add govuk-form-group--error to govuk-form-group
        var element = $(this);

        element.closest('.govuk-form-group').addClass('govuk-form-group--error');

        // get the attribute value for inputs with the name field
        var nameAttribute = element.attr('data-valmsg-for');

        // get first input element with the name described above
        var inputField = $(`[name=${nameAttribute}]`)[0];
        var text = element.text();

        $('.govuk-error-summary .govuk-list.govuk-error-summary__list').append(`<li><a href="#${inputField.id}" data-id="${inputField.id}">${text}</a></li>`)
    });
}

// export the testfile javascript code for access in a webpage
// to acces, the format will be: app.Test.[Javascript function]
export {
    //Test
};