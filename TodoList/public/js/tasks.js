$(function () {

    //View Models

    //Task View Model
    function taskViewModel(id, title, description, status, createdBy, assignedTo, ownerViewModel) {
        this.taskId = id;
        this.title = ko.observable(title);
        this.description = ko.observable(description);
        this.status = ko.observable(status);
        this.createdBy = ko.observable(createdBy);
        this.assignedTo = ko.observable(assignedTo);
        this.remove = function () { ownerViewModel.removeTask(this.taskId) }
        this.notification = function (b) { notify = b }

        var self = this;

        this.title.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

        this.description.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

        this.status.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

        this.createdBy.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

        this.assignedTo.subscribe(function (newValue) {
            ownerViewModel.updateTask(ko.toJS(self));
        });

    }

    //Task List View Model
    function taskListViewModel() {

        //Handlers for our Hub callbacks

        this.hub = $.connection.tasks;
        this.tasks = ko.observableArray([]);
        this.newTaskText = ko.observable();
        this.description = ko.observable();
        this.status = ko.observable();
        this.createdBy = ko.observable();
        this.assignedTo = ko.observable();
        var tasks = this.tasks;
        var self = this;
        var notify = true;

        //Initializes the view model
        this.init = function () {
            this.hub.server.getAll();
        }

        //Handlers for our Hub callbacks

        this.hub.client.taskAll = function (allTasks) {

            var mappedTasks = $.map(allTasks, function (item) {
                return new taskViewModel(item.taskId, item.title, item.description, item.status, item.createdBy, item.assignedTo, self);
            });

            tasks(mappedTasks);
        }

        this.hub.client.taskUpdated = function (t) {
            var task = ko.utils.arrayFilter(tasks(), function (value) { return value.taskId == t.taskId; })[0];
            notify = false;
            task.title(t.title);
            task.description(t.description);
            task.status(t.status);
            task.createdBy(t.createdBy);
            task.assignedTo(t.assignedTo);
            notify = true;
        };

        this.hub.client.reportError = function (error) {
            $("#error").text(error);
            $("#error").fadeIn(1000, function () {
                $("#error").fadeOut(3000);
            });
        }

        this.hub.client.taskAdded = function (t) {
            tasks.push(new taskViewModel(t.taskId, t.title, t.description, t.status, t.createdBy, t.assignedTo, self));
        };

        this.hub.client.taskRemoved = function (id) {
            var task = ko.utils.arrayFilter(tasks(), function (value) { return value.taskId == id; })[0];
            tasks.remove(task);
        };

        //View Model 'Commands'

        //To create a task
        this.addTask = function () {
            var t = { "title": this.newTaskText(), "description": this.description(), "status": this.status(), "createdBy": this.createdBy(), "assignedTo": this.assignedTo() };
            this.hub.server.add(t).done(function () {

            }).fail(function (e) {
                console.warn(e);
            });

            this.newTaskText("");
        }

        //To remove a task
        this.removeTask = function (id) {
            this.hub.server.remove(id);
        }

        //To update this task
        this.updateTask = function (task) {
            if (notify)
                this.hub.server.update(task);
        }

        //Gets the incomplete tasks
        this.incompleteTasks = ko.dependentObservable(function () {
            return ko.utils.arrayFilter(this.tasks(), function (task) {
                return task.status() != 3;
            });
        }, this);

    }

    var vm = new taskListViewModel();
    ko.applyBindings(vm);
    // Start the connection
    $.connection.hub.start(function () { vm.init(); });

});