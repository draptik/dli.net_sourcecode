using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.Samples.MenuModel
{
    public class ThreeCourseMeal : IMeal
    {
        private readonly ICourse entrée;
        private readonly ICourse mainCourse;
        private readonly ICourse dessert;

        public ThreeCourseMeal(ICourse entrée,
            ICourse mainCourse, ICourse dessert)
        {
            if (entrée == null)
            {
                throw new ArgumentNullException("entrée");
            }
            if (mainCourse == null)
            {
                throw new ArgumentNullException("mainCourse");
            }
            if (dessert == null)
            {
                throw new ArgumentNullException("dessert");
            }

            this.entrée = entrée;
            this.mainCourse = mainCourse;
            this.dessert = dessert;
        }

        public ICourse Dessert
        {
            get { return this.dessert; }
        }

        public ICourse Entrée
        {
            get { return this.entrée; }
        }

        public ICourse MainCourse
        {
            get { return this.mainCourse; }
        }
    }
}
