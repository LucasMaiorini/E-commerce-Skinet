using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        //Criteria gets T, return bool
        Expression<Func<T, bool>> Criteria { get; }
        //Includes gets T, returns object
        List<Expression<Func<T, object>>> Includes { get; }
        //Order By gets T, returns object
        Expression<Func<T, object>> OrderBy { get; }
        //Order By Descending gets T, returns object
        Expression<Func<T, object>> OrderByDescending { get; }

        //Property for pagination. We will be able to Skip a certain amount of records.
        int Skip { get; }
        //Property for pagination. We will be able to Take a certain amount of records.
        int Take { get; }
        //Property used in SpecificationEvaluator.cs. Defines if results will be paged
        bool IsPagingEnabled { get; }
    }
}