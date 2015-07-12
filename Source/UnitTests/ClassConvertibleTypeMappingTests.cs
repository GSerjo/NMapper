﻿using System;
using System.ComponentModel;
using System.Globalization;
using Nelibur.ObjectMapper;
using Xunit;

namespace UnitTests
{
    public sealed class ClassConvertibleTypeMappingTests
    {
        public ClassConvertibleTypeMappingTests()
        {
            TypeDescriptor.AddAttributes(typeof(SourceClass), new TypeConverterAttribute(typeof(SourceClassConverter)));
        }

        [Fact]
        public void Map_ConvertibleType_Success()
        {
            TinyMapper.Bind<SourceClass, TargetClass>();
            var source = new SourceClass
            {
                FirstName = "First",
                LastName = "Last"
            };

            var result = TinyMapper.Map<TargetClass>(source);

            Assert.Equal(string.Format("{0} {1}", source.FirstName, source.LastName), result.FullName);
        }


        public class TargetClass
        {
            public string FullName { get; set; }
        }


        public class SourceClass
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }


        public sealed class SourceClassConverter : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(TargetClass);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                var concreteValue = (SourceClass)value;
                var result = new TargetClass
                {
                    FullName = string.Format("{0} {1}", concreteValue.FirstName, concreteValue.LastName)
                };
                return result;
            }
        }
    }
}