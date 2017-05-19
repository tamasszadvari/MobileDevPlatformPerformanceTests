import { Component } from '@angular/core';

import { NavController, NavParams } from 'ionic-angular';
import { AlertController } from 'ionic-angular';
import { SQLite, SQLiteObject } from '@ionic-native/sqlite';
import { File } from '@ionic-native/file';

import { ListPage } from '../list/list';

@Component({
  selector: 'page-main-page',
  templateUrl: 'main-page.html'
})
export class MainPage {
  icons: string[];
  items: Array<{title: string, id: number}>;

  constructor(public navCtrl: NavController, public navParams: NavParams, private alertCtrl: AlertController, 
              private sqlite: SQLite, private file: File) {
    this.items = [
        { title: "Clean up and prepare for tests", id: 0 },
        { title: "Add 1,000 records to SQLite", id: 1 },
        { title: "Display all records", id: 2 },
        { title: "Display all records that contain 1", id: 3 },
        { title: "Save large file", id: 4 },
        { title: "Load and display large file", id: 5 }
    ];
  }

  itemTapped(event, item) {
    switch (item.id) {
    case 0:
        this.cleanUp();
        break;
    case 1:
        this.addRecords();
        break;
    case 2:
        this.showAllRecords();
        break;
    case 3:
        this.showAllRecordsWith1();
        break;
    case 4:
        this.saveLargeFile();
        break;
    case 5:
        this.showFile();
        break;
    }
  }

  cleanUp() {
    this.sqlite.create({
        name: 'data.db',
        location: 'default'
    }).then((db: SQLiteObject) => {
        db.executeSql('DROP TABLE IF EXISTS testTable', {});
        db.executeSql('CREATE TABLE IF NOT EXISTS testTable ( id INTEGER PRIMARY KEY AUTOINCREMENT, firstName varchar(30), lastName varchar(30), misc TEXT)', {})
          .then(() => console.log('Executed SQL'))
          .catch(e => {
              console.log(e);
              this.presentError("Could not create table: " + e.toString());
            });

        var fileName = "testFile.txt";
        this.file.createFile(this.file.dataDirectory,fileName,true).then(_ => {
            this.presentSuccess('Cleanup and Prepare for Tests Successful');
        }).catch(err => {
            console.log('Could not create file');
            this.presentError('Could not create file:' + err.toString());
        });
    }).catch(e => {
        console.log(e);
        this.presentError('Could not create database:' + e.toString());
    });
  }

  addRecords() {
    this.sqlite.create({
        name: 'data.db',
        location: 'default'
    }).then((db: SQLiteObject) => {
        var success = true;

        for (var i = 0; i <= 999; i++) {
            if (!success){
                break;
            }

            var lastName = "person" + i.toString();
            db.executeSql("INSERT INTO testTable (firstName, lastName, misc) VALUES (?,?,?)", ["test", lastName, "12345678901234567890123456789012345678901234567890"])
              .catch((e) => {
                  success = false;
                  this.presentError("An error has occurred adding records: " + e.toString());
              });
        }

        if (success) {
            this.presentSuccess("All records written to database");
        }
    }).catch(e => {
        console.log(e);
        this.presentError('Could not open database:' + e.toString());
    });
  }

  showAllRecords() {
      this.showRecords("SELECT * FROM testTable;");
  }

  showAllRecordsWith1() {
      this.showRecords("SELECT * FROM testTable WHERE lastName LIKE '%1%';");
  }

  showRecords(sqlStatement) {
      this.sqlite.create({
        name: 'data.db',
        location: 'default'
    }).then((db: SQLiteObject) => {
        db.executeSql(sqlStatement, []).then((res) => {
            var items = new Array<string>();
            for (let i = 0; i < res.rows.length; i++) {
                items.push(res.rows.item(i).firstName + " " + res.rows.item(i).lastName)
            }
            this.navCtrl.push(ListPage, {
                rows: items
            }).catch((e) => {
                this.presentError("Error during page showing: " + e.toString());    
            });
        }).catch ((err) => {
            this.presentError("Error during reading from database: " + err.toString());
        });
    }).catch(e => {
        console.log(e);
        this.presentError('Could not open database:' + e.toString());
    });
  }

  saveLargeFile() {
    var fileName = "testFile.txt";
    this.file.createFile(this.file.dataDirectory,fileName,true).then(_ => {
        this.writer(fileName, 0);
    }).catch(err => {
        console.log('Could not open file');
        this.presentError('Could not open file:' + err.toString());
    });
  }

  writer(fileName, count) {
    var message = "Writing line to file at index: ";
         
    this.file.writeFile(this.file.dataDirectory,fileName, message + count + "\\n", {append: true, replace: false}).then(_ => {
        count++;
        if (count <= 999) {
            this.writer (fileName, count);
        } else {
            this.presentSuccess("All lines written to file.");
        }
    }).catch((e) => {
        this.presentError("Error during writing to file: " + e.toString())
    });
  }

  showFile() {
    var fileName = "testFile.txt";
    this.file.createFile(this.file.dataDirectory,fileName,true).then(_ => {
        this.file.readAsText(this.file.dataDirectory, fileName).then((res) => {
            var lines = res.split("\\n");
            this.navCtrl.push(ListPage, {
                rows: lines
            });
        }).catch((e) => {
            this.presentError("Error during reading from file: " + e.toString())
        });
    }).catch(err => {
        console.log('Could not open file');
        this.presentError('Could not open file:' + err.toString());
    });
  }

  presentError(message) {
    let alert = this.alertCtrl.create({
        title: 'Error',
        subTitle: message,
        buttons: ['OK']
    });
    alert.present();
  }

  presentSuccess(message) {
    let alert = this.alertCtrl.create({
        title: 'Success',
        subTitle: message,
        buttons: ['OK']
    });
    alert.present();
  }
}
