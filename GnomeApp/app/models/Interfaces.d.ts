
 
 


/// <reference path="Enums.ts" />

declare module GnomeServer.Models {
	interface IGnome {
		ID: KnockoutObservable<number>;
		Name: KnockoutObservable<string>;
		Stats: KnockoutObservable<IGnomeStats>;
		Location: KnockoutObservable<ILocation>;
		BodyParts: KnockoutObservableArray<IGnomeBodyPartStatus>;
		Skills: KnockoutObservableArray<IGnomeSkill>;
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


