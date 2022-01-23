import { Component, OnInit } from '@angular/core';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {Account, AccountClient, CreateTransactionCommand, Transaction, TransactionClient} from "../web-api-client";
import {MatDialog} from "@angular/material/dialog";
import {CreateAccountComponent} from "../account/create-account/create-account.component";

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss']
})
export class TransactionComponent implements OnInit {

  constructor(
    private transactionClient: TransactionClient,
    private accountClient: AccountClient,
    private dialog: MatDialog
  ) { }
  public accounts: Account[] = [];
  public transactions: Transaction[] = [];
  ngOnInit(): void {
    this.accountClient.getCurrentUserAccounts().subscribe(response => {
      this.accounts = response;
      console.log('Accounts:', response)
    });

    this.transactionClient.getCurrentUserTransactions().subscribe(response =>{
      this.transactions = response
      console.log("Transactions", response)
    })
  }
  profileForm = new FormGroup({
    senderAccount: new FormControl('', [
      Validators.required,
      Validators.minLength(19),
      Validators.maxLength(19)
      ]),
    receiverAccount: new FormControl('', [
      Validators.required,
      Validators.minLength(19),
      Validators.maxLength(19)
    ]),
    amount: new FormControl('', [
      Validators.required
    ]),
    description: new FormControl('', [
      Validators.required
    ]),
  });

  onSubmit() {
    this.transactionClient.create(this.profileForm.value).subscribe(response =>{
      console.log(response)
    })
  }

  createAccountDialog(){
    this.dialog.open(CreateAccountComponent)
  }
}
