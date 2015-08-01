import ko = require('knockout');
import models = GnomeServer.Models;

export class GnomeSummary implements models.IGnomeSummary {
    Population: KnockoutObservable<Number>;
    Gnomes: KnockoutObservableArray<GnomeServer.Models.IGnome>;

    constructor(obj: any) {
        this.Population = ko.observable<Number>(obj.Population);
        this.Gnomes = ko.observableArray<GnomeServer.Models.IGnome>(obj.Gnomes.map((item: Gnome) => { return new Gnome(item); }));
    }
}

class GnomeProfession implements GnomeServer.Models.IGnomeProfession {
    Name: KnockoutObservable<String>;
    Skills: KnockoutObservableArray<String>;

    constructor(obj: any) {
        this.Name = ko.observable(obj.Name);
        this.Skills = ko.observableArray<String>(obj.Skills);
    }
}

export class Gnome implements models.IGnome {
    ID: KnockoutObservable<Number>;
    Name: KnockoutObservable<String>;
    Stats: KnockoutObservable<GnomeServer.Models.IGnomeStats>;
    Location: KnockoutObservable<GnomeServer.Models.IGnomeLocation>;
    BodyParts: KnockoutObservableArray<GnomeServer.Models.IGnomeBodyPartStatus>;
    Skills: KnockoutObservableArray<GnomeServer.Models.IGnomeSkill>;
    Profession: KnockoutObservable<GnomeServer.Models.IGnomeProfession>;

    // An array of skills that are applicable to the Gnome's current profession.
    ProfessionSkills: KnockoutComputed<models.IGnomeSkill[]> = ko.pureComputed({
        owner: this,
        read: () => {
            var professionSkills = this.Skills().filter((value: models.IGnomeSkill) => {
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
            var nonprofessionSkills = this.Skills().filter((value: models.IGnomeSkill) => {
                return this.Profession().Skills().indexOf(value.Name()) < 0;
            });
            return GnomeSkill.SortBySkill(nonprofessionSkills, true);
        },
        deferEvaluation: true
    });

    constructor(obj: any) {
        this.ID = ko.observable(obj.ID);
        this.Name = ko.observable(obj.Name);
        this.Stats = ko.observable<GnomeServer.Models.IGnomeStats>(new GnomeStats(obj.Stats));
        this.Location = ko.observable<GnomeServer.Models.IGnomeLocation>(new GnomeLocation(obj.Location));
        this.BodyParts = ko.observableArray<GnomeServer.Models.IGnomeBodyPartStatus>(obj.BodyParts.map((item: GnomeBodyPartStatus) => { return new GnomeBodyPartStatus(item); }));
        this.Skills = ko.observableArray<GnomeServer.Models.IGnomeSkill>(GnomeSkill.SortBySkill(obj.Skills.map((item: GnomeSkill) => { return new GnomeSkill(item); }), true));
        this.Profession = ko.observable(new GnomeProfession(obj.Profession));
    }
}

export class GnomeStats implements models.IGnomeStats {
    Happiness: KnockoutObservable<Number>;
    BloodLevel: KnockoutObservable<Number>;
    Rest: KnockoutObservable<Number>;
    Hunger: KnockoutObservable<Number>;
    Thirst: KnockoutObservable<Number>;

    constructor(obj: any) {
        this.Happiness = ko.observable(obj.Happiness);
        this.BloodLevel = ko.observable(obj.BloodLevel);
        this.Rest = ko.observable(obj.Rest);
        this.Hunger = ko.observable(obj.Hunger);
        this.Thirst = ko.observable(obj.Thirst);
    }
}

export class GnomeLocation implements models.IGnomeLocation {
    X: KnockoutObservable<Number>;
    Y: KnockoutObservable<Number>;
    Z: KnockoutObservable<Number>;
    
    Text: KnockoutComputed<String> = ko.pureComputed({
        owner: this,
        read: () => {
            return "(Layer " + this.Y() + " @ " + this.X() + ", " + this.Z() + ")";
        }
    });
    
    constructor(obj: any) {
        this.X = ko.observable(obj.X);
        this.Y = ko.observable(obj.Y);
        this.Z = ko.observable(obj.Z);
    }
}

export class GnomeBodyPartStatus implements models.IGnomeBodyPartStatus {
    BodyPart: KnockoutObservable<String>;
    Statuses: KnockoutObservableArray<String>;
    
    StatusText: KnockoutComputed<String> = ko.pureComputed({
        owner: this,
        read: () => {
            return this.Statuses().join(", ");
        },
        deferEvaluation: true
    });

    constructor(obj: any) {
        this.BodyPart = ko.observable(obj.BodyPart);
        this.Statuses = ko.observableArray<String>(obj.Statuses);
    }
}

export class GnomeSkill implements models.IGnomeSkill {
    Name: KnockoutObservable<String>;
    Skill: KnockoutObservable<Number>;

    constructor(obj: any) {
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