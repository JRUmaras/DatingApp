import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';

//import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};

  @ViewChild('loginForm') loginForm: NgForm;

  //constructor(public accountService: AccountService, private router: Router, private toastrService: ToastrService) { }
  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe(response => {
      this.router.navigate(['/members']);
    }, error => {
      console.log(error);
      //this.toastrService.error(error.error);
    }, () => {
      this.loginForm.resetForm();
    });
  }

  logout() {
    this.accountService.logout();
    this.router.navigate(['/']);
  }
}
