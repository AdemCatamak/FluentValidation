#region License
// Copyright (c) .NET Foundation and contributors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// The latest version of this file can be found at https://github.com/FluentValidation/FluentValidation
#endregion

namespace FluentValidation.Tests {
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using Xunit;
	using Validators;


	public class ExactCountValidatorTester {

		public ExactCountValidatorTester() {
			CultureScope.SetDefaultCulture();
		}

		private readonly List<Order> _orders
			= new List<Order>() {
				                    null,
				                    new Order() { Amount = 10, ProductName = "product-1" },
				                    new Order() { Amount = 20, ProductName = "product-1" },
				                    new Order() { Amount = 15, ProductName = "product-2" }
			                    };

		[Fact]
		public void When_the_item_count_is_an_exact_count_the_validator_should_pass() {
			var validator = new TestValidator { v => v.RuleFor(x => x.Orders).Count(4) };
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_an_exact_count_the_validator_should_pass() {
			var validator = new TestValidator { v => v.RuleFor(x => x.Orders).Count(3, order => order != null) };
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_item_count_is_smaller_the_validator_should_fail() {
			var validator = new TestValidator {v => v.RuleFor(x => x.Orders).Count(10) };
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_filtered_item_count_is_smaller_the_validator_should_fail() {
			var validator = new TestValidator {v => v.RuleFor(x => x.Orders).Count(10, order => order != null) };
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_item_count_is_larger_the_validator_should_fail() {
			var validator = new TestValidator { v => v.RuleFor(x => x.Orders).Count(1) };
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_filtered_item_count_is_larger_the_validator_should_fail() {
			var validator = new TestValidator { v => v.RuleFor(x => x.Orders).Count(1, order => order != null) };
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_validator_fails_the_error_message_should_be_set() {
			var validator = new TestValidator { v => v.RuleFor(x => x.Orders).Count(2, order => order != null) };
			var result = validator.Validate(new Person() { Orders = _orders});
			result.Errors.Single().ErrorMessage.ShouldEqual("The count of 'Orders' must be 2. You entered 3 items.");
		}

		[Fact]
		public void Min_and_max_properties_should_be_set() {
			var validator = new ExactCountValidator<Person>(5);
			validator.Min.ShouldEqual(5);
			validator.Max.ShouldEqual(5);
		}

		[Fact]
		public void When_exact_count_rule_failes_error_should_have_exact_count_error_errorcode() {
			var validator = new TestValidator { v => v.RuleFor(x => x.Orders).Count(2) };

			var result = validator.Validate(new Person() { Orders = _orders });
			var error = result.Errors.SingleOrDefault(e => e.ErrorCode == "ExactCountValidator");

			error.ShouldNotBeNull();
			error.PropertyName.ShouldEqual("Orders");
			error.AttemptedValue.ShouldEqual("4");

			error.FormattedMessagePlaceholderValues.Count.ShouldEqual(5);
			error.FormattedMessagePlaceholderValues.ContainsKey("PropertyName").ShouldBeTrue();
			error.FormattedMessagePlaceholderValues.ContainsKey("PropertyValue").ShouldBeTrue();
			error.FormattedMessagePlaceholderValues.ContainsKey("MinCount").ShouldBeTrue();
			error.FormattedMessagePlaceholderValues.ContainsKey("MaxCount").ShouldBeTrue();
			error.FormattedMessagePlaceholderValues.ContainsKey("TotalCount").ShouldBeTrue();

			error.FormattedMessagePlaceholderValues["PropertyName"].ShouldEqual("Orders");
			error.FormattedMessagePlaceholderValues["PropertyValue"].ShouldEqual("test");
			error.FormattedMessagePlaceholderValues["MinCount"].ShouldEqual(2);
			error.FormattedMessagePlaceholderValues["MaxCount"].ShouldEqual(2);
			error.FormattedMessagePlaceholderValues["TotalCount"].ShouldEqual(4);
		}
	}
}
