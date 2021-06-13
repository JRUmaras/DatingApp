import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { IPagination } from 'src/app/_helpers/pagination';

import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

    //members$: Observable<Member[]>;

    members: Member[];
    pagination: IPagination;
    userParams: UserParams;
    user: User;

    get numberOfMatches() {
        return this.pagination?.totalCount ?? 0;
    }

    constructor(private membersService: MembersService, private accountService: AccountService) {
        this.accountService.currentUser$
            .pipe(take(1))
            .subscribe(user => {
                this.user = user;
                this.userParams = new UserParams(user);
               this.pagination = {
                pageNumber: this.userParams.pageNumber,
                pageSize: this.userParams.pageSize,
                totalCount: 0,
                totalPages: 1
               } 
            });
     }

    ngOnInit(): void {
        //this.members$ = this.membersService.getMembers();
        this.loadMembers();
    }

    loadMembers() {
        this.membersService.getMembers(this.userParams)
            .subscribe(paginatedMembers => {
                this.members = paginatedMembers.items;
                this.pagination = paginatedMembers.pagination;
            });
    }

    resetFilters() {
        this.userParams = new UserParams(this.user);
        this.loadMembers();
    }

    pageChanged(event: any) {
        this.userParams.pageNumber = event.page;
        this.loadMembers();
    }
}
