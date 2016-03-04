$(function () {

    //View Models

    //User View Model
    function userViewModel(id, firstName, lastName, active, ownerViewModel) {
        this.userId = id;
        this.firstName = ko.observable(firstName);
        this.lastName = ko.observable(lastName);
        this.active = ko.observable(active);
        this.remove = function () { ownerViewModel.removeUser(this.userId) }
        this.notification = function (b) { notify = b }

        var self = this;

        this.firstName.subscribe(function (newValue) {
            ownerViewModel.updateUser(ko.toJS(self));
        });

        this.lastName.subscribe(function (newValue) {
            ownerViewModel.updateUser(ko.toJS(self));
        });

        this.active.subscribe(function (newValue) {
            ownerViewModel.updateUser(ko.toJS(self));
        });
    }

    //User List View Model
    function userListViewModel() {

        //Handlers for our Hub callbacks
        this.hub = $.connection.users;
        this.users = ko.observableArray([]);
        this.firstName = ko.observable();
        this.lastName = ko.observable();

        var users = this.users;
        var self = this;
        var notify = true;

        //Initializes the view model
        this.init = function () {
            this.hub.server.getAll();
        }

        //Handlers for our Hub callbacks
        this.hub.client.userAll = function (allUsers) {

            var mappedUsers = $.map(allUsers, function (item) {
                return new userViewModel(item.userId, item.firstName, item.lastName, item.active, self);
            });

            users(mappedUsers);
        }

        this.hub.client.userUpdated = function (t) {
            var user = ko.utils.arrayFilter(users(), function (value) { return value.userId == t.userId; })[0];
            notify = false;
            user.firstName(t.firstName);
            user.lastName(t.lastName);
            user.active(t.active);
            notify = true;
        };

        this.hub.client.reportError = function (error) {
            $("#error").text(error);
            $("#error").fadeIn(1000, function () {
                $("#error").fadeOut(3000);
            });
        }

        this.hub.client.userAdded = function (t) {
            users.push(new userViewModel(t.userId, t.firstName, t.lastName, t.active, self));
        };

        this.hub.client.userRemoved = function (id) {
            var user = ko.utils.arrayFilter(users(), function (value) { return value.userId == id; })[0];
            users.remove(user);
        };

        //View Model 'Commands'

        //To create a user
        this.addUser = function () {
            var t = { "firstName": this.firstName(), "lastName": this.lastName(), "active": false };
            this.hub.server.add(t).done(function () {

            }).fail(function (e) {
                console.warn(e);
            });

            this.firstName("");
            this.lastName("");
        }

        //To remove a user
        this.removeUser = function (id) {
            this.hub.server.remove(id);
        }

        //To update this user
        this.updateUser = function (user) {
            if (notify)
                this.hub.server.update(user);
        }

        //Gets the inactive users
        this.activeUsers = ko.dependentObservable(function () {
            return ko.utils.arrayFilter(this.users(), function (user) { return user.active() });
        }, this);

    }

    var vm = new userListViewModel();
    ko.applyBindings(vm);
    // Start the connection
    $.connection.hub.start(function () { vm.init(); });
});