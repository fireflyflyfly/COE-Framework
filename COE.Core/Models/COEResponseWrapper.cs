using System.Collections.Generic;

namespace COE.Core.Models
{
    public class COEResponseWrapper<T>
    {
        public bool HasError { get; set; }
        public string MessageType { get; set; }
        public string MsgText { get; set; }
        public bool Unwrap { get; set; }
        public List<T> Data { get; set; }
    }
}