using FlowFinance.GraphQL.Types;
using FlowFinance.Services;
using GraphQL;
using GraphQL.Types;
using System;
using System.Collections.Generic;
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
                    new QueryArgument<NonNullGraphType<ApplicationInputType>> { Name = "application", Description = "Application Input" }
                ),
                resolve: context =>
                {
                    var applicationInput = context.GetArgument<ApplicationInput>("application");
                    return flowFinancePaymentService.ProcessApplication(applicationInput);
                });
        }
    }
}
