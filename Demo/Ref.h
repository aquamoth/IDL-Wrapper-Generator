
#pragma once

template <class T> class Ref;

template <class T>
class IMakePtr
{
	friend class Ref<T>;
protected:
	virtual T* Create() const = 0;
};

template <class T>
class ISafePtr : public T
{
private:
	ISafePtr();
	virtual unsigned long STDMETHODCALLTYPE AddRef() = 0;
	virtual unsigned long STDMETHODCALLTYPE Release() = 0;
};

template <class T>
class Ptr
{
private:
	T* m_pPtr;

	Ptr<T>& Set(T* m_pRhs)
	{
		Release();
		m_pPtr = m_pRhs;
		return *this;
	}

	Ptr<T>& Set(const Ptr<T>& rhs)
	{
		if (&rhs == this)
		{
			return *this;
		}
		return Set(rhs.m_pPtr);
	}

public:
	T* ptr() const
	{
		CHECKBOOL_M_T(!IsNull(), E_POINTER, L"Null reference");
		return m_pPtr;
	}

	static Ptr<T> Null()
	{
		static Ptr<T> null();
		return null;
	}

	Ptr() :
		m_pPtr(nullptr)
	{
	}

	Ptr(T* m_pItf) :
		m_pPtr(nullptr)
	{
		Set(m_pItf);
	}

	Ptr(const Ptr<T>& rhs) :
		m_pPtr(nullptr)
	{
		Set(rhs.ptr());
	}

	virtual ~Ptr()
	{
		Release();
	}

	bool IsNull() const
	{
		return !m_pPtr;
	}

	bool IsLess(const Ptr<T>& rhs) const
	{
		return m_pPtr < rhs.m_pPtr;
	}

	bool IsGreater(const Ptr<T>& rhs) const
	{
		return m_pPtr > rhs.m_pPtr;
	}

	bool IsEq(const Ptr<T>& rhs) const
	{
		return m_pPtr == rhs.m_pPtr;
	}

	T** operator&()
	{
		Release();
		return &m_pPtr;
	}

	Ptr<T>& operator=(const Ptr<T>& rhs)
	{
		return Set(rhs.ptr());
	}

	ISafePtr<T>* operator->() const
	{
		return reinterpret_cast<ISafePtr<T> *>(ptr());
	}

	bool operator!() const
	{
		return IsNull();
	}

	bool operator<(const Ref<T>& rhs) const
	{
		return IsLess(rhs);
	}

	bool operator>(const Ref<T>& rhs) const
	{
		return IsGreater(rhs);
	}

	bool operator!=(const Ref<T>& rhs) const
	{
		return !IsEq(rhs);
	}

	bool operator==(const Ref<T>& rhs) const
	{
		return IsEq(rhs);
	}

	void Release() throw()
	{
		if (IsNull())
		{
			return;
		}
		m_pPtr = nullptr;
	}

	HRESULT CopyTo(T** ppT) throw()
	{
		CHECKPOINTER_R(ppT);

		try
		{
			if (IsNull())
			{
				return S_OK;
			}
			*ppT = ptr();
			return S_OK;
		}
		catch (const HRESULT& hr) { return hr; }
		catch (const std::bad_alloc&) { return E_OUTOFMEMORY; }
		catch (...) { return E_FAIL; }
	}
};

template<class T>
class Ref
{
public:
	Ref() :
		m_pPtr(nullptr)
	{
	}

	Ref(T* m_pItf) :
		m_pPtr(nullptr)
	{
		Set(m_pItf);
	}

	Ref(const Ref<T>& rhs) :
		m_pPtr(nullptr)
	{
		Set(rhs);
	}

	Ref(const IMakePtr<T>& factory) :
		m_pPtr(nullptr)
	{
		Set(factory.Create());
	}

	virtual ~Ref()
	{
		Release();
	}

	T* ptr() const
	{
		if (IsNull()) throw E_POINTER;
		return m_pPtr;
	}

	T* rawptr() const
	{
		return m_pPtr;
	}

	static Ref<T> Null()
	{
		static Ref<T> null();
		return null;
	}

	bool IsNull() const
	{
		return !m_pPtr;
	}

	bool IsLess(const Ref<T>& rhs) const
	{
		return m_pPtr < rhs.m_pPtr;
	}

	bool IsGreater(const Ref<T>& rhs) const
	{
		return m_pPtr > rhs.m_pPtr;
	}

	bool IsEq(const Ref<T>& rhs) const
	{
		return m_pPtr == rhs.m_pPtr;
	}

	T** operator&()
	{
		Release();
		return &m_pPtr;
	}

	Ref<T>& operator=(const IMakePtr<T>& rhs)
	{
		return Set(rhs.Create());
	}

	Ref<T>& operator=(const Ref<T>& rhs)
	{
		return Set(rhs);
	}

	ISafePtr<T>* operator->() const
	{
		return reinterpret_cast<ISafePtr<T> *>(ptr());
	}

	bool operator!() const
	{
		return IsNull();
	}

	bool operator<(const Ref<T>& rhs) const
	{
		return IsLess(rhs);
	}

	bool operator>(const Ref<T>& rhs) const
	{
		return IsGreater(rhs);
	}

	bool operator!=(const Ref<T>& rhs) const
	{
		return !IsEq(rhs);
	}

	bool operator==(const Ref<T>& rhs) const
	{
		return IsEq(rhs);
	}

	void Release() const throw()
	{
		if (IsNull())
		{
			return;
		}
		T* pTemp = m_pPtr;
		m_pPtr = nullptr;
		pTemp->Release();
	}

	HRESULT CopyTo(T** ppT) const throw()
	{
		CHECKPOINTER_R(ppT);

		try
		{
			if (IsNull())
			{
				return S_OK;
			}
			*ppT = ptr();
			AddRef();

			return S_OK;
		}
		catch (const HRESULT& hr) { return hr; }
		catch (const std::bad_alloc&) { return E_OUTOFMEMORY; }
		catch (...) { return E_FAIL; }
	}

	template <class Q>
	HRESULT QueryInterface(Q** pp) const throw()
	{
		return QueryInterface(__uuidof(Q), (void**)pp);
	}

	HRESULT QueryInterface(const IID& riid, void** ppV) const throw()
	{
		CHECKPOINTER_R(ppV);

		try
		{
			CHECKBOOL_M_R(!IsNull(), E_POINTER, L"Null reference");

			return ptr()->QueryInterface(riid, ppV);
		}
		catch (const HRESULT& hr) { return hr; }
		catch (const std::bad_alloc&) { return E_OUTOFMEMORY; }
		catch (...) { return E_FAIL; }
	}

private:
	void AddRef() const
	{
		if (!IsNull())
		{
			ptr()->AddRef();
		}
	}

	Ref<T>& Set(T* m_pRhs)
	{
		Release();
		m_pPtr = m_pRhs;
		AddRef();
		return *this;
	}

	Ref<T>& Set(const Ref<T>& rhs)
	{
		if (&rhs == this)
		{
			return *this;
		}
		return Set(rhs.m_pPtr);
	}

	mutable T* m_pPtr;
};

template<class T>
class RefWrapper
{
public:
	RefWrapper(Ref<T> ref) :
		m_ref(ref)
	{
	}

	T* ptr() const
	{
		return m_ref.ptr();
	}

	Ref<T> ref() const
	{
		return m_ref;
	}

private:
	Ref<T> m_ref;
};
