//Varun Dewan 2019
var $ = {
    get: function(selector) {
        var ele = document.querySelectorAll(selector);
        for (var i = 0; i < ele.length; i++) {
            this.init(ele[i]);
        }
        return ele;
    },
    template: function(html) {
        var template = document.createElement('div');
        template.innerHTML = html.trim();
        return this.init(template.childNodes[0]);
    },
    init: function(ele) {
        ele.on = function(event, func) { this.addEventListener(event, func); }
        return ele;
    }
};

//Build the plugin
var drop = function(info) {
    var o = {
        isEnglish: info.isEnglish,
        options: info.options,
        selected: info.selected || [],
        preselected: info.preselected || [],
        open: false,
        html: {
            select: $.get(info.selector)[0],
            options: $.get(info.selector + ' option'),
            parent: undefined,
        },
        init: function() {
            //Setup Drop HTML
            this.html.parent = $.get(info.selector)[0].parentNode
            this.html.drop = $.template('<div class="drop"></div>')
            this.html.dropDisplay = $.template('<div class="drop-display">Display</div>')
            this.html.dropOptions = $.template('<div class="drop-options">Options</div>')
            this.html.dropScreen = $.template('<div class="drop-screen"></div>')

            this.html.parent.insertBefore(this.html.drop, this.html.select)
            this.html.drop.appendChild(this.html.dropDisplay)
            this.html.drop.appendChild(this.html.dropOptions)
            this.html.drop.appendChild(this.html.dropScreen)
                //Hide old select
            this.html.drop.appendChild(this.html.select);

            //Core Events
            var that = this;
            this.html.dropDisplay.on('click', function() { that.toggle() });
            this.html.dropScreen.on('click', function() { that.toggle() });
            //Run Render
            this.load()
            this.preselect()
            this.render();
        },
        toggle: function() {
            this.html.drop.classList.toggle('open');
        },
        addOption: function(e, element) {
            var index = Number(element.dataset.index);
            this.clearStates()
            this.selected.push({
                index: Number(index),
                state: 'add',
                removed: false
            })
            this.options[index].state = 'remove';
            this.render()
        },
        removeOption: function(e, element) {
            e.stopPropagation();
            this.clearStates()
            var index = Number(element.dataset.index);
            this.selected.forEach(function (select) {
                if (select.index == index && !select.removed) {
                    select.removed = true
                    select.state = 'remove'
                }
            })
            this.options[index].state = 'add'
            this.render();
        },
        load: function() {
            this.options = [];
            for (var i = 0; i < this.html.options.length; i++) {
                var option = this.html.options[i]
                this.options[i] = {
                    html: option.innerHTML,
                    value: option.value,
                    selected: option.selected,
                    state: ''
                }
            }
        },
        preselect: function() {
            var that = this;
            this.selected = [];
            this.preselected.forEach(function(pre) {
                that.selected.push({
                    index: pre,
                    state: 'add',
                    removed: false
                })
                that.options[pre].state = 'remove';
            })
        },
        render: function() {
            this.renderDrop()
            this.renderOptions()
        },
        renderDrop: function() {
            var that = this;
            var parentHTML = $.template('<div class="tagsHolder"></div>')
            parentHTML.appendChild($.template('<svg xmlns="http://www.w3.org/2000/svg" style="fill:#757575;" height="24" viewBox="0 0 24 24" width="24"><path d="M7 10l5 5 5-5z"/><path d="M0 0h24v24H0z" fill="none"/></svg>'))
            if (info.isEnglish) {
                this.placeHolder = $.template('<p class="placeHolderMessage">Select tags for your design</p>')
            }
            else {
                this.placeHolder = $.template('<p class="placeHolderMessage">تگ های طرح خود را انتخاب کنید</p>')
            }
            
            parentHTML.appendChild(this.placeHolder)
            this.selected.forEach(function(select, index) {
                var option = that.options[select.index];
                var childHTML = $.template('<span class="item ' + select.state + '">' + option.html + '</span>')
                var childCloseHTML = $.template(
                    '<i class="material-icons btnclose" data-index="' + select.index + '">&#xe5c9;</i></span>')
                childCloseHTML.on('click', function (e) { that.removeOption(e, this) })
                that.placeHolder.innerHTML = '';
                childHTML.appendChild(childCloseHTML)
                parentHTML.appendChild(childHTML)
            })
            var check = true;
            for (var i = 0; i < that.options.length; i++) {
                if (that.isSelected(i)) {
                    check = false;
                }
            }
            if (check) {
                if (info.isEnglish) {
                    that.placeHolder.innerHTML = 'Select tags for your design';                }
                else {
                    that.placeHolder.innerHTML = 'تگ های طرح خود را انتخاب کنید';                }
            }
            this.html.dropDisplay.innerHTML = '';
            this.html.dropDisplay.appendChild(parentHTML)
        },
        renderOptions: function() {
            var that = this;
            var parentHTML = $.template('<div></div>')
            this.options.forEach(function(option, index) {
                var childHTML = $.template(
                    '<a data-index="' + index + '" class="' + option.state + '">' + option.html + '</a>')
                childHTML.on('click', function(e) { that.addOption(e, this) })
                parentHTML.appendChild(childHTML)
            })
            this.html.dropOptions.innerHTML = '';
            this.html.dropOptions.appendChild(parentHTML)
        },
        clearStates: function() {
            var that = this;
            this.selected.forEach(function(select, index) {
                select.state = that.changeState(select.state)
            })
            this.options.forEach(function(option) {
                option.state = that.changeState(option.state)
            })
        },
        changeState: function(state) {
            switch (state) {
                case 'remove':
                    return 'hide'
                case 'hide':
                    return 'hide'
                default:
                    return ''
            }
        },
        isSelected: function(index) {
            var check = false
            this.selected.forEach(function(select) {
                if (select.index == index && select.removed == false) check = true
            })
            return check
        }
    };
    o.init();
    return o;
}