#define PINVOKELIB_EXPORTS
#ifdef PINVOKELIB_EXPORTS
#define PINVOKELIB_API __declspec(dllexport)
#else
#define PINVOKELIB_API __declspec(dllimport)
#endif

#include <strsafe.h>
#include <objbase.h>
#include <stdio.h>
#include <string>

#pragma comment(lib,"ole32.lib")

struct objf{
    int a;
    int b;
    int c;
    int idx[5];
    float conf[5];
    int r;
    char *nm;
    int sz1;
    char *nm2;
    int sz2;
};

typedef struct _MYARRAYSTRUCT{
    bool flag;
    int vals[3];
} MYARRAYSTRUCT;

typedef struct _MYSTRSTRUCT2{
    char* buffer;
    UINT size;
} MYSTRSTRUCT2;

#ifdef __cplusplus
extern "C"
{
#endif

PINVOKELIB_API bool testfunction(int size, objf **objs, int *objNum){
    *objNum = 2;
    *objs = (objf*)CoTaskMemAlloc(*objNum * sizeof(objf));
    if ( objs == NULL ){
        fprintf(stderr, "CoTaskMemAlloc error\n");
        return false;
    }
    for (int i=0; i<*objNum; i++){
        objf* pCurr = *objs+i;
        for(int j=0; j<5; j++){
            pCurr->idx[j]=j;
            pCurr->conf[j]=0.2;
        }
        {
            std::string s1 = "hello1";
            STRSAFE_LPCSTR teststr = s1.c_str();
            size_t len = 0;
            StringCchLengthA(teststr, STRSAFE_MAX_CCH, &len);
            len++;
            pCurr->sz1 = len;
            LPSTR buffer = (LPSTR)CoTaskMemAlloc(len);
            StringCchCopyA(buffer, len, teststr);
            pCurr->nm = (char *)buffer;
        }
        {
            std::string s2 = "abcd 11aa";
            STRSAFE_LPCSTR teststr = s2.c_str();
            size_t len = 0;
            StringCchLengthA(teststr, STRSAFE_MAX_CCH, &len);
            len++;
            pCurr->sz2 = len;
            LPSTR buffer2 = (LPSTR)CoTaskMemAlloc(len);
            StringCchCopyA(buffer2, len, teststr);
            pCurr->nm2 = (char *)buffer2;
        }
    }
    return true;
}

PINVOKELIB_API void TestOutArrayOfStructs( int* pSize, MYSTRSTRUCT2** ppStruct ){
    const int cArraySize = 5;
    *pSize = 0;
    *ppStruct = (MYSTRSTRUCT2*)CoTaskMemAlloc( cArraySize * sizeof( MYSTRSTRUCT2 ));

    if ( ppStruct != NULL ){
        MYSTRSTRUCT2* pCurStruct = *ppStruct;
        LPSTR buffer;
        *pSize = cArraySize;

        STRSAFE_LPCSTR teststr = "***";
        size_t len = 0;
        StringCchLengthA(teststr, STRSAFE_MAX_CCH, &len);
        len++;

        for ( int i = 0; i < cArraySize; i++, pCurStruct++ ){
            pCurStruct->size = len;
            buffer = (LPSTR)CoTaskMemAlloc(len);
            StringCchCopyA( buffer, len, teststr );
            pCurStruct->buffer = (char *)buffer;
        }
    }
}

PINVOKELIB_API void TestArrayInStruct( MYARRAYSTRUCT* pStruct ){
    pStruct->flag = true;
    pStruct->vals[0] += 100;
    pStruct->vals[1] += 100;
    pStruct->vals[2] += 100;
}

#ifdef __cplusplus
}
#endif