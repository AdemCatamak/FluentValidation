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
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using Xunit;
	using Validators;


	public class CountValidatorTests {
		public CountValidatorTests() {
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
		public void When_the_item_count_is_between_the_range_specified_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 10));
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_between_the_range_specified_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 10, order => order != null ));
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_item_count_is_between_the_lambda_range_specified_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 1, MaxLength = 10 });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_between_the_lambda_range_specified_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 1, MaxLength = 10 });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_item_count_is_smaller_than_the_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(5, 10));
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_filtered_the_item_count_is_smaller_than_the_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(4, 10, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_item_count_is_smaller_than_the_lambda_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 5, MaxLength = 10 });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_filtered_item_count_is_smaller_than_the_lambda_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength, order => order != null ));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 4, MaxLength = 10 });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_item_count_is_larger_than_the_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 3));
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_filtered_item_count_is_larger_than_the_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 2, order => order != null ));
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_item_count_is_larger_than_the_lambda_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 1, MaxLength = 3 });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_filtered_the_item_count_is_larger_than_the_lambda_range_then_the_validator_should_fail() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 1, MaxLength = 2 });
			result.IsValid.ShouldBeFalse();
		}

		[Fact]
		public void When_the_item_count_is_exactly_the_size_of_the_upper_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 4));
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_exactly_the_size_of_the_upper_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 3, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_item_count_is_exactly_the_size_of_the_lambda_upper_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 1, MaxLength = 4 });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_exactly_the_size_of_the_lambda_upper_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 1, MaxLength = 3 });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_item_count_is_exactly_the_size_of_the_lower_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(4, 5));
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_exactly_the_size_of_the_lower_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(3, 5, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders});
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_item_count_is_exactly_the_size_of_the_lambda_lower_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 4, MaxLength = 5 });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_filtered_item_count_is_exactly_the_size_of_the_lambda_lower_bound_then_the_validator_should_pass() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(x => x.MinLength, x => x.MaxLength, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders, MinLength = 3, MaxLength = 5 });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_max_is_smaller_than_the_min_then_the_validator_should_throw() {
			Assert.Throws<ArgumentOutOfRangeException>(() =>
				                                           new TestValidator(v => v.RuleFor(x => x.Orders).Count(10, 1))
			                                          );
		}

		[Fact]
		public void When_the_validator_fails_the_error_message_should_be_set() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).Count(1, 2, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders });
			result.Errors.Single().ErrorMessage.ShouldEqual("The count of 'Orders' must be between 1 and 2. You entered 3 items.");
		}

		[Fact]
		public void Min_and_max_properties_should_be_set() {
			var validator = new CountValidator<Person>(1, 5);
			validator.Min.ShouldEqual(1);
			validator.Max.ShouldEqual(5);
		}

		[Fact]
		public void When_input_is_null_then_the_validator_should_pass() {
			var validator = new TestValidator {
				                                  v => v.RuleFor(x => x.Orders).Count(5)
			                                  };

			var result = validator.Validate(new Person { Orders = null });
			result.IsValid.ShouldBeTrue();
		}

		[Fact]
		public void When_the_minlength_validator_fails_the_error_message_should_be_set() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).MinimumCount(4, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders });
			result.Errors.Single().ErrorMessage.ShouldEqual("The count of 'Orders' must be at least 4. You entered 3 items.");
		}


		[Fact]
		public void When_the_maxlength_validator_fails_the_error_message_should_be_set() {
			var validator = new TestValidator(v => v.RuleFor(x => x.Orders).MaximumCount(2, order => order != null));
			var result = validator.Validate(new Person { Orders = _orders });
			result.Errors.Single().ErrorMessage.ShouldEqual("The count of 'Orders' must be 2 or less. You entered 3 items.");
		}
	}
}
