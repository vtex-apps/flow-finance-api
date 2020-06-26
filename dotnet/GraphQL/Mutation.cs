using FlowFinance.GraphQL.Types;
using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using GraphQL.Upload.AspNetCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FlowFinance.GraphQL
{
    [GraphQLMetadata("Mutation")]
    public class Mutation : ObjectGraphType<object>
    {
        public Mutation(IFlowFinancePaymentService flowFinancePaymentService)
        {
            Name = "Mutation";

            Field<BooleanGraphType>(
                "checkPreQualify",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "cnpj", Description = "pre-qualify" }
                ),
                resolve: context =>
                {
                    var cnpj = context.GetArgument<string>("cnpj");
                    return flowFinancePaymentService.PreQualify(cnpj);
                });

            Field<ApplicationResultType>(
                "processApplication",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ApplicationInputType>> { Name = "application", Description = "Application Input" },
                    new QueryArgument<NonNullGraphType<UploadGraphType>> { Name = "businessInfoFile" },
                    new QueryArgument<NonNullGraphType<UploadGraphType>> { Name = "personalInfoFile" }
                ),
                resolve: context =>
                {
                    var applicationInput = context.GetArgument<ApplicationInput>("application");
                    var businessInfoFile = context.GetArgument<IFormFile>("businessInfoFile");
                    var personalInfoFile = context.GetArgument<IFormFile>("personalInfoFile");

                    return flowFinancePaymentService.ProcessApplication(applicationInput, businessInfoFile, personalInfoFile);
                });
        }
    }
}
