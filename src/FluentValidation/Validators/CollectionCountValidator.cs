namespace FluentValidation.Validators {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	public class CountValidator<T> : PropertyValidator<T,ICollection>, ICountValidator {
		public override string Name => "CountValidator";

		public int Min { get; }
		public int Max { get; }

		public Func<T, int> MinFunc { get; set; }
		public Func<T, int> MaxFunc { get; set; }

		public CountValidator(int min, int max) {
			Max = max;
			Min = min;

			if (max != -1 && max < min) {
				throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
			}
		}

		public CountValidator(Func<T, int> min, Func<T, int> max) {
			MaxFunc = max;
			MinFunc = min;
		}

		public override bool IsValid(ValidationContext<T> context, ICollection value) {
			if (value == null) return true;

			var min = Min;
			var max = Max;

			if (MaxFunc != null && MinFunc != null) {
				max = MaxFunc(context.InstanceToValidate);
				min = MinFunc(context.InstanceToValidate);
			}

			int count = value.Count;

			if (count < min || (count > max && max != -1)) {
				context.MessageFormatter
					.AppendArgument("MinCount", min)
					.AppendArgument("MaxCount", max)
					.AppendArgument("TotalCount", count);

				return false;
			}

			return true;
		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class ExactCountValidator<T> : CountValidator<T>, IExactCountValidator {
		public override string Name => "ExactCountValidator";

		public ExactCountValidator(int length) : base(length,length) {

		}

		public ExactCountValidator(Func<T, int> count)
			: base(count, count) {

		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class MaximumCountValidator<T> : CountValidator<T>, IMaximumCountValidator {
		public override string Name => "MaximumCountValidator";

		public MaximumCountValidator(int max)
			: base(0, max) {

		}

		public MaximumCountValidator(Func<T, int> max)
			: base(obj => 0, max) {

		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class MinimumCountValidator<T> : CountValidator<T>, IMinimumCountValidator {

		public override string Name => "MinimumCountValidator";

		public MinimumCountValidator(int min)
			: base(min, -1) {
		}

		public MinimumCountValidator(Func<T, int> min)
			: base(min, obj => -1) {

		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class CountValidator<T, TItemModel> : PropertyValidator<T,ICollection<TItemModel>>, ICountValidator {
		public override string Name => "CountValidator";

		public int Min { get; }
		public int Max { get; }

		public Func<T, int> MinFunc { get; set; }
		public Func<T, int> MaxFunc { get; set; }

		public Func<TItemModel, bool> Filter { get; set; }

		public CountValidator(int min, int max, Func<TItemModel, bool> filter = null) {
			Max = max;
			Min = min;
			Filter = filter;

			if (max != -1 && max < min) {
				throw new ArgumentOutOfRangeException(nameof(max), "Max should be larger than min.");
			}
		}

		public CountValidator(Func<T, int> min, Func<T, int> max,  Func<TItemModel, bool> filter = null) {
			MaxFunc = max;
			MinFunc = min;
			Filter = filter;
		}

		public override bool IsValid(ValidationContext<T> context, ICollection<TItemModel> value) {
			if (value == null) return true;

			var min = Min;
			var max = Max;

			if (MaxFunc != null && MinFunc != null) {
				max = MaxFunc(context.InstanceToValidate);
				min = MinFunc(context.InstanceToValidate);
			}

			int count = value.Count(item => Filter?.Invoke(item) ?? true);

			if (count < min || (count > max && max != -1)) {
				context.MessageFormatter
				       .AppendArgument("MinCount", min)
				       .AppendArgument("MaxCount", max)
				       .AppendArgument("TotalCount", count);

				return false;
			}

			return true;
		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class ExactCountValidator<T, TItemModel> : CountValidator<T, TItemModel>, IExactCountValidator {
		public override string Name => "ExactCountValidator";

		public ExactCountValidator(int count, Func<TItemModel, bool> filter = null) : base(count,count, filter) {
		}

		public ExactCountValidator(Func<T, int> count, Func<TItemModel, bool> filter = null)
			: base(count, count, filter) {

		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class MaximumCountValidator<T, TItemModel> : CountValidator<T, TItemModel>, IMaximumCountValidator {
		public override string Name => "MaximumCountValidator";

		public MaximumCountValidator(int max, Func<TItemModel, bool> filter = null)
			: base(0, max, filter) {

		}

		public MaximumCountValidator(Func<T, int> max, Func<TItemModel, bool> filter = null)
			: base(obj => 0, max, filter) {

		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}

	public class MinimumCountValidator<T, TItemModel> : CountValidator<T, TItemModel>, IMinimumCountValidator {

		public override string Name => "MinimumCountValidator";

		public MinimumCountValidator(int min, Func<TItemModel, bool> filter = null)
			: base(min, -1, filter) {
		}

		public MinimumCountValidator(Func<T, int> min, Func<TItemModel, bool> filter = null)
			: base(min, obj => -1, filter) {

		}

		protected override string GetDefaultMessageTemplate(string errorCode) {
			return Localized(errorCode, Name);
		}
	}
	public interface ICountValidator : IPropertyValidator {
		int Min { get; }
		int Max { get; }
	}

	public interface IMaximumCountValidator : ICountValidator { }

	public interface IMinimumCountValidator : ICountValidator { }

	public interface IExactCountValidator : ICountValidator { }
}
