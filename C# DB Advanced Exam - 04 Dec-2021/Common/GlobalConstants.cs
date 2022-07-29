using System;
using System.Collections.Generic;
using System.Text;

namespace Theatre.Common
{
    public class GlobalConstants
    {
        //Theatre
        public const int THEATRE_NAME_MAX_LENGHT = 40;
        public const int THEATRE_NAME_MIN_LENGHT = 4;
        public const int THEATRE_DIRECTOR_MAX_LENGHT = 30;
        public const int THEATRE_DIRECTOR_MIN_LENGHT = 4;

        //Title
        public const int PLAY_TITLE_MAX_LENGHT = 50;
        public const int PLAY_TITLE_MIN_LENGHT = 4;
        public const int PLAY_DESCRIPTION_MAX_LENGHT = 700;
        public const int PLAY_SCREENWRITER_MAX_LENGHT = 30;
        public const int PLAY_SCREENWRITER_MIN_LENGHT = 4;

        //Cast
        public const int CAST_FULLNAME_MAX_LENGHT = 30;
        public const int CAST_FULLNAME_MIN_LENGHT = 4;
        public const int CAST_PHONENUMBER_MAX_LENGHT = 15;
        public const string CAST_REGGEX_PHONENUMBER = @"\+44-\d{2}-\d{3}-\d{4}";




    }
}
