#include <stdio.h>
#include <windows.h>

struct objf{
    int idx[5];
    float conf[5];
    int r;
};

extern "C" bool _declspec(dllexport) _stdcall testfunction(int size, objf **objs, int *objNum){
    *objNum = 3;
    *objs = (objf*)CoTaskMemAlloc(*objNum * sizeof(objf));
    for (int i=0; i<*objNum; i++){
        objf* pCurr = *objs+i;
        for(int j=0; j<5; j++){
            pCurr->idx[j]=j;
            pCurr->conf[j]=0.2;
        }
        pCurr->r = 33;
    }
    return true;
}
