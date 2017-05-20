package com.tamasszadvari.perftest2_java;

import android.app.Activity;
import android.net.Uri;
import android.os.Bundle;
import android.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

public class DisplayTextFileFragment extends Fragment {

    private OnFragmentInteractionListener mListener;

    public static DisplayTextFileFragment newInstance() {
        DisplayTextFileFragment fragment = new DisplayTextFileFragment();
        Bundle args = new Bundle();
        fragment.setArguments(args);
        return fragment;
    }

    public DisplayTextFileFragment() {
        // Required empty public constructor
    }

    @Override
    public void onStart() {
        super.onStart();

        ListView lstFileContents = (ListView)this.getActivity().findViewById(R.id.lstFileContents);
        FileTableAdapter adapter = new FileTableAdapter(this.getActivity());
        lstFileContents.setAdapter(adapter);
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_display_text_file, container, false);
    }

    // TODO: Rename method, update argument and hook method into UI event
    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    interface OnFragmentInteractionListener {
        // TODO: Update argument type and name
        void onFragmentInteraction(Uri uri);
    }

}
