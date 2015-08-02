import system = require('durandal/system');

class Index {
    activate = () => {
        system.log('Lifecycle: activate : home');
    };

    binding = () => {
        system.log('Lifecycle: binding : home');
        return { cacheViews: false };
    }

    bindingComplete = () => {
        system.log('Lifecycle : bindingComplete : home');
    };

    attached = () => {
        system.log('Lifecycle : attached : home');
    };

    compositionComplete = () => {
        system.log('Lifecycle : compositionComplete : home');
    };

    detached = () => {
        system.log('Lifecycle : detached : home');
    };
}

var instance: ISingleton = new Index();
export = instance;