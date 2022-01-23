import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AccountClient} from "../../web-api-client";

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss']
})
export class CreateAccountComponent implements OnInit {

  constructor(
    private accountClient: AccountClient
  ) { }
  createNewAccountForm = new FormGroup({
    name: new FormControl('', [
      Validators.required,
    ]),
  });
  ngOnInit(): void {
  }

  createNewAccount() {
    this.accountClient.create(this.createNewAccountForm.value).subscribe()
  }
}
