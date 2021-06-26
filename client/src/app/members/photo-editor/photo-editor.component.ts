import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs/operators';

import { IMember } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { Photo } from 'src/app/_models/photo';
import { AccountService } from 'src/app/_services/account.service';
import { environment } from 'src/environments/environment';
import { MembersService } from 'src/app/_services/members.service';

@Component({
    selector: 'app-photo-editor',
    templateUrl: './photo-editor.component.html',
    styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {

    @Input() member: IMember;

    uploader: FileUploader;
    hasBaseDropzoneOver = false;
    baseUrl = environment.apiUrl;

    user: User;

    get uploadQueueIsEmpty() : boolean {
        return this.uploader?.queue?.length === 0 ?? false;
    }

    constructor(private accountService: AccountService, private membersService: MembersService) { 
        this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    }

    ngOnInit(): void {
        this.initializeUploader();
    }

    initializeUploader() {
        this.uploader = new FileUploader({
            url: this.baseUrl + 'users/add-photo',
            authToken: `Bearer ${this.user.token}`,
            isHTML5: true,
            allowedFileType: ['image'],
            removeAfterUpload: true,
            autoUpload: false,
            maxFileSize: 10 * 1024 // 10 MB
        });

        this.uploader.onAfterAddingFile = file => {
            file.withCredentials = false;
        }

        this.uploader.onSuccessItem = (item, response, status, headers) => {
            if (response) {
                const photo: Photo = JSON.parse(response);
                
                this.member.photos.push(photo);

                if (photo.isMain) {
                    this.user.photoUrl = photo.url;
                    this.accountService.setCurrentUser(this.user);
                    this.member.photoUrl = photo.url;
                }
            }
        }
    }

    fileOverBase(event: any) {
        this.hasBaseDropzoneOver = event;
    }

    setMainPhoto(photo: Photo) {
        this.membersService.setMainPhotoForMember(photo)
            .subscribe(() => {
                this.user.photoUrl = photo.url;
                this.accountService.setCurrentUser(this.user);

                this.member.photoUrl = photo.url;
                this.member.photos.forEach(p => {
                    p.isMain = p.id === photo.id;
                })
            });
    }

    deletePhoto(photo: Photo) {
        this.membersService.deletePhoto(photo)
            .subscribe(() => {
                for (let i = 0; i < this.member.photos.length; i++)
                {
                    if (this.member.photos[i].id === photo.id)
                    {
                        this.member.photos.splice(i, 1);
                        break;
                    }
                }

                if (photo.isMain) {
                    this.user.photoUrl = undefined;
                    this.member.photoUrl = undefined;
                    this.accountService.setCurrentUser(this.user);
                }
            });
    }
}
