using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FMODUnity;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance => null;

    public TimelineInfo timelineInfo;
    private GCHandle _timelineHandle;

    [Header("Song To Play")]
    [SerializeField] private FMODUnity.EventReference eventReference;
    
    //-----------------
    
    private FMOD.Studio.EventInstance _eventInstance;
    private static FMOD.Studio.EVENT_CALLBACK _beatCallback;

    public delegate void BeatEventDelegate();
    public static event BeatEventDelegate beatUpdated;

    public delegate void MarkerListenerDelegate();
    public static event MarkerListenerDelegate markerUpdated;
    
    public static int lastBeat = 0;
    public static string lastMarkerString = null;

    [StructLayout(LayoutKind.Sequential)]
    public struct TimelineInfo
    {
        public static int currentBeat = 0;
        public static FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    private void Awake()
    {
        if (!eventReference.IsNull)
        {
            _eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstance.start();
        }
        else
        {
#if UNITY_EDITOR
            throw new Exception("FMOD event is fucking null!");
#endif
        }
    }

    private void Start()
    {
        if (!eventReference.IsNull)
        {
            timelineInfo = new TimelineInfo();
            _beatCallback = new FMOD.Studio.EVENT_CALLBACK(_beatEventCallback);
            _timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
            _eventInstance.setUserData(GCHandle.ToIntPtr(_timelineHandle));
            _eventInstance.setCallback(_beatCallback,
                FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        }
    }

    private void Update()
    {
        if (lastMarkerString != TimelineInfo.lastMarker)
        {
            lastMarkerString = TimelineInfo.lastMarker;

            if (markerUpdated != null)
            {
                markerUpdated();
            }
        }

        if (lastBeat != TimelineInfo.currentBeat)
        {
            lastBeat = TimelineInfo.currentBeat;
            if (beatUpdated != null)
            {
                beatUpdated();
            }
        }
    }

    private void OnDestroy()
    {
        _eventInstance.setUserData(IntPtr.Zero);
        _eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _eventInstance.release();
        _timelineHandle.Free();
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        GUILayout.Box($"Current Beat= {TimelineInfo.currentBeat}, {(string)TimelineInfo.lastMarker}");
    }
#endif

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    private static FMOD.RESULT _beatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr,
        IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr!=IntPtr.Zero)
        {
            GCHandle timelineHandle=GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                {
                    var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr,
                        typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                    TimelineInfo.currentBeat = parameter.beat;
                }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                {
                    var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr,
                        typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                    TimelineInfo.lastMarker = parameter.name;
                }
                    break;
            }
        }

        return FMOD.RESULT.OK;
    }
}
