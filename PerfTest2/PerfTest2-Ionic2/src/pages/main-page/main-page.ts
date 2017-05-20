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
  items: string[];

  constructor(public navCtrl: NavController, public navParams: NavParams, private alertCtrl: AlertController, 
              private sqlite: SQLite, private file: File) {
    this.items = [
        "Clean up and prepare for tests",
        "Add 1,000 records to SQLite",
        "Display all records",
        "Display all records that contain 1",
        "Save large file",
        "Load and display large file"
    ];
  }

  itemTapped(event, item) {
    let id = this.items.indexOf(item);
    switch (id) {
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
          .then(() => {
            console.log('Executed SQL')
            this.file.createFile(this.file.dataDirectory,"testFile.txt",true).then(_ => {
                console.log('Executed file creation')
                this.presentSuccess('Cleanup and Prepare for Tests Successful');
            }).catch(e => {
                this.presentError('Could not create file:' + e.toString());
            });
        }).catch(e => {
            this.presentError("Could not create table: " + e.toString());
        });
    }).catch(e => {
        this.presentError('Could not create database:' + e.toString());
    });
  }

  addRecords() {
    this.sqlite.create({
        name: 'data.db',
        location: 'default'
    }).then((db: SQLiteObject) => {
        let success = true;

        for (let i = 0; i <= 999; i++) {
            if (!success){
                break;
            }

            let lastName = "person" + i.toString();
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
            let items = new Array<string>(res.rows.length);
            for (let i = 0; i < res.rows.length; i++) {
                items[i] = res.rows.item(i).firstName + " " + res.rows.item(i).lastName;
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
        this.presentError('Could not open database:' + e.toString());
    });
  }

  saveLargeFile() {
    var fileName = "testFile.txt";
    this.file.createFile(this.file.dataDirectory,fileName,true).then(_ => {
        this.writeToFile(fileName, 0);
    }).catch(err => {
        this.presentError('Could not open file:' + err.toString());
    });
  }

  writeToFile(fileName, count) {
    var message = "Writing line to file at index: ";
         
    this.file.writeFile(this.file.dataDirectory,fileName, message + count + "\\n", {append: true, replace: false}).then(_ => {
        count++;
        if (count <= 999) {
            this.writeToFile (fileName, count);
        } else {
            this.presentSuccess("All lines written to file.");
        }
    }).catch((e) => {
        this.presentError("Error during writing to file: " + e.toString())
    });
  }

  showFile() {
    let fileName = "testFile.txt";
    this.file.createFile(this.file.dataDirectory,fileName,true).then(_ => {
        this.file.readAsText(this.file.dataDirectory, fileName).then((res) => {
            let lines = res.split("\\n");
            this.navCtrl.push(ListPage, {
                rows: lines
            });
        }).catch((e) => {
            this.presentError("Error during reading from file: " + e.toString())
        });
    }).catch(err => {
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
