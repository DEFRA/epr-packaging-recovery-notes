import jQuery from "jquery";

// these are not referenced directly, but do not remove! They are required
import validate from "jquery-validation";
import unobtrusiveValidation from "jquery-validation-unobtrusive";
import { initAll } from 'govuk-frontend';

// this is just an example on how to import the scripts
// after the first one is imported this can be removed
import Test from "./pages/testfile";

(function () {
    // the goverment scripts require initAll to be run
    initAll();
})();

// export the testfile javascript code for access in a webpage
// to acces, the format will be: app.Test.[Javascript function]
export {
    Test
};