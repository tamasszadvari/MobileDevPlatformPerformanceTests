import { Component } from '@angular/core';

import { NavController, NavParams } from 'ionic-angular';

@Component({
  selector: 'page-list',
  templateUrl: 'list.html'
})
export class ListPage {
  items: string[];

  constructor(public navCtrl: NavController, public navParams: NavParams) {
    this.items = navParams.get('rows');
  }
}
