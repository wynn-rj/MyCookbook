namespace MyCookbook
{
    
    public static class Units
    {
        //Display names for the units
        public static string[] unitNames = { "whole", "ounce(s)", "pound(s)", "teaspoon(s)", "tablespoon(s)", "gram(s)", "fl. ounce(s)", "cup(s)", "pint(s)", "quart(s)", "gallon(s)", "mililiter(s)", "can(s) (15oz)" };
        

        public const int CAN = 15;
        public const int WHOLE = 0;

        //Units and there value in ounces
        public static class Dry
        {
            public const int OUNCE = 1;
            public const int POUND = 16;
            public const float TEASPOON = (1 / 6f);
            public const float TABLESPOON = (1 / 2f);
            public const float GRAM = 0.035274f;
        }

        public static class Wet
        {
            public const int FLUIDOUNCE = 1;
            public const int CUP = 8;
            public const int PINT = 16;
            public const int QUART = 32;
            public const int GALLON = 128;
            public const float MILILITER = (1 / 29.57f);
            public const float TEASPOON = (1 / 6f);
            public const float TABLESPOON = (1 / 2f);
        }

        
        /// <summary>
        /// Converts from name to value
        /// </summary>
        /// <param name="unitType">Name of unit</param>
        /// <returns>Value of unit. Returns -1 if the unit is unknown.</returns>
        public static float GetValueOfUnit(string unitType)
        {
            switch (unitType)
            {
                case "whole":
                    return WHOLE;
                    
                case "ounce(s)":
                    return Dry.OUNCE;

                case "pound(s)":
                    return Dry.POUND;

                case "teaspoon(s)":
                    return Dry.TEASPOON;

                case "tablespoon(s)":
                    return Dry.TABLESPOON;

                case "gram(s)":
                    return Dry.GRAM;

                case "fl. ounce(s)":
                    return Wet.FLUIDOUNCE;

                case "cup(s)":
                    return Wet.CUP;

                case "pint(s)":
                    return Wet.PINT;

                case "quart(s)":
                    return Wet.QUART;

                case "gallon(s)":
                    return Wet.GALLON;

                case "mililiter(s)":
                    return Wet.MILILITER;

                case "can(s) (15oz)":
                    return CAN;
                    
                default:
                    return -1;

            }
        }
    }
}
