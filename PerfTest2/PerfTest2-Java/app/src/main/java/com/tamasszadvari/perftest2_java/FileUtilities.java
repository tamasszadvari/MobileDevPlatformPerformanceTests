package com.tamasszadvari.perftest2_java;

import android.content.Context;
import android.os.Environment;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;

public class FileUtilities {
    private BufferedWriter fileHandle;
    private Context context;
    private File textFile;

    private BufferedWriter getFileHandle () throws Exception {
        if (fileHandle == null) {
            fileHandle = new BufferedWriter(new FileWriter(textFile));
        }

        return fileHandle;
    }

    public FileUtilities(Context context) {
        this.context = context;
        String filePath = context.getExternalFilesDir(Environment.DIRECTORY_DOWNLOADS) + File.separator + "testFile.txt";
        textFile = new File(filePath);
    }

    public void closeFile() throws Exception {
        if (fileHandle != null) {
            fileHandle.close();
            fileHandle = null;
        }
    }

    public void deleteFile() {
        if (textFile.exists()) {
            textFile.delete();
        }
    }

    public void createFile() throws Exception {
        if (!textFile.exists()) {
            textFile.createNewFile();
        }
    }

    public void writeLineToFile(String line) throws Exception {
        if (!textFile.exists()) {
            textFile.createNewFile();
        }

        getFileHandle().write(line);
    }

    public ArrayList<String> readFileContents() throws Exception {
        BufferedReader reader = new BufferedReader(new FileReader(textFile));
        ArrayList<String> returnValue = new ArrayList<>();
        String line;

        while((line = reader.readLine()) != null){
            returnValue.add(line);
        }

        reader.close();

        return returnValue;
    }
}