import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';

import { IMember } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
    selector: 'app-member-detail',
    templateUrl: './member-detail.component.html',
    styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

    member: IMember;
    galleryOptions: NgxGalleryOptions[];
    galleryImages: NgxGalleryImage[];

    get hasPhotos(): boolean {
        return this.galleryImages.length > 0;
    }

    constructor(private memberService: MembersService, private route: ActivatedRoute) { }

    ngOnInit(): void {
        this.loadMember();

        this.galleryOptions = [
            {
                width: '500px',
                height: '500px',
                imagePercent: 100,
                thumbnailsColumns: 4,
                imageAnimation: NgxGalleryAnimation.Slide,
                preview: false
            }
        ];
    }

    getImages() : NgxGalleryImage[] {
        const imageUrls = [];

        for (const photo of this.member?.photos) {
            imageUrls.push(new NgxGalleryImage({
                small: photo.url,
                medium: photo.url,
                big: photo.url
            }));
        }

        return imageUrls
    }

    memberInit(member: IMember): void {
        this.member = member;
        this.galleryImages = this.getImages();
    }

    loadMember() {
        this.memberService.getMember(this.route.snapshot.paramMap.get('username'))
            .subscribe(member => this.memberInit(member));
    }
}
