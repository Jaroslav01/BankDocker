import { Component, OnInit } from '@angular/core';
import {Transaction, TransactionClient } from 'src/app/web-api-client';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrls: ['./transaction-history.component.scss']
})
export class TransactionHistoryComponent implements OnInit {

  constructor(
    private transactionClient: TransactionClient,

  ) { }

  public transactions: Transaction[] | null = null;

  ngOnInit(): void {
    this.transactionClient.getCurrentUserTransactions().subscribe(response =>{
      this.transactions = response
      console.log("Transactions", response)
    })
  }

}
