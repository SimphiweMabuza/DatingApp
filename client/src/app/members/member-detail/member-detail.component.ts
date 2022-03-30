import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from '../../_models/members';
import { MembersService } from '../../_services/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

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
    ]

  }

  getImages(): NgxGalleryImage[] {
    const imageUrl = [];
    for (const photo of this.member.Photos) {
      imageUrl.push({
        small: photo?.Url,
        medium: photo?.Url,
        big: photo?.Url

      })
    }

    return imageUrl;
  }
  loadMember() {
    this.memberService.getMember(this.route.snapshot.paramMap.get('username')).subscibe(member => {
      this.member = member;

      this.galleryImages = this.getImages();
    })
  }

}
