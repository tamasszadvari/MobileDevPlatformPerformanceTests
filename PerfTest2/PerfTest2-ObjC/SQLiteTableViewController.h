//
//  SQLiteTableViewController.h
//  PerfTest2-ObjC
//
#import <UIKit/UIKit.h>
#import <sqlite3.h>
#import "queryType.h"
#import "sqliteUtilities.h"

@interface SQLiteTableViewController : UITableViewController <UITableViewDelegate, UITableViewDataSource>

@property (nonatomic, assign, readwrite) queryType TableQueryType;

@end
