
 
 


/// <reference path="Enums.ts" />

declare module GnomeServer.Models {
	interface IGnomeSummary {
		Population: KnockoutObservable<Number>;
		Gnomes: KnockoutObservableArray<IGnome>;
	}
	interface IGnome {
		ID: KnockoutObservable<Number>;
		Name: KnockoutObservable<String>;
		Stats: KnockoutObservable<IGnomeStats>;
		Location: KnockoutObservable<IGnomeLocation>;
		BodyParts: KnockoutObservableArray<IGnomeBodyPartStatus>;
		Skills: KnockoutObservableArray<IGnomeSkill>;
		Profession: KnockoutObservable<IGnomeProfession>;
	}
	interface IGnomeStats {
		Happiness: KnockoutObservable<Number>;
		BloodLevel: KnockoutObservable<Number>;
		Rest: KnockoutObservable<Number>;
		Hunger: KnockoutObservable<Number>;
		Thirst: KnockoutObservable<Number>;
	}
	interface IGnomeLocation {
		X: KnockoutObservable<Number>;
		Y: KnockoutObservable<Number>;
		Z: KnockoutObservable<Number>;
	}
	interface IGnomeBodyPartStatus {
		BodyPart: KnockoutObservable<String>;
		Statuses: KnockoutObservableArray<String>;
	}
	interface IGnomeSkill {
		Name: KnockoutObservable<String>;
		Skill: KnockoutObservable<Number>;
	}
	interface IGnomeProfession {
		Name: KnockoutObservable<String>;
		Skills: KnockoutObservableArray<String>;
	}
}


