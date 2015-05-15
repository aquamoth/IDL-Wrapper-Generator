/*
 * Auto-generated code by C:\Users\maas\Workspace\Git\IDL-Wrapper-Generator\T4\BuildIdlBaseTemplates.tt
 * Created at 2015-05-15 14:13:53.
 */

#pragma once

#include "DemoProxy.h"
#include "MxFrameworkInterfacesProxy.h"

template<class T>
class IMxCollectionBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_Length(/*[out,retval]*/long* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            long pValValue = _get_Length();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_Begin(/*[out,retval]*/long* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            long pValValue = _get_Begin();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_End(/*[out,retval]*/long* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            long pValValue = _get_End();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_IsEmpty(/*[out,retval]*/VARIANT_BOOL* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            bool pValValue = _get_IsEmpty();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual long _get_Length() = 0;
    virtual long _get_Begin() = 0;
    virtual long _get_End() = 0;
    virtual bool _get_IsEmpty() = 0;
};

template<class T>
class IMxQueueBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_Size(/*[out,ref,retval]*/long* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            long pValValue = _get_Size();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_WriteCount(/*[out,ref,retval]*/long* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            long pValValue = _get_WriteCount();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_ReadCount(/*[out,ref,retval]*/long* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            long pValValue = _get_ReadCount();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE Close()
    {
        try
        {
            close();
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual long _get_Size() = 0;
    virtual long _get_WriteCount() = 0;
    virtual long _get_ReadCount() = 0;
    virtual void close() = 0;
};

template<class T>
class IMxFrameTrackerBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE Begin(/*[]*/REFIID frame, /*[in]*/BSTR componentName, /*[in]*/BSTR componentVersion, /*[out,retval]*/int* pKey)
    {
        CHECKPOINTER_R(componentName);
        CHECKPOINTER_R(componentVersion);
        CHECKPOINTER_R(pKey);
        try
        {
            int pKeyValue = begin(frame, componentName, componentVersion);
            CHECKHR_R(Export(pKeyValue, pKey));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE Write(/*[in]*/int session, /*[in]*/BSTR key, /*[in]*/BSTR value)
    {
        CHECKPOINTER_R(key);
        CHECKPOINTER_R(value);
        try
        {
            write(session, key, value);
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE Commit(/*[in]*/int session)
    {
        try
        {
            commit(session);
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE Rollback(/*[in]*/int session)
    {
        try
        {
            rollback(session);
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual int begin(REFIID frame, /*[in]*/const std::wstring& componentName, /*[in]*/const std::wstring& componentVersion) = 0;
    virtual void write(/*[in]*/int session, /*[in]*/const std::wstring& key, /*[in]*/const std::wstring& value) = 0;
    virtual void commit(/*[in]*/int session) = 0;
    virtual void rollback(/*[in]*/int session) = 0;
};

template<class T>
class IMxModuleIdBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_SourceComputer(/*[out,ref,retval]*/BSTR* pComputer)
    {
        CHECKPOINTER_R(pComputer);
        try
        {
            std::wstring pComputerValue = _get_SourceComputer();
            CHECKHR_R(Export(pComputerValue, pComputer));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_SourceProcess(/*[out,ref,retval]*/BSTR* pProcess)
    {
        CHECKPOINTER_R(pProcess);
        try
        {
            std::wstring pProcessValue = _get_SourceProcess();
            CHECKHR_R(Export(pProcessValue, pProcess));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_SourceModule(/*[out,ref,retval]*/BSTR* pModule)
    {
        CHECKPOINTER_R(pModule);
        try
        {
            std::wstring pModuleValue = _get_SourceModule();
            CHECKHR_R(Export(pModuleValue, pModule));
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual std::wstring _get_SourceComputer() = 0;
    virtual std::wstring _get_SourceProcess() = 0;
    virtual std::wstring _get_SourceModule() = 0;
};

template<class T>
class IMxLogValueBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_Id(/*[out,ref,retval]*/GUID* pid)
    {
        CHECKPOINTER_R(pid);
        try
        {
            GUID pidValue = _get_Id();
            CHECKHR_R(Export(pidValue, pid));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_LoggTime(/*[out,ref,retval]*/__int64* time)
    {
        CHECKPOINTER_R(time);
        try
        {
            __int64 timeValue = _get_LoggTime();
            CHECKHR_R(Export(timeValue, time));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_LoggerSource(/*[out,ref,retval]*/IMxModuleId** pSource)
    {
        CHECKPOINTER_R(pSource);
        try
        {
            IMxModuleIdProxy pSourceValue = _get_LoggerSource();
            CHECKHR_R(pSourceValue.ref().CopyTo(pSource));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_Description(/*[out,ref,retval]*/BSTR* description)
    {
        CHECKPOINTER_R(description);
        try
        {
            std::wstring descriptionValue = _get_Description();
            CHECKHR_R(Export(descriptionValue, description));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_LogType(/*[out,ref,retval]*/ELogType* type)
    {
        CHECKPOINTER_R(type);
        try
        {
            ELogType typeValue = _get_LogType();
            CHECKHR_R(Export(typeValue, type));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE AddDescription(/*[in]*/BSTR description)
    {
        CHECKPOINTER_R(description);
        try
        {
            addDescription(description);
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual GUID _get_Id() = 0;
    virtual __int64 _get_LoggTime() = 0;
    virtual IMxModuleIdProxy _get_LoggerSource() = 0;
    virtual std::wstring _get_Description() = 0;
    virtual ELogType _get_LogType() = 0;
    virtual void addDescription(/*[in]*/const std::wstring& description) = 0;
};

template<class T>
class IMxLogErrorValueBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_SourceFileLine(/*[out,ref,retval]*/BSTR* pFile)
    {
        CHECKPOINTER_R(pFile);
        try
        {
            std::wstring pFileValue = _get_SourceFileLine();
            CHECKHR_R(Export(pFileValue, pFile));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_ErrorCode(/*[out,ref,retval]*/long* code)
    {
        CHECKPOINTER_R(code);
        try
        {
            long codeValue = _get_ErrorCode();
            CHECKHR_R(Export(codeValue, code));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_Severity(/*[out,ref,retval]*/ESeverity* code)
    {
        CHECKPOINTER_R(code);
        try
        {
            ESeverity codeValue = _get_Severity();
            CHECKHR_R(Export(codeValue, code));
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual std::wstring _get_SourceFileLine() = 0;
    virtual long _get_ErrorCode() = 0;
    virtual ESeverity _get_Severity() = 0;
};

template<class T>
class IMxLogEventValueBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_EventCode(/*[out,ref,retval]*/long* eventCode)
    {
        CHECKPOINTER_R(eventCode);
        try
        {
            long eventCodeValue = _get_EventCode();
            CHECKHR_R(Export(eventCodeValue, eventCode));
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual long _get_EventCode() = 0;
};

template<class T>
class IMxLogTraceValueBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_TraceLevel(/*[out,ref,retval]*/ETraceLevel* level)
    {
        CHECKPOINTER_R(level);
        try
        {
            ETraceLevel levelValue = _get_TraceLevel();
            CHECKHR_R(Export(levelValue, level));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_SourceFileLine(/*[out,ref,retval]*/BSTR* pFile)
    {
        CHECKPOINTER_R(pFile);
        try
        {
            std::wstring pFileValue = _get_SourceFileLine();
            CHECKHR_R(Export(pFileValue, pFile));
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual ETraceLevel _get_TraceLevel() = 0;
    virtual std::wstring _get_SourceFileLine() = 0;
};

template<class T>
class IMxLogBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE LogError(/*[in]*/BSTR pAppName, /*[in]*/long errorCode, /*[in]*/ESeverity severity, /*[in]*/BSTR pFileLine, /*[in]*/BSTR pFunction, /*[in]*/BSTR pDescription)
    {
        CHECKPOINTER_R(pAppName);
        CHECKPOINTER_R(pFileLine);
        CHECKPOINTER_R(pFunction);
        CHECKPOINTER_R(pDescription);
        try
        {
            logError(pAppName, errorCode, severity, pFileLine, pFunction, pDescription);
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE LogWarning(/*[in]*/BSTR pAppName, /*[in]*/long code, /*[in]*/ESeverity severity, /*[in]*/BSTR pFileLine, /*[in]*/BSTR pFunction, /*[in]*/BSTR pDescription)
    {
        CHECKPOINTER_R(pAppName);
        CHECKPOINTER_R(pFileLine);
        CHECKPOINTER_R(pFunction);
        CHECKPOINTER_R(pDescription);
        try
        {
            logWarning(pAppName, code, severity, pFileLine, pFunction, pDescription);
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE LogTrace(/*[in]*/BSTR pAppName, /*[in]*/ETraceLevel TraceLevel, /*[in]*/BSTR pFileLine, /*[in]*/BSTR pFunction, /*[in]*/BSTR pDescription)
    {
        CHECKPOINTER_R(pAppName);
        CHECKPOINTER_R(pFileLine);
        CHECKPOINTER_R(pFunction);
        CHECKPOINTER_R(pDescription);
        try
        {
            logTrace(pAppName, TraceLevel, pFileLine, pFunction, pDescription);
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual void logError(/*[in]*/const std::wstring& pAppName, /*[in]*/long errorCode, /*[in]*/ESeverity severity, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) = 0;
    virtual void logWarning(/*[in]*/const std::wstring& pAppName, /*[in]*/long code, /*[in]*/ESeverity severity, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) = 0;
    virtual void logTrace(/*[in]*/const std::wstring& pAppName, /*[in]*/ETraceLevel TraceLevel, /*[in]*/const std::wstring& pFileLine, /*[in]*/const std::wstring& pFunction, /*[in]*/const std::wstring& pDescription) = 0;
};

template<class T>
class IMxSystemPathsBase : public T
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_DataRootDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_DataRootDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_LogFileDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_LogFileDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_ConfigDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_ConfigDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_ConfigBinaryDataDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_ConfigBinaryDataDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_BinaryDataDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_BinaryDataDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_FrameValueDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_FrameValueDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_LineCalibrationDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_LineCalibrationDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_ReferenceCalibrationDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_ReferenceCalibrationDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_VolumeCalibrationDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_VolumeCalibrationDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_StatisticsDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_StatisticsDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_EventsDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_EventsDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }
    virtual HRESULT STDMETHODCALLTYPE get_MeasurementsDirectory(/*[out,ref,retval]*/BSTR* pVal)
    {
        CHECKPOINTER_R(pVal);
        try
        {
            std::wstring pValValue = _get_MeasurementsDirectory();
            CHECKHR_R(Export(pValValue, pVal));
            return S_OK;
        }
        mx_catch;
    }

protected:
    virtual std::wstring _get_DataRootDirectory() = 0;
    virtual std::wstring _get_LogFileDirectory() = 0;
    virtual std::wstring _get_ConfigDirectory() = 0;
    virtual std::wstring _get_ConfigBinaryDataDirectory() = 0;
    virtual std::wstring _get_BinaryDataDirectory() = 0;
    virtual std::wstring _get_FrameValueDirectory() = 0;
    virtual std::wstring _get_LineCalibrationDirectory() = 0;
    virtual std::wstring _get_ReferenceCalibrationDirectory() = 0;
    virtual std::wstring _get_VolumeCalibrationDirectory() = 0;
    virtual std::wstring _get_StatisticsDirectory() = 0;
    virtual std::wstring _get_EventsDirectory() = 0;
    virtual std::wstring _get_MeasurementsDirectory() = 0;
};

