import app = require('durandal/app');
import system = require('durandal/system');
import ko = require('knockout');
import $ = require('jquery');

import models = require("../models/models");
import GnomeSummary = models.GnomeSummary;

class Index {
    endpointRoot = "http://localhost:8081/";

    name = "Gnomes";
    gameIsPaused: KnockoutObservable<Boolean> = ko.observable<Boolean>(true);
    gnomeSummary: KnockoutObservable<models.GnomeSummary> = ko.observable<models.GnomeSummary>();
    selectedGnomeID: KnockoutObservable<Number> = ko.observable(0);

    selectedGnome: KnockoutComputed<GnomeServer.Models.IGnome> = ko.pureComputed({
        owner: this,
        read: () => {
            var matches = this.gnomeSummary().Gnomes().filter((value) => {
                var ID1 = value.ID();
                var ID2 = this.selectedGnomeID && this.selectedGnomeID() || -1;
                return ID1 === ID2;
            });
            if (matches.length > 0) {
                var match = matches[0];
                return match;
            } else {
                return null;
            }
        },
        deferEvaluation: true
    });

    addGnome = () => {
        var endpoint = this.endpointRoot + "Gnome/Add";
        $.get(endpoint).always(() => {
            this.loadData();
        });
    }

    reassignProfessions = () => {
        var endpoint = this.endpointRoot + "Gnome/Assign";
        $.get(endpoint).always(() => {
            this.loadData();
        });
    }

    selectGnome = (gnome) => {
        this.selectedGnomeID(gnome.ID());
    }

    refresh = () => {
        this.loadData();
    }
    
    loadData(): JQueryPromise<any> {

        var endpoint = this.endpointRoot + "Gnome/";
        var promise = $.getJSON(endpoint).then(data => {
            var obj = new GnomeSummary(data);
            this.gnomeSummary(obj);
            this.selectedGnomeID(-1);
            var gnomes = this.gnomeSummary().Gnomes();
            if (gnomes) {
                var gnome = gnomes[0];
                var gnomeID = gnome.ID();
                this.selectedGnomeID(gnomeID);
            }
            console.log("promise complete");
        });

        // If the callback returns a "promise"...
        // Durandal will wait for it to complete before continuing.
        return promise;
    }

    activate = () => {
        system.log('Lifecycle: activate : gnome/index');

        app.on("gamePaused").then(() => {
            this.gameIsPaused(true);
        });

        app.on("gameUnpaused").then(() => {
            this.gameIsPaused(false);
        });

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