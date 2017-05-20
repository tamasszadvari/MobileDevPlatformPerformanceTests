//
//  FileViewController.m
//  PerfTest2-ObjC
//

#import "FileViewController.h"

@interface FileViewController() {
    NSArray *results;
}

@end

@implementation FileViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
    [self loadFileResults];
}

-(NSInteger)tableView:(UITableView*)tableView numberOfRowsInSection:(NSInteger)section {
    return (results == nil) ? 0 : [results count];
}

-(UITableViewCell*)tableView:(UITableView*)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath {
    static NSString *simpleTableIdentifier = @"FileReuseIdentifier";
    
    UITableViewCell *cell =
        [tableView dequeueReusableCellWithIdentifier:simpleTableIdentifier] ?:
        [[UITableViewCell alloc] initWithStyle:UITableViewCellStyleDefault reuseIdentifier:simpleTableIdentifier];
    
    cell.textLabel.text = results[indexPath.item];
    return cell;
}


-(void) loadFileResults {
    NSError *error;
    fileUtilities *utilities = [[fileUtilities alloc]init];
    
    [utilities openFile:&error];
    
    if (error) {
        return;
    }
    
    results = [utilities readFileContents:&error];
    [utilities closeFile:&error];
}

@end
