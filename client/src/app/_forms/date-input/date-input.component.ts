import { Component, Input, OnInit, Self } from '@angular/core';
import { ControlValueAccessor, NgControl } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BaseInput } from '../_shared/base-input';

@Component({
    selector: 'app-date-input',
    templateUrl: './date-input.component.html',
    styleUrls: ['./date-input.component.css', '../_shared/shared-form-styles.css']
})
export class DateInputComponent extends BaseInput implements ControlValueAccessor  {

    @Input() placeholder: string;
    @Input() maxDate: Date;
    @Input() validatorsAndMessages: { [validationName: string] : string };
    
    bsConfig: Partial<BsDatepickerConfig>;

    constructor(@Self() public ngControl: NgControl) {
        super(ngControl);

        this.ngControl.valueAccessor = this;
        this.bsConfig = {
            containerClass: 'theme-red',
            dateInputFormat: 'DD MMMM YYYY'
        }
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
