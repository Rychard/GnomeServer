import app = require('durandal/app');
import system = require('durandal/system');
import ko = require('knockout');
import $ = require('jquery');

import models = require("../models/models");
import Utils = require("../utils");

class Index {
    name = "Test";
    gameIsPaused: KnockoutObservable<Boolean> = ko.observable<Boolean>(true);

    refresh = () => {
        this.loadData();
    }

    loadData(): JQueryPromise<any> {

        var endpoint = Utils.getRootUrl() + "Test/";
        var promise = $.getJSON(endpoint).then(data => {

            // TODO

            console.log("promise complete");
        });

        // If the callback returns a "promise"...
        // Durandal will wait for it to complete before continuing.
        return promise;
    }

    activate = () => {
        system.log('Lifecycle: activate : test/index');

        app.on("gamePaused").then(() => {
            this.gameIsPaused(true);
        });

        app.on("gameUnpaused").then(() => {
            this.gameIsPaused(false);
        });

        //return this.loadData();
    };

    binding = () => {
        system.log('Lifecycle: binding : test/index');
        return { cacheViews: false };
    }

    bindingComplete = () => {
        system.log('Lifecycle : bindingComplete : test/index');
    };

    attached = () => {
        system.log('Lifecycle : attached : test/index');
    };

    compositionComplete = () => {
        system.log('Lifecycle : compositionComplete : test/index');
    };

    detached = () => {
        system.log('Lifecycle : detached : test/index');
    };
}

var instance: ISingleton = new Index();
export = instance;