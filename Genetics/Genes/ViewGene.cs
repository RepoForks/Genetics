using System;
using System.Xml;
using System.Xml.Linq;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Org.XmlPull.V1;

using Genetics.Mappings;

namespace Genetics.Genes
{
    public class ViewGene : IGene
    {
		private static readonly Type javaObjectType;

		static ViewGene()
		{
			javaObjectType = typeof(IJavaObject);
		}

        public bool Splice(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            var assigned = false;
            var value = GeneticsExtensions.FindViewById(source, resourceId);
            if (value != null)
            {
                var valueType = value.GetType();
                var memberType = memberMapping.MemberType;

                if (memberType.IsAssignableFrom(valueType))
                {
                    // first see if it a direct inheritance
                    memberMapping.SetterMethod(target, value);
                    assigned = true;
                }
                else if (javaObjectType.IsAssignableFrom(memberType) && javaObjectType.IsAssignableFrom(memberType))
                {
                    // it may be inheritance inside Java
                    memberMapping.SetterMethod(target, value.JavaCast(memberType));
                    assigned = true;
                }
            }
            else
            {
                Geneticist.HandleError(
                    "Unknown view container type '{0}'.",
                    source.GetType().FullName);
            }
            return assigned;
        }

        public void Sever(object target, object source, string resourceType, int resourceId, Context context, MemberMapping memberMapping)
        {
            memberMapping.SetterMethod(target, null);
        }
    }
}