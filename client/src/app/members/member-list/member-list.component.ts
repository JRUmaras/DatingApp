import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { IPagination } from 'src/app/_helpers/pagination';

import { Member } from 'src/app/_models/member';
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
    pageNumber = 1;
    pageSize = 5;

    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        //this.members$ = this.membersService.getMembers();
        this.loadMembers();
    }

    loadMembers() {
        this.membersService.getMembers(this.pageNumber, this.pageSize)
            .subscribe(paginatedMembers => {
                console.log(paginatedMembers);
                this.members = paginatedMembers.items;
                this.pagination = paginatedMembers.pagination;
            });
    }
}
