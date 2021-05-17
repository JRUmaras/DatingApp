import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';

@Component({
    selector: 'app-text-input',
    templateUrl: './text-input.component.html',
    styleUrls: ['./text-input.component.css']
})
export class TextInputComponent implements ControlValueAccessor {
    @Input() placeholder: string;
    @Input() type = 'text';
    @Input() validatorsAndMessages: { [validationName: string] : string };

    get tooltip() : string {

        if (!this.isInvalid()) return undefined;

        let tooltip: string = "";

        for (let validatorName in this.validatorsAndMessages) {
            if (!this.ngControl.control.hasError(validatorName)) continue;
            tooltip += this.validatorsAndMessages[validatorName] + "\n";
        }
        
        return tooltip;
    }

    constructor(@Self() public ngControl: NgControl) { 
        this.ngControl.valueAccessor = this;
    }

    writeValue(obj: any): void {
    }

    registerOnChange(fn: any): void {
    }
    
    registerOnTouched(fn: any): void {
    }

    isInvalid() : boolean {
        return this.ngControl.touched && this.ngControl.invalid;
    }
}
