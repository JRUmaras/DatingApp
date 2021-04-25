import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';

import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';

import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
    selector: 'app-member-edit',
    templateUrl: './member-edit.component.html',
    styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

    @ViewChild('editForm') editForm: NgForm;
    @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
        if (this.stateUnsaved) {
            $event.returnValue = true;
        }
    }

    member: Member;
    user: User;

    get stateUnsaved() : boolean {
        return this.editForm?.dirty ?? false;
    }

    get unsavedChangesWarningStyle() {
        return { 'visibility': this.stateUnsaved ? 'visible' : 'hidden' }
    }

    constructor(private accountService: AccountService, private memberServices: MembersService, private toastrService: ToastrService) { 
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user)
    }

    ngOnInit(): void {
        this.loadMember();
    }

    loadMember() {
        this.memberServices.getMember(this.user.username).subscribe(member => this.member = member)
    }

    updateMember() {
        console.log(this.member);
        this.toastrService.success('Saved');
        this.editForm.reset(this.member);
    }
}
