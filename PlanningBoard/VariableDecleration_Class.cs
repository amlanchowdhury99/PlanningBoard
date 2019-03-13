using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PlanningBoard
{
    public class VariableDecleration_Class
    {
        public static string sMSGBOX = "Planning Board Management System Software".ToUpper();

        public static double txtNumber;
        public static int sShowParticularCombo = 0;
        public static int LCActiveTrack = 0;

        public static int sLoginStatus = 0;
        public static string sUserName = "";
        public static string sUserType = "";

        public static int txtPercen = 0;

        public static string Barcode = "";
        public static string PriBar = "";
        public static string DatabaseUser = "sa";
        public static string DatabasePassword = "123";
        public static string DatabaseServer = "(local)";
        public static string DatabaseName = "PlanningBoard";


        public enum Size
        {
            XS = 1,
            S = 2,
            M = 3,
            L = 4,
            XL = 5,
            XXL = 6
        }

        public enum BodyPart
        {
            Body = 1,
            Sleeve = 2
        }

        public enum WeekDays
        {
            Sunday = 1,
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Saturday = 7
        }

        public enum Month
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 4,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum UserType
        {
            Admin = 1,
            General = 2,
            Guest = 3
        }

        public enum Status
        {
            [Description("In-Active")]
            InActive = 0,
            [Description("Active")]
            Active = 1,
            [Description("Pending")]
            Pending = 2,
            [Description("Complete")]
            Complete = 3
        }

    }
}
