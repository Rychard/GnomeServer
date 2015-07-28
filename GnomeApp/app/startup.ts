import system = require('durandal/system');
import app = require('durandal/app');
import viewLocator = require('durandal/viewLocator');

export function start() {
    //system.debug(true);

    app.title = 'Ensemble';

    app.configurePlugins({
        router: true,
        dialog: true,
        widget: {
            kinds: ['dump']
        }
    });

    app.start().then(() => {
        viewLocator.useConvention();
        app.setRoot('shell');
    });
}