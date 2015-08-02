define(["require", "exports", 'durandal/app', 'durandal/system', 'knockout', 'jquery', "../models/models"], function (require, exports, app, system, ko, $, models) {
    var GnomeSummary = models.GnomeSummary;
    var Index = (function () {
        function Index() {
            var _this = this;
            this.endpointRoot = "http://localhost:8081/";
            this.name = "Gnomes";
            this.gameIsPaused = ko.observable(true);
            this.gnomeSummary = ko.observable();
            this.selectedGnomeID = ko.observable(0);
            this.selectedGnome = ko.pureComputed({
                owner: this,
                read: function () {
                    var matches = _this.gnomeSummary().Gnomes().filter(function (value) {
                        var ID1 = value.ID();
                        var ID2 = _this.selectedGnomeID && _this.selectedGnomeID() || -1;
                        return ID1 === ID2;
                    });
                    if (matches.length > 0) {
                        var match = matches[0];
                        return match;
                    }
                    else {
                        return null;
                    }
                },
                deferEvaluation: true
            });
            this.addGnome = function () {
                var endpoint = _this.endpointRoot + "Gnome/Add";
                $.get(endpoint).always(function () {
                    _this.loadData();
                });
            };
            this.reassignProfessions = function () {
                var endpoint = _this.endpointRoot + "Gnome/Assign";
                $.get(endpoint).always(function () {
                    _this.loadData();
                });
            };
            this.selectGnome = function (gnome) {
                _this.selectedGnomeID(gnome.ID());
            };
            this.refresh = function () {
                _this.loadData();
            };
            this.activate = function () {
                system.log('Lifecycle: activate : gnome/index');
                app.on("gamePaused").then(function () {
                    _this.gameIsPaused(true);
                });
                app.on("gameUnpaused").then(function () {
                    _this.gameIsPaused(false);
                });
                return _this.loadData();
            };
            this.binding = function () {
                system.log('Lifecycle: binding : gnome/index');
                return { cacheViews: false };
            };
            this.bindingComplete = function () {
                system.log('Lifecycle : bindingComplete : gnome/index');
            };
            this.attached = function () {
                system.log('Lifecycle : attached : gnome/index');
            };
            this.compositionComplete = function () {
                system.log('Lifecycle : compositionComplete : gnome/index');
            };
            this.detached = function () {
                system.log('Lifecycle : detached : gnome/index');
            };
        }
        Index.prototype.loadData = function () {
            var _this = this;
            var endpoint = this.endpointRoot + "Gnome/";
            var promise = $.getJSON(endpoint).then(function (data) {
                var obj = new GnomeSummary(data);
                _this.gnomeSummary(obj);
                _this.selectedGnomeID(-1);
                var gnomes = _this.gnomeSummary().Gnomes();
                if (gnomes) {
                    var gnome = gnomes[0];
                    var gnomeID = gnome.ID();
                    _this.selectedGnomeID(gnomeID);
                }
                console.log("promise complete");
            });
            // If the callback returns a "promise"...
            // Durandal will wait for it to complete before continuing.
            return promise;
        };
        return Index;
    })();
    var instance = new Index();
    return instance;
});
//# sourceMappingURL=index.js.map