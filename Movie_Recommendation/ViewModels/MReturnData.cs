using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.ViewModels
{
    public class MReturnData<T>
    {
        public MReturnData()
        {
            Success = true;
        }
        public bool Success { get; set; }
        public String Message { get; set; }
        public T Data { get; set; }
    }
    public class Error
    {
     public string Message { get; set; }
     public int Code { get; set; }
        public Error( Exception ex)
        {
            Code = 500;
            Message = ex.Message;
        }
    }
}
