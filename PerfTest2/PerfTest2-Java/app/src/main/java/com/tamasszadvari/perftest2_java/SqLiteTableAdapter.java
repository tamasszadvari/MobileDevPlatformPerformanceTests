package com.tamasszadvari.perftest2_java;

import android.content.Context;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import java.util.ArrayList;

public class SqLiteTableAdapter extends BaseAdapter {
    private Context context;
    private ArrayList<String> records;
    private SqLiteDisplayType displayType;

    public SqLiteTableAdapter(Context context, SqLiteDisplayType displayType)
    {
        this.context = context;
        this.displayType = displayType;
        loadRecords();
    }

    private void loadRecords() {
        SqLiteUtilities utilities  = new SqLiteUtilities(this.context);
        try {
            utilities.openConnection();
            records = (displayType == SqLiteDisplayType.ShowAll)
                    ? utilities.getAllRecords()
                    : utilities.getRecordsWith1();

            utilities.closeConnection();
        } catch (Exception ex) {
            Log.e("PerfTest2_Java", "exception", ex);
        }
    }

    @Override
    public int getCount() {
        return records.size();
    }

    @Override
    public Object getItem(int item) {
        // TODO Auto-generated method stub
        return records.get(item);
    }

    @Override
    public long getItemId(int position) {
        // TODO Auto-generated method stub
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup viewGroup) {
        if(convertView == null)
        {
            LayoutInflater inflater = (LayoutInflater)context.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            convertView = inflater.inflate(android.R.layout.simple_list_item_1, null);
        }

        TextView txtItem = (TextView)convertView.findViewById(android.R.id.text1);
        txtItem.setText(records.get(position));

        return convertView;
    }
}