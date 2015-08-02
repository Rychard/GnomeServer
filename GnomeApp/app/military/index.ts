import app = require('durandal/app');
import system = require('durandal/system');
import ko = require('knockout');
import $ = require('jquery');

import models = require("../models/models");
import Utils = require("../utils");

class Index {
    name = "Military";
    gameIsPaused: KnockoutObservable<Boolean> = ko.observable<Boolean>(true);
    
    refresh = () => {
        this.loadData();
    }
    
    loadData(): JQueryPromise<any> {

        var endpoint = Utils.getRootUrl() + "Military/";
        var promise = $.getJSON(endpoint).then(data => {

            // TODO

            console.log("promise complete");
        });

        // If the callback returns a "promise"...
        // Durandal will wait for it to complete before continuing.
        return promise;
    }

    activate = () => {
        system.log('Lifecycle: activate : military/index');

        app.on("gamePaused").then(() => {
            this.gameIsPaused(true);
        });

        app.on("gameUnpaused").then(() => {
            this.gameIsPaused(false);
        });

        //return this.loadData();
    };

    binding = () => {
        system.log('Lifecycle: binding : military/index');
        return { cacheViews: false };
    }

    bindingComplete = () => {
        system.log('Lifecycle : bindingComplete : military/index');
    };

    attached = () => {
        system.log('Lifecycle : attached : military/index');
    };

    compositionComplete = () => {
        system.log('Lifecycle : compositionComplete : military/index');
    };

    detached = () => {
        system.log('Lifecycle : detached : military/index');
    };
}

var instance: ISingleton = new Index();
export = instance;