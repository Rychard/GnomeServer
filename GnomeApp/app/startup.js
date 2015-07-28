define(["require", "exports", 'durandal/app', 'durandal/viewLocator'], function (require, exports, app, viewLocator) {
    function start() {
        //system.debug(true);
        app.title = 'Ensemble';
        app.configurePlugins({
            router: true,
            dialog: true,
            widget: {
                kinds: ['dump']
            }
        });
        app.start().then(function () {
            viewLocator.useConvention();
            app.setRoot('shell');
        });
    }
    exports.start = start;
});
//# sourceMappingURL=startup.js.map