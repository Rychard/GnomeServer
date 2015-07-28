import app = require('durandal/app');
import system = require('durandal/system');
import history = require("plugins/history");
import router = require('plugins/router');
import ko = require('knockout');
import $ = require('jquery');

import models = require("../models/models");
import Gnome = models.Gnome;
import GnomeSummary = models.GnomeSummary;

class Index {
    name = "Gnomes";
    gnomeSummary : GnomeSummary;
    
    loadData(): JQueryPromise<any> {

        var endpoint = "http://localhost:8081/Gnome/";
        var promise = $.getJSON(endpoint).then(data => {
            // Convert every item in the response to a StatusReport instance.

            this.gnomeSummary = new GnomeSummary(data);

            //this.gnomes = data.map((item: Gnome) => { return new Gnome(item); });
            console.log("promise complete");
        });

        // If the callback returns a "promise"...
        // Durandal will wait for it to complete before continuing.
        return promise;
    }

    activate = () => {
        system.log('Lifecycle: activate : gnome/index');
        return this.loadData();
    };

    binding = () => {
        system.log('Lifecycle: binding : gnome/index');
        return { cacheViews: false };
    }

    bindingComplete = () => {
        system.log('Lifecycle : bindingComplete : gnome/index');
    };

    attached = () => {
        system.log('Lifecycle : attached : gnome/index');
    };

    compositionComplete = () => {
        system.log('Lifecycle : compositionComplete : gnome/index');
    };

    detached = () => {
        system.log('Lifecycle : detached : gnome/index');
    };
}

var instance: ISingleton = new Index();
export = instance;

