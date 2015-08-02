import ko = require('knockout');
import models = GnomeServer.Models;

export class GnomeSummary implements models.IGnomeSummary {
    Population: KnockoutObservable<number>;
    Gnomes: KnockoutObservableArray<GnomeServer.Models.IGnome>;

    constructor(obj: models.jsonGnomeSummary) {
        this.Population = ko.observable<number>(obj.Population);
        this.Gnomes = ko.observableArray<GnomeServer.Models.IGnome>(obj.Gnomes.map((item: models.jsonGnome) => { return new Gnome(item); }));
    }
}

class GnomeProfession implements GnomeServer.Models.IGnomeProfession {
    Name: KnockoutObservable<string>;
    Skills: KnockoutObservableArray<string>;

    constructor(obj: models.jsonGnomeProfession) {
        this.Name = ko.observable(obj.Name);
        this.Skills = ko.observableArray<string>(obj.Skills);
    }
}

export class Gnome implements models.IGnome {
    ID: KnockoutObservable<number>;
    Name: KnockoutObservable<string>;
    Stats: KnockoutObservable<GnomeServer.Models.IGnomeStats>;
    Location: KnockoutObservable<GnomeServer.Models.ILocation>;
    BodyParts: KnockoutObservableArray<GnomeServer.Models.IGnomeBodyPartStatus>;
    LaborSkills: KnockoutObservableArray<GnomeServer.Models.IGnomeSkill>;
    CombatSkills: KnockoutObservableArray<GnomeServer.Models.IGnomeSkill>;
    Profession: KnockoutObservable<GnomeServer.Models.IGnomeProfession>;

    // An array of skills that are applicable to the Gnome's current profession.
    ProfessionSkills: KnockoutComputed<models.IGnomeSkill[]> = ko.pureComputed({
        owner: this,
        read: () => {
            var professionSkills = this.LaborSkills().filter((value: models.IGnomeSkill) => {
                return this.Profession().Skills().indexOf(value.Name()) >= 0;
            });
            return GnomeSkill.SortBySkill(professionSkills, true);
        },
        deferEvaluation: true
    });

    // An array of skills that are not used by the Gnome's current profession.
    NonProfessionSkills: KnockoutComputed<models.IGnomeSkill[]> = ko.pureComputed({
        owner: this,
        read: () => {
            var nonprofessionSkills = this.LaborSkills().filter((value: models.IGnomeSkill) => {
                return this.Profession().Skills().indexOf(value.Name()) < 0;
            });
            return GnomeSkill.SortBySkill(nonprofessionSkills, true);
        },
        deferEvaluation: true
    });

    constructor(obj: models.jsonGnome) {
        this.ID = ko.observable(obj.ID);
        this.Name = ko.observable(obj.Name);
        this.Stats = ko.observable<GnomeServer.Models.IGnomeStats>(new GnomeStats(obj.Stats));
        this.Location = ko.observable<GnomeServer.Models.ILocation>(new Location(obj.Location));
        this.BodyParts = ko.observableArray<models.IGnomeBodyPartStatus>(obj.BodyParts.map((item: models.jsonGnomeBodyPartStatus) => { return new GnomeBodyPartStatus(item); }));
        this.LaborSkills = ko.observableArray<models.IGnomeSkill>(GnomeSkill.SortBySkill(obj.LaborSkills.map((item: models.jsonGnomeSkill) => { return new GnomeSkill(item); }), true));
        this.CombatSkills = ko.observableArray<models.IGnomeSkill>(GnomeSkill.SortBySkill(obj.CombatSkills.map((item: models.jsonGnomeSkill) => { return new GnomeSkill(item); }), true));
        this.Profession = ko.observable(new GnomeProfession(obj.Profession));
    }
}

export class GnomeStats implements models.IGnomeStats {
    Happiness: KnockoutObservable<number>;
    BloodLevel: KnockoutObservable<number>;
    Rest: KnockoutObservable<number>;
    Hunger: KnockoutObservable<number>;
    Thirst: KnockoutObservable<number>;

    constructor(obj: models.jsonGnomeStats) {
        this.Happiness = ko.observable(obj.Happiness);
        this.BloodLevel = ko.observable(obj.BloodLevel);
        this.Rest = ko.observable(obj.Rest);
        this.Hunger = ko.observable(obj.Hunger);
        this.Thirst = ko.observable(obj.Thirst);
    }
}

export class Location implements models.ILocation {
    X: KnockoutObservable<number>;
    Y: KnockoutObservable<number>;
    Z: KnockoutObservable<number>;
    
    Text: KnockoutComputed<string> = ko.pureComputed({
        owner: this,
        read: () => {
            return "(Layer " + this.Y() + " @ " + this.X() + ", " + this.Z() + ")";
        }
    });
    
    constructor(obj: models.jsonLocation) {
        this.X = ko.observable(obj.X);
        this.Y = ko.observable(obj.Y);
        this.Z = ko.observable(obj.Z);
    }
}

export class GnomeBodyPartStatus implements models.IGnomeBodyPartStatus {
    BodyPart: KnockoutObservable<string>;
    Statuses: KnockoutObservableArray<string>;
    
    StatusText: KnockoutComputed<string> = ko.pureComputed({
        owner: this,
        read: () => {
            return this.Statuses().join(", ");
        },
        deferEvaluation: true
    });

    constructor(obj: models.jsonGnomeBodyPartStatus) {
        this.BodyPart = ko.observable(obj.BodyPart);
        this.Statuses = ko.observableArray<string>(obj.Statuses);
    }
}

export class GnomeSkill implements models.IGnomeSkill {
    Name: KnockoutObservable<string>;
    Skill: KnockoutObservable<number>;

    constructor(obj: models.jsonGnomeSkill) {
        this.Name = ko.observable(obj.Name);
        this.Skill = ko.observable(obj.Skill);
    }
    
    // Returns the input array, sorted in ascending/descending order.
    public static SortBySkill(arr: models.IGnomeSkill[], descending: boolean) : GnomeSkill[] {
        var sorted = arr.sort(this.CompareByskill);
        if (descending) { sorted.reverse(); }
        return sorted;
    }
    
    // Used to determine the sequence of IGnomeSkill objects.
    private static CompareByskill(a: models.IGnomeSkill, b: models.IGnomeSkill) {
        if (a.Skill() < b.Skill()) { return -1; }
        if (a.Skill() > b.Skill()) { return 1; }
        return 0;
    }
}