//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 数据库课程设计作业.domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class Yunzhuan
    {
        public string tname { get; set; }
        public string snname { get; set; }
        public Nullable<System.TimeSpan> yzarrive { get; set; }
        public Nullable<System.TimeSpan> yzleave { get; set; }
    
        public virtual Station Station { get; set; }
        public virtual Train Train { get; set; }
    }
}
