define(["require", "exports", 'durandal/system', 'jquery', "../models/models"], function (require, exports, system, $, models) {
    var GnomeSummary = models.GnomeSummary;
    var Index = (function () {
        function Index() {
            var _this = this;
            this.name = "Gnomes";
            this.activate = function () {
                system.log('Lifecycle: activate : gnome/index');
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
            var endpoint = "http://localhost:8081/Gnome/";
            var promise = $.getJSON(endpoint).then(function (data) {
                // Convert every item in the response to a StatusReport instance.
                _this.gnomeSummary = new GnomeSummary(data);
                //this.gnomes = data.map((item: Gnome) => { return new Gnome(item); });
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