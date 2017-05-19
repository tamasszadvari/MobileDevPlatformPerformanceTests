//
//  SQLiteTableViewController.m
//  PerfTest2-ObjC
//
//  Created by kevin Ford on 1/2/15.
//  Modified by Tamás Szádvári on 19/5/17
//  Copyright (c) 2015 kevin Ford. All rights reserved.
//

#import "SQLiteTableViewController.h"

@interface SQLiteTableViewController() {
    NSMutableArray *results;
}

@end

@implementation SQLiteTableViewController

@synthesize TableQueryType;

- (void)viewDidLoad {
    [super viewDidLoad];
    
    results = [[NSMutableArray alloc] init];
    
    [self loadSqlResults];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

-(NSInteger)tableView:(UITableView*)tableView numberOfRowsInSection:(NSInteger)section {
    return results == nil ? 0 : [results count];
}

-(UITableViewCell*)tableView:(UITableView*)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *simpleTableIdentifier = @"SQLiteReuseIdentifier";
    
    UITableViewCell *cell =
        [tableView dequeueReusableCellWithIdentifier:simpleTableIdentifier] ?:
        [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:simpleTableIdentifier];
    
    cell.textLabel.text = results[indexPath.item];
    return cell;
}

-(void)loadSqlResults {
    sqliteUtilities* utilities = [[sqliteUtilities alloc]init];
    NSString* errorMessage;
    NSError* error;
    
    [utilities openConnection:&error];
    if (error) {
        errorMessage = @"Error opening connection";
    } else {
        results = (TableQueryType == (queryType)All)
            ? [utilities getAllRecords:&error]
            : [utilities getRecordsWith1:&error];
        
        if (error) {
            errorMessage = [NSString stringWithFormat:@"%@%@", @"Error executing select statement: ",error.description];
        }
    }
    
    [utilities closeConnection:&error];
    if (error) {
        errorMessage = @"Error closing connection";
    }
    
    if (error) {
        UIAlertController*  alert =[UIAlertController alertControllerWithTitle:@"Error" message:errorMessage preferredStyle: UIAlertControllerStyleAlert];
        UIAlertAction *alertAction = [UIAlertAction actionWithTitle: @"OK"
                                                    style: UIAlertActionStyleDestructive
                                                    handler: NULL];
        [alert addAction: alertAction];
        [self presentViewController: alert animated: YES completion: nil];
    }
}

@end
