using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Recommendation.Extensions
{
    public static class TempData
    {
		public static void SetData(this ITempDataDictionary @this, AlertLevel level, string title, string message)
		{
			@this["Message"] = message;
			@this["Title"] = title;
			@this["Type"] = level.ToString();
		}
	}
}
