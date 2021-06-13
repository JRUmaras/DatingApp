import { Component, Input, OnInit, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { IUserParams } from 'src/app/_interfaces/IUserParams';
import { UserParams } from 'src/app/_models/userParams';

@Component({
    selector: 'app-members-filter',
    templateUrl: './members-filter.component.html',
    styleUrls: ['./members-filter.component.css']
})
export class MembersFilterComponent implements OnInit {

    @Input() userParams: IUserParams;
    @Output() onApplyFilter: EventEmitter<IUserParams> = new EventEmitter();
    @Output() onResetFilter: EventEmitter<IUserParams> = new EventEmitter();

    genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Females'}];

    constructor() { }

    ngOnInit(): void {
    }

    onSubmit() {
        this.onApplyFilter.emit(this.userParams);
    }

    resetFilters() {
        this.onResetFilter.emit(null);
    }
}
