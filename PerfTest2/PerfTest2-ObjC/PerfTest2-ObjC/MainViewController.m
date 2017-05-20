//
//  ViewController.m
//  PerfTest2-ObjC
//

#import "MainViewController.h"

@interface MainViewController () {
    NSString *menuItems[ 6 ];
    NSString *dbPath;
    queryType nativationQueryType;
}

@end

@implementation MainViewController

const int menuCleanUp = 0;
const int menuAddRecords = 1;
const int menuDisplayAll = 2;
const int menuDisplayWithWhere = 3;
const int menuSaveLargeFile = 4;
const int menuLoadAndDisplayFile = 5;

- (void)viewDidLoad {
    [super viewDidLoad];
    
    dbPath = NSTemporaryDirectory();
    dbPath = [dbPath stringByAppendingPathComponent:@"testDB.sql3"];
    [[NSUserDefaults standardUserDefaults] setValue:dbPath forKey:@"dbPath"];
    [[NSUserDefaults standardUserDefaults] synchronize];
    
    menuItems[menuCleanUp] = @"Clean up and prepare for tests";
    menuItems[menuAddRecords] = @"Add 1,000 records to SQLite";
    menuItems[menuDisplayAll] = @"Display all records";
    menuItems[menuDisplayWithWhere] = @"Display all records that contain 1";
    menuItems[menuSaveLargeFile] = @"Save large file";
    menuItems[menuLoadAndDisplayFile] = @"Load and display large file";
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    if ([segue.identifier isEqualToString:@"sguToSQLiteTableView"]) {
        SQLiteTableViewController *destViewController = segue.destinationViewController;
        destViewController.TableQueryType = nativationQueryType;
    }
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section {
    return 6;
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *simpleTableIdentifier = @"MyReuseIdentifier";
    
    UITableViewCell *cell =
        [tableView dequeueReusableCellWithIdentifier:simpleTableIdentifier] ?:
        [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:simpleTableIdentifier];

    cell.textLabel.text = menuItems[indexPath.item];
    
    return cell;
}

- (void)tableView:(UITableView *)tableView didSelectRowAtIndexPath:(NSIndexPath *)indexPath {
    if (indexPath.row == menuCleanUp) {
        [self cleanUp];
    } else if (indexPath.row == menuAddRecords) {
        [self addRecords];
    } else if (indexPath.row == menuDisplayAll) {
        [self showAllRecords];
    } else if (indexPath.row == menuDisplayWithWhere) {
        [self showRecordsWith];
    } else if (indexPath.row == menuSaveLargeFile) {
        [self saveLargeFile];
    } else if (indexPath.row == menuLoadAndDisplayFile) {
        [self loadAndDisplayFile];
    }
}

- (void)cleanUp {
    NSString* errMsg = @"";
    bool hasError = false;
    sqliteUtilities* sqlUtilities;
    fileUtilities* fUtilities;
    
    sqlUtilities = [[sqliteUtilities alloc]init];
    fUtilities = [[fileUtilities alloc]init];
    
    //Get Temporary Directory
    NSError *error;
    
    [sqlUtilities deleteFile:&error];
    if (error) {
        hasError = true;
        errMsg = @"Problem removing old version of SQLite DB.";
    }
    
    if (!hasError) {
        [sqlUtilities openConnection:&error];
        if (error) {
            hasError = true;
            errMsg = @"Problem removing old version of SQLite DB.";
        }
    }
    
    if (!hasError) {
        [sqlUtilities createTable:&error];
        if (error) {
            hasError = true;
            errMsg = @"Problem creating table.";
        }
    }
    
    [sqlUtilities closeConnection:&error];
    if (error) {
        hasError = true;
        errMsg = [NSString stringWithFormat:@"Problem closing connection: %@", error.description];
    }
    
    
    if (!hasError) {
        [fUtilities deleteFile:&error];
        if (error) {
            hasError = true;
            errMsg = @"Problem deleting old file.";
        }
    }
    
    if (!hasError) {
        [fUtilities createFile:&error];
        if (error) {
            hasError = true;
            errMsg = @"Problem creating new file.";
        }
    }
    
    UIAlertController *alert = !hasError
        ? [UIAlertController alertControllerWithTitle:@"Cleanup and Prepare for Tests Successful" message:@"Completed Test Setup" preferredStyle: UIAlertControllerStyleAlert]
        : [UIAlertController alertControllerWithTitle:@"Cleanup and Prepare for Tests Error" message:[NSString stringWithFormat:@"%@%@", @"Error: ", errMsg] preferredStyle: UIAlertControllerStyleAlert];
    
    UIAlertAction *alertAction = [UIAlertAction actionWithTitle: @"OK"
                                                          style: UIAlertActionStyleDestructive
                                                        handler: NULL];
    
    [alert addAction: alertAction];
    [self presentViewController: alert animated: YES completion: nil];
}

- (void)addRecords {
    sqliteUtilities* utilities = [[sqliteUtilities alloc]init];
    NSError* error;
    NSString* errorMessage;
    UIAlertController *alert;
    
    if (!error) {
        for (int i = 0; i <= 999; i++) {
            [utilities addRecord:@"test" withLastName:@"person" withIndex:i withMisc:@"12345678901234567890123456789012345678901234567890" withError:&error];
        
            if (error) {
                errorMessage = @"Error writing line to database";
                break;
            }
        }
    } else {
        errorMessage = @"Error opening connection";
    }
    
    [utilities closeConnection:&error];
    if (error) {
        errorMessage = @"Error closing connection";
    }
    alert = error
        ? [UIAlertController alertControllerWithTitle:@"Error" message: errorMessage preferredStyle: UIAlertControllerStyleAlert]
        : [UIAlertController alertControllerWithTitle:@"Success" message:@"All records written to database" preferredStyle: UIAlertControllerStyleAlert];
    
    UIAlertAction *alertAction = [UIAlertAction actionWithTitle: @"OK"
                                                          style: UIAlertActionStyleDestructive
                                                        handler: NULL];
    
    [alert addAction: alertAction];
    [self presentViewController: alert animated: YES completion: nil];
}

- (void)showAllRecords {
    nativationQueryType = (queryType)All;
    [self performSegueWithIdentifier: @"sguToSQLiteTableView"
                              sender: self];
}

- (void)showRecordsWith {
    nativationQueryType = (queryType)Contain1;
    [self performSegueWithIdentifier: @"sguToSQLiteTableView"
                              sender: self];
}

- (void)saveLargeFile {
    fileUtilities* utilities = [[fileUtilities alloc]init];
    NSError* error;
    NSString* errorMessage;
    UIAlertController *alert;
    
    [utilities openFile:&error];
    if (!error) {
        for (int i = 0; i <= 999; i++) {
            NSString *myString = [NSString stringWithFormat:@"Writing line to file at index: %d\r\n", i];
            const char *utfString = [myString UTF8String];
            NSData *textLine = [NSData dataWithBytes: utfString length: strlen (utfString)];
            [utilities writeLineToFile:&error withTextToWrite:textLine];
        
            if (error) {
                errorMessage = @"Error writing line to file";
                break;
            }
        }
    } else {
        errorMessage = @"Error opening connection";
    }
    
    [utilities closeFile:&error];
    if (error) {
        errorMessage = @"Error closing connection";
    }
    alert = error
        ? [UIAlertController alertControllerWithTitle:@"Error" message: errorMessage preferredStyle: UIAlertControllerStyleAlert]
        : [UIAlertController alertControllerWithTitle:@"Success" message:@"All lines written to file" preferredStyle: UIAlertControllerStyleAlert];
    
    UIAlertAction *alertAction = [UIAlertAction actionWithTitle: @"OK"
                                                          style: UIAlertActionStyleDestructive
                                                        handler: NULL];
    
    [alert addAction: alertAction];
    [self presentViewController: alert animated: YES completion: nil];
}

- (void)loadAndDisplayFile {
    [self performSegueWithIdentifier: @"sguToFileTableView"
                              sender: self];
}

@end
