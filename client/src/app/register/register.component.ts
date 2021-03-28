import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegistration = new EventEmitter();

  model: any = {}

  constructor(private accountService: AccountService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe(response => {
      console.log(response);
      this.cancelRegistration.emit(false);;
    }, error => {
      console.log(error);
      this.toastrService.error(error.error)
    });
  }

  onCancelButtonClicked() {
    this.cancelRegistration.emit(false);
  }
}
