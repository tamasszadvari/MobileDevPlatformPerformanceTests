package com.tamasszadvari.perftest2_java;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import java.util.ArrayList;

/**
 * Created by KevinF on 1/14/2015.
 * Modified by Tamás Szádvári on 19/5/2017
 */
public class SqLiteUtilities extends SQLiteOpenHelper {

    private static final int DATABASE_VERSION = 1;
    private static final String DATABASE_NAME = "testDB";
    private static final String TABLE_NAME = "testTable";

    private SQLiteDatabase dbConn = null;

    private SQLiteDatabase getDatabase() throws Exception {
        if (dbConn == null) {
            openConnection();
        }

        return dbConn;
    }

    public SqLiteUtilities(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL("CREATE TABLE IF NOT EXISTS " + TABLE_NAME +
                " ( id INTEGER PRIMARY KEY AUTOINCREMENT, firstName varchar(30), lastName varchar(30), misc TEXT )");
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("DROP TABLE IF EXISTS " + TABLE_NAME);

        this.onCreate(db);
    }

    public void openConnection() throws Exception {
        if (dbConn!= null) {
            closeConnection();
        }
        dbConn = this.getWritableDatabase();
    }

    public void closeConnection() throws Exception {
        if (dbConn == null) {
            throw new Exception("Connection not open to close");
        } else {
            dbConn.close();
            dbConn = null;
        }
    }

    public void createTable() throws Exception {
        onUpgrade(getDatabase(), 0, 0);
    }

    public  void addRecord(String firstName, String lastName, int index, String misc) throws Exception {
        ContentValues values = new ContentValues();
        values.put("firstName", firstName);
        values.put("lastName", lastName + index);
        values.put("misc", misc);
        getDatabase().insertOrThrow(TABLE_NAME, null, values);
    }

    public ArrayList<String> getAllRecords() throws Exception {
        ArrayList<String> returnValue = new ArrayList<>();
        Cursor cursor = getDatabase().rawQuery("SELECT * FROM " + TABLE_NAME, null);

        if (cursor.moveToFirst()) {
            do {
                returnValue.add(cursor.getString(1) + " " + cursor.getString(2));
            } while (cursor.moveToNext());
        }

        cursor.close();

        return returnValue;
    }

    public ArrayList<String> getRecordsWith1() throws Exception {
        ArrayList<String> returnValue = new ArrayList<>();
        Cursor cursor = getDatabase().rawQuery("SELECT * FROM " + TABLE_NAME + " WHERE lastName LIKE '%1%'", null);

        if (cursor.moveToFirst()) {
            do {
                returnValue.add(cursor.getString(1) + " " + cursor.getString(2));
            } while (cursor.moveToNext());
        }

        cursor.close();

        return returnValue;
    }
}
