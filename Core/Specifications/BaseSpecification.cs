using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        #region Variables
        // The private set defines that the variables will be defined in the class

        //Criteria is the where clause
        public Expression<Func<T, bool>> Criteria { get; }

        //Includes is a list of Includes, the related Entities
        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();


        // An expression that represents the orderBy ordering filter
        public Expression<Func<T, object>> OrderBy { get; private set; }

        // An expression that represents the orderByDescending ordering filter
        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        //Property for pagination. We will be able to Skip a certain amount of records.
        public int Skip { get; private set; }

        //Property for pagination. We will be able to Take a certain amount of records.
        public int Take { get; private set; }

        //Property used in SpecificationEvaluator.cs. Defines if results will be paged
        public bool IsPagingEnabled { get; private set; }

        #endregion

        public BaseSpecification() { }

        /// <summary>
        /// Constructor that sets the specification with where clause 'criteria'
        /// </summary>
        /// <param name="criteria"> represents the where clause</param>
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            this.Criteria = criteria;
        }


        /// <summary>
        /// Adds a 'Include' to a list of Includes.
        /// </summary>
        /// <param name="includeExpression">An Include must be a related entity</param>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            this.Includes.Add(includeExpression);
        }

        //this method needs to be evaluated by our Specification Evaluator
        /// <summary>
        /// Sets the orderBy filter.
        /// </summary>
        /// <param name="orderByExpression">Expression that represents an order by query</param>
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            this.OrderBy = orderByExpression;
        }
        /// <summary>
        /// Sets the orderByDescending filter.
        /// </summary>
        /// <param name="orderByDescendingExpression">Expression that represents an order by descending query</param>
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            this.OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Gets a Skip and a Take number. 
        /// The Skip represents how many records will be skipped in a sequence to start to return the results.
        /// The Take represents how many records do you want in page.
        /// </summary>
        /// <param name="skip">To skip an amount of records</param>
        /// <param name="take">To take an amout of records</param>
        protected void ApplyPaging(int skip, int take)
        {
            this.Skip = skip;
            this.Take = take;
            //Defines if results will be paged
            IsPagingEnabled = true;
        }
    }
}