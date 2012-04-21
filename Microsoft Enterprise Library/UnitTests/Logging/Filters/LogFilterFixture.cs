//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
#if !NUNIT
using Microsoft.VisualStudio.TestTools.UnitTesting;
#else
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Filters.Tests
{
	[TestClass]
	public class LogFilterFixture
	{
		private LogFilterHelper filterHelper;
		private MockLogFilterErrorHandler handler;
		private LogEntry log;

		private CategoryFilter categoryFilter;
		private PriorityFilter priorityFilter;
		private LogEnabledFilter enabledFilter;

		[TestInitialize]
		public void SetUp()
		{
			ICollection<string> categoryFilters = new string[] { "foo" };
			categoryFilter = new CategoryFilter("category", categoryFilters, CategoryFilterMode.DenyAllExceptAllowed);
			priorityFilter = new PriorityFilter("priority", 5);
			enabledFilter = new LogEnabledFilter("enable", true);
			ICollection<ILogFilter> filters = new List<ILogFilter>(3);
			filters.Add(enabledFilter);
			filters.Add(categoryFilter);
			filters.Add(priorityFilter);

			handler = new MockLogFilterErrorHandler(true);

			filterHelper = new LogFilterHelper(filters, handler);

			log = CommonUtil.GetDefaultLogEntry();
		}

		[TestCleanup]
		public void TearDown()
		{
		}

		[TestMethod]
		public void CategoryAllowFilterWithCategoryInAllowList()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "foo" };

			Assert.IsTrue(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<CategoryFilter>().ShouldLog(log.Categories));
		}

		[TestMethod]
		public void CategoryAllowFilterWithCategoryNotInAllowList()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "bar" }; // unlisted category

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<CategoryFilter>().ShouldLog(log.Categories));
		}

		[TestMethod]
		public void CategoryDenyFilterWithCategoryInDenyList()
		{
			SetAllowAllExceptDenied();
			log.Categories = new string[] { "foo" };
			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<CategoryFilter>().ShouldLog(log.Categories));
		}

		[TestMethod]
		public void CategoryDenyFilterWithCategoryNotInDenyList()
		{
			SetAllowAllExceptDenied();
			log.Categories = new string[] { "bar" }; // unlisted category
			Assert.IsTrue(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<CategoryFilter>().ShouldLog(log.Categories));
		}

		[TestMethod]
		public void PriorityAboveMinAndCategoryInAllowList()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "foo" };
			log.Priority = priorityFilter.MinimumPriority + 1;

			Assert.IsTrue(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityAboveMinAndCategoryInDenyList()
		{
			SetAllowAllExceptDenied();
			log.Categories = new string[] { "foo" };
			log.Priority = priorityFilter.MinimumPriority + 1;

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
			Assert.IsFalse(filterHelper.GetFilter<CategoryFilter>().ShouldLog(log.Categories));
		}

		[TestMethod]
		public void PriorityBelowMinAndCategoryInAllowList()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "foo" };
			log.Priority = priorityFilter.MinimumPriority - 1;

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityBelowMinAndCategoryInDenyList()
		{
			categoryFilter.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
			log.Categories = new string[] { "foo" };
			log.Priority = priorityFilter.MinimumPriority - 1;

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityBelowMinAndCategoryNotInAllowList()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "bar" }; // unlisted category
			log.Priority = priorityFilter.MinimumPriority - 1;

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityBelowMinAndCategoryNotInDenyList()
		{
			SetAllowAllExceptDenied();
			log.Categories = new string[] { "bar" }; // unlisted category
			log.Priority = priorityFilter.MinimumPriority - 1;

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityAboveMinAndCategoryNotInAllowList()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "bar" }; // unlisted category
			log.Priority = priorityFilter.MinimumPriority + 1;

			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
			Assert.IsFalse(filterHelper.GetFilter<CategoryFilter>().ShouldLog(log.Categories));
		}

		[TestMethod]
		public void PriorityAboveMinAndCategoryNotInDenyList()
		{
			SetAllowAllExceptDenied();
			log.Categories = new string[] { "bar" }; // unlisted category
			log.Priority = priorityFilter.MinimumPriority + 1;

			Assert.IsTrue(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityFilterWithExactlyTheMinimumPriority()
		{
			SetDenyAllExceptAllowedMode();
			log.Priority = priorityFilter.MinimumPriority;
			log.Categories = new string[] { "foo" };
			Assert.IsTrue(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void PriorityFilterWithZeroPriority()
		{
			SetDenyAllExceptAllowedMode();
			log.Priority = 0;
			log.Categories = new string[] { "foo" };
			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<PriorityFilter>().ShouldLog(log.Priority));
		}

		[TestMethod]
		public void LogDisabled()
		{
			SetDenyAllExceptAllowedMode();
			log.Categories = new string[] { "foo" };

			SetDisabled();
			Assert.IsFalse(filterHelper.CheckFilters(log));
			Assert.IsFalse(filterHelper.GetFilter<LogEnabledFilter>().Enabled);

			SetEnabled();
			Assert.IsTrue(filterHelper.CheckFilters(log));
			Assert.IsTrue(filterHelper.GetFilter<LogEnabledFilter>().Enabled);
		}

		private void SetAllowAllExceptDenied()
		{
			this.categoryFilter.CategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;
		}

		private void SetDenyAllExceptAllowedMode()
		{
			this.categoryFilter.CategoryFilterMode = CategoryFilterMode.DenyAllExceptAllowed;
		}

		private void SetDisabled()
		{
			this.enabledFilter.Enabled = false;
		}

		private void SetEnabled()
		{
			this.enabledFilter.Enabled = true;
		}
	}
}

