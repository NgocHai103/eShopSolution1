using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
   public class ApiSuccessResult<T>:ApiResult<T>
    {
        public ApiSuccessResult(T _resultObj)
        {
            IsSuccessed = true;
            ResultObj = _resultObj;
        }
        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }
    }
}
