//
//  FileUtilities.h
//  PerfTest2-ObjC
//

#import <Foundation/Foundation.h>

@interface fileUtilities : NSObject

- (void)openFile:(NSError**)error;
- (void)closeFile:(NSError**)error;
- (void)deleteFile:(NSError**)error;
- (void)createFile:(NSError**)error;
- (void)writeLineToFile:(NSError**)error withTextToWrite:(NSData*)textToWrite;
- (NSArray*)readFileContents:(NSError**)error;

@end
