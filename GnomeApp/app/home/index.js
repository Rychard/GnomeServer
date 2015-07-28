define(["require", "exports", 'durandal/system'], function (require, exports, system) {
    var Index = (function () {
        function Index() {
            this.activate = function () {
                system.log('Lifecycle: activate : home');
            };
            this.binding = function () {
                system.log('Lifecycle: binding : home');
                return { cacheViews: false };
            };
            this.bindingComplete = function () {
                system.log('Lifecycle : bindingComplete : home');
            };
            this.attached = function () {
                system.log('Lifecycle : attached : home');
            };
            this.compositionComplete = function () {
                system.log('Lifecycle : compositionComplete : home');
            };
            this.detached = function () {
                system.log('Lifecycle : detached : home');
            };
        }
        return Index;
    })();
    var instance = new Index();
    return instance;
});
//# sourceMappingURL=index.js.map