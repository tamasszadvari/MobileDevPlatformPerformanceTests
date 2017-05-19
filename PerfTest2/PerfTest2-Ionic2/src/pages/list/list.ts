import { Component } from '@angular/core';

import { NavController, NavParams } from 'ionic-angular';

@Component({
  selector: 'page-list',
  templateUrl: 'list.html'
})
export class ListPage {
  icons: string[];
  items: Array<{title: string}>;
  rows: Array<string>

  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.rows = navParams.get('rows');
    this.items = new Array();
    this.fillItems();
  }

  fillItems () {
    for (let i=0; i< this.rows.length; i++) {
        this.items.push({
        title: this.rows[i]
      });
    }
  }
}
