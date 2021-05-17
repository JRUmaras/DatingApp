import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validator, ValidatorFn, Validators } from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../_services/account.service';

import { TextInputComponent } from '../_forms/text-input/text-input.component';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    @Output() cancelRegistration = new EventEmitter();

    registerForm: FormGroup;

    model: any = {}

    validatorMessages = {
        'required': 'Required',
        'minlength': 'Too short',
        'IsMatch': 'Must match a valid password'
    }

    readonly minPasswordLength = 5;

    get isUsernameInvalid() : boolean {
        return this.isFormControlInvalid('username');
    }

    get isPasswordInvalid() : boolean {
        return this.isFormControlInvalid('password');
    }

    get isConfirmPasswordInvalid() : boolean {
        return this.isFormControlInvalid('confirmPassword');
    }

    get password() : string {
        return this.registerForm?.controls['password']?.value ?? undefined;
    }

    constructor(private accountService: AccountService, private toastrService: ToastrService, private formBuilder: FormBuilder) { }

    ngOnInit(): void {
        this.initRegisterForm();
    }

    initRegisterForm() {
        this.registerForm = this.formBuilder.group({
            city: ['', Validators.required],
            confirmPassword: ['',[Validators.required, this.confirmPasswordCrossValidator(() => this.registerForm?.controls?.password)]],
            country: ['', Validators.required],
            dateOfBirth: ['', Validators.required],
            gender: ['male'],
            knownAs: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(this.minPasswordLength)]],
            username: ['', Validators.required],
        });

        // We need the following line because if we enter password, then a matching confirm password,
        // the confirm password control will be valid. If we then change password,
        // the confirm password remains valid and allows submission. This way we force
        // revalidation of the confirm password value.
        this.registerForm.controls.password.valueChanges.subscribe(_password => {
            this.registerForm.controls.confirmPassword.updateValueAndValidity();
        });
    }

    register() {
        console.log(this.registerForm.value);

        // this.accountService.register(this.model).subscribe(response => {
        //   console.log(response);
        //   this.cancelRegistration.emit(false);
        // }, error => {
        //   console.log(error);
        //   this.toastrService.error(error.error)
        // });
    }

    onCancelButtonClicked() {
        this.cancelRegistration.emit(false);
    }

    isFormControlInvalid(formControlName: string) : boolean {
        const usernameControl = this.registerForm.controls[formControlName];
        return usernameControl.invalid && usernameControl.touched;
    }

    confirmPasswordCrossValidator(getPasswordAbstractControl: () => AbstractControl) {
        return (control: FormControl) => {

            const passwordControl = getPasswordAbstractControl();

            return (passwordControl?.valid ?? false) && ((control?.value === passwordControl.value) ?? false)
                ? null 
                : { IsMatch: true };
        }
    }
}
