using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public partial class Meal : IMeal
    {
        private readonly IEnumerable<ICourse> courses;

        public Meal(IEnumerable<ICourse> courses)
        {
            if (courses == null)
            {
                throw new ArgumentNullException("courses");
            }

            this.courses = courses;
        }
    }

    public partial class Meal
    {
        public IEnumerable<ICourse> Courses
        {
            get { return this.courses; }
        }
    }
}
