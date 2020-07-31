/**
 * bootstrap-multiselect.js
 * https://github.com/davidstutz/bootstrap-multiselect
 *
 * Copyright 2012, 2013 David Stutz
 *
 * Dual licensed under the BSD-3-Clause and the Apache License, Version 2.0.
 */
!function($) {

    "use strict";// jshint ;_;

    if (typeof ko !== 'undefined' && ko.bindingHandlers && !ko.bindingHandlers.multiselect) {
        ko.bindingHandlers.multiselect = {
            init: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {},
            update: function(element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {

               var config = ko.utils.unwrapObservable(valueAccessor());
               var selectOptions = allBindingsAccessor().options();
               var ms = $(element).data('multiselect');

               if (!ms) {
                  $(element).multiselect(config);
               }
               else {
                  ms.updateOriginalOptions();
                  if (selectOptions && selectOptions.length !== ms.originalOptions.length) {
                     $(element).multiselect('rebuild');
                  }
               }
            }
        };
    }

    function Multiselect(select, options) {

        this.options = this.mergeOptions(options);
        this.$select = $(select);

        // Initialization.
        // We have to clone to create a new reference.
        this.originalOptions = this.$select.clone()[0].options;
        this.query = '';
        this.searchTimeout = null;

        this.options.multiple = this.$select.attr('multiple') === "multiple";
        this.options.onChange = $.proxy(this.options.onChange, this);
        this.options.onDropdownShow = $.proxy(this.options.onDropdownShow, this);
        this.options.onDropdownHide = $.proxy(this.options.onDropdownHide, this);

        // Build select all if enabled.
        this.buildContainer();
        this.buildButton();
        this.buildSelectAll();
        this.buildDropdown();
        this.buildDropdownOptions();
        this.buildFilter();
        this.updateButtonText();

        this.$select.hide().after(this.$container);
    };

    Multiselect.prototype = {

        // Default options.
        defaults: {
            // Default text function will either print 'None selected' in case no
            // option is selected, or a list of the selected options up to a length of 3 selected options by default.
            // If more than 3 options are selected, the number of selected options is printed.
            buttonText: function(options, select) {
                if (options.length === 0) {
                    return this.nonSelectedText + ' <b class="caret"></b>';
                }
                else {
                    if (options.length > this.numberDisplayed) {
                        return options.length + ' ' + this.nSelectedText + ' <b class="caret"></b>';
                    }
                    else {
                        var selected = '';
                        options.each(function() {
                            var label = ($(this).attr('label') !== undefined) ? $(this).attr('label') : $(this).html();

                            selected += label + ', ';
                        });
                        return selected.substr(0, selected.length - 2) + ' <b class="caret"></b>';
                    }
                }
            },
            // Like the buttonText option to update the title of the button.
            buttonTitle: function(options, select) {
                if (options.length === 0) {
                    return this.nonSelectedText;
                }
                else {
                    var selected = '';
                    options.each(function () {
                        selected += $(this).text() + ', ';
                    });
                    return selected.substr(0, selected.length - 2);
                }
            },
            // Create label
            label: function( element ){
                return $(element).attr('label') || $(element).html();
            },
            // Is triggered on change of the selected options.
            onChange : function(option, checked) {

            },
            // Triggered immediately when dropdown shown
            onDropdownShow: function(event) {

            },
            // Triggered immediately when dropdown hidden
            onDropdownHide: function(event) {

            },
            buttonClass: 'btn btn-default',
            dropRight: false,
            selectedClass: 'active',
            buttonWidth: 'auto',
            buttonContainer: '<div class="btn-group" />',
            // Maximum height of the dropdown menu.
            // If maximum height is exceeded a scrollbar will be displayed.
            maxHeight: false,
            includeSelectAllOption: false,
            selectAllText: ' Select all',
            selectAllValue: 'multiselect-all',
            enableFiltering: false,
            enableCaseInsensitiveFiltering: false,
            filterPlaceholder: 'Search',
            // possible options: 'text', 'value', 'both'
            filterBehavior: 'text',
            preventInputChangeEvent: false,
            nonSelectedText: 'None selected',
            nSelectedText: 'selected',
            numberDisplayed: 3
        },

        // Templates.
        templates: {
            button: '<button type="button" class="multiselect dropdown-toggle" data-toggle="dropdown"></button>',
            ul: '<ul class="multiselect-container dropdown-menu"></ul>',
            filter: '<div class="input-group"><span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span><input class="form-control multiselect-search" type="text"></div>',
            li: '<li><a href="javascript:void(0);"><label></label></a></li>',
            liGroup: '<li><label class="multiselect-group"></label></li>'
        },

        constructor: Multiselect,

        buildContainer: function() {
            this.$container = $(this.options.buttonContainer);
            this.$container.on('show.bs.dropdown', this.options.onDropdownShow);
            this.$container.on('hide.bs.dropdown', this.options.onDropdownHide);
        },

        buildButton: function() {
            // Build button.
            this.$button = $(this.templates.button).addClass(this.options.buttonClass);

            // Adopt active state.
            if (this.$select.prop('disabled')) {
                this.disable();
            }
            else {
                this.enable();
            }

            // Manually add button width if set.
            if (this.options.buttonWidth) {
                this.$button.css({
                    'width' : this.options.buttonWidth
                });
            }

            // Keep the tab index from the select.
            var tabindex = this.$select.attr('tabindex');
            if (tabindex) {
                this.$button.attr('tabindex', tabindex);
            }

            this.$container.prepend(this.$button);
        },

        // Build dropdown container ul.
        buildDropdown: function() {

            // Build ul.
            this.$ul = $(this.templates.ul);

            if (this.options.dropRight) {
                this.$ul.addClass('pull-right');
            }

            // Set max height of dropdown menu to activate auto scrollbar.
            if (this.options.maxHeight) {
                // TODO: Add a class for this option to move the css declarations.
                this.$ul.css({
                    'max-height': this.options.maxHeight + 'px',
                    'overflow-y': 'auto',
                    'overflow-x': 'hidden'
                });
            }

            this.$container.append(this.$ul);
        },

        // Build the dropdown and bind event handling.
        buildDropdownOptions: function() {

            this.$select.children().each($.proxy(function(index, element) {
                // Support optgroups and options without a group simultaneously.
                var tag = $(element).prop('tagName')
                    .toLowerCase();
                
                if (tag === 'optgroup') {
                    this.createOptgroup(element);
                }
                else if (tag === 'option') {
                    this.createOptionValue(element);
                }
                // Other illegal tags will be ignored.
            }, this));

            // Bind the change event on the dropdown elements.
            $('li input', this.$ul).on('change', $.proxy(function(event) {
                var checked = $(event.target).prop('checked') || false;
                var isSelectAllOption = $(event.target).val() === this.options.selectAllValue;

                // Apply or unapply the configured selected class.
                if (this.options.selectedClass) {
                    if (checked) {
                        $(event.target).parents('li')
                            .addClass(this.options.selectedClass);
                    }
                    else {
                        $(event.target).parents('li')
                            .removeClass(this.options.selectedClass);
                    }
                }

                // Get the corresponding option.
                var value = $(event.target).val();
                var $option = this.getOptionByValue(value);

                var $optionsNotThis = $('option', this.$select).not($option);
                var $checkboxesNotThis = $('input', this.$container).not($(event.target));

                if (isSelectAllOption) {
                    if (this.$select[0][0].value === this.options.selectAllValue) {
                        var values = [];
                        var options = $('option[value!="' + this.options.selectAllValue + '"]', this.$select);
                        for (var i = 0; i < options.length; i++) {
                            // Additionally check whether the option is visible within the dropcown.
                            if (options[i].value !== this.options.selectAllValue && this.getInputByValue(options[i].value).is(':visible')) {
                                values.push(options[i].value);
                            }
                        }
                        
                        if (checked) {
                            this.select(values);
                        }
                        else {
                            this.deselect(values);
                        }
                    }
                }

                if (checked) {
                    $option.prop('selected', true);

                    if (this.options.multiple) {
                        // Simply select additional option.
                        $option.prop('selected', true);
                    }
                    else {
                        // Unselect all other options and corresponding checkboxes.
                        if (this.options.selectedClass) {
                            $($checkboxesNotThis).parents('li').removeClass(this.options.selectedClass);
                        }

                        $($checkboxesNotThis).prop('checked', false);
                        $optionsNotThis.prop('selected', false);

                        // It's a single selection, so close.
                        this.$button.click();
                    }

                    if (this.options.selectedClass === "active") {
                        $optionsNotThis.parents("a").css("outline", "");
                    }
                }
                else {
                    // Unselect option.
                    $option.prop('selected', false);
                }

                this.$select.change();
                this.options.onChange($option, checked);
                this.updateButtonText();

                if(this.options.preventInputChangeEvent) {
                    return false;
                }
            }, this));

            $('li a', this.$ul).on('touchstart click', function(event) {
                event.stopPropagation();

                if (event.shiftKey) {
                    var checked = $(event.target).prop('checked') || false;
                    
                    if (checked) {
                        var prev = $(event.target).parents('li:last')
                            .siblings('li[class="active"]:first');

                        var currentIdx = $(event.target).parents('li')
                            .index();
                        var prevIdx = prev.index();

                        if (currentIdx > prevIdx) {
                            $(event.target).parents("li:last").prevUntil(prev).each(
                                function() {
                                    $(this).find("input:first").prop("checked", true)
                                        .trigger("change");
                                }
                            );
                        }
                        else {
                            $(event.target).parents("li:last").nextUntil(prev).each(
                                function() {
                                    $(this).find("input:first").prop("checked", true)
                                        .trigger("change");
                                }
                            );
                        }
                    }
                }
                
                $(event.target).blur();
            });

            // Keyboard support.
            this.$container.on('keydown', $.proxy(function(event) {
                if ($('input[type="text"]', this.$container).is(':focus')) {
                    return;
                }
                if ((event.keyCode === 9 || event.keyCode === 27) && this.$container.hasClass('open')) {
                    // Close on tab or escape.
                    this.$button.click();
                }
                else {
                    var $items = $(this.$container).find("li:not(.divider):visible a");

                    if (!$items.length) {
                        return;
                    }

                    var index = $items.index($items.filter(':focus'));

                    // Navigation up.
                    if (event.keyCode === 38 && index > 0) {
                        index--;
                    }
                    // Navigate down.
                    else if (event.keyCode === 40 && index < $items.length - 1) {
                        index++;
                    }
                    else if (!~index) {
                        index = 0;
                    }

                    var $current = $items.eq(index);
                    $current.focus();

                    if (event.keyCode === 32 || event.keyCode === 13) {
                        var $checkbox = $current.find('input');

                        $checkbox.prop("checked", !$checkbox.prop("checked"));
                        $checkbox.change();
                    }

                    event.stopPropagation();
                    event.preventDefault();
                }
            }, this));
        },

        // Will build an dropdown element for the given option.
        createOptionValue: function(element) {
            if ($(element).is(':selected')) {
                $(element).prop('selected', true);
            }

            // Support the label attribute on options.
            var label = this.options.label(element);
            var value = $(element).val();
            var inputType = this.options.multiple ? "checkbox" : "radio";

            var $li = $(this.templates.li);
            $('label', $li).addClass(inputType);
            $('label', $li).append('<input type="' + inputType + '" />');

            var selected = $(element).prop('selected') || false;
            var $checkbox = $('input', $li);
            $checkbox.val(value);

            if (value === this.options.selectAllValue) {
                $checkbox.parent().parent()
                    .addClass('multiselect-all');
            }

            $('label', $li).append(" " + label);

            this.$ul.append($li);

            if ($(element).is(':disabled')) {
                $checkbox.attr('disabled', 'disabled')
                    .prop('disabled', true)
                    .parents('li')
                    .addClass('disabled');
            }

            $checkbox.prop('checked', selected);

            if (selected && this.options.selectedClass) {
                $checkbox.parents('li')
                    .addClass(this.options.selectedClass);
            }
        },

        // Create optgroup.
        createOptgroup: function(group) {
            var groupName = $(group).prop('label');

            // Add a header for the group.
            var $li = $(this.templates.liGroup);
            $('label', $li).text(groupName);

            this.$ul.append($li);

            // Add the options of the group.
            $('option', group).each($.proxy(function(index, element) {
                this.createOptionValue(element);
            }, this));
        },

        // Add the select all option to the select.
        buildSelectAll: function() {
            var alreadyHasSelectAll = this.$select[0][0] ? this.$select[0][0].value === this.options.selectAllValue : false;
            
            // If options.includeSelectAllOption === true, add the include all checkbox.
            if (this.options.includeSelectAllOption && this.options.multiple && !alreadyHasSelectAll) {
                this.$select.prepend('<option value="' + this.options.selectAllValue + '">' + this.options.selectAllText + '</option>');
            }
        },

        // Build and bind filter.
        buildFilter: function() {

            // Build filter if filtering OR case insensitive filtering is enabled and the number of options exceeds (or equals) enableFilterLength.
            if (this.options.enableFiltering || this.options.enableCaseInsensitiveFiltering) {
                var enableFilterLength = Math.max(this.options.enableFiltering, this.options.enableCaseInsensitiveFiltering);
                
                if (this.$select.find('option').length >= enableFilterLength) {

                    this.$filter = $(this.templates.filter);
                    $('input', this.$filter).attr('placeholder', this.options.filterPlaceholder);
                    this.$ul.prepend(this.$filter);

                    this.$filter.val(this.query).on('click', function(event) {
                        event.stopPropagation();
                    }).on('keydown', $.proxy(function(event) {
                        // This is useful to catch "keydown" events after the browser has updated the control.
                        clearTimeout(this.searchTimeout);

                        this.searchTimeout = this.asyncFunction($.proxy(function() {

                            if (this.query !== event.target.value) {
                                this.query = event.target.value;

                                $.each($('li', this.$ul), $.proxy(function(index, element) {
                                    var value = $('input', element).val();
                                    var text = $('label', element).text();
                                    
                                    if (value !== this.options.selectAllValue && text) {
                                        // by default lets assume that element is not
                                        // interesting for this search
                                        var showElement = false;

                                        var filterCandidate = '';
                                        if ((this.options.filterBehavior === 'text' || this.options.filterBehavior === 'both')) {
                                            filterCandidate = text;
                                        }
                                        if ((this.options.filterBehavior === 'value' || this.options.filterBehavior === 'both')) {
                                            filterCandidate = value;
                                        }

                                        if (this.options.enableCaseInsensitiveFiltering && filterCandidate.toLowerCase().indexOf(this.query.toLowerCase()) > -1) {
                                            showElement = true;
                                        }
                                        else if (filterCandidate.indexOf(this.query) > -1) {
                                            showElement = true;
                                        }

                                        if (showElement) {
                                            $(element).show();
                                        }
                                        else {
                                            $(element).hide();
                                        }
                                    }
                                }, this));
                            }
                            
                            // TODO: check whether select all option needs to be updated.
                        }, this), 300, this);
                    }, this));
                }
            }
        },

        // Destroy - unbind - the plugin.
        destroy: function() {
            this.$container.remove();
            this.$select.show();
        },

        // Refreshs the checked options based on the current state of the select.
        refresh: function() {
            $('option', this.$select).each($.proxy(function(index, element) {
                var $input = $('li input', this.$ul).filter(function() {
                    return $(this).val() === $(element).val();
                });

                if ($(element).is(':selected')) {
                    $input.prop('checked', true);

                    if (this.options.selectedClass) {
                        $input.parents('li')
                            .addClass(this.options.selectedClass);
                    }
                }
                else {
                    $input.prop('checked', false);

                    if (this.options.selectedClass) {
                        $input.parents('li')
                            .removeClass(this.options.selectedClass);
                    }
                }

                if ($(element).is(":disabled")) {
                    $input.attr('disabled', 'disabled')
                        .prop('disabled', true)
                        .parents('li')
                        .addClass('disabled');
                }
                else {
                    $input.prop('disabled', false)
                        .parents('li')
                        .removeClass('disabled');
                }
            }, this));

            this.updateButtonText();
        },

        // Select an option by its value or multiple options using an array of values.
        select: function(selectValues) {
            if(selectValues && !$.isArray(selectValues)) {
                selectValues = [selectValues];
            }

            for (var i = 0; i < selectValues.length; i++) {
                var value = selectValues[i];

                var $option = this.getOptionByValue(value);
                var $checkbox = this.getInputByValue(value);

                if (this.options.selectedClass) {
                    $checkbox.parents('li')
                        .addClass(this.options.selectedClass);
                }

                $checkbox.prop('checked', true);
                $option.prop('selected', true);
                this.options.onChange($option, true);
            }

            this.updateButtonText();
        },

        // Deselect an option by its value or using an array of values.
        deselect: function(deselectValues) {
            if(deselectValues && !$.isArray(deselectValues)) {
                deselectValues = [deselectValues];
            }

            for (var i = 0; i < deselectValues.length; i++) {

                var value = deselectValues[i];

                var $option = this.getOptionByValue(value);
                var $checkbox = this.getInputByValue(value);

                if (this.options.selectedClass) {
                    $checkbox.parents('li')
                        .removeClass(this.options.selectedClass);
                }

                $checkbox.prop('checked', false);
                $option.prop('selected', false);
                this.options.onChange($option, false);
            }

            this.updateButtonText();
        },

        // Rebuild the whole dropdown menu.
        rebuild: function() {
            this.$ul.html('');

            // Remove select all option in select.
            $('option[value="' + this.options.selectAllValue + '"]', this.$select).remove();

            // Important to distinguish between radios and checkboxes.
            this.options.multiple = this.$select.attr('multiple') === "multiple";

            this.buildSelectAll();
            this.buildDropdownOptions();
            this.updateButtonText();
            this.buildFilter();
        },

        // Build select using the given data as options.
        dataprovider: function(dataprovider) {
            var optionDOM = "";
            dataprovider.forEach(function (option) {
                optionDOM += '<option value="' + option.value + '">' + option.label + '</option>';
            });

            this.$select.html(optionDOM);
            this.rebuild();
        },

        // Enable button.
        enable: function() {
            this.$select.prop('disabled', false);
            this.$button.prop('disabled', false)
                .removeClass('disabled');
        },

        // Disable button.
        disable: function() {
            this.$select.prop('disabled', true);
            this.$button.prop('disabled', true)
                .addClass('disabled');
        },

        // Set options.
        setOptions: function(options) {
            this.options = this.mergeOptions(options);
        },

        // Get options by merging defaults and given options.
        mergeOptions: function(options) {
            return $.extend({}, this.defaults, options);
        },

        // Update button text and button title.
        updateButtonText: function() {
            var options = this.getSelected();

            // First update the displayed button text.
            $('button', this.$container).html(this.options.buttonText(options, this.$select));

            // Now update the title attribute of the button.
            $('button', this.$container).attr('title', this.options.buttonTitle(options, this.$select));

        },

        // Get all selected options.
        getSelected: function() {
            return $('option[value!="' + this.options.selectAllValue + '"]:selected', this.$select).filter(function() {
                return $(this).prop('selected');
            });
        },

        // Get the corresponding option by ts value.
        getOptionByValue: function(value) {
            return $('option', this.$select).filter(function() {
                return $(this).val() === value;
            });
        },

        // Get an input in the dropdown by its value.
        getInputByValue: function(value) {
            return $('li input', this.$ul).filter(function() {
                return $(this).val() === value;
            });
        },

        updateOriginalOptions: function() {
            this.originalOptions = this.$select.clone()[0].options;
        },

        asyncFunction: function(callback, timeout, self) {
            var args = Array.prototype.slice.call(arguments, 3);
            return setTimeout(function() {
                callback.apply(self || window, args);
            }, timeout);
        }
    };

    $.fn.multiselect = function(option, parameter) {
        return this.each(function() {
            var data = $(this).data('multiselect');
            var options = typeof option === 'object' && option;

            // Initialize the multiselect.
            if (!data) {
                $(this).data('multiselect', ( data = new Multiselect(this, options)));
            }

            // Call multiselect method.
            if (typeof option === 'string') {
                data[option](parameter);
            }
        });
    };

    $.fn.multiselect.Constructor = Multiselect;

    // Automatically init selects by their data-role.
    $(function() {
        $("select[data-role=multiselect]").multiselect();
    });

}(window.jQuery);


/*
* MultiSelect v0.9.12
* Copyright (c) 2012 Louis Cuny
*
* This program is free software. It comes without any warranty, to
* the extent permitted by applicable law. You can redistribute it
* and/or modify it under the terms of the Do What The Fuck You Want
* To Public License, Version 2, as published by Sam Hocevar. See
* http://sam.zoy.org/wtfpl/COPYING for more details.
*/

!function ($) {

    "use strict";


    /* MULTISELECT CLASS DEFINITION
     * ====================== */

    var MultiSelect = function (element, options) {
        this.options = options;
        this.$element = $(element);
        this.$container = $('<div/>', { 'class': "ms-container" });
        this.$selectableContainer = $('<div/>', { 'class': 'ms-selectable' });
        this.$selectionContainer = $('<div/>', { 'class': 'ms-selection' });
        this.$selectableUl = $('<ul/>', { 'class': "ms-list", 'tabindex': '-1' });
        this.$selectionUl = $('<ul/>', { 'class': "ms-list", 'tabindex': '-1' });
        this.scrollTo = 0;
        this.elemsSelector = 'li:visible:not(.ms-optgroup-label,.ms-optgroup-container,.' + options.disabledClass + ')';
    };

    MultiSelect.prototype = {
        constructor: MultiSelect,

        init: function () {
            var that = this,
                ms = this.$element;

            if (ms.next('.ms-container').length === 0) {
                ms.css({ position: 'absolute', left: '-9999px' });
                ms.attr('id', ms.attr('id') ? ms.attr('id') : Math.ceil(Math.random() * 1000) + 'multiselect');
                this.$container.attr('id', 'ms-' + ms.attr('id'));
                this.$container.addClass(that.options.cssClass);
                ms.find('option').each(function () {
                    that.generateLisFromOption(this);
                });

                this.$selectionUl.find('.ms-optgroup-label').hide();

                if (that.options.selectableHeader) {
                    that.$selectableContainer.append(that.options.selectableHeader);
                }
                that.$selectableContainer.append(that.$selectableUl);
                if (that.options.selectableFooter) {
                    that.$selectableContainer.append(that.options.selectableFooter);
                }

                if (that.options.selectionHeader) {
                    that.$selectionContainer.append(that.options.selectionHeader);
                }
                that.$selectionContainer.append(that.$selectionUl);
                if (that.options.selectionFooter) {
                    that.$selectionContainer.append(that.options.selectionFooter);
                }

                that.$container.append(that.$selectableContainer);
                that.$container.append(that.$selectionContainer);
                ms.after(that.$container);

                that.activeMouse(that.$selectableUl);
                that.activeKeyboard(that.$selectableUl);

                var action = that.options.dblClick ? 'dblclick' : 'click';

                that.$selectableUl.on(action, '.ms-elem-selectable', function () {
                    that.select($(this).data('ms-value'));
                });
                that.$selectionUl.on(action, '.ms-elem-selection', function () {
                    that.deselect($(this).data('ms-value'));
                });

                that.activeMouse(that.$selectionUl);
                that.activeKeyboard(that.$selectionUl);

                ms.on('focus', function () {
                    that.$selectableUl.focus();
                })
            }

            var selectedValues = ms.find('option:selected').map(function () { return $(this).val(); }).get();
            that.select(selectedValues, 'init');

            if (typeof that.options.afterInit === 'function') {
                that.options.afterInit.call(this, this.$container);
            }
        },

        'generateLisFromOption': function (option, index, $container) {
            var that = this,
                ms = that.$element,
                attributes = "",
                $option = $(option);

            for (var cpt = 0; cpt < option.attributes.length; cpt++) {
                var attr = option.attributes[cpt];

                if (attr.name !== 'value' && attr.name !== 'disabled') {
                    attributes += attr.name + '="' + attr.value + '" ';
                }
            }
            var selectableLi = $('<li ' + attributes + '><span>' + that.escapeHTML($option.text()) + '</span></li>'),
                selectedLi = selectableLi.clone(),
                value = $option.val(),
                elementId = that.sanitize(value);

            selectableLi
              .data('ms-value', value)
              .addClass('ms-elem-selectable')
              .attr('id', elementId + '-selectable');

            selectedLi
              .data('ms-value', value)
              .addClass('ms-elem-selection')
              .attr('id', elementId + '-selection')
              .hide();

            if ($option.prop('disabled') || ms.prop('disabled')) {
                selectedLi.addClass(that.options.disabledClass);
                selectableLi.addClass(that.options.disabledClass);
            }

            var $optgroup = $option.parent('optgroup');

            if ($optgroup.length > 0) {
                var optgroupLabel = $optgroup.attr('label'),
                    optgroupId = that.sanitize(optgroupLabel),
                    $selectableOptgroup = that.$selectableUl.find('#optgroup-selectable-' + optgroupId),
                    $selectionOptgroup = that.$selectionUl.find('#optgroup-selection-' + optgroupId);

                if ($selectableOptgroup.length === 0) {
                    var optgroupContainerTpl = '<li class="ms-optgroup-container"></li>',
                        optgroupTpl = '<ul class="ms-optgroup"><li class="ms-optgroup-label"><span>' + optgroupLabel + '</span></li></ul>';

                    $selectableOptgroup = $(optgroupContainerTpl);
                    $selectionOptgroup = $(optgroupContainerTpl);
                    $selectableOptgroup.attr('id', 'optgroup-selectable-' + optgroupId);
                    $selectionOptgroup.attr('id', 'optgroup-selection-' + optgroupId);
                    $selectableOptgroup.append($(optgroupTpl));
                    $selectionOptgroup.append($(optgroupTpl));
                    if (that.options.selectableOptgroup) {
                        $selectableOptgroup.find('.ms-optgroup-label').on('click', function () {
                            var values = $optgroup.children(':not(:selected, :disabled)').map(function () { return $(this).val() }).get();
                            that.select(values);
                        });
                        $selectionOptgroup.find('.ms-optgroup-label').on('click', function () {
                            var values = $optgroup.children(':selected:not(:disabled)').map(function () { return $(this).val() }).get();
                            that.deselect(values);
                        });
                    }
                    that.$selectableUl.append($selectableOptgroup);
                    that.$selectionUl.append($selectionOptgroup);
                }
                index = index == undefined ? $selectableOptgroup.find('ul').children().length : index + 1;
                selectableLi.insertAt(index, $selectableOptgroup.children());
                selectedLi.insertAt(index, $selectionOptgroup.children());
            } else {
                index = index == undefined ? that.$selectableUl.children().length : index;

                selectableLi.insertAt(index, that.$selectableUl);
                selectedLi.insertAt(index, that.$selectionUl);
            }
        },

        'addOption': function (options) {
            var that = this;

            if (options.value !== undefined && options.value !== null) {
                options = [options];
            }
            $.each(options, function (index, option) {
                if (option.value !== undefined && option.value !== null &&
                    that.$element.find("option[value='" + option.value + "']").length === 0) {
                    var $option = $('<option value="' + option.value + '">' + option.text + '</option>'),
                        index = parseInt((typeof option.index === 'undefined' ? that.$element.children().length : option.index)),
                        $container = option.nested == undefined ? that.$element : $("optgroup[label='" + option.nested + "']")

                    $option.insertAt(index, $container);
                    that.generateLisFromOption($option.get(0), index, option.nested);
                }
            })
        },

        'escapeHTML': function (text) {
            return $("<div>").text(text).html();
        },

        'activeKeyboard': function ($list) {
            var that = this;

            $list.on('focus', function () {
                $(this).addClass('ms-focus');
            })
            .on('blur', function () {
                $(this).removeClass('ms-focus');
            })
            .on('keydown', function (e) {
                switch (e.which) {
                    case 40:
                    case 38:
                        e.preventDefault();
                        e.stopPropagation();
                        that.moveHighlight($(this), (e.which === 38) ? -1 : 1);
                        return;
                    case 37:
                    case 39:
                        e.preventDefault();
                        e.stopPropagation();
                        that.switchList($list);
                        return;
                    case 9:
                        if (that.$element.is('[tabindex]')) {
                            e.preventDefault();
                            var tabindex = parseInt(that.$element.attr('tabindex'), 10);
                            tabindex = (e.shiftKey) ? tabindex - 1 : tabindex + 1;
                            $('[tabindex="' + (tabindex) + '"]').focus();
                            return;
                        } else {
                            if (e.shiftKey) {
                                that.$element.trigger('focus');
                            }
                        }
                }
                if ($.inArray(e.which, that.options.keySelect) > -1) {
                    e.preventDefault();
                    e.stopPropagation();
                    that.selectHighlighted($list);
                    return;
                }
            });
        },

        'moveHighlight': function ($list, direction) {
            var $elems = $list.find(this.elemsSelector),
                $currElem = $elems.filter('.ms-hover'),
                $nextElem = null,
                elemHeight = $elems.first().outerHeight(),
                containerHeight = $list.height(),
                containerSelector = '#' + this.$container.prop('id');

            $elems.removeClass('ms-hover');
            if (direction === 1) { // DOWN

                $nextElem = $currElem.nextAll(this.elemsSelector).first();
                if ($nextElem.length === 0) {
                    var $optgroupUl = $currElem.parent();

                    if ($optgroupUl.hasClass('ms-optgroup')) {
                        var $optgroupLi = $optgroupUl.parent(),
                            $nextOptgroupLi = $optgroupLi.next(':visible');

                        if ($nextOptgroupLi.length > 0) {
                            $nextElem = $nextOptgroupLi.find(this.elemsSelector).first();
                        } else {
                            $nextElem = $elems.first();
                        }
                    } else {
                        $nextElem = $elems.first();
                    }
                }
            } else if (direction === -1) { // UP

                $nextElem = $currElem.prevAll(this.elemsSelector).first();
                if ($nextElem.length === 0) {
                    var $optgroupUl = $currElem.parent();

                    if ($optgroupUl.hasClass('ms-optgroup')) {
                        var $optgroupLi = $optgroupUl.parent(),
                            $prevOptgroupLi = $optgroupLi.prev(':visible');

                        if ($prevOptgroupLi.length > 0) {
                            $nextElem = $prevOptgroupLi.find(this.elemsSelector).last();
                        } else {
                            $nextElem = $elems.last();
                        }
                    } else {
                        $nextElem = $elems.last();
                    }
                }
            }
            if ($nextElem.length > 0) {
                $nextElem.addClass('ms-hover');
                var scrollTo = $list.scrollTop() + $nextElem.position().top -
                               containerHeight / 2 + elemHeight / 2;

                $list.scrollTop(scrollTo);
            }
        },

        'selectHighlighted': function ($list) {
            var $elems = $list.find(this.elemsSelector),
                $highlightedElem = $elems.filter('.ms-hover').first();

            if ($highlightedElem.length > 0) {
                if ($list.parent().hasClass('ms-selectable')) {
                    this.select($highlightedElem.data('ms-value'));
                } else {
                    this.deselect($highlightedElem.data('ms-value'));
                }
                $elems.removeClass('ms-hover');
            }
        },

        'switchList': function ($list) {
            $list.blur();
            this.$container.find(this.elemsSelector).removeClass('ms-hover');
            if ($list.parent().hasClass('ms-selectable')) {
                this.$selectionUl.focus();
            } else {
                this.$selectableUl.focus();
            }
        },

        'activeMouse': function ($list) {
            var that = this;

            this.$container.on('mouseenter', that.elemsSelector, function () {
                $(this).parents('.ms-container').find(that.elemsSelector).removeClass('ms-hover');
                $(this).addClass('ms-hover');
            });

            this.$container.on('mouseleave', that.elemsSelector, function () {
                $(this).parents('.ms-container').find(that.elemsSelector).removeClass('ms-hover');;
            });
        },

        'refresh': function () {
            this.destroy();
            this.$element.multiSelect(this.options);
        },

        'destroy': function () {
            $("#ms-" + this.$element.attr("id")).remove();
            this.$element.css('position', '').css('left', '')
            this.$element.removeData('multiselect');
        },

        'select': function (value, method) {
            if (typeof value === 'string') { value = [value]; }

            var that = this,
                ms = this.$element,
                msIds = $.map(value, function (val) { return (that.sanitize(val)); }),
                selectables = this.$selectableUl.find('#' + msIds.join('-selectable, #') + '-selectable').filter(':not(.' + that.options.disabledClass + ')'),
                selections = this.$selectionUl.find('#' + msIds.join('-selection, #') + '-selection').filter(':not(.' + that.options.disabledClass + ')'),
                options = ms.find('option:not(:disabled)').filter(function () { return ($.inArray(this.value, value) > -1); });

            if (method === 'init') {
                selectables = this.$selectableUl.find('#' + msIds.join('-selectable, #') + '-selectable'),
                selections = this.$selectionUl.find('#' + msIds.join('-selection, #') + '-selection');
            }

            if (selectables.length > 0) {
                selectables.addClass('ms-selected').hide();
                selections.addClass('ms-selected').show();

                options.prop('selected', true);

                that.$container.find(that.elemsSelector).removeClass('ms-hover');

                var selectableOptgroups = that.$selectableUl.children('.ms-optgroup-container');
                if (selectableOptgroups.length > 0) {
                    selectableOptgroups.each(function () {
                        var selectablesLi = $(this).find('.ms-elem-selectable');
                        if (selectablesLi.length === selectablesLi.filter('.ms-selected').length) {
                            $(this).find('.ms-optgroup-label').hide();
                        }
                    });

                    var selectionOptgroups = that.$selectionUl.children('.ms-optgroup-container');
                    selectionOptgroups.each(function () {
                        var selectionsLi = $(this).find('.ms-elem-selection');
                        if (selectionsLi.filter('.ms-selected').length > 0) {
                            $(this).find('.ms-optgroup-label').show();
                        }
                    });
                } else {
                    if (that.options.keepOrder && method !== 'init') {
                        var selectionLiLast = that.$selectionUl.find('.ms-selected');
                        if ((selectionLiLast.length > 1) && (selectionLiLast.last().get(0) != selections.get(0))) {
                            selections.insertAfter(selectionLiLast.last());
                        }
                    }
                }
                if (method !== 'init') {
                    ms.trigger('change');
                    if (typeof that.options.afterSelect === 'function') {
                        that.options.afterSelect.call(this, value);
                    }
                }
            }
        },

        'deselect': function (value) {
            if (typeof value === 'string') { value = [value]; }

            var that = this,
                ms = this.$element,
                msIds = $.map(value, function (val) { return (that.sanitize(val)); }),
                selectables = this.$selectableUl.find('#' + msIds.join('-selectable, #') + '-selectable'),
                selections = this.$selectionUl.find('#' + msIds.join('-selection, #') + '-selection').filter('.ms-selected').filter(':not(.' + that.options.disabledClass + ')'),
                options = ms.find('option').filter(function () { return ($.inArray(this.value, value) > -1); });

            if (selections.length > 0) {
                selectables.removeClass('ms-selected').show();
                selections.removeClass('ms-selected').hide();
                options.prop('selected', false);

                that.$container.find(that.elemsSelector).removeClass('ms-hover');

                var selectableOptgroups = that.$selectableUl.children('.ms-optgroup-container');
                if (selectableOptgroups.length > 0) {
                    selectableOptgroups.each(function () {
                        var selectablesLi = $(this).find('.ms-elem-selectable');
                        if (selectablesLi.filter(':not(.ms-selected)').length > 0) {
                            $(this).find('.ms-optgroup-label').show();
                        }
                    });

                    var selectionOptgroups = that.$selectionUl.children('.ms-optgroup-container');
                    selectionOptgroups.each(function () {
                        var selectionsLi = $(this).find('.ms-elem-selection');
                        if (selectionsLi.filter('.ms-selected').length === 0) {
                            $(this).find('.ms-optgroup-label').hide();
                        }
                    });
                }
                ms.trigger('change');
                if (typeof that.options.afterDeselect === 'function') {
                    that.options.afterDeselect.call(this, value);
                }
            }
        },

        'select_all': function () {
            var ms = this.$element,
                values = ms.val();

            ms.find('option:not(":disabled")').prop('selected', true);
            this.$selectableUl.find('.ms-elem-selectable').filter(':not(.' + this.options.disabledClass + ')').addClass('ms-selected').hide();
            this.$selectionUl.find('.ms-optgroup-label').show();
            this.$selectableUl.find('.ms-optgroup-label').hide();
            this.$selectionUl.find('.ms-elem-selection').filter(':not(.' + this.options.disabledClass + ')').addClass('ms-selected').show();
            this.$selectionUl.focus();
            ms.trigger('change');
            if (typeof this.options.afterSelect === 'function') {
                var selectedValues = $.grep(ms.val(), function (item) {
                    return $.inArray(item, values) < 0;
                });
                this.options.afterSelect.call(this, selectedValues);
            }
        },

        'deselect_all': function () {
            var ms = this.$element,
                values = ms.val();

            ms.find('option').prop('selected', false);
            this.$selectableUl.find('.ms-elem-selectable').removeClass('ms-selected').show();
            this.$selectionUl.find('.ms-optgroup-label').hide();
            this.$selectableUl.find('.ms-optgroup-label').show();
            this.$selectionUl.find('.ms-elem-selection').removeClass('ms-selected').hide();
            this.$selectableUl.focus();
            ms.trigger('change');
            if (typeof this.options.afterDeselect === 'function') {
                this.options.afterDeselect.call(this, values);
            }
        },

        sanitize: function (value) {
            var hash = 0, i, character;
            if (value.length == 0) return hash;
            var ls = 0;
            for (i = 0, ls = value.length; i < ls; i++) {
                character = value.charCodeAt(i);
                hash = ((hash << 5) - hash) + character;
                hash |= 0; // Convert to 32bit integer
            }
            return hash;
        }
    };

    /* MULTISELECT PLUGIN DEFINITION
     * ======================= */

    $.fn.multiSelect = function () {
        var option = arguments[0],
            args = arguments;

        return this.each(function () {
            var $this = $(this),
                data = $this.data('multiselect'),
                options = $.extend({}, $.fn.multiSelect.defaults, $this.data(), typeof option === 'object' && option);

            if (!data) { $this.data('multiselect', (data = new MultiSelect(this, options))); }

            if (typeof option === 'string') {
                data[option](args[1]);
            } else {
                data.init();
            }
        });
    };

    $.fn.multiSelect.defaults = {
        keySelect: [32],
        selectableOptgroup: false,
        disabledClass: 'disabled',
        dblClick: false,
        keepOrder: false,
        cssClass: ''
    };

    $.fn.multiSelect.Constructor = MultiSelect;

    $.fn.insertAt = function (index, $parent) {
        return this.each(function () {
            if (index === 0) {
                $parent.prepend(this);
            } else {
                $parent.children().eq(index - 1).after(this);
            }
        });
    }

}(window.jQuery);