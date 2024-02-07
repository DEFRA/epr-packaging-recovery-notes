import { Accordion } from 'govuk-frontend';

// Override the government initControls so that
// we can 
Accordion.prototype.initControls = function () {
    if (this.canDisplayShowAll()) {
        // Create "Show all" button and set attributes
        this.$showAllButton = document.createElement('button');
        this.$showAllButton.setAttribute('type', 'button');
        this.$showAllButton.setAttribute('class', this.showAllClass);
        this.$showAllButton.setAttribute('aria-expanded', 'false');

        // Create icon, add to element
        this.$showAllIcon = document.createElement('span');
        this.$showAllIcon.classList.add(this.upChevronIconClass);
        this.$showAllButton.appendChild(this.$showAllIcon);

        // Create control wrapper and add controls to it
        var $accordionControls = document.createElement('div');
        $accordionControls.setAttribute('class', this.controlsClass);
        $accordionControls.appendChild(this.$showAllButton);
        this.$module.insertBefore($accordionControls, this.$module.firstChild);

        // Build additional wrapper for Show all toggle text and place after icon
        this.$showAllText = document.createElement('span');
        this.$showAllText.classList.add(this.showAllTextClass);
        this.$showAllButton.appendChild(this.$showAllText);

        // Handle click events on the show/hide all button
        this.$showAllButton.addEventListener('click', this.onShowOrHideAllToggle.bind(this));
    }
    // Handle 'beforematch' events, if the user agent supports them
    if ('onbeforematch' in document) {
        document.addEventListener('beforematch', this.onBeforeMatch.bind(this));
    }
}

Accordion.prototype.updateShowAllButton = function (expanded) {
    var newButtonText = expanded
        ? this.i18n.t('hideAllSections')
        : this.i18n.t('showAllSections');
    if (this.$showAllButton != null) {
        this.$showAllButton.setAttribute('aria-expanded', expanded.toString());
        this.$showAllText.innerText = newButtonText;

        // Swap icon, toggle class
        if (expanded) {
            this.$showAllIcon.classList.remove(this.downChevronIconClass);
        } else {
            this.$showAllIcon.classList.add(this.downChevronIconClass);
        }
    }
};

Accordion.prototype.canDisplayShowAll = function () {
    return this.$sections > 1;
}

export default Accordion