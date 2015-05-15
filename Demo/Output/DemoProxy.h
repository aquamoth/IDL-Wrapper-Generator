/*
 * Auto-generated code by C:\Users\maas\Workspace\Git\IDL-Wrapper-Generator\T4\BuildIdlProxies.tt
 * Created at 2015-05-15 14:13:54.
 */

#pragma once

#include "Demo.h"
#include "MxFrameworkInterfacesProxy.h"

class IMxSystemPathsProxy;
class IMxLogProxy;

class IMxCollectionProxy
{
public:
    IMxCollectionProxy() { }
    IMxCollectionProxy(const Ref<IMxCollection>& principal) : m_principal(principal) { }
    Ref<IMxCollection> ref() const { return m_principal; }
    bool operator<(const IMxCollectionProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxCollectionProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxCollectionProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxCollectionProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxCollection** operator&() { return m_principal.operator&(); }
    operator Ref<IMxCollection>() const { return m_principal; }
    ISafePtr<IMxCollection>* operator->() const { return m_principal.operator->(); }
    long get_Length() const;
    long get_Begin() const;
    long get_End() const;
    bool get_IsEmpty() const;

private:
    Ref<IMxCollection> m_principal;
};

class IMxQueueProxy
{
public:
    IMxQueueProxy() { }
    IMxQueueProxy(const Ref<IMxQueue>& principal) : m_principal(principal) { }
    Ref<IMxQueue> ref() const { return m_principal; }
    bool operator<(const IMxQueueProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxQueueProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxQueueProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxQueueProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxQueue** operator&() { return m_principal.operator&(); }
    operator Ref<IMxQueue>() const { return m_principal; }
    ISafePtr<IMxQueue>* operator->() const { return m_principal.operator->(); }
    long get_Size() const;
    long get_WriteCount() const;
    long get_ReadCount() const;
    void Close() const;

private:
    Ref<IMxQueue> m_principal;
};

class IMxFrameTrackerProxy
{
public:
    IMxFrameTrackerProxy() { }
    IMxFrameTrackerProxy(const Ref<IMxFrameTracker>& principal) : m_principal(principal) { }
    Ref<IMxFrameTracker> ref() const { return m_principal; }
    bool operator<(const IMxFrameTrackerProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxFrameTrackerProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxFrameTrackerProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxFrameTrackerProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxFrameTracker** operator&() { return m_principal.operator&(); }
    operator Ref<IMxFrameTracker>() const { return m_principal; }
    ISafePtr<IMxFrameTracker>* operator->() const { return m_principal.operator->(); }
    int Begin(REFIID frame, /*[in]*/const std::wstring& componentName, /*[in]*/const std::wstring& componentVersion) const;
    void Write(/*[in]*/int session, /*[in]*/const std::wstring& key, /*[in]*/const std::wstring& value) const;
    void Commit(/*[in]*/int session) const;
    void Rollback(/*[in]*/int session) const;

private:
    Ref<IMxFrameTracker> m_principal;
};

class IMxModuleIdProxy
{
public:
    IMxModuleIdProxy() { }
    IMxModuleIdProxy(const Ref<IMxModuleId>& principal) : m_principal(principal) { }
    Ref<IMxModuleId> ref() const { return m_principal; }
    bool operator<(const IMxModuleIdProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxModuleIdProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxModuleIdProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxModuleIdProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxModuleId** operator&() { return m_principal.operator&(); }
    operator Ref<IMxModuleId>() const { return m_principal; }
    ISafePtr<IMxModuleId>* operator->() const { return m_principal.operator->(); }
    std::wstring get_SourceComputer() const;
    std::wstring get_SourceProcess() const;
    std::wstring get_SourceModule() const;

private:
    Ref<IMxModuleId> m_principal;
};

class IMxLogValueProxy
{
public:
    IMxLogValueProxy() { }
    IMxLogValueProxy(const Ref<IMxLogValue>& principal) : m_principal(principal) { }
    Ref<IMxLogValue> ref() const { return m_principal; }
    bool operator<(const IMxLogValueProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxLogValueProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxLogValueProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxLogValueProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxLogValue** operator&() { return m_principal.operator&(); }
    operator Ref<IMxLogValue>() const { return m_principal; }
    ISafePtr<IMxLogValue>* operator->() const { return m_principal.operator->(); }
    GUID get_Id() const;
    __int64 get_LoggTime() const;
    IMxModuleIdProxy get_LoggerSource() const;
    std::wstring get_Description() const;
    ELogType get_LogType() const;
    void AddDescription(/*[in]*/const std::wstring& description) const;

private:
    Ref<IMxLogValue> m_principal;
};

class IMxLogErrorValueProxy
{
public:
    IMxLogErrorValueProxy() { }
    IMxLogErrorValueProxy(const Ref<IMxLogErrorValue>& principal) : m_principal(principal) { }
    Ref<IMxLogErrorValue> ref() const { return m_principal; }
    bool operator<(const IMxLogErrorValueProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxLogErrorValueProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxLogErrorValueProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxLogErrorValueProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxLogErrorValue** operator&() { return m_principal.operator&(); }
    operator Ref<IMxLogErrorValue>() const { return m_principal; }
    ISafePtr<IMxLogErrorValue>* operator->() const { return m_principal.operator->(); }
    std::wstring get_SourceFileLine() const;
    long get_ErrorCode() const;
    ESeverity get_Severity() const;

private:
    Ref<IMxLogErrorValue> m_principal;
};

class IMxLogEventValueProxy
{
public:
    IMxLogEventValueProxy() { }
    IMxLogEventValueProxy(const Ref<IMxLogEventValue>& principal) : m_principal(principal) { }
    Ref<IMxLogEventValue> ref() const { return m_principal; }
    bool operator<(const IMxLogEventValueProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxLogEventValueProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxLogEventValueProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxLogEventValueProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxLogEventValue** operator&() { return m_principal.operator&(); }
    operator Ref<IMxLogEventValue>() const { return m_principal; }
    ISafePtr<IMxLogEventValue>* operator->() const { return m_principal.operator->(); }
    long get_EventCode() const;

private:
    Ref<IMxLogEventValue> m_principal;
};

class IMxLogTraceValueProxy
{
public:
    IMxLogTraceValueProxy() { }
    IMxLogTraceValueProxy(const Ref<IMxLogTraceValue>& principal) : m_principal(principal) { }
    Ref<IMxLogTraceValue> ref() const { return m_principal; }
    bool operator<(const IMxLogTraceValueProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxLogTraceValueProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxLogTraceValueProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxLogTraceValueProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxLogTraceValue** operator&() { return m_principal.operator&(); }
    operator Ref<IMxLogTraceValue>() const { return m_principal; }
    ISafePtr<IMxLogTraceValue>* operator->() const { return m_principal.operator->(); }
    ETraceLevel get_TraceLevel() const;
    std::wstring get_SourceFileLine() const;

private:
    Ref<IMxLogTraceValue> m_principal;
};

class IMxLogProxy
{
public:
    IMxLogProxy() { }
    IMxLogProxy(const Ref<IMxLog>& principal) : m_principal(principal) { }
    Ref<IMxLog> ref() const { return m_principal; }
    bool operator<(const IMxLogProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxLogProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxLogProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxLogProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxLog** operator&() { return m_principal.operator&(); }
    operator Ref<IMxLog>() const { return m_principal; }
    ISafePtr<IMxLog>* operator->() const { return m_principal.operator->(); }
    void LogError(/*[in]*/const std::wstring& pAppName, /*[in]*/long errorCode, /*[in]*/ESeverity severity, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) const;
    void LogWarning(/*[in]*/const std::wstring& pAppName, /*[in]*/long code, /*[in]*/ESeverity severity, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) const;
    void LogTrace(/*[in]*/const std::wstring& pAppName, /*[in]*/ETraceLevel TraceLevel, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) const;

private:
    Ref<IMxLog> m_principal;
};

class IMxSystemPathsProxy
{
public:
    IMxSystemPathsProxy() { }
    IMxSystemPathsProxy(const Ref<IMxSystemPaths>& principal) : m_principal(principal) { }
    Ref<IMxSystemPaths> ref() const { return m_principal; }
    bool operator<(const IMxSystemPathsProxy& rhs) const { return m_principal.operator<(rhs.m_principal); }
    bool operator>(const IMxSystemPathsProxy& rhs) const { return m_principal.operator>(rhs.m_principal); }
    bool operator!=(const IMxSystemPathsProxy& rhs) const { return m_principal.operator!=(rhs.m_principal); }
    bool operator==(const IMxSystemPathsProxy& rhs) const { return m_principal.operator==(rhs.m_principal); }
    IMxSystemPaths** operator&() { return m_principal.operator&(); }
    operator Ref<IMxSystemPaths>() const { return m_principal; }
    ISafePtr<IMxSystemPaths>* operator->() const { return m_principal.operator->(); }
    std::wstring get_DataRootDirectory() const;
    std::wstring get_LogFileDirectory() const;
    std::wstring get_ConfigDirectory() const;
    std::wstring get_ConfigBinaryDataDirectory() const;
    std::wstring get_BinaryDataDirectory() const;
    std::wstring get_FrameValueDirectory() const;
    std::wstring get_LineCalibrationDirectory() const;
    std::wstring get_ReferenceCalibrationDirectory() const;
    std::wstring get_VolumeCalibrationDirectory() const;
    std::wstring get_StatisticsDirectory() const;
    std::wstring get_EventsDirectory() const;
    std::wstring get_MeasurementsDirectory() const;

private:
    Ref<IMxSystemPaths> m_principal;
};

long IMxCollectionProxy::get_Length() const
{
    long pVal;
    CHECKHR_T( m_principal->get_Length( &pVal ) );
    return pVal;
}

long IMxCollectionProxy::get_Begin() const
{
    long pVal;
    CHECKHR_T( m_principal->get_Begin( &pVal ) );
    return pVal;
}

long IMxCollectionProxy::get_End() const
{
    long pVal;
    CHECKHR_T( m_principal->get_End( &pVal ) );
    return pVal;
}

bool IMxCollectionProxy::get_IsEmpty() const
{
    VARIANT_BOOL pVal;
    CHECKHR_T( m_principal->get_IsEmpty( &pVal ) );
    return VARIANT_FALSE != pVal;
}

long IMxQueueProxy::get_Size() const
{
    long pVal;
    CHECKHR_T( m_principal->get_Size( &pVal ) );
    return pVal;
}

long IMxQueueProxy::get_WriteCount() const
{
    long pVal;
    CHECKHR_T( m_principal->get_WriteCount( &pVal ) );
    return pVal;
}

long IMxQueueProxy::get_ReadCount() const
{
    long pVal;
    CHECKHR_T( m_principal->get_ReadCount( &pVal ) );
    return pVal;
}

void IMxQueueProxy::Close() const
{
    CHECKHR_T( m_principal->Close(  ) );
}

int IMxFrameTrackerProxy::Begin(REFIID frame, /*[in]*/const std::wstring& componentName, /*[in]*/const std::wstring& componentVersion) const
{
    int pKey;
    CHECKHR_T( m_principal->Begin( frame, CMxBstr::in(componentName), CMxBstr::in(componentVersion), &pKey ) );
    return pKey;
}

void IMxFrameTrackerProxy::Write(/*[in]*/int session, /*[in]*/const std::wstring& key, /*[in]*/const std::wstring& value) const
{
    CHECKHR_T( m_principal->Write( session, CMxBstr::in(key), CMxBstr::in(value) ) );
}

void IMxFrameTrackerProxy::Commit(/*[in]*/int session) const
{
    CHECKHR_T( m_principal->Commit( session ) );
}

void IMxFrameTrackerProxy::Rollback(/*[in]*/int session) const
{
    CHECKHR_T( m_principal->Rollback( session ) );
}

std::wstring IMxModuleIdProxy::get_SourceComputer() const
{
    std::wstring pComputer;
    CHECKHR_T( m_principal->get_SourceComputer( CMxBstr::out(pComputer) ) );
    return pComputer;
}

std::wstring IMxModuleIdProxy::get_SourceProcess() const
{
    std::wstring pProcess;
    CHECKHR_T( m_principal->get_SourceProcess( CMxBstr::out(pProcess) ) );
    return pProcess;
}

std::wstring IMxModuleIdProxy::get_SourceModule() const
{
    std::wstring pModule;
    CHECKHR_T( m_principal->get_SourceModule( CMxBstr::out(pModule) ) );
    return pModule;
}

GUID IMxLogValueProxy::get_Id() const
{
    GUID pid;
    CHECKHR_T( m_principal->get_Id( &pid ) );
    return pid;
}

__int64 IMxLogValueProxy::get_LoggTime() const
{
    __int64 time;
    CHECKHR_T( m_principal->get_LoggTime( &time ) );
    return time;
}

IMxModuleIdProxy IMxLogValueProxy::get_LoggerSource() const
{
    Ref<IMxModuleId> pSource;
    CHECKHR_T( m_principal->get_LoggerSource( &pSource ) );
    return IMxModuleIdProxy(pSource);
}

std::wstring IMxLogValueProxy::get_Description() const
{
    std::wstring description;
    CHECKHR_T( m_principal->get_Description( CMxBstr::out(description) ) );
    return description;
}

ELogType IMxLogValueProxy::get_LogType() const
{
    ELogType type;
    CHECKHR_T( m_principal->get_LogType( &type ) );
    return type;
}

void IMxLogValueProxy::AddDescription(/*[in]*/const std::wstring& description) const
{
    CHECKHR_T( m_principal->AddDescription( CMxBstr::in(description) ) );
}

std::wstring IMxLogErrorValueProxy::get_SourceFileLine() const
{
    std::wstring pFile;
    CHECKHR_T( m_principal->get_SourceFileLine( CMxBstr::out(pFile) ) );
    return pFile;
}

long IMxLogErrorValueProxy::get_ErrorCode() const
{
    long code;
    CHECKHR_T( m_principal->get_ErrorCode( &code ) );
    return code;
}

ESeverity IMxLogErrorValueProxy::get_Severity() const
{
    ESeverity code;
    CHECKHR_T( m_principal->get_Severity( &code ) );
    return code;
}

long IMxLogEventValueProxy::get_EventCode() const
{
    long eventCode;
    CHECKHR_T( m_principal->get_EventCode( &eventCode ) );
    return eventCode;
}

ETraceLevel IMxLogTraceValueProxy::get_TraceLevel() const
{
    ETraceLevel level;
    CHECKHR_T( m_principal->get_TraceLevel( &level ) );
    return level;
}

std::wstring IMxLogTraceValueProxy::get_SourceFileLine() const
{
    std::wstring pFile;
    CHECKHR_T( m_principal->get_SourceFileLine( CMxBstr::out(pFile) ) );
    return pFile;
}

void IMxLogProxy::LogError(/*[in]*/const std::wstring& pAppName, /*[in]*/long errorCode, /*[in]*/ESeverity severity, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) const
{
    CHECKHR_T( m_principal->LogError( CMxBstr::in(pAppName), errorCode, severity, CMxBstr::in(pFileLine), CMxBstr::in(pFunction), CMxBstr::in(pDescription) ) );
}

void IMxLogProxy::LogWarning(/*[in]*/const std::wstring& pAppName, /*[in]*/long code, /*[in]*/ESeverity severity, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) const
{
    CHECKHR_T( m_principal->LogWarning( CMxBstr::in(pAppName), code, severity, CMxBstr::in(pFileLine), CMxBstr::in(pFunction), CMxBstr::in(pDescription) ) );
}

void IMxLogProxy::LogTrace(/*[in]*/const std::wstring& pAppName, /*[in]*/ETraceLevel TraceLevel, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) const
{
    CHECKHR_T( m_principal->LogTrace( CMxBstr::in(pAppName), TraceLevel, CMxBstr::in(pFileLine), CMxBstr::in(pFunction), CMxBstr::in(pDescription) ) );
}

std::wstring IMxSystemPathsProxy::get_DataRootDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_DataRootDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_LogFileDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_LogFileDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_ConfigDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_ConfigDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_ConfigBinaryDataDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_ConfigBinaryDataDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_BinaryDataDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_BinaryDataDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_FrameValueDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_FrameValueDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_LineCalibrationDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_LineCalibrationDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_ReferenceCalibrationDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_ReferenceCalibrationDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_VolumeCalibrationDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_VolumeCalibrationDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_StatisticsDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_StatisticsDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_EventsDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_EventsDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

std::wstring IMxSystemPathsProxy::get_MeasurementsDirectory() const
{
    std::wstring pVal;
    CHECKHR_T( m_principal->get_MeasurementsDirectory( CMxBstr::out(pVal) ) );
    return pVal;
}

