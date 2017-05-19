package com.tamasszadvari.perftest2_java;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Fragment;
import android.app.FragmentManager;
import android.app.FragmentTransaction;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ListView;

public class MainActivity extends Activity implements AdapterView.OnItemClickListener, SqLiteTableFragment.OnFragmentInteractionListener, DisplayTextFileFragment.OnFragmentInteractionListener {
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Fragment currentFragment = new MainMenuFragment();
        FragmentTransaction trans = getFragmentManager().beginTransaction();
        trans.add(R.id.main_area, currentFragment);
        trans.addToBackStack(null);
        trans.commit();
    }

    @Override
    public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
        ListView lstMainMenu = (ListView)this.findViewById(R.id.lstMainMenu);
        MainMenuAdapter adapter = (MainMenuAdapter)lstMainMenu.getAdapter();

        if (position == adapter.CLEAN_UP_TEST) {
            cleanUp();
        } else if (position == adapter.ADD_SQL_RECORDS) {
            addRecords();
        } else if (position == adapter.DISPLAY_ALL_RECORDS) {
            showAllRecords();
        } else if (position == adapter.DISPLAY_RECORDS_WITH_WHERE) {
            showRecordsWith();
        } else if (position == adapter.SAVE_LARGE_FILE) {
            saveLargeFile();
        } else if (position == adapter.DISPLAY_LARGE_FILE) {
            loadAndDisplayFile();
        }
    }

    private void cleanUp() {
        SqLiteUtilities sqlUtilities = new SqLiteUtilities(this);
        FileUtilities fUtilities = new FileUtilities(this);
        AlertDialog.Builder alert  = new AlertDialog.Builder(this);

        try {
            sqlUtilities.openConnection();
            sqlUtilities.createTable();
            sqlUtilities.closeConnection();

            fUtilities.deleteFile();
            fUtilities.createFile();

            alert.setMessage("Completed test setup");
            alert.setTitle("Cleanup and Prepare for Tests Successful");
        } catch (Exception ex) {
            alert.setMessage("An error has occurred: " + ex.getMessage());
            alert.setTitle("Error");
        } finally {
            alert.setPositiveButton("OK", null);
            alert.setCancelable(true);
            alert.create().show();
        }
    }

    private void addRecords() {
        SqLiteUtilities utilities = new SqLiteUtilities(this);
        AlertDialog.Builder alert  = new AlertDialog.Builder(this);

        try {
            utilities.openConnection();

            for (int i = 0; i <= 999; i++) {
                utilities.addRecord("test", "person", i, "12345678901234567890123456789012345678901234567890");
            }

            alert.setMessage("All records written to database");
            alert.setTitle("Success");
        } catch (Exception ex) {
            alert.setMessage("An error has occurred adding records: " + ex.getMessage());
            alert.setTitle("Error");
        } finally {
            alert.setPositiveButton("OK", null);
            alert.setCancelable(true);
            alert.create().show();
        }
    }

    private void showAllRecords() {
        FragmentManager fragmentManager = getFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        SqLiteTableFragment fragment = new SqLiteTableFragment();
        Bundle bundle = new Bundle();
        bundle.putSerializable("displayType", SqLiteDisplayType.ShowAll);
        fragment.setArguments(bundle);
        fragmentTransaction.replace(R.id.main_area, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    private void showRecordsWith() {
        FragmentManager fragmentManager = getFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        SqLiteTableFragment fragment = new SqLiteTableFragment();
        Bundle bundle = new Bundle();
        bundle.putSerializable("displayType", SqLiteDisplayType.ShowContaining1);
        fragment.setArguments(bundle);
        fragmentTransaction.replace(R.id.main_area, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    private  void saveLargeFile() {
        FileUtilities utilities = new FileUtilities(this);
        AlertDialog.Builder alert  = new AlertDialog.Builder(this);

        try {
           for (int i = 0; i <= 999; i++) {
                utilities.writeLineToFile("Writing line to file at index: " + i + System.getProperty ("line.separator"));
            }

            utilities.closeFile();

            alert.setMessage("All lines written to file");
            alert.setTitle("Success");
        } catch (Exception ex) {
            alert.setMessage("An error has occurred adding lines to file: " + ex.getMessage());
            alert.setTitle("Error");
        } finally {
            alert.setPositiveButton("OK", null);
            alert.setCancelable(true);
            alert.create().show();
        }
    }

    private void loadAndDisplayFile() {
        FragmentManager fragmentManager = getFragmentManager();
        FragmentTransaction fragmentTransaction = fragmentManager.beginTransaction();
        DisplayTextFileFragment fragment = new DisplayTextFileFragment();
        fragmentTransaction.replace(R.id.main_area, fragment);
        fragmentTransaction.addToBackStack(null);
        fragmentTransaction.commit();
    }

    @Override
    public void onFragmentInteraction(Uri uri) {

    }
}
