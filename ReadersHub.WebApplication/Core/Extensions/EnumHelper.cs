using _21stSolution.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadersHub.WebApplication.Core.Extensions
{
    public static class EnumHelper
    {
        public static List<SelectItemDto> GetEnumDropdownItems<T>() where T : struct, IConvertible
        {
            var enumType = typeof(T);
            var enumTypeName = enumType.Name;

            return (from object enumValue in System.Enum.GetValues(enumType)
                    select new SelectItemDto
                    {
                        Value = ((short)enumValue).ToString(),
                        Text = enumTypeName + "." + System.Enum.GetName(enumType, enumValue)
                    }).ToList();
        }
    }
}