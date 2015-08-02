 
/// <reference path="Enums.ts" />

declare module GnomeServer.Models {
    interface jsonGnome {
        ID: number;
        Name: string;
        Stats: jsonGnomeStats;
        Location: jsonLocation;
        BodyParts: jsonGnomeBodyPartStatus[];
        CombatSkills: jsonGnomeSkill[];
        LaborSkills: jsonGnomeSkill[];
        Profession: jsonGnomeProfession;
    }
    interface jsonGnomeStats {
        Happiness: number;
        BloodLevel: number;
        Rest: number;
        Hunger: number;
        Thirst: number;
    }
    interface jsonLocation {
        X: number;
        Y: number;
        Z: number;
    }
    interface jsonGnomeBodyPartStatus {
        BodyPart: string;
        Statuses: string[];
    }
    interface jsonGnomeSkill {
        Name: string;
        Skill: number;
    }
    interface jsonGnomeProfession {
        Name: string;
        Skills: string[];
    }
    interface jsonGnomeSummary {
        Population: number;
        Gnomes: jsonGnome[];
    }
    interface jsonWorkshop {
        Name: string;
        AcceptGeneratedJobs: boolean;
        Efficiency: number;
        Location: jsonLocation;
    }
}


declare module GnomeServer.Models {
    interface IGnome {
        ID: KnockoutObservable<number>;
        Name: KnockoutObservable<string>;
        Stats: KnockoutObservable<IGnomeStats>;
        Location: KnockoutObservable<ILocation>;
        BodyParts: KnockoutObservableArray<IGnomeBodyPartStatus>;
        CombatSkills: KnockoutObservableArray<IGnomeSkill>;
        LaborSkills: KnockoutObservableArray<IGnomeSkill>;
        Profession: KnockoutObservable<IGnomeProfession>;
    }
    interface IGnomeStats {
        Happiness: KnockoutObservable<number>;
        BloodLevel: KnockoutObservable<number>;
        Rest: KnockoutObservable<number>;
        Hunger: KnockoutObservable<number>;
        Thirst: KnockoutObservable<number>;
    }
    interface ILocation {
        X: KnockoutObservable<number>;
        Y: KnockoutObservable<number>;
        Z: KnockoutObservable<number>;
    }
    interface IGnomeBodyPartStatus {
        BodyPart: KnockoutObservable<string>;
        Statuses: KnockoutObservableArray<string>;
    }
    interface IGnomeSkill {
        Name: KnockoutObservable<string>;
        Skill: KnockoutObservable<number>;
    }
    interface IGnomeProfession {
        Name: KnockoutObservable<string>;
        Skills: KnockoutObservableArray<string>;
    }
    interface IGnomeSummary {
        Population: KnockoutObservable<number>;
        Gnomes: KnockoutObservableArray<IGnome>;
    }
    interface IWorkshop {
        Name: KnockoutObservable<string>;
        AcceptGeneratedJobs: KnockoutObservable<boolean>;
        Efficiency: KnockoutObservable<number>;
        Location: KnockoutObservable<ILocation>;
    }
}

