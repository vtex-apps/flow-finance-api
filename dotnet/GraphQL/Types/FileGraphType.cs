using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowFinance.GraphQL.Types
{
    public class File
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public string Path { get; set; }
    }

    public class FileGraphType : ObjectGraphType<File>
    {
        public FileGraphType()
        {
            Field(f => f.Id).Name("id");
            Field(f => f.Name).Name("filename");
            Field(f => f.MimeType).Name("mimetype");
            Field(f => f.Path).Name("path");
        }
    }
}
