import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';
import { Member } from 'src/app/_models/members';

@Component({
  selector: 'app-list-view',
  templateUrl: './list-view.component.html',
  styleUrls: ['./list-view.component.css']
})
export class ListViewComponent implements OnInit {
  members: Partial<Member[]>;
  predicate = 'viewd';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;

  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadViews();
  }
  
  loadViews() {
    this.memberService.getViews(this.predicate, this.pageNumber, this.pageSize).subscribe(response => {
      this.members = response.result;
      this.pagination = response.pagination;
    })
  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadViews();
  }

}