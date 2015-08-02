import app = require('durandal/app');
import ko = require('knockout');
import Utils = require("../../utils");

class Speed {
    isDialogActive = false;
    isDisabled: KnockoutObservable<Boolean>;
    speedCurrent: KnockoutObservable<number>;
    speedIsPaused: KnockoutObservable<Boolean>;

    speedDecrease = () => {
        var current = this.speedCurrent();
        if (this.speedCurrent() > 1) {
            var newValue = current - 1;
            this.speedCurrent(newValue);
            this.setSpeed();    
        }
    }

    speedPause = () => {
        this.pause();
    }

    speedPlay = () => {
        this.play();
    }

    speedIncrease = () => {
        var current = this.speedCurrent();
        
        // Set an arbitrary limit of 20x.
        // From what I can tell, the simulation starts skipping stuff after 5x.
        if (this.speedCurrent() < 20) {
            var newValue = current + 1;
            this.speedCurrent(newValue);
            this.setSpeed();    
        }
    }

    speedCanPause: KnockoutComputed<Boolean> = ko.pureComputed({
        owner: this,
        read: () => {
            return !this.speedIsPaused();
        }
    });

    speedCanPlay: KnockoutComputed<Boolean> = ko.pureComputed({
        owner: this,
        read: () => {
            return this.speedIsPaused();
        }
    });

    speedText: KnockoutComputed<String> = ko.pureComputed({
        owner: this,
        read: () => {
            if (this.speedIsPaused()) {
                return "(Paused) " + this.speedCurrent().toString();
            } else {
                return this.speedCurrent().toString();
            }
        },
        deferEvaluation: true
    });

    onError = (errorMessage: string) => {
        if (!this.isDialogActive) {
            this.isDialogActive = true;
            this.isDisabled(true);
            var response = app.showMessage('An error occurred while attempting to Gnomoria.<br />The following message was specified:<br /><br />' + errorMessage + '<br /><br />Please verify that Gnomoria is running, and then click \'Retry\'.', 'Connection Error', ['Retry']);
            response.then((clickedButtonText) => {
                this.isDialogActive = false;
                this.isDisabled(true);
            });
        }
    }

    getState = () => {
        var endpoint = Utils.getRootUrl() + "Game/State";
        $.get(endpoint, (data) => {
            var state = (data.State !== "SinglePlayerGame");
            console.log("IsDisabled: " + state);
            this.isDisabled(state);
        }).fail((data) => {
            this.onError("Unable to access current Game State.");
        });
    }

    getSpeed = () => {
        var endpoint = Utils.getRootUrl() + "Game/Speed";
        $.get(endpoint, (data) => {
            this.speedCurrent(parseInt(data.Speed));
            this.speedIsPaused(data.IsPaused.toLowerCase() === "true");
        }).fail((data) => {
            this.onError("Unable to access current Game Speed.");
        });
    }

    setSpeed = () => {
        var endpoint = Utils.getRootUrl() + "Game/Speed?speed=" + this.speedCurrent().toString();
        $.post(endpoint).fail((data) => {
            this.onError("Could not set desired Game Speed.");
        });
    }

    pause = () => {
        var endpoint = Utils.getRootUrl() + "Game/Pause";
        $.post(endpoint, () => {
            this.speedIsPaused(true);
        }).fail((data) => {
            this.onError("Could not pause the game.");
        });
    }

    play = () => {
        var endpoint = Utils.getRootUrl() + "Game/Play";
        $.post(endpoint, () => {
            this.speedIsPaused(false);
        }).fail((data) => {
            this.onError("Could not unpause the game.");
        });
    }

    activate = () => {
        this.speedIsPaused = ko.observable<Boolean>(true);
        this.speedCurrent = ko.observable<number>(0);
        this.isDisabled = ko.observable<Boolean>(true);

        // Poll the server for this data every x seconds.
        this.update();

        this.speedCurrent.subscribe((newValue) => {
            app.trigger('gameSpeedChanged', newValue);
        });

        this.speedIsPaused.subscribe((newValue) => {
            if (newValue) {
                app.trigger('gamePaused');
            } else {
                app.trigger('gameUnpaused');
            }
        });
    };

    update = () => {
        setTimeout(() => {
            // Prevent multiple dialogs from stacking.
            if (!this.isDialogActive || !this.isDisabled()) {
                this.getState();
                this.getSpeed();
            }
            this.update();
        }, 2500);
    }
}

export = Speed;