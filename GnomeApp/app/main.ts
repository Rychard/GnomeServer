requirejs.config({
    paths: {
        'text': '../Scripts/text',
        'durandal': '../Scripts/durandal',
        'plugins': '../Scripts/durandal/plugins',
        'transitions': '../Scripts/durandal/transitions',
        'knockout': '../Scripts/knockout-3.3.0.debug',
        'bootstrap': '../Scripts/bootstrap',
        'jquery': '../Scripts/jquery-2.1.4'
    },
    shim: {
        'bootstrap': {
            deps: ['jquery'],
            exports: 'jQuery'
        }
    }
});

define(['./startup'], startup => {
    startup.start();
});