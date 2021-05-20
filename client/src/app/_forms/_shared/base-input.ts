import { NgControl } from "@angular/forms";

export class BaseInput {

    public validatorsAndMessages: { [validationName: string] : string } = undefined;

    constructor(public ngControl: NgControl) {
    }

    get tooltip(): string {

        if (!this.isInvalid() || this.validatorsAndMessages === undefined) return undefined;

        let tooltip: string = "";

        for (let validatorName in this.validatorsAndMessages) {
            if (!this.ngControl.control.hasError(validatorName)) continue;
            tooltip += this.validatorsAndMessages[validatorName] + "\n";
        }
        
        return tooltip;
    }

    isInvalid() : boolean {
        return this.ngControl.touched && this.ngControl.invalid;
    }
}