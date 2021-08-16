import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

import { IMember } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
    selector: 'app-member-card',
    templateUrl: './member-card.component.html',
    styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

    @Input() member: IMember;

    constructor(private membersService: MembersService, private toastrService: ToastrService) { }

    ngOnInit(): void {
    }

    addLike() {
        this.membersService
            .addLike(this.member.username)
            .subscribe(() => {
                this.toastrService.success(`You've liked ${this.member.username}! ;)`);
            });
    }
}
