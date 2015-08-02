import app = require('durandal/app');
import system = require('durandal/system');
import ko = require('knockout');
import $ = require('jquery');

import models = require("../models/models");

class Index {
    endpointRoot = "http://localhost:8081/";

    name = "World";
    gameIsPaused: KnockoutObservable<Boolean> = ko.observable<Boolean>(true);

    refresh = () => {
        this.loadData();
    }

    loadData(): JQueryPromise<any> {

        var endpoint = this.endpointRoot + "World/";
        var promise = $.getJSON(endpoint).then(data => {

            // TODO

            console.log("promise complete");
        });

        // If the callback returns a "promise"...
        // Durandal will wait for it to complete before continuing.
        return promise;
    }

    activate = () => {
        system.log('Lifecycle: activate : world/index');

        app.on("gamePaused").then(() => {
            this.gameIsPaused(true);
        });

        app.on("gameUnpaused").then(() => {
            this.gameIsPaused(false);
        });

        //return this.loadData();
    };

    binding = () => {
        system.log('Lifecycle: binding : world/index');
        return { cacheViews: false };
    }

    bindingComplete = () => {
        system.log('Lifecycle : bindingComplete : world/index');
    };

    attached = () => {
        system.log('Lifecycle : attached : world/index');
    };

    compositionComplete = () => {
        system.log('Lifecycle : compositionComplete : world/index');
    };

    detached = () => {
        system.log('Lifecycle : detached : world/index');
    };
}

var instance: ISingleton = new Index();
export = instance;