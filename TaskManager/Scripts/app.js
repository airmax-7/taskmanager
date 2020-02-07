function ViewModel() {
    var self = this;

    var tokenKey = 'accessToken';
    var apiBaseUrl = 'http://localhost/taskmanagerapi';
    var tokenUrl = apiBaseUrl + '/Token';
    var registerUrl = apiBaseUrl + '/api/Account/Register';
    var logoutUrl = apiBaseUrl + '/api/Account/Logout';
    var boardsUrl = apiBaseUrl + '/api/boards';
    var tasksUrl = apiBaseUrl + '/api/tasks';

    self.error = ko.observable();

    function ajaxHelper(uri, method, data) {
        self.error(''); // Clear error message

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        return $.ajax({
            type: method,
            url: uri,
            dataType: 'json',
            contentType: 'application/json',
            headers: headers,
            data: data ? JSON.stringify(data) : null
        }).fail(function (jqXHR, textStatus, errorThrown) {
            self.error(errorThrown);
        });
    }

    self.view = ko.observable('LOGIN');

    self.result = ko.observable();
    self.user = ko.observable();

    self.registerEmail = ko.observable();
    self.registerPassword = ko.observable();
    self.registerPassword2 = ko.observable();

    self.loginEmail = ko.observable();
    self.loginPassword = ko.observable();
    self.errors = ko.observableArray([]);

    self.boards = ko.observableArray([]);
    self.selectedBoard = ko.observable();
    self.newBoard = {
        Name: ko.observable()
    };

    self.todoTasks = ko.observableArray([]);
    self.doingTasks = ko.observableArray([]);
    self.doneTasks = ko.observableArray([]);
    self.selectedTask = ko.observable();
    self.newTask = {
        Title: ko.observable()
    };
    self.isAddingTask = ko.observable(false);
    self.isAddingBoard = ko.observable(false);

    function showError(jqXHR) {

        self.result(jqXHR.status + ': ' + jqXHR.statusText);

        var response = jqXHR.responseJSON;
        if (response) {
            if (response.Message) self.errors.push(response.Message);
            if (response.ModelState) {
                var modelState = response.ModelState;
                for (var prop in modelState) {
                    if (modelState.hasOwnProperty(prop)) {
                        var msgArr = modelState[prop]; // expect array here
                        if (msgArr.length) {
                            for (var i = 0; i < msgArr.length; ++i) self.errors.push(msgArr[i]);
                        }
                    }
                }
            }
            if (response.error) self.errors.push(response.error);
            if (response.error_description) self.errors.push(response.error_description);
        }
    }

    self.callApi = function () {
        self.result('');
        self.errors.removeAll();

        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'GET',
            url: apiBaseUrl + '/api/values',
            headers: headers
        }).done(function (data) {
            self.result(data);
        }).fail(showError);
    }

    self.register = function () {
        self.result('');
        self.errors.removeAll();

        var data = {
            Email: self.registerEmail(),
            Password: self.registerPassword(),
            ConfirmPassword: self.registerPassword2()
        };

        $.ajax({
            type: 'POST',
            url: registerUrl,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data)
        }).done(function (data) {
            self.result("Done!");
        }).fail(showError);
    }

    self.login = function () {
        self.result('');
        self.errors.removeAll();

        var loginData = {
            grant_type: 'password',
            username: self.loginEmail(),
            password: self.loginPassword()
        };

        $.ajax({
            type: 'POST',
            url: tokenUrl,
            data: loginData
        }).done(function (data) {
            self.user(data.userName);
            // Cache the access token in session storage.
            sessionStorage.setItem(tokenKey, data.access_token);

            // go to boards list after logging in.
            self.navigateToBoards();
        }).fail(showError);
    }

    self.logout = function () {
        // Log out from the cookie based logon.
        var token = sessionStorage.getItem(tokenKey);
        var headers = {};
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        $.ajax({
            type: 'POST',
            url: logoutUrl,
            headers: headers
        }).done(function (data) {
            // Successfully logged out. Delete the token.
            self.user('');
            sessionStorage.removeItem(tokenKey);
        }).fail(showError);
    }

    self.getBoards = function () {
        self.boards('');
        self.errors.removeAll();

        ajaxHelper(boardsUrl, 'GET').done(function (data) {
            self.boards(data);
        });
    }

    self.navigateToBoards = function () {
        var boards = self.getBoards();
        self.selectedBoard(null);
        self.view('BOARDS');
    }

    self.navigateToBoard = function (board) {
        console.log('navigate to board: ' + board.id);
        self.selectedBoard(board);
        var todo = ko.utils.arrayFilter(self.selectedBoard().Tasks, function (item) {
            return item.Status == 0;
        });
        self.todoTasks(todo);

        var doing = ko.utils.arrayFilter(self.selectedBoard().Tasks, function (item) {
            return item.Status == 1;
        });
        self.doingTasks(doing);

        var done = ko.utils.arrayFilter(self.selectedBoard().Tasks, function (item) {
            return item.Status == 2;
        });
        self.doneTasks(done);
    }

    self.navigateToAddBoard = function () {
        self.isAddingBoard(true);
    }

    self.cancelAddBoard = function () {
        self.isAddingBoard(false);
    }

    self.addBoard = function () {
        console.log('add board...');
        var board = {
            Name: self.newBoard.Name()
        };
        ajaxHelper(boardsUrl, 'POST', board).done(function (item) {
            self.boards.push(item);
            self.selectedBoard(item);
        });
        self.isAddingBoard(false);
    }

    self.addTask = function (status) {
        var task = {
            Title: self.newTask.Title(),
            Status: status,
            BoardId: self.selectedBoard().Id
        };
        ajaxHelper(tasksUrl, 'POST', task).done(function (item) {
            if (status == 0)
                self.todoTasks.push(item);
            else if (status == 1)
                self.doingTasks.push(item);
            else if (status == 2)
                self.doneTasks.push(item);
        });
        self.isAddingTask(false);
    }

    self.navigateToLogin = function () {
        self.view('LOGIN');
    }

    self.navigateToAddTask = function () {
        self.isAddingTask(true);
    }

    self.cancelAddTask = function () {
        self.isAddingTask(false);
    }

    self.navigateToTask = function (task) {
        
        self.selectedTask(task);

        self.view('TASK');
    }

    self.initialize = function () {
        var token = sessionStorage.getItem(tokenKey);
        if (token) {
            self.navigateToBoards();
        }
        else {
            self.navigateToLogin();
        }
    }

    // initialize page
    self.initialize();
}

var app = new ViewModel();
ko.applyBindings(app);