using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Linq {
	class Program {
		static void Main(string[] args) {
			var user = new User
			{
				FirstName = "Stepan",
				LastName = "Myr",
				IsActive = false
			};

			var phone = new Phone
			{
				CountryId = 32,
				Number = "+380977898999",
				IsActive = true
			};

			HtmlHelper<User> userHelper = new HtmlHelper<User>(new ViewContext(), new ViewPage{ViewData = new ViewDataDictionary{Model = user}});
			HtmlHelper<Phone> phoneHelper = new HtmlHelper<Phone>(new ViewContext(), new ViewPage{ ViewData = new ViewDataDictionary{Model = phone}});

			//Invoke with custom functions
			var activeValueForUser = GetValueWithFunc(userHelper, ReturnUserIsActive);
			var activeValueForPhone = GetValueWithFunc(phoneHelper, ReturnPhoneIsActive);

			//Invoke with lamba functions
			activeValueForUser = GetValueWithFunc(userHelper, t => t.IsActive);
			activeValueForPhone = GetValueWithFunc(phoneHelper, t => t.IsActive);

			//Invoke with expressions
			var phoneValueForPhone = GetValueWithExpression(phoneHelper, t => t.Number);
		}

		//Take Func. We can't get metadata for properties
		public static string GetValueWithFunc<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Func<TModel, TProperty> expression) {

			object value = expression(htmlHelper.ViewData.Model);

			string strLabel = "-";
			if (value != null) {
				if (value is bool) {
					if ((bool)value) {
						strLabel = "Yes";
					} else {
						strLabel = "No";
					}
				} else {
					strLabel = value.ToString();
				}
			}

			return strLabel;
		}

		public static string GetValueWithExpression<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) {
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			var value = metadata.Model;

			string strLabel = "-";
			if (value != null) {
				if (value is bool) {
					if ((bool)value) {
						strLabel = "Yes";
					} else {
						strLabel = "No";
					}
				} else {
					strLabel = value.ToString();
				}

				if (metadata.AdditionalValues.ContainsKey("PhoneNumber"))
				{
					strLabel = strLabel + "<a href=\"callto://" + strLabel.Replace(" ", string.Empty) + "\" class=\"clear-underline outgoing-call\"><i class=\"icon-outgoing-call metro-color\"></i></a>";
				}
			}

			return strLabel;
		}

		public static bool ReturnUserIsActive(User user)
		{
			return user.IsActive;
		}

		public static bool ReturnPhoneIsActive(Phone phone) {
			return phone.IsActive;
		}
	}


	class User {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool IsActive { get; set; }
	}

	class Phone {
		[GluedPhoneAttribute]
		public string Number { get; set; }
		public bool IsActive { get; set; }
		public int CountryId { get; set; }
	}
}
