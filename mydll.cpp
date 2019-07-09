#include <stdio.h>
#include <windows.h>

struct objf{
    int idx[5];
    float conf[5];
    int r;
    char nm[256];
    char nm2[1280];
};

extern "C" bool _declspec(dllexport) _stdcall testfunction(int size, objf **objs, int *objNum){
    *objNum = 3;
    *objs = (objf*)CoTaskMemAlloc(*objNum * sizeof(objf));
    printf("int size:%zd\n", sizeof(int));
    printf("float size:%zd\n", sizeof(float));

    for (int i=0; i<*objNum; i++){
        objf* pCurr = *objs+i;
        memset(pCurr->nm,0,256);
        for(int j=0; j<5; j++){
            pCurr->idx[j]=j;
            pCurr->conf[j]=0.2;
        }
        strncpy(pCurr->nm,"hello",5);
        pCurr->r = 33;
    }
    return true;
}
